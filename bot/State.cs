using System;
using System.Collections.Generic;
using System.Linq;

namespace bot
{
    public class State
    {
        public readonly Player Me;
        public readonly Player Enemy;
        public readonly List<Order> Orders;

        public State(Player me, Player enemy, List<Order> orders)
        {
            Me = me;
            Enemy = enemy;
            Orders = orders;
        }

        public void MyExecute(BotCommand command) => Execute(command, Me);
        public void EnemyExecute(BotCommand command) => Execute(command, Enemy);

        private void Execute(BotCommand command, Player executingPlayer)
        {
            switch (command)
            {
                case Brew brew:
                    ExecuteBrew(brew, executingPlayer);
                    break;
                case CastCommand castCommand:
                    ExecuteCast(castCommand, executingPlayer);
                    break;
                case Rest:
                    ExecuteRest(executingPlayer);
                    break;
            }
        }

        private void ExecuteRest(Player executingPlayer)
        {
            foreach (var cast in executingPlayer.Casts) cast.IsCastable = true;
        }

        private void ExecuteCast(CastCommand castCommand, Player executingPlayer)
        {
            var cast = executingPlayer.Casts.Single(c => c.Id == castCommand.CastId);
            if (!cast.IsCastable)
                throw new InvalidOperationException("Cast is not castable");
            
            executingPlayer.Inventory.EnsurePossibleToApply(cast.Delta);
            executingPlayer.Inventory.Apply(cast.Delta);
            
            cast.IsCastable = false;
        }

        private void ExecuteBrew(Brew command, Player executingPlayer)
        {
            var order = Orders.Single(ga => ga.Id == command.OrderId);

            executingPlayer.Inventory.EnsurePossibleToApply(order.Delta);
            executingPlayer.Inventory.Apply(order.Delta);
            
            executingPlayer.Score += order.Price;
        }

        public State Copy() => new(Me.Copy(), Enemy.Copy(), Orders);

        public override string ToString() => 
            $"State{{Me={Me},\n\n Enemy={Enemy},\n\n Orders={string.Join('\n', Orders.Select(a => a.ToString()))}}}";
    }

    public class Player
    {
        public readonly IngredientsVolume Inventory;
        public readonly List<Cast> Casts;
        public int Score;

        public Player(IngredientsVolume inventory, List<Cast> casts, int score)
        {
            Inventory = inventory;
            Score = score;
            Casts = casts;
        }
        
        public bool IsPossibleToCast(int castId)
        {
            var cast = Casts.Single(c => c.Id == castId);
            return cast.IsCastable && Inventory.IsPossibleToApply(cast.Delta);
        }

        public Player Copy()
        {
            return new Player(
                Inventory.Copy(),
                new List<Cast>(Casts.Select(c => c.Copy())),
                Score);
        }
        
        public override string ToString() => 
            $"Player{{Inventory={Inventory}, Score={Score}, Casts={string.Join('\n', Casts.Select(a => a.ToString()))}";
    }

    public record Order(int Id, IngredientsVolume Delta, int Price)
    {
        public override string ToString() => 
            $"Order{{Id={Id}, Delta={Delta}, Price={Price}}}";
    }

    public record Cast(int Id, IngredientsVolume Delta, bool IsCastable)
    {
        public bool IsCastable { get; set; } = IsCastable;

        public override string ToString() => 
            $"Cast{{Id={Id}, Delta={Delta}, IsCastable={IsCastable}}}";

        public Cast Copy() => this with { Delta = Delta.Copy() };
    }
    

    public class IngredientsVolume
    {
        public const int MaxIngredientsCount = 10;
        
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

        public bool IsPossibleToApply(IngredientsVolume delta) =>
            IsEnoughIngredientsToApply(delta) &&
            IsLessOrEqualMaxIngredientsCountAfterApply(delta);

        public void EnsurePossibleToApply(IngredientsVolume delta)
        {
            if (!IsEnoughIngredientsToApply(delta))
                throw new InvalidOperationException("Not enough ingredients to apply");
            
            if (!IsLessOrEqualMaxIngredientsCountAfterApply(delta))
                throw new InvalidOperationException("Cant apply because too much ingredients");;
        }

        public IngredientsVolume GetIngredientsDelta(IngredientsVolume order)
        {
            var zero = order.ZeroIngredient - ZeroIngredient;
            var first = order.FirstIngredient - FirstIngredient;
            var second = order.SecondIngredient - SecondIngredient;
            var third = order.ThirdIngredient - ThirdIngredient;

            return new IngredientsVolume(zero, first, second, third);
        }

        private bool IsEnoughIngredientsToApply(IngredientsVolume delta) =>
            ZeroIngredient + delta.ZeroIngredient >= 0 &&
            FirstIngredient + delta.FirstIngredient >= 0 &&
            SecondIngredient + delta.SecondIngredient >= 0 &&
            ThirdIngredient + delta.ThirdIngredient >= 0;

        private bool IsLessOrEqualMaxIngredientsCountAfterApply(IngredientsVolume delta) =>
            ZeroIngredient + delta.ZeroIngredient +
            FirstIngredient + delta.FirstIngredient +
            SecondIngredient + delta.SecondIngredient +
            ThirdIngredient + delta.ThirdIngredient
            <= MaxIngredientsCount;

        public override string ToString() => 
            $"Ingredients[{ZeroIngredient}, {FirstIngredient}, {SecondIngredient}, {ThirdIngredient}]";

        public IngredientsVolume Copy() =>
            new(ZeroIngredient,
                FirstIngredient,
                SecondIngredient,
                ThirdIngredient);
    }
}