using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaWikiPublisher.UI
{
    /// <summary>
    /// Interaction logic for SelectorButtonBlock.xaml
    /// </summary>
    public partial class SelectorButtonBlock : UserControl
    {
        public SelectorButtonBlock()
        {
            InitializeComponent();
        }

        #region TargetProperty

        public ItemsControl Target
        {
            get { return (ItemsControl)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(
                "Target",
                typeof(ItemsControl),
                typeof(SelectorButtonBlock),
                new PropertyMetadata(default(ItemsControl), OnTargetDependencyPropertyChanged));

        private static void OnTargetDependencyPropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var sender = obj as SelectorButtonBlock;

            if (sender != null)
            {
                sender.OnTargetPropertyChanged((ItemsControl)args.OldValue, (ItemsControl)args.NewValue);
            }
        }

        protected virtual void OnTargetPropertyChanged(ItemsControl oldValue, ItemsControl newValue)
        {
        }

        #endregion

        #region IsSelectedPropertyProperty

        public string IsSelectedProperty
        {
            get { return (string)GetValue(IsSelectedPropertyProperty); }
            set { SetValue(IsSelectedPropertyProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedPropertyProperty =
            DependencyProperty.Register(
                "IsSelectedProperty",
                typeof(string),
                typeof(SelectorButtonBlock),
                new PropertyMetadata(default(string), OnIsSelectedPropertyDependencyPropertyChanged));

        private static void OnIsSelectedPropertyDependencyPropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var sender = obj as SelectorButtonBlock;

            if (sender != null)
            {
                sender.OnIsSelectedPropertyPropertyChanged((string)args.OldValue, (string)args.NewValue);
            }
        }

        protected virtual void OnIsSelectedPropertyPropertyChanged(string oldValue, string newValue)
        {
        }

        #endregion

        private void SelectAllButtonClicked(object sender, RoutedEventArgs e)
        {
            ApplySelectionValue(true);
        }

        private void SelectNoneButtonClicked(object sender, RoutedEventArgs e)
        {
            ApplySelectionValue(false);
        }

        private void ApplySelectionValue(bool isSelected)
        {
            foreach (var item in Target.Items)
            {
                var type = item.GetType();
                var property = type.GetProperty(IsSelectedProperty, BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {
                    property.SetValue(item, isSelected, null);
                }
            }
        }
    }
}
