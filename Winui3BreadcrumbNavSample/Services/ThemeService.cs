using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winui3BreadcrumbNavSample.Pages;

namespace Winui3BreadcrumbNavSample.Services
{
    public class ThemeService
    {
        public static void ChangeTheme(ElementTheme theme)
        {
            if (SettingsPage.MainWindow.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = theme;
            }
        }

        public static bool IsDarkTheme()
        {
            bool RetVal = false;

            if (SettingsPage.MainWindow.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.RequestedTheme == ElementTheme.Dark)
                {
                    RetVal = true;
                }
            }

            return RetVal;
        }

        public static bool IsLightTheme()
        {
            bool RetVal = false;

            if (SettingsPage.MainWindow.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.RequestedTheme == ElementTheme.Light)
                {
                    RetVal = true;
                }
            }

            return RetVal;
        }
    }
}
