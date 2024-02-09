using System.Globalization;

namespace Lab1.DataSourceEncoding.Utils;

public static class ConsoleUtils {
    public static T? GetValueFromUser<T>(string msg) {
        while (true) {
            Console.Write(msg);
            var userAnswer = Console.ReadLine();
            try {
                return (T?)Convert.ChangeType(userAnswer, typeof(T), CultureInfo.InvariantCulture);
            } catch (Exception) {
                WriteLine(ConsoleColor.Red, "Invalid value type. Try again...");
            }
        }
    }

    public static void WriteLine(ConsoleColor color, string text) {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static void Write(ConsoleColor color, string text) {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}
