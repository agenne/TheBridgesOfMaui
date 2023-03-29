﻿using Microsoft.Extensions.Logging;
using WebViewInterop;

namespace TheBridgesOfMaui;

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
			})
			.ConfigureMauiHandlers(handlers =>
      {
      });

#if DEBUG
		builder.Logging.AddDebug();
#endif
//    handlers.AddHandler(typeof(HybridWebView), typeof(HybridWebViewHandler));

    return builder.Build();
	}
}