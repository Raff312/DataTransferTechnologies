using Lab1.DataSourceEncoding.Utils;

namespace Lab1.DataSourceEncoding;

public partial class Program {
    private static void Main() {
        while (ProcessCommandInput()) { }
    }

    private static bool ProcessCommandInput() {
        Console.WriteLine();
        WriteCommandDescriptions();
        string? commandText = null;
        while (string.IsNullOrWhiteSpace(commandText)) {
            Console.Write("> ");
            commandText = Console.ReadLine();
        }

        var parts = commandText.Split(' ', ',');
        var command = parts[0];

        try {
            var commandDefinition = GetCommandDefinitionByCode(command);
            if (commandDefinition != null) {
                if (commandDefinition.Action == null) {
                    Console.Write("Goodbye...");
                    return false;
                }

                Console.WriteLine($"Executing command '{command}'...\n");
                commandDefinition.Action();
                ConsoleUtils.WriteLine(ConsoleColor.Green, $"\nCommand '{command}' completed.");
            } else {
                ConsoleUtils.WriteLine(ConsoleColor.Yellow, $"\nUnknown command '{command}'");
            }
        } catch (Exception e) {
            ConsoleUtils.WriteLine(ConsoleColor.Red, $"\nException during command '{command}': {e.Message}");
        }

        return true;
    }

    private static void WriteCommandDescriptions() {
        foreach (var commandDefinition in CommandDefinitions) {
            Console.Write("'");
            ConsoleUtils.Write(ConsoleColor.Green, commandDefinition.Codes[0]);
            for (var i = 1; i < commandDefinition.Codes.Length; i++) {
                Console.Write("', '");
                ConsoleUtils.Write(ConsoleColor.Green, commandDefinition.Codes[i]);
            }
            Console.Write("' - ");
            ConsoleUtils.WriteLine(ConsoleColor.White, commandDefinition.Description);
        }
    }
}
