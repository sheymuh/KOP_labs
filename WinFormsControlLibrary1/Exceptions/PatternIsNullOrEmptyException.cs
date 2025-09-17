namespace WinFormsControlLibrary1.Exceptions;

public class PatternIsNullOrEmptyException : Exception
{
    public PatternIsNullOrEmptyException() : base("Pattern is null or empty") { }
}
