using Dominoes.Helpers;
using Dominoes.Model;
using OneOf;
using OneOf.Types;
using System.Diagnostics;

namespace Dominoes
{
    public class DominoGraph : IDominoCircleBuilder
    {
        public List<Domino> Dominoes { get; set; }
        private readonly Dictionary<DominoValue, List<Domino>> _graph;
        private readonly Dictionary<DominoValue, int> _degree;

        public DominoGraph(List<Domino> dominoes)
        {
            Dominoes = dominoes;
            _graph = [];
            _degree = [];

            // Build the graph where each key is a number and its value is the list of dominoes with that number
            //Two:   [Two|One], [Two|Three]
            //One:   [Two| One], [One| Three]
            //Three: [Two| Three], [One| Three]
            foreach (var domino in Dominoes)
            {
                if (!_graph.ContainsKey(domino.Left))
                    _graph[domino.Left] = new List<Domino>();
                if (!_graph.ContainsKey(domino.Right))
                    _graph[domino.Right] = new List<Domino>();

                _graph[domino.Left].Add(domino);
                _graph[domino.Right].Add(domino);

                // Track the degree of each number
                if (!_degree.ContainsKey(domino.Left))
                    _degree[domino.Left] = 0;
                if (!_degree.ContainsKey(domino.Right))
                    _degree[domino.Right] = 0;

                _degree[domino.Left]++;
                _degree[domino.Right]++;
            }
        }

        /// <returns>Either a list of dominoes that form a circle or an error string with a reason.</returns>
        public OneOf<List<Domino>, Error<string>> FormCircle()
        {
            Debug.Assert(_graph is not null);
            Debug.Assert(_degree is not null);
            Debug.Assert(Dominoes is not null);

            if (Dominoes.Count == 0)
                return new Error<string>("No dominoes to form a circle.");

            // If we only have one element in the graph we must be able to form a circle
            if (_graph.Count == 1)
                return Dominoes;

            // Check that every number has an even degree
            foreach (var kvp in _degree)
            {
                if (kvp.Value % 2 != 0)
                    return new Error<string>("A circular chain cannot be formed because the degree of some numbers is odd.");
            }

            var visited = new HashSet<int>();
            var firstNumber = Dominoes[0].Left;

            try
            {
                if (!IsGraphConnected(firstNumber, visited))
                    return new Error<string>("A circular chain cannot be formed because the graph is not connected.");

                var chain = new List<Domino>();
                foreach (var domino in Dominoes)
                {
                    var visitedDominoes = new HashSet<Domino>();
                    if (CanFormChain(domino, domino.Left, visitedDominoes, chain, 1))
                    {
                        return chain;
                    }
                    chain.Clear();
                }
                return new Error<string>("A circular chain cannot be formed.");
            }
            catch (Exception e)
            {
                return new Error<string>(e.Message);
            }
        }

        // Check if all nodes are reachable from any starting point
        private bool IsGraphConnected(int number, HashSet<int> visited)
        {
            if (_graph.Count == 0)
                return false;

            visited.Add(number);
            foreach (var domino in _graph[number])
            {
                int nextNumber = (domino.Left == number) ? domino.Right : domino.Left;
                if (!visited.Contains(nextNumber))
                {
                    IsGraphConnected(nextNumber, visited);
                }
            }
            // If any number in the graph is not visited, it means the graph is not connected
            foreach (var kvp in _graph)
            {
                if (!visited.Contains(kvp.Key))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanFormChain(Domino currentDomino, int currentNumber, HashSet<Domino> visited, List<Domino> chain, int count)
        {
            if (_graph.Count == 0)
                return false;

            if (visited.Contains(currentDomino))
            {
                Debug.WriteLine($"[SKIP] Domino {currentDomino} (already visited).");
                return false;
            }

            visited.Add(currentDomino);
            chain.Add(currentDomino);

            Debug.WriteLine($"[USE ] Domino {currentDomino}, chain so far: {string.Join(" -> ", chain.Select(d => d.ToString()))} (count={count}).");
            // Check if can link back to the start
            if (count == Dominoes.Count && currentDomino.Right == chain[0].Left)
            {
                Debug.WriteLine("[SUCCESS] Circle complete!");
                return true;
            }
            int nextNumber = (currentDomino.Left == currentNumber) ? currentDomino.Right : currentDomino.Left;

            // Visit all dominoes that can continue the chain
            foreach (var nextDomino in _graph[nextNumber])
            {
                if (!visited.Contains(nextDomino))
                {
                    bool flipped = false;
                    if (nextDomino.Left != nextNumber)
                    {
                        nextDomino.FlipInPlace();
                        flipped = true;
                    }

                    Debug.WriteLine($"[TRY ] {nextDomino} as-is, matching side {nextNumber}.");
                    if (CanFormChain(nextDomino, nextNumber, visited, chain, count + 1))
                        return true;

                    if (flipped)
                    {
                        nextDomino.FlipInPlace();
                    }
                }
            }
            Debug.WriteLine($"[BACK] Backtracking from {currentDomino}.");
            visited.Remove(currentDomino);
            chain.RemoveAt(Math.Max(0, chain.Count - 1)); // Remove the domino from the chain if we can't continue
            return false;
        }
    }
}