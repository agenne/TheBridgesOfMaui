using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebViewInterop
{
  public class SourceChangedEventArgs : EventArgs
  {
    public UrlWebViewSource Source
    {
      get;
      private set;
    }

    public SourceChangedEventArgs(UrlWebViewSource source)
    {
      Source = source;
    }
  }

}
