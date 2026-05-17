using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "TurnBattle/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("基本情報")]
    public string characterName;
    public Sprite characterSprite;

    [Header("ステータス")]
    public int maxHP = 100;
    public int attack = 10;
    public int defense = 5;
    public int speed = 10;

    [Header("属性相性")]
    public List<ElementAffinity> affinities = new List<ElementAffinity>();

    [Header("覚えている技")]
    public List<SkillData> skills = new List<SkillData>();

    public ElementReaction GetReaction(BattleElement element)
    {
        foreach (ElementAffinity affinity in affinities)
        {
            if (affinity.element == element)
            {
                return affinity.reaction;
            }
        }

        // 未設定なら普通扱い
        return ElementReaction.Normal;
    }

    public float GetElementMultiplier(BattleElement element)
    {
        foreach (ElementAffinity affinity in affinities)
        {
            if (affinity.element == element)
            {
                return affinity.GetMultiplier();
            }
        }

        // 未設定なら普通扱い
        return 1.0f;
    }
}