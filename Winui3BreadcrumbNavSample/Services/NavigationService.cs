using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winui3BreadcrumbNavSample.Services
{
    public class NavigationService
    {
        public static NavigationView MainNavigation { get; private set; }
        public static BreadcrumbBar MainBreadcrumb { get; private set; }

        public static Frame MainFrame { get; private set; }

        public static ObservableCollection<string> BreadCrumbs = new ObservableCollection<string>();

        public static void Init(NavigationView navigationView, BreadcrumbBar breadcrumbBar, Frame frame)
        {
            MainNavigation = navigationView;
            MainBreadcrumb = breadcrumbBar;
            MainFrame = frame;
        }

        public static void UpdateBreadcrumb(string Content, bool RemovePreviousText)
        {
            if (RemovePreviousText)
            {
                BreadCrumbs.Clear();
            }

            BreadCrumbs.Add(Content);

            MainBreadcrumb.ItemsSource = BreadCrumbs;
        }

        public static void NavigateHiearchical(Type TargetPageType)
        {
            if (TargetPageType == null) { return; }

            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromRight;
            MainFrame.Navigate(TargetPageType, null, info);
        }

        public static void NavigateNormal(Type TargetPageType)
        {
            MainFrame.Navigate(TargetPageType);
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

        private void RemoveItemsFromBreadcrumbs(int index)
        {

        }

        public static void NavigationGoBack(int index)
        {
            //we need to somehow remove all items from the array 
            int indexToRemoveAfter = index;

            if (indexToRemoveAfter < BreadCrumbs.Count - 1)
            {
                int itemsToRemove = BreadCrumbs.Count - indexToRemoveAfter -1;
                for (int i = 0; i < itemsToRemove; i++)
                {
                    BreadCrumbs.RemoveAt(indexToRemoveAfter + 1);
                }
            }
            //navigate
            Type page = MainFrame.BackStack[index].SourcePageType;
            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromLeft;
            MainFrame.Navigate(page, null, info);
        }

        public static void NavigationGoBack()
        {
            MainFrame.GoBack();
        }
    }
}
