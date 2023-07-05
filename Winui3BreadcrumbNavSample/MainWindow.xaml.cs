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
        }

        private void MainNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer == Page1Item)
            {
                NavigationService.NavigateNormal(typeof(Page1));
            }
            if (args.SelectedItemContainer == Page2Item)
            {
                NavigationService.NavigateNormal(typeof(Page2));
            }
            if (args.SelectedItemContainer == Page3Item)
            {
                NavigationService.NavigateNormal(typeof(Page3));
            }
            if (args.IsSettingsSelected)
            {
                NavigationService.NavigateNormal(typeof(SettingsPage));
            }
        }

        private void MainBreadcrumb_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            NavigationService.NavigationGoBack(args.Index);
        }
    }
}
