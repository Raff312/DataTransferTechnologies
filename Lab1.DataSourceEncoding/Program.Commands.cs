using Lab1.DataSourceEncoding.Utils;

namespace Lab1.DataSourceEncoding;

public partial class Program {
    private const string DefaultInputFilePath = "./InputData/test.txt";
    private const string DefaultOutputFilePath = "./OutputData/file.32a";

    private static readonly CommandDefinition[] CommandDefinitions = {
        new("1") {
            Description = "Run",
            Action = Run
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
        var inputFilePath = ConsoleUtils.GetValueFromUser<string>("Enter input file path: ");
        var outputFilePath = ConsoleUtils.GetValueFromUser<string>("Enter output file path: ");

        inputFilePath = string.IsNullOrWhiteSpace(inputFilePath) ? DefaultInputFilePath : inputFilePath;
        outputFilePath = string.IsNullOrWhiteSpace(outputFilePath) ? DefaultOutputFilePath : outputFilePath;

        var archiver = new Archiver();
        archiver.Archive(inputFilePath, outputFilePath);

        Console.WriteLine("Archiving completed.");
    }

    private sealed class CommandDefinition(params string[] codes) {
        public string[] Codes { get; } = codes;
        public string Description { get; set; } = string.Empty;
        public Action? Action { get; set; }
    }
}
