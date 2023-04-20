namespace bot
{
    public record Brew(int OrderId) : BotCommand;

    public record CastCommand(int CastId) : BotCommand;

    public record Rest : BotCommand;
    public record Wait : BotCommand;
}