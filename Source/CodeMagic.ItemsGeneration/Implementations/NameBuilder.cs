using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.ItemsGeneration.Implementations
{
    internal class NameBuilder
    {
        public NameBuilder(string initialName)
        {
            Prefixes = new List<string>();
            Center = new List<string> { initialName };
            Postfixes = new List<string>();
        }

        public List<string> Prefixes { get; }

        public List<string> Center { get; }

        public List<string> Postfixes { get; }

        public override string ToString()
        {
            var leftPart = string.Join(" ", Prefixes.Distinct());
            var center = string.Join(" ", Center.Distinct());
            var rightPart = BuildRightPart();

            if (Prefixes.Any())
            {
                center = $" {center}";
            }

            return leftPart + center + rightPart;
        }

        private string BuildRightPart()
        {
            var result = string.Empty;
            var clearPostfixes = Postfixes.Distinct().ToArray();

            for (int index = 0; index < clearPostfixes.Length; index++)
            {
                var postfix = clearPostfixes[index];
                if (index == 0)
                {
                    result += $" of {postfix}";
                    continue;
                }

                if (index < clearPostfixes.Length - 1)
                {
                    result += $", {postfix}";
                    continue;
                }

                result += $" and {postfix}";
            }

            return result;
        }
    }
}