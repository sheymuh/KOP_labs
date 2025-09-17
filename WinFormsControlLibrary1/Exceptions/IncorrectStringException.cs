namespace WinFormsControlLibrary1.Exceptions;

public class IncorrectStringException : Exception
{
    public IncorrectStringException() : base("String isn't match the pattern") { }
}
