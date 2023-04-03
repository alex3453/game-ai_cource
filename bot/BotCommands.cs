namespace bot;

public record Brew(int OrderId) : BotCommand;
public record Wait() : BotCommand;