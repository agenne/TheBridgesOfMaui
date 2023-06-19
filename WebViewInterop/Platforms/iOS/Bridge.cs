using Foundation;
using WebKit;

namespace WebViewInterop
{
  public partial class Bridge
  {
    private WKWebView _webView;

    private class WebViewBridgeMessageHandler : WKScriptMessageHandler
    {
      private Bridge _bridge;

      public WebViewBridgeMessageHandler(Bridge bridge)
      {
        _bridge = bridge;
      }

      public override void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
      {
        var dict = message.Body as NSDictionary;
        NSObject methodName;
        if (dict.TryGetValue(new NSString("MethodName"), out methodName))
        {
          switch (methodName.ToString())
          {
            case "alert":
              {
                var args = (dict["MethodArguments"] as NSDictionary);
                _bridge.InternalAlert(args["message"].ToString());
                break;
              }
            case "captureSignature":
              {
                var args = (dict["MethodArguments"] as NSDictionary);
                _bridge.InternalCaptureSignature(args["options"].ToString());
                break;
              }
          }
        }
      }
    }

    public void Connect(WKWebView platformView)
    {
      _webView = platformView;
      var script = new WKUserScript(new NSString(GetJavascriptForBridge()), WKUserScriptInjectionTime.AtDocumentEnd, false);
      platformView.Configuration.UserContentController.AddUserScript(script);
      platformView.Configuration.UserContentController.AddScriptMessageHandler(new WebViewBridgeMessageHandler(this), "webViewBridgeHandler");
    }

    private string GetJavascriptForBridge()
    {
      var js = @"window.webViewBridge = {
        alert: function(message) {
          window.webkit.messageHandlers.webViewBridgeHandler.postMessage({ 'MethodName': 'alert', 'MethodArguments': {'message': message} });
        },
        captureSignature: function(options) {
          window.webkit.messageHandlers.webViewBridgeHandler.postMessage({ 'MethodName': 'captureSignature', 'MethodArguments': {'options': options} });
        }
      };
      ";
      return js;
    }

    public void Disconnect(WKWebView platformView)
    {
      _webView = null;
      platformView.Configuration.UserContentController.RemoveAllUserScripts();
      platformView.Configuration.UserContentController.RemoveScriptMessageHandler("webViewBridgeHandler");
    }

    public async Task<string> EvaluateJavascriptAsync(string script)
    {
      var strResult = await _webView.EvaluateJavaScriptAsync(script);
      if (strResult != null)
      {
        return strResult.ToString();
      }
      else
      {
        return string.Empty;
      }
    }
  }
}
