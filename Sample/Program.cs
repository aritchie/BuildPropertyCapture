using BuildPropertyCapture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddBuildProperties();
var app = builder.Build();

var props = app.Services.GetRequiredService<IBuildProperties>();
foreach (var prop in props.Properties)
{
    Console.WriteLine($"{prop.Key} = {prop.Value}");
}

app.Run();