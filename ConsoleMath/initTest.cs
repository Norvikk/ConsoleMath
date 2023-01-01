using System.Globalization;

namespace ConsoleMath;

public static class InitTest
{
    public static void TestFunctionality()
    {
        var requiresBreaking = new bool[4];
        const string testCase1 = "1 + 1";
        const string testCase2 = "two minus 1";
        const string testCase3 = "three * 3";
        const string testCase4 = "10 divided by 100";
       
        Arithmetic.AssignArithmeticsSource();
        requiresBreaking[0] = Affirmative(Convert.ToString(Arithmetic.Listen(testCase1), CultureInfo.InvariantCulture), "2", "int plus_operator int");
        Arithmetic.AssignArithmeticsSource();
        requiresBreaking[1] =Affirmative(Convert.ToString(Arithmetic.Listen(testCase2), CultureInfo.InvariantCulture), "1", "text minus_text int");
        Arithmetic.AssignArithmeticsSource();
        requiresBreaking[2] =Affirmative(Convert.ToString(Arithmetic.Listen(testCase3), CultureInfo.InvariantCulture), "9", "text multiplication_operator int");
        Arithmetic.AssignArithmeticsSource();
        requiresBreaking[3] =Affirmative(Convert.ToString(Arithmetic.Listen(testCase4), CultureInfo.InvariantCulture), "0.1", "int divided_text int");

        if (requiresBreaking.Any(@bool => !@bool)) throw new Exception("PROGRAM FUNCTIONALITY TEST DIDN'T SUCCEED");
        
        Console.WriteLine("\n");

            static bool Affirmative(string @is, string @must, string message)
        {

            if (@is == @must)
            {
                Console.WriteLine($"||PASSED -> \t {message}");
                return true;
            }

            Console.WriteLine($"||ERROR--BREAKING -> \t {message}");
            return false;
        }
        
    }
}