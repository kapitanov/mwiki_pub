using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MediaWikiPublisher.UI
{
    public static class MenuMerger
    {
        public static readonly DependencyProperty MergeTargetProperty =
            DependencyProperty.RegisterAttached("MergeTarget", typeof(string), typeof(MenuMerger), new PropertyMetadata(default(string), MergeTargetChanged));

        public static void SetMergeTarget(MenuItem element, string value)
        {
            element.SetValue(MergeTargetProperty, value);
        }

        public static string GetMergeTarget(MenuItem element)
        {
            return (string)element.GetValue(MergeTargetProperty);
        }

        private static void MergeTargetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var menu = (MenuItem)obj;
            ((INotifyCollectionChanged)menu.Items).CollectionChanged += (_, e) => ApplyVisibility(menu);
            ApplyVisibility(menu);
        }

        private static void ApplyVisibility(MenuItem item)
        {
            item.Visibility = item.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public static readonly DependencyProperty MergeWithProperty =
            DependencyProperty.RegisterAttached("MergeWith", typeof(string), typeof(MenuMerger), new PropertyMetadata(default(string), MergeWithChanged));

        public static void SetMergeWith(Menu element, string value)
        {
            element.SetValue(MergeWithProperty, value);
        }


        private static void MergeWithChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var menu = (Menu)obj;
            var target = (string)args.NewValue;
            menu.Loaded += (_, e) => MergeToTarget(menu, target);
        }

        public static string GetMergeWith(Menu element)
        {
            return (string)element.GetValue(MergeWithProperty);
        }

        private static void MergeToTarget(Menu menu, string target)
        {
            var root = FindRoot(menu);
            var mergeTarget = FindChild<MenuItem>(root, m => GetMergeTarget(m) == target);
            if (mergeTarget != null)
            {
                var panel = ((Panel)menu.Parent);
                panel.Children.Remove(menu);

                var items = menu.Items.Cast<MenuItem>().ToList();
                menu.Items.Clear();

                foreach (var item in items)
                {
                    mergeTarget.Items.Add(item);
                }

                panel.Unloaded += (_, e) =>
                {
                    foreach (var item in items)
                    {
                        mergeTarget.Items.Remove(item);
                    }
                };
            }
        }


        private static DependencyObject FindRoot(DependencyObject child)
        {
            if (child == null)
            {
                return null;
            }

            var root = FindRoot(VisualTreeHelper.GetParent(child));
            if (root == null)
            {
                return child;
            }

            return root;
        }

        private static T FindChild<T>(DependencyObject child, Predicate<T> predicate)
                  where T : DependencyObject
        {
            var t = child as T;
            if (t != null)
            {
                if (predicate(t))
                {
                    return t;
                }
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(child); i++)
            {
                t =FindChild<T>(VisualTreeHelper.GetChild(child, i), predicate); 
                if(t != null )
                {
                    return t;
                }
            }

            return null;
        }
    }
}
