namespace WindowsEnvironmentVariableManager.Cli;

public static class CliExitCode
{
    public const int Success = 0;

    public const int InvalidArguments = 1;

    public const int NotFound = 2;

    public const int AccessDenied = 3;

    public const int UnsupportedPlatform = 4;

    public const int UnexpectedFailure = 5;
}
