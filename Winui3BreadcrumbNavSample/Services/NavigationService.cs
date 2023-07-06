using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;

namespace Winui3BreadcrumbNavSample.Services
{
    public class NavigationService
    {
        //thanks to https://github.com/microsoft/devhome/blob/main/settings/DevHome.Settings/Models/Breadcrumb.cs#L10
        public class Breadcrumb
        {
            public Breadcrumb(string label, Type page)
            {
                Label = label;
                Page = page;
            }
            public string Label { get; }

            public Type Page { get; }

            public override string ToString() => Label;

            public void NavigateToFromBreadcrumb(int BreadcrumbItemIndex)
            {
                NavigationService.NavigateInternal(Page, BreadcrumbItemIndex);
            }
        }
        public static NavigationView MainNavigation { get; private set; }
        public static BreadcrumbBar MainBreadcrumb { get; private set; }

        public static Frame MainFrame { get; private set; }

        public static ObservableCollection<Breadcrumb> BreadCrumbs = new ObservableCollection<Breadcrumb>();

        public static void Init(NavigationView navigationView, BreadcrumbBar breadcrumbBar, Frame frame)
        {
            MainNavigation = navigationView;
            MainBreadcrumb = breadcrumbBar;
            MainFrame = frame;
        }

        private static void UpdateBreadcrumb()
        {
            MainBreadcrumb.ItemsSource = BreadCrumbs;
        }

        public static void Navigate(Type TargetPageType, string BreadcrumbItemLabel, bool bClearPrevious)
        {
            if (bClearPrevious)
            {
                BreadCrumbs.Clear();
                MainFrame.BackStack.Clear();
            }
            BreadCrumbs.Add(new Breadcrumb(BreadcrumbItemLabel, TargetPageType));
            UpdateBreadcrumb();

            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromRight;
            MainFrame.Navigate(TargetPageType, null, info);
        }

        private static void NavigateInternal(Type page, int BreadcrumbBarIndex)
        {
            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromLeft;
            MainFrame.Navigate(page, null, info);

            int indexToRemoveAfter = BreadcrumbBarIndex;

            if (indexToRemoveAfter < BreadCrumbs.Count - 1)
            {
                int itemsToRemove = BreadCrumbs.Count - indexToRemoveAfter - 1;

                for (int i = 0; i < itemsToRemove; i++)
                {
                    BreadCrumbs.RemoveAt(indexToRemoveAfter + 1);
                }
            }
        }

        public static void ChangeBreadcrumbVisibility(bool IsBreadcrumbVisible)
        {
            if (IsBreadcrumbVisible)
            {
                MainBreadcrumb.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            }
            else
            {
                MainBreadcrumb.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            }
        }

        public static void NavigationGoBack(int index)
        {
            //THIS DOES NOT WORK!!
            //we need to somehow remove all items from the array 
            int indexToRemoveAfter = index;

            if (indexToRemoveAfter < BreadCrumbs.Count - 1)
            {
                int itemsToRemove = BreadCrumbs.Count - indexToRemoveAfter -1;

                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = MainFrame.XamlRoot;
                dialog.Content = itemsToRemove;
                dialog.Title = MainFrame.BackStack.Count.ToString();
                dialog.CloseButtonText = "ok";
                dialog.ShowAsync();

                for (int i = 0; i < itemsToRemove; i++)
                {
                    BreadCrumbs.RemoveAt(indexToRemoveAfter + 1);
                    try
                    {
                        MainFrame.BackStack.RemoveAt(indexToRemoveAfter +2);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }
            //navigate
            Type page = MainFrame.BackStack[index].SourcePageType;
            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromLeft;
            MainFrame.GoBack();
        }

        public static void NavigationGoBack()
        {
            MainFrame.GoBack();
        }
    }
}
