using System.ComponentModel;
using ConsoleAppFramework;
using WindowsEnvironmentVariableManager.Core;

namespace WindowsEnvironmentVariableManager.Cli;

public sealed class EnvironmentVariableCommands
{
    /// <summary>
    /// List environment variable names for a scope.
    /// </summary>
    /// <param name="scope">
    /// Environment variable scope. Accepted values: user or machine.
    /// </param>
    // CA1822 Warning suppression reason: `ConsoleAppFramework.Add<T>()`
    // requires public instance command handlers.
#pragma warning disable CA1822
    public int List(string scope = "user")
#pragma warning restore CA1822
    {
        return Execute(() =>
        {
            EnvironmentVariableScope parsedScope = ParseScope(scope);
            IReadOnlyList<EnvironmentVariableEntry> entries =
                EnvironmentVariableService.List(
                    parsedScope.ToEnvironmentVariableTarget()
                );

            foreach (EnvironmentVariableEntry entry in entries)
            {
                Console.WriteLine(entry.Name);
            }

            return CliExitCode.Success;
        });
    }

    /// <summary>
    /// Get an environment variable value.
    /// </summary>
    /// <param name="name">Environment variable name.</param>
    /// <param name="scope">
    /// Environment variable scope. Accepted values: user or machine.
    /// </param>
    // CA1822 Warning suppression reason: `ConsoleAppFramework.Add<T>()`
    // requires public instance command handlers.
#pragma warning disable CA1822
    public int Get([Argument] string name, string scope = "user")
#pragma warning restore CA1822
    {
        return Execute(() =>
        {
            EnvironmentVariableScope parsedScope = ParseScope(scope);
            EnvironmentVariableEntry? entry = EnvironmentVariableService.Get(
                name,
                parsedScope.ToEnvironmentVariableTarget()
            );

            if (entry is null)
            {
                CliConsole.WriteError(
                    $"Variable \"{name}\" was not found in {parsedScope} scope."
                );
                return CliExitCode.NotFound;
            }

            CliConsole.WriteInfo(entry.Value);
            return CliExitCode.Success;
        });
    }

    /// <summary>
    /// Set an environment variable value.
    /// </summary>
    /// <param name="name">Environment variable name.</param>
    /// <param name="value">Environment variable value.</param>
    /// <param name="scope">
    /// Environment variable scope. Accepted values: user or machine.
    /// </param>
    // CA1822 Warning suppression reason: `ConsoleAppFramework.Add<T>()`
    // requires public instance command handlers.
#pragma warning disable CA1822
    public int Set(
        [Argument] string name,
        [Argument] string value,
        string scope = "user"
    )
#pragma warning restore CA1822
    {
        EnvironmentVariableScope parsedScope = ParseScope(scope);

        return Execute(
            () =>
            {
                EnvironmentVariableEntry entry = EnvironmentVariableService.Set(
                    name,
                    value,
                    parsedScope.ToEnvironmentVariableTarget()
                );

                CliConsole.WriteSuccess(
                    $"Set \"{entry.Name}\" in {parsedScope} scope."
                );
                CliConsole.WriteWarning(
                    "The current shell session was not updated."
                );
                return CliExitCode.Success;
            },
            parsedScope,
            isMutating: true
        );
    }

    /// <summary>
    /// Delete an environment variable.
    /// </summary>
    /// <param name="name">Environment variable name.</param>
    /// <param name="scope">
    /// Environment variable scope. Accepted values: user or machine.
    /// </param>
    // CA1822 Warning suppression reason: `ConsoleAppFramework.Add<T>()`
    // requires public instance command handlers.
#pragma warning disable CA1822
    public int Delete([Argument] string name, string scope = "user")
#pragma warning restore CA1822
    {
        EnvironmentVariableScope parsedScope = ParseScope(scope);

        return Execute(
            () =>
            {
                bool existed = EnvironmentVariableService.Delete(
                    name,
                    parsedScope.ToEnvironmentVariableTarget()
                );

                if (!existed)
                {
                    CliConsole.WriteError(
                        $"Variable \"{name}\" was not found in {parsedScope} scope."
                    );
                    return CliExitCode.NotFound;
                }

                CliConsole.WriteSuccess(
                    $"Deleted \"{name}\" from {parsedScope} scope."
                );
                CliConsole.WriteWarning(
                    "The current shell session was not updated."
                );
                return CliExitCode.Success;
            },
            parsedScope,
            isMutating: true
        );
    }

    private static int Execute(
        Func<int> action,
        EnvironmentVariableScope? scope = null,
        bool isMutating = false
    )
    {
        try
        {
            return action();
        }
        catch (PlatformNotSupportedException exception)
        {
            CliConsole.WriteError(exception.Message);
            return CliExitCode.UnsupportedPlatform;
        }
        catch (UnauthorizedAccessException exception)
        {
            WriteAccessDeniedMessage(exception, scope, isMutating);
            return CliExitCode.AccessDenied;
        }
        catch (Win32Exception exception)
        {
            WriteAccessDeniedMessage(exception, scope, isMutating);
            return CliExitCode.AccessDenied;
        }
        catch (ArgumentException exception)
        {
            CliConsole.WriteError(exception.Message);
            return CliExitCode.InvalidArguments;
        }
        catch (Exception exception)
        {
            CliConsole.WriteError(exception.Message);
            return CliExitCode.UnexpectedFailure;
        }
    }

    private static EnvironmentVariableScope ParseScope(string scope)
    {
        return EnvironmentVariableScope.Parse(scope);
    }

    private static void WriteAccessDeniedMessage(
        Exception exception,
        EnvironmentVariableScope? scope,
        bool isMutating
    )
    {
        if (
            isMutating
            && scope is { } parsedScope
            && parsedScope == EnvironmentVariableScope.Machine
        )
        {
            CliConsole.WriteError(
                "Cannot modify machine-scope environment variables without elevation. Re-run this command as Administrator or use --scope user."
            );
            return;
        }

        CliConsole.WriteError(exception.Message);
    }
}
