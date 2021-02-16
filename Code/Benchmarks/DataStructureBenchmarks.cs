using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class DataStructureBenchmarks
    {
        public Dictionary<string, string>? Dictionary;
        public List<string>? List;

        [Params(100, 1000, 10000)]
        public int NumberOfStrings { get; set; }

        public string SearchTerm { get; set; } = string.Empty;

        [Benchmark]
        public bool FindLastStringInList() => List!.Contains(SearchTerm);

        [Benchmark(Baseline = true)]
        public bool FindStringInDictionary() => Dictionary!.ContainsKey(SearchTerm);

        [GlobalSetup(Target = nameof(FindLastStringInList))]
        public void SetupList()
        {
            var random = new Random(42);

            var list = new List<string>();
            var newString = string.Empty;
            for (var i = 0; i < NumberOfStrings; i++)
            {
                newString = CreateNewString(random);
                list.Add(newString);
            }

            List = list;
            SearchTerm = newString;
        }

        [GlobalSetup(Target = nameof(FindStringInDictionary))]
        public void SetupDictionary()
        {
            var random = new Random(42);

            var dictionary = new Dictionary<string, string>();
            var newString = string.Empty;
            for (var i = 0; i < NumberOfStrings; i++)
            {
                newString = CreateNewString(random);
                dictionary.Add(newString, newString);
            }

            Dictionary = dictionary;
            SearchTerm = newString;
        }

        private static string CreateNewString(Random random)
        {
            var stringLength = random.Next(5, 20);
            Span<char> span = stackalloc char[stringLength];
            for (var i = 0; i < span.Length; i++)
            {
                span[i] = (char) random.Next('a', 'z' + 1);
            }

            return span.ToString();
        }
    }
}