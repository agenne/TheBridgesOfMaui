using Microsoft.Extensions.Logging;
using WebViewInterop;
using CommunityToolkit.Maui;
using WebViewInterop.Handlers;

namespace TheBridgesOfMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
      .UseMauiCommunityToolkit()
      .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.ConfigureMauiHandlers(handlers =>
      {
        handlers.AddHandler(typeof(BridgetWebView), typeof(BridgetWebViewHandler));
      });

#if DEBUG
		builder.Logging.AddDebug();
#endif

    return builder.Build();
	}
}
