using ABI.Impl.Maui.Windows.Interfaces;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebViewInterop.Platforms.Windows
{
  public partial class Bridge : IWebViewBridge
  {
    private string _currentStartupId;

    public async Task Connect(WebView2 webView)
    {
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

    public void Disconnect()
    {

    }

    public void Alert(string message)
    {
      Application.Current.MainPage.DisplayAlert("Information", message.ToString(), "OK");
    }
  }
}
