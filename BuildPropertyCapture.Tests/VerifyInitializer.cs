using System.Runtime.CompilerServices;

namespace BuildPropertyCapture.Tests;


public class VerifyInitializer
{
    [ModuleInitializer]
    public static void Init() =>
        VerifySourceGenerators.Initialize();
}