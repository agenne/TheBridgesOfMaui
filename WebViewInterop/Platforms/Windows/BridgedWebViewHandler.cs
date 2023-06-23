using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace WebViewInterop.Handlers;

public class BridgedWebViewHandler : ViewHandler<IBridgedWebView, WebView2>
{
  public static PropertyMapper<IBridgedWebView, BridgedWebViewHandler> BridgedWebViewMapper = new PropertyMapper<IBridgedWebView, BridgedWebViewHandler>(ViewHandler.ViewMapper);

  private Bridge _bridge;

  public BridgedWebViewHandler() : base(BridgedWebViewMapper)
  {
  }

  protected override WebView2 CreatePlatformView()
  {
    var webView = new WebView2();
    _bridge = new Bridge();
    InitializeWebView(webView);
    return webView;
  }

  private void InitializeWebView(WebView2 webView)
  {
  }

  protected override async void ConnectHandler(WebView2 platformView)
  {
    base.ConnectHandler(platformView);

    await _bridge.Connect(platformView);

    //platformView.Source = new Uri("https://www.google.com");

    platformView.CoreWebView2.SetVirtualHostNameToFolderMapping("wwwroot", "WebApp", CoreWebView2HostResourceAccessKind.Allow);
    platformView.CoreWebView2.Navigate("https://wwwroot/Index.html");
  }

  protected override void DisconnectHandler(WebView2 platformView)
  {
    base.DisconnectHandler(platformView);
    _bridge.Disconnect(platformView);
  }
}
