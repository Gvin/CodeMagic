using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations
{
    internal class NameBuilder
    {
        private readonly string[] initialDescription;

        public NameBuilder(string initialName, string[] initialDescription)
        {
            Prefixes = new List<string>();
            Center = new List<string> { initialName };
            Postfixes = new List<string>();
            this.initialDescription = initialDescription ?? new string[0];
            AdditionalDescription = new Dictionary<string, string>();
        }

        private Dictionary<string, string> AdditionalDescription { get; }

        public void AddDescription(string key, string value)
        {
            if (AdditionalDescription.ContainsKey(key))
                return;

            AdditionalDescription.Add(key, value);
        }

        public List<string> Prefixes { get; }

        public List<string> Center { get; }

        public List<string> Postfixes { get; }

        public string[] GetDescription()
        {
            var result = new List<string>(initialDescription);
            result.AddRange(AdditionalDescription.Values);
            return result.ToArray();
        }

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