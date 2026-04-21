using System.Collections;

namespace WindowsEnvironmentVariableManager.Core;

public sealed class EnvironmentVariableService
{
    public static IReadOnlyList<EnvironmentVariableEntry> List(
        EnvironmentVariableTarget target
    )
    {
        EnsureSupportedTarget(target);

        IDictionary variables = Environment.GetEnvironmentVariables(target);
        List<EnvironmentVariableEntry> records = [];

        foreach (DictionaryEntry entry in variables)
        {
            string? name = entry.Key as string;

            if (string.IsNullOrEmpty(name) || entry.Value is not string value)
            {
                continue;
            }

            records.Add(new EnvironmentVariableEntry(name, value, target));
        }

        records.Sort(
            static (left, right) =>
                StringComparer.OrdinalIgnoreCase.Compare(left.Name, right.Name)
        );

        return records;
    }

    public static EnvironmentVariableEntry? Get(
        string name,
        EnvironmentVariableTarget target
    )
    {
        EnsureSupportedTarget(target);
        ValidateName(name);

        string? value = Environment.GetEnvironmentVariable(name, target);
        return value is null
            ? null
            : new EnvironmentVariableEntry(name, value, target);
    }

    public static EnvironmentVariableEntry Set(
        string name,
        string value,
        EnvironmentVariableTarget target
    )
    {
        ArgumentNullException.ThrowIfNull(value);
        EnsureSupportedTarget(target);
        ValidateName(name);

        Environment.SetEnvironmentVariable(name, value, target);
        Environment.SetEnvironmentVariable(name, value);

        string persistedValue =
            Environment.GetEnvironmentVariable(name, target) ?? value;

        return new EnvironmentVariableEntry(name, persistedValue, target);
    }

    public static bool Delete(string name, EnvironmentVariableTarget target)
    {
        EnsureSupportedTarget(target);
        ValidateName(name);

        bool existed =
            Environment.GetEnvironmentVariable(name, target) is not null;
        Environment.SetEnvironmentVariable(name, null, target);
        Environment.SetEnvironmentVariable(name, null);
        return existed;
    }

    private static void EnsureSupportedTarget(EnvironmentVariableTarget target)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException(
                "This tool currently supports registry-backed environment "
                    + "variables on Windows only."
            );
        }

        if (
            target
            is not EnvironmentVariableTarget.User
                and not EnvironmentVariableTarget.Machine
        )
        {
            throw new ArgumentException(
                "Only User and Machine scopes are supported.",
                nameof(target)
            );
        }
    }

    private static void ValidateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (name.Contains('='))
        {
            throw new ArgumentException(
                "Environment variable names cannot contain \"=\".",
                nameof(name)
            );
        }

        if (name.Length >= 255)
        {
            throw new ArgumentException(
                "Environment variable names for User and Machine scopes must "
                    + "be shorter than 255 characters.",
                nameof(name)
            );
        }
    }
}
