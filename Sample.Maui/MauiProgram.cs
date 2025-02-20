using Microsoft.Extensions.Logging;
#if !HOTRELOAD
using Sentry;
#endif

namespace Sample.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
#if !HOTRELOAD
			.UseSentry(opts =>
			{
				opts.Dsn = "";
			})
#endif
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddBuildProperties();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<MainViewModel>();

		return builder.Build();
	}
}
