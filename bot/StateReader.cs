using System;
using System.Collections.Generic;

namespace bot
{
    public static class StateReader
    {
        public static State ReadState(this ConsoleReader console)
        {
            var orders = new List<Order>();
            var myCasts = new List<Cast>();
            var enemyCasts = new List<Cast>();
            
            var actionCount = int.Parse(console.ReadLine());
            for (int i = 0; i < actionCount; i++)
            {
                var inputs = console.ReadLine().Split(' ');
                var action = ReadGameAction(inputs);
                switch (action.Type)
                {
                    case "BREW":
                        orders.Add(ParseOrder(action));
                        break;
                    case "CAST":
                        myCasts.Add(ParseCast(action));
                        break;
                    case "OPPONENT_CAST":
                        enemyCasts.Add(ParseCast(action));
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown action type {action.Type}");
                }
            }
        
            var myInputs = console.ReadLine().Split(' ');
            var me = ReadPlayer(myInputs, myCasts);
        
            var enemyInputs = console.ReadLine().Split(' ');
            var enemy = ReadPlayer(enemyInputs, enemyCasts);
        
            return new State(me, enemy, orders);
        }

        private static Cast ParseCast(GameAction action) =>
            new(action.Id, action.Delta, action.Castable);

        private static Order ParseOrder(GameAction action) => new(action.Id, action.Delta, action.Price);

        private static Player ReadPlayer(string[] inputs, List<Cast> casts)
        {
            var inv0 = int.Parse(inputs[0]);
            var inv1 = int.Parse(inputs[1]);
            var inv2 = int.Parse(inputs[2]);
            var inv3 = int.Parse(inputs[3]);
            var inventory = new IngredientsVolume(inv0, inv1, inv2, inv3);

            var score = int.Parse(inputs[4]);

            return new Player(inventory, casts, score);
        }

        private static GameAction ReadGameAction(string[] inputs)
        {
            var actionId = int.Parse(inputs[0]); // the unique ID of this spell or recipe

            var actionType = inputs[1];

            var delta0 = int.Parse(inputs[2]); // tier-0 ingredient change
            var delta1 = int.Parse(inputs[3]); // tier-1 ingredient change
            var delta2 = int.Parse(inputs[4]); // tier-2 ingredient change
            var delta3 = int.Parse(inputs[5]); // tier-3 ingredient change
            var delta = new IngredientsVolume(delta0, delta1, delta2, delta3);
            
            var price = int.Parse(inputs[6]);
            var tomeIndex = int.Parse(inputs[7]);
            var taxCount = int.Parse(inputs[8]);
            var castable = inputs[9] != "0";
            var repeatable = inputs[10] != "0";

            return new GameAction(actionId, actionType, delta, price, tomeIndex, taxCount, castable, repeatable);
        }
    }

    public record GameAction(int Id,
        string Type,
        IngredientsVolume Delta,
        int Price,
        int TomeIndex,
        int TaxCount,
        bool Castable,
        bool Repeatable);
}