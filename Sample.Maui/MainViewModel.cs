using BuildPropertyCapture;

namespace Sample.Maui;


public class MainViewModel
{
    public List<BuildProp> Props => BuildVariables
        .Items?
        .Select(x => new BuildProp(x.Key, x.Value))
        .OrderBy(x => x.Key)
        .ToList() ?? new List<BuildProp>();
}

public record BuildProp(string Key, string Value);