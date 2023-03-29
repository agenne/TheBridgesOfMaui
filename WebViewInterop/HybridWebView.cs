namespace WebViewInterop
{
  public class HybridWebView : View, IWebView
  {
    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
      propertyName: "Source",
      returnType: typeof(UrlWebViewSource),
      declaringType: typeof(HybridWebView),
      defaultValue: new UrlWebViewSource() { Url = "about:blank" },
      propertyChanged: OnSourceChanged);

    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as HybridWebView;
      view.ChangeSource(newValue as UrlWebViewSource);
    }

    protected virtual void ChangeSource(UrlWebViewSource urlWebViewSource)
    {
      Dispatcher.Dispatch(() =>
      {
        SourceChanged?.Invoke(this, new SourceChangedEventArgs(urlWebViewSource));
      });
    }
  }
}