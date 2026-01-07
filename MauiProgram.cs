using Microsoft.Extensions.Logging;

namespace FateRank;
/// <summary>
/// The static entry point for the .NET MAUI application.
/// This class configures the dependency injection container, fonts, and services.
/// </summary>
public static class MauiProgram
{
	/// <summary>
    /// Creates and builds the MauiApp instance.
    /// This is called by the native platform entry points (iOS/Android/Windows/Mac).
    /// </summary>
    /// <returns>The configured MauiApp ready to be run.</returns>
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		// Adds a debug logger so i can see Console.WriteLine output in VSC
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
