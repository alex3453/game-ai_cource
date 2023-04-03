namespace bot;

public class State
{
    public readonly Player Me;
    public readonly Player Enemy;
    public readonly GameAction[] Actions;

    public State(Player me, Player enemy, GameAction[] actions)
    {
        Me = me;
        Enemy = enemy;
        Actions = actions;
    }
}

public class Player
{
    public readonly IngredientsVolume Inventory; // ingredient counts
    public readonly int Score;

    public Player(IngredientsVolume inventory, int score)
    {
        Inventory = inventory;
        Score = score;
    }
}

public class GameAction
{
    public readonly int Id; // the unique ID of this spell or recipe
    public readonly GameActionType Type; // in the first league: BREW; later: CAST, OPPONENT_CAST, LEARN, BREW
    public readonly IngredientsVolume Delta; // ingredients changes
    public readonly int Price; // the price in rupees if this is a potion
    public readonly int TomeIndex; // in the first two leagues: always 0; later: the index in the tome if this is a tome spell, equal to the read-ahead tax; For brews, this is the value of the current urgency bonus
    public readonly int TaxCount; // in the first two leagues: always 0; later: the amount of taxed tier-0 ingredients you gain from learning this spell; For brews, this is how many times you can still gain an urgency bonus
    public readonly bool Castable; // in the first league: always 0; later: 1 if this is a castable player spell
    public readonly bool Repeatable; // for the first two leagues: always 0; later: 1 if this is a repeatable player spell

    public GameAction(
        int id, 
        GameActionType type, 
        IngredientsVolume delta, 
        int price, 
        int tomeIndex = 0, 
        int taxCount = 0, 
        bool castable = false, 
        bool repeatable = false)
    {
        Id = id;
        Type = type;
        Delta = delta;
        Price = price;
        TomeIndex = tomeIndex;
        TaxCount = taxCount;
        Castable = castable;
        Repeatable = repeatable;
    }
}

public enum GameActionType
{
    Brew
}

public class IngredientsVolume
{
    public int ZeroIngredient;
    public int FirstIngredient;
    public int SecondIngredient;
    public int ThirdIngredient;

    public IngredientsVolume(
        int zeroIngredient, 
        int firstIngredient, 
        int secondIngredient, 
        int thirdIngredient)
    {
        ZeroIngredient = zeroIngredient;
        FirstIngredient = firstIngredient;
        SecondIngredient = secondIngredient;
        ThirdIngredient = thirdIngredient;
    }

    public void Apply(IngredientsVolume delta)
    {
        ZeroIngredient += delta.ZeroIngredient;
        FirstIngredient += delta.FirstIngredient;
        SecondIngredient += delta.SecondIngredient;
        ThirdIngredient += delta.ThirdIngredient;
    }
}