using Terminal.Gui;

namespace WindowsEnvironmentVariableManager.Ui;

/// <summary>
/// The primary interactive interface, utilizing a flat, borderless ANSI
/// TrueColor design paradigm.
/// </summary>
public class MainWindow : Toplevel
{
    public MainWindow()
    {
        // Enforce full-screen grid allocation
        Width = Dim.Fill();
        Height = Dim.Fill();

        // Define 24-bit RGB TrueColor palettes to mimic a modern flat GUI
        var sidebarAttr = new Terminal.Gui.Attribute(
            new Color(200, 200, 200),
            new Color(24, 24, 24)
        );
        var mainPaneAttr = new Terminal.Gui.Attribute(
            new Color(200, 200, 200),
            new Color(30, 30, 30)
        );

        var sidebarScheme = new ColorScheme
        {
            Normal = sidebarAttr,
            Focus = sidebarAttr,
        };
        var mainPaneScheme = new ColorScheme
        {
            Normal = mainPaneAttr,
            Focus = mainPaneAttr,
        };

        // Instantiate the left sidebar container
        var sidebar = new View
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(25),
            Height = Dim.Fill(),
            ColorScheme = sidebarScheme,
            BorderStyle = LineStyle.None,
        };

        // Sidebar structural controls
        var navLabel = new Label
        {
            Text = "EXPLORER",
            X = 1,
            Y = 1,
            ColorScheme = sidebarScheme,
        };

        var navList = new ListView
        {
            X = 1,
            Y = Pos.Bottom(navLabel) + 1,
            // Suppress the CS8604 null reference warning using the
            // null-forgiving operator (!)
            Width = Dim.Fill()! - 1,
            Height = Dim.Fill(),
            ColorScheme = sidebarScheme,
        };
        // Explicitly bind the data source utilizing the mandated
        // ObservableCollection pattern
        navList.SetSource(["System Variables", "User Variables"]);

        sidebar.Add(navLabel, navList);

        // Instantiate the main editor area container
        var mainPane = new View
        {
            X = Pos.Right(sidebar),
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ColorScheme = mainPaneScheme,
            BorderStyle = LineStyle.None,
        };

        // Main pane structural controls
        var editorLabel = new Label
        {
            Text = "EDITOR",
            X = 1,
            Y = 1,
            ColorScheme = mainPaneScheme,
        };

        var statusLabel = new Label
        {
            Text = "Select a scope from the explorer to view variables.",
            X = 1,
            Y = Pos.Bottom(editorLabel) + 1,
            ColorScheme = mainPaneScheme,
        };

        // Right-align the exit trigger within the main pane
        var quitButton = new Button
        {
            X = Pos.AnchorEnd(8),
            Y = Pos.AnchorEnd(2),
            ColorScheme = mainPaneScheme,
            Text = "Exit",
        };
        quitButton.Accepting += (sender, args) => Application.RequestStop();

        mainPane.Add(editorLabel, statusLabel, quitButton);

        // Mount the views to the Toplevel root
        Add(sidebar, mainPane);
    }
}
