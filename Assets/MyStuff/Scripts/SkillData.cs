using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "TurnBattle/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("基本情報")]
    public string skillName;
    public Sprite skillIcon;

    [Header("技の内容")]
    public SkillKind skillKind = SkillKind.Attack;
    public SkillTarget target = SkillTarget.Enemy;
    public BattleElement element = BattleElement.Fire;

    [Header("数値")]
    public int power = 10;

    [Range(0, 100)]
    public int accuracy = 100;

    [Header("演出")]
    public GameObject effectPrefab;
    public float effectDestroyTime = 2.0f;
}