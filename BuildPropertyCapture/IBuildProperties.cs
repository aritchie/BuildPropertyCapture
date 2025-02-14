namespace BuildPropertyCapture;

public interface IBuildProperties
{
    IReadOnlyDictionary<string, string> Properties { get; }
}