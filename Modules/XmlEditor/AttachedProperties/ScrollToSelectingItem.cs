using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using XmlEditor.ViewModels;

namespace XmlEditor.AttachedProperties
{
    /// <summary>
    /// DataGrid的滚动条自动滚到选中的项目
    /// </summary>
    public abstract class ScrollToSelectingItem<T>
    {
        public static readonly DependencyProperty SelectingItemProperty = DependencyProperty.RegisterAttached(
            "SelectingItem",
            typeof(T),
            typeof(ScrollToSelectingItem<T>),
            new PropertyMetadata(default(T), OnSelectingItemChanged));

        public static T GetSelectingItem(DependencyObject target)
        {
            return (T)target.GetValue(SelectingItemProperty);
        }

        public static void SetSelectingItem(DependencyObject target, T value)
        {
            target.SetValue(SelectingItemProperty, value);
        }

        static void OnSelectingItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not DataGrid grid || grid.SelectedItem == null)
            {
                return;
            }

            grid.Dispatcher.InvokeAsync(() =>
            {
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.SelectedItem, null);
            });
        }
    }
}
