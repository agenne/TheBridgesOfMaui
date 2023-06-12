using Microsoft.Extensions.Logging;
using WebViewInterop;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel.Communication;

#if ANDROID
using WebViewInterop.Platforms.Droid;
#endif
#if IOS
using WebViewInterop.Platforms.iOS;
#endif
#if WINDOWS
using WebViewInterop.Platforms.Windows;
#endif

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
