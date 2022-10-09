using StorMatch.Pages;
using StorMatch.Views;

namespace StorMatch;

public static class MauiProgram
{
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
		builder.Services.AddSingleton<ConditionsPage>();
		builder.Services.AddSingleton<ConditionsViewModel>();
        builder.Services.AddScoped<ConditionsViewModel>();
        return builder.Build();
	}
}
