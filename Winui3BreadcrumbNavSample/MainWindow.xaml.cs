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
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Winui3BreadcrumbNavSample.Assets;
using Winui3BreadcrumbNavSample.Pages;
using Winui3BreadcrumbNavSample.Services;
using static Winui3BreadcrumbNavSample.Services.NavigationService;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winui3BreadcrumbNavSample
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            //set a custom titlebar
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            //initialize navigation
            NavigationService.Init(MainNavigation, MainBreadcrumb, MainFrame);

            SettingsPage.MainWindow = this;
        }

        private void MainNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            MainFrame.BackStack.Clear();
            if (args.SelectedItemContainer == Page1Item)
            {
                NavigationService.Navigate(typeof(Page1), NavigateAnimationType.Entrance);
            }
            if (args.SelectedItemContainer == Page2Item)
            {
                NavigationService.Navigate(typeof(Page2), NavigateAnimationType.Entrance);
            }
            if (args.SelectedItemContainer == Page3Item)
            {
                NavigationService.Navigate(typeof(Page3), NavigateAnimationType.Entrance);
            }
            if (args.IsSettingsSelected)
            {
                NavigationService.Navigate(typeof(SettingsPage), NavigateAnimationType.Entrance);
            }
        }

        private void MainBreadcrumb_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            if (args.Index < NavigationService.BreadCrumbs.Count - 1)
            {
                var crumb = (Breadcrumb)args.Item;
                NavigationService.NavigateFromBreadcrumb(crumb.Page, args.Index); 
            }
        }

        private void MainNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            MainNavigation.SelectedItem = Page1Item;
        }
    }
}
