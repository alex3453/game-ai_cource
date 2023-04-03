using System.Linq;

namespace bot
{
    public class Solver
    {
        public BotCommand GetCommand(State state, Countdown countdown)
        {
            var mostExpensivePossibleOrder = state.Actions
                .Where(ga => ga.Type == GameActionType.Brew)
                .Where(ga => state.Me.Inventory.IsPossibleToApply(ga.Delta))
                .OrderByDescending(ga => ga.Price)
                .FirstOrDefault();

            if (mostExpensivePossibleOrder == null)
                return new Wait();

            return new Brew(mostExpensivePossibleOrder.Id);
        }
    }
}