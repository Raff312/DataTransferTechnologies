namespace Lab2.Caesar;

public static class TextAnalyzers {
    public static IDictionary<char, int> CountSymbols(string text) {
        var counts = new Dictionary<char, int>();

        foreach (var c in text) {
            if (!char.IsLetter(c)) {
                continue;
            }

            var loweredC = char.ToLower(c);

            if (counts.TryGetValue(loweredC, out int value)) {
                counts[loweredC] = ++value;
            } else {
                counts.Add(loweredC, 1);
            }
        }

        return counts.OrderByDescending(x => x.Value).ToDictionary(kv => kv.Key, kv => kv.Value);
    }

    public static int GetKey(IDictionary<char, int> dict1, IDictionary<char, int> dict2, int offset = 0) {
        if (dict1.Count < offset || dict2.Count < offset) {
            return 0;
        }

        var charFromDict1Position = Encoder.Alphabet.IndexOf(dict1.ElementAt(0).Key);
        var charFromDict2Position = Encoder.Alphabet.IndexOf(dict2.ElementAt(offset).Key);

        return Math.Abs(charFromDict2Position - charFromDict1Position);
    }

    public static IDictionary<char, char> GetDecodeAlphabet(IDictionary<char, int> dict1, IDictionary<char, int> dict2) {
        var dictCount = Math.Min(dict1.Count, dict2.Count);

        var alphabet = new Dictionary<char, char>();
        for (var i = 0; i < dictCount; i++) {
            var charFromDict1 = dict1.ElementAt(i).Key;
            var charFromDict2 = dict2.ElementAt(i).Key;
            alphabet.Add(charFromDict2, charFromDict1);
        }

        return alphabet;
    }
}