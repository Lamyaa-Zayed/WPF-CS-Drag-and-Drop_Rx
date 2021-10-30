using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragDrop2.DragDropViews
{
    /// <summary>
    /// Interaction logic for View2.xaml
    /// </summary>
    public partial class View2 : UserControl
    {
        // Using a DependencyProperty as the backing store for IsChildHitTestVisibleProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsChildHitTestVisibleProperty =
            DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(View2), new PropertyMetadata(true));
        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
        }

        public View2()
        {
            InitializeComponent();
        }

        private void BlueRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                IsChildHitTestVisible = false;
                DragDrop.DoDragDrop(blueRectangle, new DataObject(DataFormats.Serializable, blueRectangle), DragDropEffects.Move);
                IsChildHitTestVisible = true;
            }
        }

        private void Canvas_DragLeave(object sender, DragEventArgs e)
        {
            //if (e.OriginalSource == canvas)
            //{
            //    object data = e.Data.GetData(DataFormats.Serializable);

            //    if (data is UIElement element)
            //    {
            //        canvas.Children.Remove(element);
            //    }
            //}

            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                canvas.Children.Remove(element);
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            //object data = e.Data.GetData(DataFormats.Serializable);

            //if (data is UIElement element)
            //{
            //    Point dropPosition = e.GetPosition(canvas);
            //    Canvas.SetLeft(element, dropPosition.X);
            //    Canvas.SetTop(element, dropPosition.Y);

            //    if (!canvas.Children.Contains(element))
            //    {
            //        canvas.Children.Add(element);
            //    }
            //}
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                Point dropPosition = e.GetPosition(canvas);
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);

                if (!canvas.Children.Contains(element))
                {
                    canvas.Children.Add(element);
                }
            }

        }

    }
}
