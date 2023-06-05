using System.Text.RegularExpressions;

namespace string_calculator_kata;

public static partial class StringCalculator
{
    public static int Add(string numbers)
    {
        if (string.IsNullOrWhiteSpace(numbers))
        {
            return 0;
        }

        var delimiter = GetDelimiter(ref numbers);
        var negativeNumbers = new List<int>();
        var sum = 0;

        try
        {
            numbers = FormatNumbers(numbers, delimiter);
            sum = ParseNumbers(numbers, delimiter, negativeNumbers);
        }
        catch (Exception)
        {
            // ignore
            return 0;
        }

        ThrowExceptionForNegativeNumbers(negativeNumbers);

        return sum;
    }

    private static string GetDelimiter(ref string numbers)
    {
        const string defaultDelimiter = ",";
        var delimiterMatch = MyRegex().Match(numbers);

        if (delimiterMatch.Success)
        {
            var delimiter = delimiterMatch.Value.Substring(2, delimiterMatch.Length - 3);
            var delimiters = GetDelimiters(delimiter);
            numbers = numbers.Substring(numbers.IndexOf("\n", StringComparison.Ordinal) + 1);
            return delimiters;
        }

        return defaultDelimiter;
    }

    private static string GetDelimiters(string delimiterMatch)
    {
        var delimiterPattern = @"\[(.*?)\]";
        var matches = Regex.Matches(delimiterMatch, delimiterPattern);

        var delimiters = matches
            .Select(m => Regex.Escape(m.Groups[1].Value))
            .ToArray();
        
        return delimiters.Length == 0 ? Regex.Escape(delimiterMatch) : string.Join("|", delimiters);
    }

    private static string FormatNumbers(string numbers, string delimiter)
    {
        numbers = numbers.Replace("\n", delimiter);
        numbers = Regex.Replace(numbers, $"({delimiter})+", delimiter);
        return numbers;
    }

    private static int ParseNumbers(string numbers, string delimiter, List<int> negativeNumbers)
    {
        var splitNumbers = numbers.Split(delimiter);
        var sum = 0;

        foreach (var num in splitNumbers)
        {
            var parsedNumber = int.Parse(num);

            if (parsedNumber < 0)
            {
                negativeNumbers.Add(parsedNumber);
            }
            else if (parsedNumber <= 1000)
            {
                sum += parsedNumber;
            }
        }

        return sum;
    }

    private static void ThrowExceptionForNegativeNumbers(List<int> negativeNumbers)
    {
        if (negativeNumbers.Any())
        {
            throw new Exception($"negatives not allowed: {string.Join(",", negativeNumbers)}");
        }
    }

    [GeneratedRegex("//.+\n")]
    private static partial Regex MyRegex();
}