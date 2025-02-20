using BuildPropertyCapture;

namespace Sample.Maui;


public class MainViewModel(IBuildProperties props)
{
    public List<BuildProp> Props => props
        .Properties
        .Select(x => new BuildProp(x.Key, x.Value))
        .OrderBy(x => x.Key)
        .ToList();
}

public record BuildProp(string Key, string Value);