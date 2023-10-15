using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
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
using Windows.UI.Popups;
using Windows.UI.WindowManagement;

namespace Winui3BreadcrumbNavSample.Services
{
    public class NavigationService
    {
        #region Attached Properties
        public static readonly DependencyProperty IsHeaderVisibleProperty =
            DependencyProperty.RegisterAttached("IsHeaderVisible", typeof(bool), typeof(NavigationService), new PropertyMetadata(null));

        public static void SetIsHeaderVisibleProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHeaderVisibleProperty, value);
        }

        public static bool GetIsHeaderVisibleProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHeaderVisibleProperty);
        }

        public static readonly DependencyProperty ClearNavigationProperty =
            DependencyProperty.RegisterAttached("ClearNavigation", typeof(bool), typeof(NavigationService), new PropertyMetadata(null));

        public static void SetClearNavigationProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(ClearNavigationProperty, value);
        }

        public static bool GetClearNavigationProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(ClearNavigationProperty);
        }

        public static readonly DependencyProperty PageTitleProperty =
            DependencyProperty.RegisterAttached("PageTitle", typeof(string), typeof(NavigationService), new PropertyMetadata(null));

        public static void SetPageTitleProperty(DependencyObject obj, string value)
        {
            obj.SetValue(PageTitleProperty, value);
        }

        public static string GetPageTitleProperty(DependencyObject obj)
        {
            return (string)obj.GetValue(PageTitleProperty);
        }
        #endregion
        #region Classes
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
        }
        #endregion

        #region Enums
        public enum NavigateAnimationType
        {
            NoAnimation,
            Entrance,
            DrillIn,
            SlideFromLeft,
            SlideFromRight
        }
        #endregion

        #region Properties
        public static NavigationView MainNavigation { get; private set; }
        public static BreadcrumbBar MainBreadcrumb { get; private set; }

        public static Frame MainFrame { get; private set; }

        public static ObservableCollection<Breadcrumb> BreadCrumbs = new ObservableCollection<Breadcrumb>();

        public static ObservableCollection<ObservableCollection<Breadcrumb>> NavigationHistory = new ObservableCollection<ObservableCollection<Breadcrumb>>();
        #endregion

        #region Constructor
        public static void Init(NavigationView navigationView, BreadcrumbBar breadcrumbBar, Frame frame, bool EnableHistory = false)
        {
            MainNavigation = navigationView;
            MainBreadcrumb = breadcrumbBar;
            MainFrame = frame;

            BreadCrumbs = new ObservableCollection<Breadcrumb>();
        }
        #endregion

        #region Private Functions
        private static void UpdateBreadcrumb()
        {
            MainBreadcrumb.ItemsSource = BreadCrumbs;
        }

        #endregion

        #region Public Functions
        public static void Navigate(Type TargetPageType, NavigateAnimationType AnimType, object parameter = null, bool NavigatingBackwardsFromBreadcrumb = false)
        {
            //prepare all variables
            bool ClearNavigation = true;
            string PageTitle = "";
            bool IsHeaderVisible = true;

            DependencyObject obj = Activator.CreateInstance(TargetPageType) as DependencyObject;

            //get all variables
            IsHeaderVisible = GetIsHeaderVisibleProperty(obj);
            PageTitle = GetPageTitleProperty(obj);
            ClearNavigation = GetClearNavigationProperty(obj);

            //prepare navigation

            //prepare breadcrumbs
            if (ClearNavigation)
            {
                BreadCrumbs.Clear();
                MainFrame.BackStack.Clear();
            }
            BreadCrumbs.Add(new Breadcrumb(PageTitle, TargetPageType));
            UpdateBreadcrumb();

            //prepare transtions
            NavigationTransitionInfo info;
            if (!NavigatingBackwardsFromBreadcrumb)
            {
                switch (AnimType)
                {
                    case NavigateAnimationType.NoAnimation:
                        info = new SuppressNavigationTransitionInfo();
                        break;
                    case NavigateAnimationType.Entrance:
                        info = new EntranceNavigationTransitionInfo();
                        break;
                    case NavigateAnimationType.DrillIn:
                        info = new DrillInNavigationTransitionInfo();
                        break;
                    case NavigateAnimationType.SlideFromRight:
                        info = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight };
                        break;
                    case NavigateAnimationType.SlideFromLeft:
                        info = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft };
                        break;
                    default:
                        info = new EntranceNavigationTransitionInfo();
                        break;
                }
            }
            else
            {
                info = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft };
            }

            MainNavigation.AlwaysShowHeader = IsHeaderVisible;
            ChangeBreadcrumbVisibility(IsHeaderVisible);

            MainFrame.Navigate(TargetPageType, parameter, info);
        }

        public static void NavigateFromBreadcrumb(Type TargetPageType, int BreadcrumbBarIndex, bool NavigatingBackwardsFromBreadcrumb = true)
        {
            //prepare all variables
            bool ClearNavigation = true;
            bool IsHeaderVisible = true;

            DependencyObject obj = Activator.CreateInstance(TargetPageType) as DependencyObject;

            //get all variables
            IsHeaderVisible = GetIsHeaderVisibleProperty(obj);
            ClearNavigation = GetClearNavigationProperty(obj);

            //prepare navigation

            //prepare transtions

            MainNavigation.AlwaysShowHeader = IsHeaderVisible;
            ChangeBreadcrumbVisibility(IsHeaderVisible);

            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromLeft;
            MainFrame.Navigate(TargetPageType, null, info);

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
        #endregion
    }
}
