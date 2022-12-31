namespace ConsoleMath;

public static class Responses
{
    private static readonly Random Rdm = new();
    private static int GetRandomInt(int low, int max) => Rdm.Next(low, max);

    public static string ResultAffirmation()
    {
        const string affirmations = "The result is -I guess it's -Resultant -Resulting -Outcome achieved: -Conclusion reached: -Answer obtained: -The outcome produced: ";
        var splitAffirmations = affirmations.Split("-");
        return splitAffirmations[Rdm.Next(0, splitAffirmations.Length)];
    }
    public static void CommandPalette()
    {
        Console.WriteLine("Possible commands ->  supported, instructions");
    }

    public static void Introduction()
    {
        const string output = "<Norvik>     ConsoleMath";
        Console.WriteLine($"{output}\n{GetSplitterLines(output, "-+")}");
    }

    public static void Supported()
    {
        Console.WriteLine("Operators supported: +-*/\n" +
                          "Equations should be written with spaces and not 1+1 or oneplusone.\n" +
                          "Symbols connected to words are also not supported like 'one plus one!!'\n\n" +
                          "The text variant of any number above 20 (twenty) is not supported, but can be easily implemented in the code.");
    }

    public static void Instructions()
    {
        var result = $"Sample input: Get {GetRandomInt(-54, 11)} minus {GetRandomInt(1, 9)}";
        Console.WriteLine($"{result}\n{GetSplitterLines(result, "-")}\n");
    }

    // Custom function for getting a fancy underline with characters used in this example
    // Example Text
    // ------------
    private static string GetSplitterLines(string sentence, string spacerStyle)
    {
        var result = "";

        for (var i = 0; i < sentence.Length / spacerStyle.Length; i++)
        {
            result += spacerStyle;
        }

        return result;
    }
}