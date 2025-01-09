using Dominoes.Helpers;
using Dominoes.Model;
using OneOf.Types;
using Spectre.Console;

namespace Dominoes
{
    public sealed class Program
    {
        private const string LEFT = "Left";
        private const string RIGHT = "Right";

        public static void Main()
        {
            while (true)
            {
                var selectedTiles = new List<Domino>();

                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("Press [bold yellow]'R'[/] for random tiles, [bold yellow]'M'[/] for manual mode.");
                AnsiConsole.MarkupLine("[grey](Press ESC to quit.)[/]");

                var choiceKey = AnsiConsole.Console.Input.ReadKey(intercept: true)?.Key;
                if (choiceKey == ConsoleKey.Escape)
                {
                    return;
                }

                // Let the user choose if they want to select the Dominos manually or randomly
                if (choiceKey == ConsoleKey.R)
                {
                    var random = new Random();

                    for (int i = 0; i < 6; i++)
                    {
                        selectedTiles.Add(new Domino(random.Next(0, 7), random.Next(0, 7)));
                    }
                }
                else if (choiceKey == ConsoleKey.M)
                {
                    var tile = new Domino(0, 0); // Start with (0|0)
                    var cursorPosition = LEFT;   // Cursor starts at LEFT side

                    while (true)
                    {
                        AnsiConsole.Clear();

                        // Instructions
                        string instructions =
                            "[bold yellow]Instructions:[/] \n" +
                            " - [bold yellow][[<-]][/] and [bold yellow][[->]][/] to navigate between domino's sides \n" +
                            " - [bold yellow][[+]][/] or up arrow key to increase the current side value \n" +
                            " - [bold yellow][[-]][/] or down arrow key to decrease the current side value.\n" +
                            " - [bold yellow][[A]][/] to add the current tile to selected tiles, \n" +
                            " - [bold yellow][[R]][/] to remove the most recently added tile, \n" +
                            " - [bold yellow][[F]][/] to finish and check for a circular chain.";

                        AnsiConsole.MarkupLine(instructions);

                        // Show current tile with highlight
                        string currentTile = $"[bold green]Current Tile: [[" +
                            (cursorPosition == LEFT
                                ? "[maroon on blue]" + tile.Left + "[/]"
                                : tile.Left.ToString()) +
                            "|" +
                            (cursorPosition == RIGHT
                                ? "[maroon on blue]" + tile.Right + "[/]"
                                : tile.Right.ToString()) +
                            "]] [/]";
                        AnsiConsole.MarkupLine(currentTile);

                        // Another line showing tile.ToString()
                        AnsiConsole.MarkupLine($"[bold green]{tile.ToVisualString()}[/]");

                        // Show the selected tiles so far
                        string selectedTilesString = selectedTiles.ToVisualString(Console.WindowWidth);
                        AnsiConsole.MarkupLine(
                            "[bold grey]Selected Tiles:[/]\n" +
                            $"[bold cyan]{selectedTilesString}[/]"
                        );

                        // Read user input
                        var key = AnsiConsole.Console.Input.ReadKey(intercept: true)?.Key;
                        switch (key)
                        {
                            case ConsoleKey.LeftArrow:
                                cursorPosition = LEFT;
                                break;

                            case ConsoleKey.RightArrow:
                                cursorPosition = RIGHT;
                                break;

                            case ConsoleKey.Add or ConsoleKey.OemPlus or ConsoleKey.UpArrow:
                                if (cursorPosition == LEFT && tile.Left < 6)
                                {
                                    tile.Left++;
                                }
                                else if (cursorPosition == RIGHT && tile.Right < 6)
                                {
                                    tile.Right++;
                                }
                                break;

                            case ConsoleKey.Subtract or ConsoleKey.OemMinus or ConsoleKey.DownArrow:
                                if (cursorPosition == LEFT && tile.Left > 0)
                                {
                                    tile.Left--;
                                }
                                else if (cursorPosition == RIGHT && tile.Right > 0)
                                {
                                    tile.Right--;
                                }
                                break;

                            case ConsoleKey.A:
                                selectedTiles.Add(new Domino(tile.Left, tile.Right));
                                break;

                            case ConsoleKey.R:
                                if (selectedTiles.Count > 0)
                                {
                                    selectedTiles.RemoveAt(selectedTiles.Count - 1);
                                }
                                break;

                            case ConsoleKey.F:
                                // Finish manual mode
                                break;
                        }

                        if (key == ConsoleKey.F)
                            break;
                    }
                }
                else
                {
                    // If user didn't press 'R' or 'M' (and didn't press ESC),
                    // just continue to the "play again" prompt.
                }

                if (selectedTiles.Count == 0)
                {
                    AnsiConsole.MarkupLine("[bold red]No tiles selected.[/]");
                }
                else if (choiceKey == ConsoleKey.R)
                {
                    AnsiConsole.MarkupLine("[bold aqua]Selected Tiles:[/]");
                    var finalTilesString = selectedTiles.ToVisualString(Console.WindowWidth);
                    AnsiConsole.MarkupLine($"[bold cyan]{finalTilesString}[/]");
                }

                var dominoGraph = new DominoGraph(selectedTiles);
                dominoGraph.FormCircle().Match(
                    results =>
                    {
                        AnsiConsole.MarkupLine("[bold green]Circular chain can be formed![/]");
                        AnsiConsole.MarkupLine(results.ToVisualString(Console.WindowWidth));
                        return new None();
                    },
                    error =>
                    {
                        AnsiConsole.MarkupLine($"[bold red]{error.Value}[/]");
                        return new None();
                    }
                );

                AnsiConsole.MarkupLine("\n[bold yellow]Press Enter to play again, or ESC to quit...[/]");
                var againKey = AnsiConsole.Console.Input.ReadKey(intercept: true)?.Key;
                if (againKey == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}