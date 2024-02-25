using Lab2.Caesar.Utils;

namespace Lab2.Caesar;

public partial class Program {
    private static readonly CommandDefinition[] CommandDefinitions = {
        new("1") {
            Description = "Run",
            Action = Run
        },
        new("2") {
            Description = "Run from file",
            Action = RunFromFile
        },
        new("0", "exit") {
            Description = "Exit from program",
            Action = null
        }
    };

    private static CommandDefinition? GetCommandDefinitionByCode(string code) {
        code = code.ToLowerInvariant();
        return CommandDefinitions.FirstOrDefault(x => x.Codes.Contains(code));
    }

    private static void Run() {
        RunInternal(ConsoleUtils.GetValueFromUser<string>("Enter text: ")!);
    }

    private static void RunFromFile() {
        RunInternal(File.ReadAllText("./Data/text2.txt"));
    }

    private static void RunInternal(string text) {
        var learningTextCounts = Learn();

        var key = ConsoleUtils.GetValueFromUser<int>("Enter key: ");

        var encodedText = Encoder.Encode(text, key);
        Console.WriteLine($"Encoded text: {encodedText}");
        var encodedTextCounts = TextAnalyzers.CountSymbols(encodedText);

        Console.WriteLine("=================");

        var possibleKey1 = TextAnalyzers.GetKey(learningTextCounts, encodedTextCounts);
        Console.WriteLine($"Possible key 1: {possibleKey1}");

        var decodedText1 = Decoder.Decode(encodedText, possibleKey1);
        Console.WriteLine($"Decoded text 1: {decodedText1}");

        var possibleKey2 = TextAnalyzers.GetKey(learningTextCounts, encodedTextCounts, 1);
        Console.WriteLine($"Possible key 2: {possibleKey2}");

        var decodedText2 = Decoder.Decode(encodedText, possibleKey2);
        Console.WriteLine($"Decoded text 2: {decodedText2}");

        Console.WriteLine("=================");

        var decodeAlphabet = TextAnalyzers.GetDecodeAlphabet(encodedTextCounts, learningTextCounts);
        var decodedBySubstitutingText = Decoder.Decode(encodedText, decodeAlphabet);
        Console.WriteLine($"Decoded by substituting: {decodedBySubstitutingText}");

        Console.WriteLine("=================");

        Console.WriteLine();
        Console.WriteLine("Learning text counts: ");
        ShowDictionary(learningTextCounts);

        Console.WriteLine();
        Console.WriteLine("Encoded text counts: ");
        ShowDictionary(encodedTextCounts);
    }

    private static IDictionary<char, int> Learn() {
        var learningText = File.ReadAllText("./Data/text.txt");
        return TextAnalyzers.CountSymbols(learningText);
    }

    private static void ShowDictionary(IDictionary<char, int> dictionary) {
        foreach (var kv in dictionary) {
            Console.WriteLine($"{kv.Key} => {kv.Value}");
        }
    }

    private sealed class CommandDefinition(params string[] codes) {
        public string[] Codes { get; } = codes;
        public string Description { get; set; } = string.Empty;
        public Action? Action { get; set; }
    }
}
