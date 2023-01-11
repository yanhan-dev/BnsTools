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
    public  class SelectingItemAttachedProperty
    {
        public static readonly DependencyProperty SelectingItemProperty = DependencyProperty.RegisterAttached(
            "SelectingItem",
            typeof(XmlNodeViewModel),
            typeof(SelectingItemAttachedProperty),
            new PropertyMetadata(default(XmlNodeViewModel), OnSelectingItemChanged));

        public static XmlNodeViewModel GetSelectingItem(DependencyObject target)
        {
            return (XmlNodeViewModel)target.GetValue(SelectingItemProperty);
        }

        public static void SetSelectingItem(DependencyObject target, XmlNodeViewModel value)
        {
            target.SetValue(SelectingItemProperty, value);
        }

        static void OnSelectingItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid == null || grid.SelectedItem == null)
                return;

            grid.Dispatcher.InvokeAsync(() =>
            {
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.SelectedItem, null);
            });
        }
    }
}
