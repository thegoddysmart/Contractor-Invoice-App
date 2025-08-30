namespace ContractorInvoiceApp.Utils
{
    class Formatter
    {
        public static string FormatBox(string text)
        {
            var lines = text.Split('\n');
            int maxWidth = lines.Max(l => l.Length);
            string border = new string('=', maxWidth + 4);

            var result = new List<string> { border };
            foreach (var line in lines)
                result.Add($"| {line.PadRight(maxWidth)} |");
            result.Add(border);

            return string.Join("\n", result);
        }
    }
}
