using Lab1.DataSourceEncoding.Utils;

namespace Lab1.DataSourceEncoding;

public partial class Program {
    private const string DefaultInputFilePath = "./InputData/file.webp";
    private const string DefaultOutputFilePath = "./OutputData/file.32a";

    private static readonly CommandDefinition[] CommandDefinitions = {
        new CommandDefinition("1") {
            Description = "Run",
            Action = Run
        },
        new CommandDefinition("0", "exit") {
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

        Archiver.ArchiveFile(inputFilePath, outputFilePath);

        Console.WriteLine("Archiving completed.");
    }

    private sealed class CommandDefinition {
        public string[] Codes { get; }
        public string Description { get; set; }
        public Action? Action { get; set; }

        public CommandDefinition(params string[] codes) {
            Codes = codes;
            Description = string.Empty;
        }
    }
}
