using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.WindowManagement;
using Winui3BreadcrumbNavSample.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winui3BreadcrumbNavSample.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public static Window MainWindow;
        bool InitFinished = false;
        public SettingsPage()
        {
            this.InitializeComponent();

            if (NavigationService.MainNavigation.PaneDisplayMode == NavigationViewPaneDisplayMode.Left)
            {
                LeftRadio.IsChecked = true;
            }
            else if (NavigationService.MainNavigation.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                TopRadio.IsChecked = true;
            }

            if (ThemeService.IsDarkTheme())
            {
                DarkRadio.IsChecked = true;
            }
            else if (ThemeService.IsLightTheme())
            {
                LightRadio.IsChecked = true;
            }

            InitFinished = true;
        }

        private void LeftRadio_Checked(object sender, RoutedEventArgs e)
        {
            if(!InitFinished) { return; }
            NavigationService.MainNavigation.PaneDisplayMode = NavigationViewPaneDisplayMode.Left;
        }

        private void TopRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (!InitFinished) { return; }
            NavigationService.MainNavigation.PaneDisplayMode = NavigationViewPaneDisplayMode.Top;
        }

        private void BaseRadio_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.SystemBackdrop = null;
            (MainWindow.Content as Grid).Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            MicaBackdrop micaBackdrop = new MicaBackdrop();
            micaBackdrop.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base;

            MainWindow.SystemBackdrop = micaBackdrop;
        }

        private void BaseAltRadio_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.SystemBackdrop = null;
            (MainWindow.Content as Grid).Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            MicaBackdrop micaBackdrop = new MicaBackdrop();
            micaBackdrop.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt;

            MainWindow.SystemBackdrop = micaBackdrop;
        }

        private void AcrylicRadio_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.SystemBackdrop = null;
            (MainWindow.Content as Grid).Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            DesktopAcrylicBackdrop backdrop = new DesktopAcrylicBackdrop();

            MainWindow.SystemBackdrop = backdrop;
        }

        private void NoneRadio_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.SystemBackdrop = null;
            (MainWindow.Content as Grid).Background = App.Current.Resources["ApplicationPageBackgroundThemeBrush"] as SolidColorBrush;
        }

        private void DarkRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (!InitFinished)
            {
                return;
            }
            ThemeService.ChangeTheme(ElementTheme.Dark);
        }

        private void LightRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (!InitFinished)
            {
                return;
            }
            ThemeService.ChangeTheme(ElementTheme.Light);
        }
    }
}
