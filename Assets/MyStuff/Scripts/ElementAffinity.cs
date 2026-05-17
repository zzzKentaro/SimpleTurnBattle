using System;
using UnityEngine;

[Serializable]
public class ElementAffinity
{
    public BattleElement element;
    public ElementReaction reaction = ElementReaction.Normal;

    public float GetMultiplier()
    {
        switch (reaction)
        {
            case ElementReaction.Weak:
                return 1.5f;

            case ElementReaction.Normal:
                return 1.0f;

            case ElementReaction.Resist:
                return 0.7f;

            case ElementReaction.Null:
                return 0.0f;

            default:
                return 1.0f;
        }
    }
}