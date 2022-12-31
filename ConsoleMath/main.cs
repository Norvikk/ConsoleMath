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
                Number = 0,
                Operator = null
            };
        }
    }

    public static void Main()
    {
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
                    Console.WriteLine("NO INPUT");
                    Main();
                    break;

                default:

                    var userOutput = Convert.ToString(Listen(listened!));
                    if (userOutput == "-2,1474836E+09")
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
    private static float Listen(string listened)
    {
        var listenedSplit = listened.ToLower().Split(" ");

        var app = 0;
        var loop = 0;


        foreach (var word in listenedSplit)
        {
            if (IsIndirectInt(word) != 0) _appList![app].Number = IsIndirectInt(word);


            else if (IsDirectInt(word) && _appList![app].Number == 0) _appList[app].Number = StringToInt(word);


            else
            {
                var result = IsOperator(word);
                if (result != "not") _appList![app].Operator = result;
            }

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

        return 0;

        static bool IsDirectInt(string word) => int.TryParse(word, out _);

        static int IsIndirectInt(string word)
        {
            var numbersSplit = TextToIntTrigger.Split(" ");

            return numbersSplit.Any(word.Contains) ? Array.IndexOf(numbersSplit, word) : 0;
        }

        static int StringToInt(string word) => Convert.ToInt32(word);

        static int Calculate()
        {
            var resultFormula = _appList!.Where(app => app.Number != 0)
                .Aggregate("", (current, app) => current + $"{app.Number} {app.Operator} ");
            var e = new Expression(resultFormula);

            return (int)Math.Round(e.calculate());
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

            return "not";
        }
    }
}