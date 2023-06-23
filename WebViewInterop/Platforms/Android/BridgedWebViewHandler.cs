using Android.Views;
using Android.Webkit;
using Microsoft.Maui.Handlers;
using static Android.Views.ViewGroup;

namespace WebViewInterop.Handlers;

public class BridgedWebViewHandler : ViewHandler<IBridgedWebView, Android.Webkit.WebView>
{
  public static PropertyMapper<IBridgedWebView, BridgedWebViewHandler> BridgedWebViewMapper = new PropertyMapper<IBridgedWebView, BridgedWebViewHandler>(ViewHandler.ViewMapper);

  private Bridge _bridge;

  public BridgedWebViewHandler() : base(BridgedWebViewMapper)
  {
  }

  protected override Android.Webkit.WebView CreatePlatformView()
  {
    var webView = new Android.Webkit.WebView(Context)
    {
      LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
    };
    _bridge = new Bridge();
    InitializeWebView(webView);
    return webView;
  }

  public void InitializeWebView(Android.Webkit.WebView webView)
  {
    webView.Settings.JavaScriptEnabled = true;

    // enable window.localStorage
    webView.Settings.DatabaseEnabled = true;
    webView.Settings.DomStorageEnabled = true;

    // allow zooming/panning
    webView.Settings.BuiltInZoomControls = false;
    webView.Settings.SetSupportZoom(false);
    webView.Settings.TextZoom = 100;

    // allow file access. without this the access from javascript to linked style sheets will fail
    // document.styleSheets[0].cssRules -> null!!!
    webView.Settings.AllowFileAccess = true;
    webView.Settings.AllowFileAccessFromFileURLs = true;
    webView.Settings.AllowUniversalAccessFromFileURLs = true;
    webView.Settings.MediaPlaybackRequiresUserGesture = false;

    // scrollbar stuff
    webView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay; // so there's no 'white line'
    webView.ScrollbarFadingEnabled = false;

    Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);

    /*
      Using WebChromeClient allows you to handle Javascript dialogs, favicons, titles, and the progress. 
      Take a look of this example: Adding alert() support to a WebView

      At first glance, there are too many differences WebViewClient & WebChromeClient. 
      But, basically: if you are developing a WebView that won't require too many features but rendering HTML, 
      you can just use a WebViewClient. On the other hand, if you want to (for instance) load the favicon of 
      the page you are rendering, you should use a WebChromeClient object and override the 
      onReceivedIcon(WebView view, Bitmap icon).
      
      http://www.phonesdevelopers.com/1752491/
   */
    CookieManager.Instance.SetAcceptThirdPartyCookies(webView, true);
    webView.SetWebChromeClient(new WebChromeClient());
  }

  protected override void ConnectHandler(Android.Webkit.WebView platformView)
  {
    base.ConnectHandler(platformView);
    
    _bridge.Connect(platformView);

    //platformView.LoadUrl("https://www.google.com");

    platformView.LoadUrl("file:///android_asset/WebApp/Index.html");
  }

  protected override void DisconnectHandler(Android.Webkit.WebView platformView)
  {
    base.DisconnectHandler(platformView);
    _bridge.Disconnect(platformView);
  }
}
