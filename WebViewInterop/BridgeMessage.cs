using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WebViewInterop;

public class SignatureCaptureMessage : ValueChangedMessage<SignatureCaptureOptions>
{
  public SignatureCaptureMessage(SignatureCaptureOptions value) : base(value)
  {

  }
}

public class SignatureCaptureResultMessage : ValueChangedMessage<SignatureCaptureResult>
{
  public SignatureCaptureResultMessage(SignatureCaptureResult value) : base(value)
  {

  }
}
