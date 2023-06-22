using Maui.Windows.Interfaces;
using Microsoft.UI.Xaml.Controls;

namespace WebViewInterop;

public partial class Bridge : IWebViewBridge
{
  private string _currentStartupId;

  private WebView2 _webView;

  public async Task Connect(WebView2 webView)
  {
    _webView = webView;
    await webView.EnsureCoreWebView2Async();

    webView.CoreWebView2.Settings.AreDevToolsEnabled = true;
    webView.CoreWebView2.Settings.AreHostObjectsAllowed = true;
    webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
    webView.CoreWebView2.Settings.IsScriptEnabled = true;
    webView.CoreWebView2.Settings.IsWebMessageEnabled = true;

    var dispatchAdapter = new WinRTAdapter.DispatchAdapter();

    webView.CoreWebView2.AddHostObjectToScript("bridge", dispatchAdapter.WrapObject(this, dispatchAdapter));
    _currentStartupId = await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"window.webViewBridge = chrome.webview.hostObjects.sync.bridge;");

  }

  public void Disconnect(WebView2 webView)
  {
    _webView = null;
    webView.CoreWebView2.RemoveScriptToExecuteOnDocumentCreated(_currentStartupId);
    webView.CoreWebView2.RemoveHostObjectFromScript("bridge");
  }

  public void Alert(string message)
  {
    AlertImplementation(message);
  }

  public void CaptureSignature(string options)
  {
    CaptureSignatureImplementation(options);
  }

  private async Task<string> EvaluateJavascriptAsync(string script)
  {
    var result = await _webView.CoreWebView2.ExecuteScriptAsync(script);
    return result;
  }
}
