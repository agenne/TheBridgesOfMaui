using CoreGraphics;
using Foundation;
using Microsoft.Maui.Handlers;
using WebKit;

namespace WebViewInterop.Handlers;

public class BridgetWebViewHandler : ViewHandler<IBridgetWebView, WKWebView>
{
  public static PropertyMapper<IBridgetWebView, BridgetWebViewHandler> BridgetWebViewMapper = new PropertyMapper<IBridgetWebView, BridgetWebViewHandler>(ViewHandler.ViewMapper);

  private Bridge _bridge;

  public BridgetWebViewHandler() : base(BridgetWebViewMapper)
  {
  }

  protected override WKWebView CreatePlatformView()
  {
    var config = new WKWebViewConfiguration();
    config.SetValueForKey(NSObject.FromObject(true), new NSString("allowUniversalAccessFromFileURLs"));
    config.Preferences.SetValueForKey(NSNumber.FromBoolean(true), new NSString("allowFileAccessFromFileURLs"));
    var webView = new WKWebView(CGRect.Empty, config);
    InitializeWebView(webView);
    _bridge = new Bridge();
    return webView;
  }

  private void InitializeWebView(WKWebView webView)
  {
    //Mit iOS 16.4 kann jetzt über die AppSettings das WebDebugging aktiviert werden. Früher ging das nur, wenn die App im Debug-Modus war.
    //Jetzt geht es also auch für die Store-App.
    //https://webkit.org/blog/13936/enabling-the-inspection-of-web-content-in-apps/

    webView.Inspectable = true;

  }

  protected override void ConnectHandler(WKWebView platformView)
  {
    base.ConnectHandler(platformView);
    _bridge.Connect(platformView);

    //platformView.LoadRequest(new NSUrlRequest(new NSUrl("https://www.google.com")));

    string path = Path.Combine(NSBundle.MainBundle.BundlePath, "WebApp");
    var uri = new NSUrl($"file://{path}/Index.html");
    var webAppPath = new NSUrl($"file://{path}");
    platformView.LoadFileUrl(uri, webAppPath);
  }

  protected override void DisconnectHandler(WKWebView platformView)
  {
    base.DisconnectHandler(platformView);
    _bridge.Disconnect(platformView);
  }
}
