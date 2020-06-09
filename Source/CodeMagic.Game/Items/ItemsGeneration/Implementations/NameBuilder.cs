using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations
{
    internal class NameBuilder
    {
        private readonly string initialName;
        private readonly string[] initialDescription;
        private readonly Dictionary<string, string> namePostfixes;
        private readonly Dictionary<string, string> nameParts;
        private readonly Dictionary<string, string> namePrefixes;
        private readonly Dictionary<string, string> additionalDescription;

        public NameBuilder(string initialName, string[] initialDescription)
        {
            this.initialName = initialName;
            namePrefixes = new Dictionary<string, string>();
            nameParts = new Dictionary<string, string>();
            namePostfixes = new Dictionary<string, string>();

            this.initialDescription = initialDescription ?? new string[0];
            additionalDescription = new Dictionary<string, string>();
        }

        public void AddNamePrefix(string key, string value)
        {
            TryAddValue(namePrefixes, key, value);
        }

        public void AddNamePostfix(string key, string value)
        {
            TryAddValue(namePostfixes, key, value);
        }

        public void AddNamePart(string key, string value)
        {
            TryAddValue(nameParts, key, value);
        }

        public void AddDescription(string key, string value)
        {
            TryAddValue(additionalDescription, key, value);
        }

        private void TryAddValue(Dictionary<string, string> dictionary, string key, string value)
        {
            if (dictionary.ContainsKey(key))
                return;

            dictionary.Add(key, value);
        }

        public string[] GetDescription()
        {
            var result = new List<string>(initialDescription);
            result.AddRange(additionalDescription.Values);
            return result.ToArray();
        }

        public override string ToString()
        {
            var leftPart = string.Join(" ", namePrefixes.Values.Distinct());
            var center = BuildName();
            var rightPart = BuildRightPart();

            if (namePrefixes.Any())
            {
                center = $" {center}";
            }

            return leftPart + center + rightPart;
        }

        private string BuildName()
        {
            var result = initialName;
            if (nameParts.Any())
            {
                var partsString = string.Join(" ", nameParts.Values.Distinct());
                result += $" {partsString}";
            }

            return result;
        }

        private string BuildRightPart()
        {
            var result = string.Empty;
            var clearPostfixes = namePostfixes.Values.Distinct().ToArray();

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