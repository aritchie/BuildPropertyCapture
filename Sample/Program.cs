using BuildPropertyCapture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample;
using System.Reflection;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddBuildProperties();
var app = builder.Build();

var props = app.Services.GetRequiredService<IBuildProperties>();
foreach (var prop in props.Properties)
{
    Console.WriteLine($"{prop.Key} = {prop.Value}");
}

//Assembly.GetCallingAssembly().GetType("");
//var type1 = Assembly.GetEntryAssembly().GetType("Thing");

var type = Type.GetType("Thing");
var members = typeof(Program).GetTypeInfo().DeclaredMembers;


app.Run();