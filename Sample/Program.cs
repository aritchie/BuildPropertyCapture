using BuildPropertyCapture;

namespace Sample;

public class Program
{
    static void Main()
    {
        if (BuildVariables.Items == null)
        {
            Console.WriteLine("BuildVariables.Items is null");
        }
        else
        {
            foreach (var prop in BuildVariables.Items)
            {
                Console.WriteLine($"{prop.Key} = {prop.Value}");
            }
        }
    }
}