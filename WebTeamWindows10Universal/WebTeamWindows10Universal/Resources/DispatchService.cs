using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WebTeamWindows10Universal.Resources
{
    public static class DispatchService
    {
        public static IAsyncResult Invoke(CoreDispatcherPriority priority, DispatchedHandler action)
        {
            var dispatchObject = Window.Current.Dispatcher;
            if (dispatchObject == null || dispatchObject.HasThreadAccess)
            {
                action();
                return null;
            }
            else
            {
                return (IAsyncResult) dispatchObject.RunAsync(priority, action);
            }
        }
    }
}
