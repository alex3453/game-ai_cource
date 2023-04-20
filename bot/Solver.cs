using System;
using System.Linq;

namespace bot
{
    public class Solver
    {
        public BotCommand GetCommand(State state, Countdown countdown)
        {
            var bestScore = double.MinValue;
            BotCommand bestCommand = null;
            
            var mostExpensivePossibleOrder = state.Orders
                .Where(order => state.Me.Inventory.IsPossibleToApply(order.Delta))
                .OrderByDescending(ga => ga.Price)
                .FirstOrDefault();

            if (mostExpensivePossibleOrder != null)
            {
                var command = new Brew(mostExpensivePossibleOrder.Id);
                
                var copy = state.Copy();
                copy.MyExecute(command);
                bestScore = Estimate(copy);
                bestCommand = command;
            }

            var possibleCasts = state.Me.Casts
                .Where(c => state.Me.IsPossibleToCast(c.Id));

            foreach (var cast in possibleCasts)
            {
                var command = new CastCommand(cast.Id);
                
                var copy = state.Copy();
                copy.MyExecute(command);
                var score = Estimate(copy);
                if (score > bestScore)
                {
                    bestCommand = command;
                    bestScore = score;
                }
            }
            
            // var restCommand = new Rest();
            //     
            // var restCopy = state.Copy();
            // restCopy.MyExecute(restCommand);
            // var restScore = Estimate(restCopy);
            // if (restScore > bestScore)
            // {
            //     bestCommand = restCommand;
            //     bestScore = restScore;
            // }

            return bestCommand ?? new Rest();
        }

        private static double Estimate(State state)
        {
            var scoreEstimate = state.Me.Score * 100;

            var order1 = EstimateOrder(state, state.Orders[0]);
            var order2 = EstimateOrder(state, state.Orders[1]);
            var order3 = EstimateOrder(state, state.Orders[2]);
            var order4 = EstimateOrder(state, state.Orders[3]);
            var order5 = EstimateOrder(state, state.Orders[4]);

            var ordersEstimate = order1 + order2 + order3 + order4 + order5;

            return scoreEstimate + ordersEstimate;
        }

        private static double EstimateOrder(State state, Order order)
        {
            var delta = state.Me.Inventory.GetIngredientsDelta(order.Delta);
            return EstimateDistance(delta.ZeroIngredient) + 
                   EstimateDistance(delta.FirstIngredient) +
                   EstimateDistance(delta.SecondIngredient) +
                   EstimateDistance(delta.ThirdIngredient);
        }

        private static double EstimateDistance(int distance)
        {
            return distance > 0 ? -distance : distance * 30;
        }
    }
}