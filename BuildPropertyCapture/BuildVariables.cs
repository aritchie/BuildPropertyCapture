namespace BuildPropertyCapture;

public static class BuildVariables
{
    public static IReadOnlyDictionary<string, string>? Items { get; private set; }


    public static void Initialize(Dictionary<string, string> items)
    {
        if (Items != null)
            throw new InvalidOperationException("You cannot initialize more than once.");
        
        Items = items.AsReadOnly();
    }
}