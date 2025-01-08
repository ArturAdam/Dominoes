using Dominoes.Helpers;

namespace Dominoes.Model
{
    public sealed record Domino(DominoValue Left, DominoValue Right)
    {
        public override string ToString()
        {
            var leftSide = DominoHelpers.GenerateDotGrid(Left);
            var rightSide = DominoHelpers.GenerateDotGrid(Right);

            var leftLines = leftSide.Split('\n');
            var rightLines = rightSide.Split('\n');

            var result = "+-------+-------+\n";
            for (int i = 0; i < 3; i++)
            {
                var leftLine = i < leftLines.Length ? leftLines[i] : "      ";
                var rightLine = i < rightLines.Length ? rightLines[i] : "      ";
                result += "| " + leftLine.PadRight(6) + "| " + rightLine.PadRight(6) + "|\n";
            }
            result += "+-------+-------+";

            return result;
        }
    }

    public enum DominoValueEnum
    {
        Blank = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6
    }

    public readonly struct DominoValue
    {
        private DominoValue(DominoValueEnum value)
        {
            if ((int)value < 0 || (int)value > 6)
                throw new ArgumentOutOfRangeException(nameof(value), "Invalid domino value. Must be between 0 and 6");

            Value = value;
        }

        public DominoValueEnum Value { get; }

        public static implicit operator DominoValue(int value)
        {
            return new DominoValue((DominoValueEnum)value);
        }

        public static implicit operator int(DominoValue dominoValue)
        {
            return (int)dominoValue.Value;
        }

        public static implicit operator DominoValueEnum(DominoValue dominoValue)
        {
            return dominoValue.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}