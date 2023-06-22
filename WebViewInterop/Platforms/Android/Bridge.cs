using Android.Webkit;
using Java.Interop;

namespace WebViewInterop;

public partial class Bridge : Java.Lang.Object
{
  private class StringCallback : Java.Lang.Object, IValueCallback
  {
    private TaskCompletionSource<string> source;

    public Task<string> Task { get { return source.Task; } }

    public StringCallback()
    {
      source = new TaskCompletionSource<string>();
    }

    public void OnReceiveValue(Java.Lang.Object value)
    {
      try
      {
        var jstr = value.ToString(); ;
        source.SetResult(jstr.Trim('"'));
      }
      catch (Exception ex)
      {
        source.SetException(ex);
      }
    }
  }

  private Android.Webkit.WebView _webView;

  private Android.App.Activity Context
  {
    get { return Microsoft.Maui.ApplicationModel.Platform.CurrentActivity; }
  }

  public void Connect(Android.Webkit.WebView webView)
  {
    _webView = webView;
    Context.RunOnUiThread(() =>
    {
      webView.AddJavascriptInterface(this, BRIDGE_NAME);
    });
  }

  public void Disconnect(Android.Webkit.WebView webView)
  {
    Context.RunOnUiThread(() =>
    {
      webView.RemoveJavascriptInterface(BRIDGE_NAME);
    });
    _webView = null;
  }

  [JavascriptInterface]
  [Export("alert")]
  public void Alert(Java.Lang.String message)
  {
    AlertImplementation(message.ToString());
  }

  [JavascriptInterface]
  [Export("captureSignature")]
  public void CaptureSignature(Java.Lang.String options)
  {
    CaptureSignatureImplementation(options.ToString());
  }

  private async Task<string> EvaluateJavascriptAsync(string script)
  {
    var javascriptResult = new StringCallback();

    Context.RunOnUiThread(() =>
    {
      _webView.EvaluateJavascript(script, javascriptResult);
    });

    var result = await javascriptResult.Task;
    return result;
  }
}
