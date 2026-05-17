public class BattleActor
{
    public CharacterData Data { get; private set; }
    public int CurrentHP { get; private set; }

    public string Name
    {
        get
        {
            if (Data == null)
            {
                return "Unknown";
            }

            return Data.characterName;
        }
    }

    public int MaxHP
    {
        get
        {
            if (Data == null)
            {
                return 1;
            }

            return Data.maxHP;
        }
    }

    public bool IsDead
    {
        get
        {
            return CurrentHP <= 0;
        }
    }

    public BattleActor(CharacterData data)
    {
        Data = data;
        CurrentHP = data.maxHP;
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;

        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }
    }

    public void Heal(int amount)
    {
        CurrentHP += amount;

        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
    }
}