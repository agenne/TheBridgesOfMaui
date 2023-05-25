using CoreGraphics;
using Foundation;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebKit;

namespace WebViewInterop.Platforms.iOS
{
  public class BridgetWebViewHandler : ViewHandler<IBridgetWebView, WKWebView>
  {
    public static PropertyMapper<IBridgetWebView, BridgetWebViewHandler> HybridWebViewMapper = new PropertyMapper<IBridgetWebView, BridgetWebViewHandler>(ViewHandler.ViewMapper);

    public BridgetWebViewHandler() : base(HybridWebViewMapper)
    {
    }

    protected override WKWebView CreatePlatformView()
    {
      var config = new WKWebViewConfiguration();
      config.SetValueForKey(NSObject.FromObject(true), new NSString("allowUniversalAccessFromFileURLs"));
      config.Preferences.SetValueForKey(NSNumber.FromBoolean(true), new NSString("allowFileAccessFromFileURLs"));
      var webView = new WKWebView(CGRect.Empty, config);

      return webView;
    }

    protected override void ConnectHandler(WKWebView platformView)
    {
      base.ConnectHandler(platformView);

      //platformView.LoadRequest(new NSUrlRequest(new NSUrl("https://www.google.com")));
      
      string path = Path.Combine(NSBundle.MainBundle.BundlePath, "WebApp");
      var uri = new NSUrl($"file://{path}/Index.html");
      var webAppPath = new NSUrl($"file://{path}");
      platformView.LoadFileUrl(uri, webAppPath);
    }
  }
}
