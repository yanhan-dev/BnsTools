using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RandomStore.Views
{
    /// <summary>
    /// Interaction logic for RandomStoreView
    /// </summary>
    public partial class RandomStoreView : UserControl
    {
        public RandomStoreView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewDrop(object sender, DragEventArgs e)
        {
            object text = e.Data.GetData(DataFormats.FileDrop);
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = string.Format("{0}", ((string[])text)[0]);
            }
        }

        private void TextBox_PreviewDrop_File(object sender, DragEventArgs e)
        {
            object text = e.Data.GetData(DataFormats.FileDrop);
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                string path = ((string[])text).First();
                if (!File.Exists(path))
                {
                    return;
                }
                tb.Text = path;
            }
        }

        private void TextBox_PreviewDrop_Directory(object sender, DragEventArgs e)
        {
            object text = e.Data.GetData(DataFormats.FileDrop);
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = Path.GetDirectoryName(((string[])text).First());
            }
        }

        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }
    }
}
