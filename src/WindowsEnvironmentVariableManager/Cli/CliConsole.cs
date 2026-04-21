namespace WindowsEnvironmentVariableManager.Cli;

public static class CliConsole
{
    public static void WriteInfo(string message)
    {
        Console.Out.WriteLine(message);
    }

    public static void WriteSuccess(string message)
    {
        WriteColoredLine(Console.Out, message, ConsoleColor.Green);
    }

    public static void WriteWarning(string message)
    {
        WriteColoredLine(Console.Out, message, ConsoleColor.Yellow);
    }

    public static void WriteError(string message)
    {
        WriteColoredLine(Console.Error, message, ConsoleColor.Red);
    }

    private static void WriteColoredLine(
        TextWriter writer,
        string message,
        ConsoleColor foregroundColor
    )
    {
        ConsoleColor originalForegroundColor = Console.ForegroundColor;
        ConsoleColor originalBackgroundColor = Console.BackgroundColor;

        try
        {
            Console.ForegroundColor = foregroundColor;
            writer.WriteLine(message);
        }
        finally
        {
            Console.ForegroundColor = originalForegroundColor;
            Console.BackgroundColor = originalBackgroundColor;
        }
    }
}
