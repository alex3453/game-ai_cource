using System;
using System.Linq;

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

    public void MyExecute(BotCommand command) => Execute(command, Me);
    public void EnemyExecute(BotCommand command) => Execute(command, Enemy);

    private void Execute(BotCommand command, Player executingPlayer)
    {
        if (command.GetType() == typeof(Brew)) ExecuteBrew((Brew)command, executingPlayer);

        if (command.GetType() == typeof(Wait)) return;
    }

    private void ExecuteBrew(Brew command, Player executingPlayer)
    {
        var brewCommand = command;
        var order = Actions.Single(ga => ga.Id == brewCommand.OrderId);
        if (order.Type != GameActionType.Brew)
            throw new InvalidOperationException("Can't brew not BREW type action");

        if (!executingPlayer.Inventory.IsPossibleToApply(order.Delta))
            throw new InvalidOperationException("Not enough ingredients for BREW this order");

        executingPlayer.Inventory.Apply(order.Delta);
        executingPlayer.Score += order.Price;
    }
}

public class Player
{
    public readonly IngredientsVolume Inventory; // ingredient counts
    public int Score;

    public Player(IngredientsVolume inventory, int score)
    {
        Inventory = inventory;
        Score = score;
    }
}

public record GameAction(int Id, 
    GameActionType Type, 
    IngredientsVolume Delta, 
    int Price, 
    int TomeIndex = 0, 
    int TaxCount = 0, 
    bool Castable = false, 
    bool Repeatable = false);

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

    public bool IsPossibleToApply(IngredientsVolume delta)
    {
        return ZeroIngredient >= delta.ZeroIngredient &&
        FirstIngredient >= delta.FirstIngredient &&
        SecondIngredient >= delta.SecondIngredient &&
        ThirdIngredient >= delta.ThirdIngredient;
    }
}