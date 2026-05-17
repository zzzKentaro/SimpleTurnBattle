public enum BattleState
{
    None,

    Start,
    PlayerCommand,
    EnemyCommand,
    ActionOrderDecide,

    PlayerAction,
    EnemyAction,
    DamageProcess,

    CheckBattleEnd,

    Win,
    Lose
}