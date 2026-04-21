using ConsoleAppFramework;
using Terminal.Gui;
using WindowsEnvironmentVariableManager.Cli;
using WindowsEnvironmentVariableManager.Ui;

var app = ConsoleApp.Create();
app.Add<EnvironmentVariableCommands>();
app.Add("tui", RunTui);
app.Run(args);

static void RunTui()
{
    // IL2026, IL3050 Warning suppression reason:
    // Terminal.Gui utilizes reflection during Application.Init() to dynamically
    // evaluate the host OS and instantiate the correct IConsoleDriver
    // (WindowsDriver, CursesDriver, etc.).
    // This is safe for NativeAOT because the default drivers are statically
    // linked within the Terminal.Gui assembly and inherently preserved by the
    // trimmer. No runtime IL generation is required to instantiate these
    // predetermined types.
#pragma warning disable IL2026, IL3050
    Application.Init();
#pragma warning restore IL2026, IL3050

    Application.Run(new MainWindow());
    Application.Shutdown();
}
