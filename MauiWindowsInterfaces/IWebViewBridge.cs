using System;
using Windows.Foundation;

namespace Maui.Windows.Interfaces
{
  public interface IWebViewBridge
  {
    void Alert(string message);
  }
}
