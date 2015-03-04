using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WebTeamWindows.Common

{
    public static class NavigationService
    {
        public static void Navigate(Type sourcePage)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(sourcePage);
        }

        public static void Navigate(Type sourcePage, object parameter)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(sourcePage, parameter);
        }

        public static void GoBack()
        {
            var frame = (Frame)Window.Current.Content;
            frame.GoBack();
        }
    }
}
