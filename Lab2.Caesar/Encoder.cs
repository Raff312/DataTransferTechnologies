using System.Text;

namespace Lab2.Caesar;

public static class Encoder {
    public static readonly string Alphabet = "абвгдежзийклмнопрстуфхцчшщъыьэюя";

    public static string Encode(string text, int key) {
        var result = new StringBuilder(text.Length);

        foreach (var c in text) {
            var loweredC = char.ToLower(c);

            if (Alphabet.Contains(loweredC)) {
                var oldPosition = Alphabet.IndexOf(loweredC);
                var newPosition = (oldPosition + key) % Alphabet.Length;

                var newChar = Alphabet[newPosition];
                result.Append(char.IsLower(c) ? newChar : char.ToUpper(newChar));
            } else {
                result.Append(c);
            }
        }

        return result.ToString();
    }
}