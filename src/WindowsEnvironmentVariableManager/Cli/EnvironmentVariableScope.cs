namespace WindowsEnvironmentVariableManager.Cli;

public readonly record struct EnvironmentVariableScope
{
    private const string InvalidScopeMessage =
        "Invalid value for --scope. Accepted values: \"user\" or \"machine\".";

    private EnvironmentVariableScope(string value) => Value = value;

    public string Value { get; }

    public static EnvironmentVariableScope User { get; } = new("user");

    public static EnvironmentVariableScope Machine { get; } = new("machine");

    public static EnvironmentVariableScope Parse(string value)
    {
        return value switch
        {
            "user" => User,
            "machine" => Machine,
            _ => throw new ArgumentException(InvalidScopeMessage),
        };
    }

    public EnvironmentVariableTarget ToEnvironmentVariableTarget()
    {
        return Value switch
        {
            "user" => EnvironmentVariableTarget.User,
            "machine" => EnvironmentVariableTarget.Machine,
            _ => throw new InvalidOperationException(InvalidScopeMessage),
        };
    }

    public override string ToString() => Value;
}
