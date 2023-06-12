using CommunityToolkit.Mvvm.Messaging;
using System.Text.Json;

namespace WebViewInterop
{
  public partial class Bridge
  {
    public Bridge()
    {
       WeakReferenceMessenger.Default.Register<SignatureCaptureResultMessage>(this, (r, m) =>
       {
         ProvideSignature(m.Value);
       });
    }

    private async void ProvideSignature(SignatureCaptureResult result)
    {
      var options = new JsonSerializerOptions();
      options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
      options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
      var json = JsonSerializer.Serialize(result, options);
      var res = await EvaluateJavascriptAsync($"window.webViewBridgeTarget.provideSignature({json})");
    }

    public async void InternalAlert(string message)
    {
      await MainThread.InvokeOnMainThreadAsync(async () =>
      {
        await Application.Current.MainPage.DisplayAlert("Information", message.ToString(), "OK");
      });
    }

    public void InternalCaptureSignature(string options)
    {
      var serializierOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
      var o = JsonSerializer.Deserialize<SignatureCaptureOptions>(options, serializierOptions);
      WeakReferenceMessenger.Default.Send(new SignatureCaptureMessage(o));
    }
  }
}
