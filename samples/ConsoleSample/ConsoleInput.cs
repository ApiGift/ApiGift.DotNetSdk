namespace ConsoleSample;

internal static class ConsoleInput
{
    public static Guid ReadGuid(string label)
    {
        while (true)
        {
            string value = ReadRequired(label);
            if (Guid.TryParse(value, out Guid guid))
                return guid;

            ConsoleOutput.WriteError("Invalid GUID.");
        }
    }

    public static Guid? ReadOptionalGuid(string label)
    {
        while (true)
        {
            string value = ReadOptional(label);
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (Guid.TryParse(value, out Guid guid))
                return guid;

            ConsoleOutput.WriteError("Invalid GUID.");
        }
    }

    public static string ReadRequired(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            string? value = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();

            ConsoleOutput.WriteError($"{label} is required.");
        }
    }

    public static string ReadOptional(string label)
    {
        Console.Write($"{label}: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}
