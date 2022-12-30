using org.mariuszgromada.math.mxparser; // String parser NuGet-package

namespace ArithmeticTextToMath;

public static class Arithmetic
{
    private static readonly Random Rdm = new();

    // Words added here will trigger the corresponding equation
    private const string AdditionTrigger = "add sum combine plus +";
    private const string SubtractionTrigger = "take subtract minus remove without -";
    private const string DivisionTrigger = "divided division divide /";
    private const string MultiplicationTrigger = "times multiplicate * multiplication";

    // Array initialization with struct
    private static ArithmeticsSource[]? _source;
    private static MathApplication[]? _appList;

    // How many equations there can be in a single entry.
    // One equation: 10 times four (or) What is 10 times four anyways?
    private const int MathApplicationCount = 15;

    // Returns a random int from the given parameters
    private static int GetRandomInt(int low, int max) => Rdm.Next(low, max);

    // Struct for specifying what terms / words trigger the corresponding equation
    private struct ArithmeticsSource
    {
        public string ConsoleMeaning;
        public string[] Calls;
    }

    // Struct for equation entries. (e.g) one[Number] plus[Operator] one[Number] <--- creates a new struct and adds it
    // Because MathApplication1.Number is occupied 
    private struct MathApplication
    {
        public int? Number;
        public string? Operator;
    }

    // Initializes the Arrays and assigns to each 
    // the data at the top of the file
    private static void AssignArithmeticsSource()
    {
        _source = new ArithmeticsSource[4];

        _source[0] = new ArithmeticsSource
        {
            ConsoleMeaning = "+",
            Calls = AdditionTrigger.Split(" ")
        };
        _source[1] = new ArithmeticsSource
        {
            ConsoleMeaning = "-",
            Calls = SubtractionTrigger.Split(" ")
        };
        _source[2] = new ArithmeticsSource
        {
            ConsoleMeaning = "/",
            Calls = DivisionTrigger.Split(" ")
        };
        _source[3] = new ArithmeticsSource
        {
            ConsoleMeaning = "*",
            Calls = MultiplicationTrigger.Split(" ")
        };

        _appList = new MathApplication[MathApplicationCount + 1];

        for (var i = 0; i < MathApplicationCount; i++)
        {
            _appList[i] = new MathApplication
            {
                Number = 0,
                Operator = null
            };
        }
    }

    public static void Main()
    {
        Introduction(); // Introduces the creator of the program ONCE PER APP RUN
        CommandPalette(); // Shows possible commands ONCE PER APP RUN
        Instructions(); // Shows a partially hard coded sample input ONCE PER APP RUN
        while (true)
        {
            AssignArithmeticsSource(); // Initializes arrays
            Console.WriteLine("Listening..."); // Initialization affirmation 
            var listened = Console.ReadLine();
            switch (listened?.ToLower()) // Command handling
            {
                case "instructions":
                    Instructions();
                    break;

                case "supported":
                    Supported();
                    break;

                case "": // Check if user input is null. If so, inform user and restart program 
                    Console.WriteLine("NO INPUT");
                    Main();
                    break;

                default: // If no commands are detected and the user gave input, try to process it
                    var userOutput = Convert.ToString(Listen(listened!));
                    if (userOutput ==  "-2,1474836E+09")
                        userOutput =
                            $"({userOutput}: ERROR)\nHave you considered using the command: SUPPORTED as an input?";
                    
                    // If the user input returns a ERROR by the NuGet package, inform the user and suggest to check the SUPPORTED command out
                    Console.WriteLine($"The result is {userOutput}");
                    break;
            }

            Console.WriteLine("Press <Enter> to clear and restart...");
            Console.ReadLine();
            Console.Clear();
        }
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

    private static void CommandPalette()
    {
        Console.WriteLine("Possible commands ->  supported, instructions");
    }

    private static void Introduction()
    {
        const string output = "<Norvik>     ConsoleMath";
        Console.WriteLine(output);
        Console.WriteLine(GetSplitterLines(output, "-+"));
    }

    private static void Supported()
    {
        Console.WriteLine("Operators supported: +-*/\n" +
                          "Not supported: The number '0' (e.g. 0 + 1).\nEquations should be written with spaces and not 1+1 or oneplusone.\nSymbols connected to words are also not supported like 'one plus one!!'\ndecimal numbers are not intended to be used therefore don't work\n\nThe text variant of any number above 20 (twenty) is not supported, but can be easily implemented in the code.");
    }

    private static void Instructions()
    {
        var result = $"Sample input: Get {GetRandomInt(-54, 11)} minus {GetRandomInt(1, 9)}";
        Console.WriteLine($"{result}\n{GetSplitterLines(result, "-")}\n");
    }

    // Main processing function
    private static float Listen(string listened)
    {
        // Split the input into an array
        var listenedSplit = listened.ToLower().Split(" ");

        // Both are iterators. 
        // App increases whenever the current MathApplication has all slots satisfied 
        // Loop increases every time the FOREACH function runs. It is used to check if the function is at the last word in the array
        var app = 0;
        var loop = 0;


        foreach (var word in listenedSplit)
        {
            // Check if the current word (e.g.) FOuR is the text variant of a number. It isn't case sensitive 
            // If it is a number, apply it to the current MathApplication struct
            if (IsIndirectInt(word) != 0) _appList![app].Number = IsIndirectInt(word);

            // Check if the given word is a direct int, (e.g.) 4
            // If it is a number, apply it to the current MathApplication struct
            else if (IsDirectInt(word) && _appList![app].Number == 0) _appList[app].Number = StringToInt(word);

            // If its not a number, finally check if it is an arithmetic operator by looping it through all hard coded triggers
            else
            {
                var result = IsOperator(word);
                if (result != "not") _appList![app].Operator = result; // If its not an operator it will return "not"
            }

            // Checks if the loop is on its last word OR if the current struct is fully satisfied
            if ((word == listenedSplit[^1] && loop == listenedSplit.Length - 1) ||
                (_appList![app].Number != 0 && !string.IsNullOrEmpty(_appList[app].Operator)))
            {
                if (word == listenedSplit[^1] && loop == listenedSplit.Length - 1)
                {
                    return Calculate();
                }

                app++;
            }

            loop++;
        }

        // If the loop is faulty code-wise it will return 0
        return 0;

        // Returns a bool if the given string is a INT or not
        static bool IsDirectInt(string word) => int.TryParse(word, out _);

        // Returns a int if string word is the text equivalent of any number up to 20 in this build (More can be added but I didn't bother)
        static int IsIndirectInt(string word)
        {
            // Simplified way of adding numbers. DO NOT ADD USELESS ENTRIES SUCH AS AN EXTRA SPACE BECAUSE IT WILL BREAK THE PROGRAM INTO PROMPTING -1
            const string numbers =
                "zero one two three four five six seven eight nine ten eleven twelve thirteen fourteen fifteen sixteen seventeen eighteen nineteen twenty";
            var numbersSplit = numbers.Split(" ");

            // If the word is a number, get its index in the array and return the index, else return 0 which is a program-wide despised number in conditional statements 
            return numbersSplit.Any(word.Contains) ? Array.IndexOf(numbersSplit, word) : 0;
        }

        // Converts the parameter word to an int and returns it
        static int StringToInt(string word) => Convert.ToInt32(word);

        // Loops in all appList entries where MathApplications are stored and inputs them into the string resultFormula looking like ->
        // << 1 + 4 * 5 / 3 * 6 * 1 - 8 >> depending on entries and then uses the NuGet dependency to calculate the string and returns the answer
        static int Calculate()
        {
            var resultFormula = _appList!.Where(app => app.Number != 0)
                .Aggregate("", (current, app) => current + $"{app.Number} {app.Operator} ");
            var e = new Expression(resultFormula);

            return (int)Math.Round(e.calculate());
        }

        // Checks if the parameter is an operator by looping into all ArithmeticSources. Adding new ArithmeticSources will work.
        static string IsOperator(string word)
        {
            foreach (var arithmetic in _source!)
            {
                if (arithmetic.Calls.Any(operatorVar => operatorVar == word))
                {
                    return arithmetic.ConsoleMeaning;
                }
            }

            return "not";
        }
    }
}