namespace bot
{
    public class Solver
    {
        public BotCommand GetCommand(State state, Countdown countdown)
        {
            return new Wait { Message = "Nothing to do..." };
        }
    }
}   