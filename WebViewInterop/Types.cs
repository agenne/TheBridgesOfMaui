namespace WebViewInterop;

public class SignatureCaptureOptions
{
  public int PenWidth { get; set; }
  public string PenColor { get; set; }
}

public class SignatureCaptureResult
{
  public bool Success { get; set; }
  public string Signature { get; set; }
}
