namespace ConsoleSample;

internal static class ConsoleOutput
{
    public static void WriteTable(
        IReadOnlyList<string> headers,
        IEnumerable<IReadOnlyList<string>> rows)
    {
        List<IReadOnlyList<string>> materializedRows = rows.ToList();
        int[] widths = headers
            .Select((header, index) => Math.Max(
                header.Length,
                materializedRows.Count == 0
                    ? 0
                    : materializedRows.Max(row => index < row.Count ? row[index]?.Length ?? 0 : 0)))
            .ToArray();

        WriteRow(headers, widths);
        Console.WriteLine(string.Join("-+-", widths.Select(width => new string('-', width))));

        foreach (IReadOnlyList<string> row in materializedRows)
            WriteRow(row, widths);
    }

    public static string Trim(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return "-";

        return value.Length <= maxLength
            ? value
            : value[..maxLength] + "...";
    }

    public static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private static void WriteRow(IReadOnlyList<string> values, IReadOnlyList<int> widths)
    {
        for (int i = 0; i < widths.Count; i++)
        {
            if (i > 0)
                Console.Write(" | ");

            string value = i < values.Count ? values[i] ?? string.Empty : string.Empty;
            Console.Write(value.PadRight(widths[i]));
        }

        Console.WriteLine();
    }
}
