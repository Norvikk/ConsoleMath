using System.Globalization;
using org.mariuszgromada.math.mxparser; // MathParser.org-mXparser (from NuGet)

namespace ConsoleMath;

public static class Arithmetic
{
    private static readonly Random Rdm = new();


    private const string AdditionTrigger = "add sum combine plus +";
    private const string SubtractionTrigger = "take subtract minus remove without -";
    private const string DivisionTrigger = "divided division divide /";
    private const string MultiplicationTrigger = "times multiplicate * multiplication";

    private const string TextToIntTrigger = "zero one two three four five six seven eight nine ten" +
                                            " eleven twelve thirteen fourteen fifteen sixteen seventeen" +
                                            " eighteen nineteen twenty";

    private static Types.ArithmeticsSource[]? _source;
    private static Types.MathApplication[]? _appList;

    private const int MathApplicationCount = 15;


    private static void AssignArithmeticsSource()
    {
        _source = new Types.ArithmeticsSource[4];

        _source[0] = new Types.ArithmeticsSource
        {
            ConsoleMeaning = "+",
            Calls = AdditionTrigger.Split(" ")
        };
        _source[1] = new Types.ArithmeticsSource
        {
            ConsoleMeaning = "-",
            Calls = SubtractionTrigger.Split(" ")
        };
        _source[2] = new Types.ArithmeticsSource
        {
            ConsoleMeaning = "/",
            Calls = DivisionTrigger.Split(" ")
        };
        _source[3] = new Types.ArithmeticsSource
        {
            ConsoleMeaning = "*",
            Calls = MultiplicationTrigger.Split(" ")
        };

        _appList = new Types.MathApplication[MathApplicationCount + 1];

        for (var i = 0; i < MathApplicationCount; i++)
        {
            _appList[i] = new Types.MathApplication
            {
                Number = string.Empty,
                Operator = string.Empty
            };
        }
    }

    private static void Playground()
    {
    }

    public static void Main()
    {
        Playground();

        Responses.Introduction();
        Responses.CommandPalette();
        Responses.Instructions();
        while (true)
        {
            AssignArithmeticsSource();
            Console.WriteLine("Listening...");
            var listened = Console.ReadLine();
            switch (listened?.ToLower())
            {
                case "instructions":
                    Responses.Instructions();
                    break;

                case "supported":
                    Responses.Supported();
                    break;

                case "":
                    Console.WriteLine("No Input");
                    Main();
                    break;

                default:
                    var userOutput = Convert.ToString(Listen(listened!), CultureInfo.InvariantCulture);
                    if (userOutput is "-2,1474836E+09" or "NaN")
                        userOutput =
                            $"({userOutput}: ERROR)\nHave you considered using the command: SUPPORTED as an input?";


                    Console.WriteLine($"{Responses.ResultAffirmation()}{userOutput}");
                    break;
            }

            Console.WriteLine("Press <Enter> to clear and restart...");
            Console.ReadLine();
            Console.Clear();
        }
    }


    // Main processing function
    private static double Listen(string listened)
    {
        var listenedSplit = listened.ToLower().Split(" ");

        var app = 0;
        var loop = 0;


        foreach (var word in listenedSplit)
        {
            var indirectInt = IsIndirectInt(word);
            if (indirectInt != -404) _appList![app].Number = word;


            else if (IsDirectInt(word) && _appList![app].Number == string.Empty)
            {
                _appList![app].Number = word;
            }


            else
            {
                var result = IsOperator(word);
                if (result != string.Empty) _appList![app].Operator = result;
            }

            if ((word == listenedSplit[^1] && loop == listenedSplit.Length - 1) ||
                (_appList![app].Number != string.Empty && !string.IsNullOrEmpty(_appList[app].Operator)))
            {
                if (word == listenedSplit[^1] && loop == listenedSplit.Length - 1)
                {
                    return Calculate();
                }

                app++;
            }

            loop++;
        }

        return 0;

        static bool IsDirectInt(string word) => int.TryParse(word, out _) || double.TryParse(word, out _);

        static int IsIndirectInt(string word)
        {
            var numbersSplit = TextToIntTrigger.Split(" ");

            return numbersSplit.Any(word.Contains) ? Array.IndexOf(numbersSplit, word) : -404;
        }


        static double Calculate()
        {
            var resultFormula = _appList!.Where(app => app.Number != string.Empty)
                .Aggregate("", (current, app) => current + $"{app.Number?.Replace(",", ".")} {app.Operator} ");
            var e = new Expression(resultFormula);
            return e.calculate();
        }

        static string IsOperator(string word)
        {
            foreach (var arithmetic in _source!)
            {
                if (arithmetic.Calls.Any(operatorVar => operatorVar == word))
                {
                    return arithmetic.ConsoleMeaning;
                }
            }

            return string.Empty;
        }
    }
}