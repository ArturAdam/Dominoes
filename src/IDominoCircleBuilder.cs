using Dominoes.Model;
using OneOf;
using OneOf.Types;

namespace Dominoes
{
    // Overkill but if we ever wanted another method of solving this and make the user choose which strategy to use
    public interface IDominoCircleBuilder
    {
        List<Domino> Dominoes { get; set; }

        OneOf<List<Domino>, Error<string>> FormCircle();
    }
}