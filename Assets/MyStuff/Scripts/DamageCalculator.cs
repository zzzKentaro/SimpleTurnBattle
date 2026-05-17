using UnityEngine;

public static class DamageCalculator
{
    public static DamageResult CalculateAttackDamage(
        BattleActor attacker,
        BattleActor defender,
        SkillData skill
    )
    {
        DamageResult result = new DamageResult();

        int hitRoll = Random.Range(1, 101);
        result.isHit = hitRoll <= skill.accuracy;

        if (!result.isHit)
        {
            result.damage = 0;
            return result;
        }

        result.reaction = defender.Data.GetReaction(skill.element);

        if (result.reaction == ElementReaction.Null)
        {
            result.isNull = true;
            result.damage = 0;
            return result;
        }

        int baseDamage = skill.power + attacker.Data.attack - defender.Data.defense;

        // 無効以外なら最低1ダメージ
        if (baseDamage < 1)
        {
            baseDamage = 1;
        }

        float multiplier = defender.Data.GetElementMultiplier(skill.element);

        int finalDamage = Mathf.RoundToInt(baseDamage * multiplier);

        if (finalDamage < 1)
        {
            finalDamage = 1;
        }

        result.damage = finalDamage;

        return result;
    }
}