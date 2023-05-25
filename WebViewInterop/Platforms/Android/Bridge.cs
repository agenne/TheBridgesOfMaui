using Android.Content;
using Android.Webkit;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebViewInterop.Platforms.Droid
{
  public partial class Bridge : Java.Lang.Object
  {
    private Android.App.Activity Context
    {
      get { return Microsoft.Maui.ApplicationModel.Platform.CurrentActivity; }
    }

    public void Connect(Android.Webkit.WebView webView)
    {
      Context.RunOnUiThread(() =>
      {
        webView.AddJavascriptInterface(this, "webViewBridge");
      });
    }

    public void Disconnect()
    {

    }

    [JavascriptInterface]
    [Export("alert")]
    public void Alert(Java.Lang.String message)
    {
      Context.RunOnUiThread(() =>
      {
        Application.Current.MainPage.DisplayAlert("Information", message.ToString(), "OK");
      });
    }
  }
}
