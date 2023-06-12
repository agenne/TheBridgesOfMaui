using CommunityToolkit.Mvvm.Messaging;
using WebViewInterop;

namespace TheBridgesOfMaui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

    WeakReferenceMessenger.Default.Register<SignatureCaptureMessage>(this, (r, m) =>
    {
      CaptureSignature(m.Value);
    });
  }

  private async void CaptureSignature(SignatureCaptureOptions options)
  {
    SignatureCapturePage page = new (options, async (result) => {
      await Application.Current.MainPage.Navigation.PopModalAsync();
      WeakReferenceMessenger.Default.Send(new SignatureCaptureResultMessage(result));
    });

    await MainThread.InvokeOnMainThreadAsync(async () =>
    {
      await Application.Current.MainPage.Navigation.PushModalAsync(page);
    });
  }
}
