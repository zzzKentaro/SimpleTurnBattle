using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("バトルに参加するキャラクター")]
    [SerializeField] private CharacterData playerData;
    [SerializeField] private CharacterData enemyData;

    [Header("キャラクター表示")]
    [SerializeField] private BattleCharacterView playerView;
    [SerializeField] private BattleCharacterView enemyView;

    [Header("UI")]
    [SerializeField] private SkillButtonUI[] skillButtons;
    [SerializeField] private BattleLogUI battleLog;
    [SerializeField] private TMP_Text turnText;

    [Header("テンポ調整")]
    [SerializeField] private float hpAnimationTime = 0.4f;
    [SerializeField] private float actionInterval = 0.4f;

    private BattleActor playerActor;
    private BattleActor enemyActor;

    private SkillData selectedPlayerSkill;
    private SkillData selectedEnemySkill;

    private BattleState currentState = BattleState.None;

    private int turnCount = 1;

    private void Start()
    {
        StartCoroutine(BattleStartRoutine());
    }

    private IEnumerator BattleStartRoutine()
    {
        currentState = BattleState.Start;

        if (playerData == null || enemyData == null)
        {
            Debug.LogError("PlayerData または EnemyData が設定されていません。");
            yield break;
        }

        playerActor = new BattleActor(playerData);
        enemyActor = new BattleActor(enemyData);

        if (playerView != null)
        {
            playerView.Setup(playerData, playerActor.CurrentHP);
        }

        if (enemyView != null)
        {
            enemyView.Setup(enemyData, enemyActor.CurrentHP);
        }

        SetupSkillButtons();
        SetSkillButtonsInteractable(false);

        UpdateTurnText();

        yield return ShowLog("バトル開始！");

        BeginPlayerCommand();
    }

    private void SetupSkillButtons()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            SkillData skill = null;

            if (i < playerData.skills.Count)
            {
                skill = playerData.skills[i];
            }

            skillButtons[i].Setup(i, skill, OnSkillButtonClicked);
        }
    }

    private void BeginPlayerCommand()
    {
        currentState = BattleState.PlayerCommand;

        selectedPlayerSkill = null;
        selectedEnemySkill = null;

        UpdateTurnText();

        if (battleLog != null)
        {
            battleLog.SetText("技を選んでください。");
        }

        SetSkillButtonsInteractable(true);
    }

    private void OnSkillButtonClicked(int index)
    {
        if (currentState != BattleState.PlayerCommand)
        {
            return;
        }

        if (index < 0 || index >= playerData.skills.Count)
        {
            return;
        }

        SkillData skill = playerData.skills[index];

        if (skill == null)
        {
            return;
        }

        selectedPlayerSkill = skill;

        SetSkillButtonsInteractable(false);

        StartCoroutine(TurnRoutine());
    }

    private IEnumerator TurnRoutine()
    {
        currentState = BattleState.EnemyCommand;

        selectedEnemySkill = ChooseEnemySkill();

        yield return ShowLog(playerActor.Name + "は " + selectedPlayerSkill.skillName + " を選んだ。");
        yield return ShowLog(enemyActor.Name + "は技を選んだ。");

        currentState = BattleState.ActionOrderDecide;

        bool isPlayerFirst = DecidePlayerFirst();

        if (isPlayerFirst)
        {
            yield return ShowLog(playerActor.Name + "の方が速い！");
        }
        else
        {
            yield return ShowLog(enemyActor.Name + "の方が速い！");
        }

        if (isPlayerFirst)
        {
            yield return ExecuteAction(playerActor, enemyActor, selectedPlayerSkill, true);

            if (enemyActor.IsDead)
            {
                yield return FinishBattle(true);
                yield break;
            }

            if (playerActor.IsDead)
            {
                yield return FinishBattle(false);
                yield break;
            }

            yield return ExecuteAction(enemyActor, playerActor, selectedEnemySkill, false);

            if (enemyActor.IsDead)
            {
                yield return FinishBattle(true);
                yield break;
            }

            if (playerActor.IsDead)
            {
                yield return FinishBattle(false);
                yield break;
            }
        }
        else
        {
            yield return ExecuteAction(enemyActor, playerActor, selectedEnemySkill, false);

            if (enemyActor.IsDead)
            {
                yield return FinishBattle(true);
                yield break;
            }

            if (playerActor.IsDead)
            {
                yield return FinishBattle(false);
                yield break;
            }

            yield return ExecuteAction(playerActor, enemyActor, selectedPlayerSkill, true);

            if (enemyActor.IsDead)
            {
                yield return FinishBattle(true);
                yield break;
            }

            if (playerActor.IsDead)
            {
                yield return FinishBattle(false);
                yield break;
            }
        }

        turnCount++;

        yield return ShowLog("次のターンへ。");

        BeginPlayerCommand();
    }

    private SkillData ChooseEnemySkill()
    {
        List<SkillData> usableSkills = new List<SkillData>();

        foreach (SkillData skill in enemyData.skills)
        {
            if (skill != null)
            {
                usableSkills.Add(skill);
            }
        }

        if (usableSkills.Count == 0)
        {
            Debug.LogWarning("敵に技が設定されていません。");
            return null;
        }

        int randomIndex = Random.Range(0, usableSkills.Count);
        return usableSkills[randomIndex];
    }

    private bool DecidePlayerFirst()
    {
        int playerSpeed = playerActor.Data.speed;
        int enemySpeed = enemyActor.Data.speed;

        if (playerSpeed > enemySpeed)
        {
            return true;
        }

        if (playerSpeed < enemySpeed)
        {
            return false;
        }

        // 同速ならランダム
        return Random.value < 0.5f;
    }

    private IEnumerator ExecuteAction(
        BattleActor user,
        BattleActor opponent,
        SkillData skill,
        bool isPlayerAction
    )
    {
        if (user.IsDead)
        {
            yield break;
        }

        if (skill == null)
        {
            yield return ShowLog(user.Name + "は何もできなかった。");
            yield break;
        }

        currentState = isPlayerAction ? BattleState.PlayerAction : BattleState.EnemyAction;

        yield return ShowLog(user.Name + "の " + skill.skillName + "！");

        BattleActor target = GetTarget(user, opponent, skill);
        BattleCharacterView targetView = GetView(target);

        PlaySkillEffect(skill, targetView);

        yield return new WaitForSeconds(actionInterval);

        if (skill.skillKind == SkillKind.Attack)
        {
            yield return ProcessAttack(user, target, skill);
        }
        else if (skill.skillKind == SkillKind.Heal)
        {
            yield return ProcessHeal(user, target, skill);
        }

        yield return new WaitForSeconds(actionInterval);
    }

    private BattleActor GetTarget(BattleActor user, BattleActor opponent, SkillData skill)
    {
        switch (skill.target)
        {
            case SkillTarget.Self:
                return user;

            case SkillTarget.Enemy:
                return opponent;

            default:
                return opponent;
        }
    }

    private IEnumerator ProcessAttack(BattleActor attacker, BattleActor defender, SkillData skill)
    {
        currentState = BattleState.DamageProcess;

        DamageResult result = DamageCalculator.CalculateAttackDamage(attacker, defender, skill);

        if (!result.isHit)
        {
            yield return ShowLog("しかし外れた！");
            yield break;
        }

        if (result.isNull)
        {
            yield return ShowLog(defender.Name + "には効果がない！");
            yield break;
        }

        int beforeHP = defender.CurrentHP;

        defender.TakeDamage(result.damage);

        BattleCharacterView defenderView = GetView(defender);

        if (defenderView != null)
        {
            StartCoroutine(defenderView.FlashDamage());
            yield return defenderView.AnimateHP(beforeHP, defender.CurrentHP, defender.MaxHP, hpAnimationTime);
        }

        string reactionText = GetReactionText(result.reaction);

        if (string.IsNullOrEmpty(reactionText))
        {
            yield return ShowLog(defender.Name + "に " + result.damage + " ダメージ！");
        }
        else
        {
            yield return ShowLog(defender.Name + "に " + result.damage + " ダメージ！ " + reactionText);
        }
    }

    private IEnumerator ProcessHeal(BattleActor user, BattleActor target, SkillData skill)
    {
        currentState = BattleState.DamageProcess;

        int healAmount = skill.power + user.Data.attack;

        if (healAmount < 1)
        {
            healAmount = 1;
        }

        int beforeHP = target.CurrentHP;

        target.Heal(healAmount);

        BattleCharacterView targetView = GetView(target);

        if (targetView != null)
        {
            yield return targetView.AnimateHP(beforeHP, target.CurrentHP, target.MaxHP, hpAnimationTime);
        }

        yield return ShowLog(target.Name + "はHPを " + healAmount + " 回復した！");
    }

    private BattleCharacterView GetView(BattleActor actor)
    {
        if (actor == playerActor)
        {
            return playerView;
        }

        if (actor == enemyActor)
        {
            return enemyView;
        }

        return null;
    }

    private void PlaySkillEffect(SkillData skill, BattleCharacterView targetView)
    {
        if (skill == null)
        {
            return;
        }

        if (skill.effectPrefab == null)
        {
            return;
        }

        Vector3 spawnPosition = transform.position;

        if (targetView != null)
        {
            spawnPosition = targetView.EffectPosition;
        }

        GameObject effect = Instantiate(skill.effectPrefab, spawnPosition, Quaternion.identity);

        if (skill.effectDestroyTime > 0)
        {
            Destroy(effect, skill.effectDestroyTime);
        }
    }

    private IEnumerator FinishBattle(bool playerWin)
    {
        currentState = BattleState.CheckBattleEnd;

        SetSkillButtonsInteractable(false);

        if (playerWin)
        {
            currentState = BattleState.Win;
            yield return ShowLog(enemyActor.Name + "は戦闘不能になった。");
            yield return ShowLog("勝利！");
        }
        else
        {
            currentState = BattleState.Lose;
            yield return ShowLog(playerActor.Name + "は戦闘不能になった。");
            yield return ShowLog("敗北……");
        }
    }

    private void SetSkillButtonsInteractable(bool interactable)
    {
        foreach (SkillButtonUI button in skillButtons)
        {
            if (button != null)
            {
                button.SetInteractable(interactable);
            }
        }
    }

    private void UpdateTurnText()
    {
        if (turnText != null)
        {
            turnText.text = "Turn " + turnCount;
        }
    }

    private IEnumerator ShowLog(string message)
    {
        if (battleLog != null)
        {
            yield return battleLog.ShowLog(message);
        }
        else
        {
            Debug.Log(message);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private string GetReactionText(ElementReaction reaction)
    {
        switch (reaction)
        {
            case ElementReaction.Weak:
                return "弱点！";

            case ElementReaction.Resist:
                return "いまひとつ。";

            case ElementReaction.Normal:
                return "";

            case ElementReaction.Null:
                return "効果がない！";

            default:
                return "";
        }
    }
}