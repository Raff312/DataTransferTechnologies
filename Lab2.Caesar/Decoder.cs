using System.Text;

namespace Lab2.Caesar;

public static class Decoder {
    public static string Decode(string text, int key) {
        var result = new StringBuilder(text.Length);

        foreach (var c in text) {
            var loweredC = char.ToLower(c);

            if (Encoder.Alphabet.Contains(loweredC)) {
                var oldPosition = Encoder.Alphabet.IndexOf(loweredC);
                var newPosition = (oldPosition - key + Encoder.Alphabet.Length) % Encoder.Alphabet.Length;

                char newChar = Encoder.Alphabet[newPosition];
                result.Append(char.IsLower(c) ? newChar : char.ToUpper(newChar));
            } else {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    public static string Decode(string text, IDictionary<char, char> alphabet) {
        var result = new StringBuilder(text.Length);

        foreach (var c in text) {
            if (alphabet.TryGetValue(c, out var decodedC)) {
                result.Append(decodedC);
            } else {
                result.Append(c);
            }
        }

        return result.ToString();
    }
}