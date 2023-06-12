using CommunityToolkit.Maui.Core;
using System.Text;
using WebViewInterop;

namespace TheBridgesOfMaui;

public partial class SignatureCapturePage : ContentPage
{
  private Action<SignatureCaptureResult> _whenFinished;
  private SignatureCaptureOptions _options;

  public SignatureCapturePage(SignatureCaptureOptions options, Action<SignatureCaptureResult> whenFinished)
	{
		InitializeComponent();
    _options = options;
    _whenFinished = whenFinished;
		SignaturePad.Lines = new System.Collections.ObjectModel.ObservableCollection<IDrawingLine>();
		SignaturePad.LineColor = Color.FromArgb(options.PenColor);
		SignaturePad.LineWidth = options.PenWidth;
  }

  public string GetSvgString()
  {
    var svg = new StringBuilder();
    int offsetX = 10, offsetY = 10;
    var minX = Convert.ToInt32(SignaturePad.Lines.SelectMany((s) => s.Points).Min((p) => p.X));
    var minY = Convert.ToInt32(SignaturePad.Lines.SelectMany((s) => s.Points).Min((p) => p.Y));
    var maxX = Convert.ToInt32(SignaturePad.Lines.SelectMany((s) => s.Points).Max((p) => p.X));
    var maxY = Convert.ToInt32(SignaturePad.Lines.SelectMany((s) => s.Points).Max((p) => p.Y));
    var width = Convert.ToInt32(maxX - minX + (offsetX * 2));
    var height = Convert.ToInt32(maxY - minY + (offsetY * 2));
    svg.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{width}\" height=\"{height}\" viewBox=\"0 0 {width} {height}\" version=\"1.1\">");
    svg.AppendLine($"<g stroke=\"{_options.PenColor}\" stroke-width=\"{_options.PenWidth}\" fill=\"none\">");
    foreach (var line in SignaturePad.Lines)
    {
      svg.AppendLine("<polyline points=\"");
      foreach (var point in line.Points)
      {
        svg.AppendLine($"{Convert.ToInt32(point.X - minX + offsetX)},{Convert.ToInt32(point.Y - minY + offsetY)} ");
      }
      svg.AppendLine("\" />");
    }
    svg.AppendLine("</g>");
    svg.AppendLine("</svg>");
    return svg.ToString();
  }


  protected override bool OnBackButtonPressed()
  {
    _whenFinished(new SignatureCaptureResult { Success = false });
    return true;
  }

  private void SaveButton_Clicked(object sender, EventArgs e)
  {
    var svg = GetSvgString();
    _whenFinished(new SignatureCaptureResult { Success = true, Signature = svg });
  }

  private void CancelButton_Clicked(object sender, EventArgs e)
  {
    _whenFinished(new SignatureCaptureResult { Success = false });
  }

  private void ClearButton_Clicked(object sender, EventArgs e)
  {
    SignaturePad.Clear();
  }
}