namespace WindowsEnvironmentVariableManager.Core;

public sealed record EnvironmentVariableEntry(
    string Name,
    string Value,
    EnvironmentVariableTarget Target
);
