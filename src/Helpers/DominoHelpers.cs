using Dominoes.Model;
using System.Text;

namespace Dominoes.Helpers
{
    public static class DominoHelpers
    {
        public static Domino Flip(this Domino domino)
        {
            return new(domino.Right, domino.Left);
        }

        public static string ToVisualString(this IEnumerable<Domino> dominoes, int width)
        {
            const int dominoWidth = 16; // Width of a single domino including spacing
            int maxDominoesPerRow = Math.Max(1, (width - dominoWidth) / dominoWidth); // Calculate how many dominoes fit in one row

            var dominoLines = dominoes.Select(domino =>
            {
                var leftSide = GenerateDotGrid(domino.Left).Split('\n');
                var rightSide = GenerateDotGrid(domino.Right).Split('\n');

                var lines = new string[5];
                lines[0] = "+-------+-------+";
                for (int i = 0; i < 3; i++)
                {
                    var leftLine = i < leftSide.Length ? leftSide[i] : "      ";
                    var rightLine = i < rightSide.Length ? rightSide[i] : "      ";
                    lines[i + 1] = "| " + leftLine.PadRight(6) + "| " + rightLine.PadRight(6) + "|";
                }
                lines[4] = "+-------+-------+";
                return lines;
            }).ToList();

            var result = new StringBuilder();
            var currentRow = new List<string[]>();

            foreach (var domino in dominoLines)
            {
                currentRow.Add(domino);

                if (currentRow.Count == maxDominoesPerRow)
                {
                    AppendRow(result, currentRow);
                    currentRow.Clear();
                }
            }

            if (currentRow.Count != 0)
            {
                AppendRow(result, currentRow);
            }

            return result.ToString();
        }

        private static void AppendRow(StringBuilder result, List<string[]> dominoes)
        {
            for (int i = 0; i < 5; i++) // Each domino has 5 rows
            {
                foreach (var domino in dominoes)
                {
                    result.Append(domino[i] + " ");
                }
                result.AppendLine(); // Move to the next line
            }
        }

        public static string GenerateDotGrid(int count)
        {
            var grid = new char[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    grid[i, j] = ' ';
                }
            }

            PlaceDots(count, grid);

            var gridStr = "";
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gridStr += grid[i, j] == ' ' ? "  " : "o ";
                }
                gridStr = gridStr.TrimEnd() + "\n";
            }

            return gridStr.TrimEnd();
        }

        private static void PlaceDots(int count, char[,] grid)
        {
            switch (count)
            {
                case 1:
                    grid[1, 1] = 'o'; // Center
                    break;

                case 2:
                    grid[0, 0] = 'o'; // Top-left
                    grid[2, 2] = 'o'; // Bottom-right
                    break;

                case 3:
                    grid[0, 0] = 'o'; // Top-left
                    grid[1, 1] = 'o'; // Center
                    grid[2, 2] = 'o'; // Bottom-right
                    break;

                case 4:
                    grid[0, 0] = 'o'; // Top-left
                    grid[0, 2] = 'o'; // Top-right
                    grid[2, 0] = 'o'; // Bottom-left
                    grid[2, 2] = 'o'; // Bottom-right
                    break;

                case 5:
                    grid[0, 0] = 'o'; // Top-left
                    grid[0, 2] = 'o'; // Top-right
                    grid[1, 1] = 'o'; // Center
                    grid[2, 0] = 'o'; // Bottom-left
                    grid[2, 2] = 'o'; // Bottom-right
                    break;

                case 6:
                    grid[0, 0] = 'o'; // Top-left
                    grid[0, 1] = 'o'; // Top-center
                    grid[0, 2] = 'o'; // Top-right
                    grid[2, 0] = 'o'; // Bottom-left
                    grid[2, 1] = 'o'; // Bottom-center
                    grid[2, 2] = 'o'; // Bottom-right
                    break;
            }
        }
    }
}