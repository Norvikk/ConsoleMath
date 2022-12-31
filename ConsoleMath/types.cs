namespace ConsoleMath;

public static class Types
{
    public struct ArithmeticsSource
    {
        public string ConsoleMeaning;
        public string[] Calls;
    }

    // Struct for equation entries. (e.g) one[Number] plus[Operator] one[Number] <--- creates a new struct and adds it
    // Because MathApplication1.Number is occupied 
    public struct MathApplication
    {
        public int? Number;
        public string? Operator;
    }
}