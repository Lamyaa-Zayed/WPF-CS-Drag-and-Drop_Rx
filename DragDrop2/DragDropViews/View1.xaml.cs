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
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class View1 : UserControl
    {

        //Using a DependencyProperty as the backing store for IsChildHitTestVisibleProperty.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsChildHitTestVisibleProperty =
            DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(View1), new PropertyMetadata(true));
        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
        }

        public View1()
        {
            InitializeComponent();
            originalDropPosition = new Point(-1, -1);
        }

        private Point originalDropPosition;

        private void RedRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsChildHitTestVisible = false;

                if((originalDropPosition.X == -1) && (originalDropPosition.Y == -1))
                {
                    //originalDropPosition = e.GetPosition(redRectangle);

                    Vector vector = VisualTreeHelper.GetOffset(redRectangle);
                    originalDropPosition.X = vector.X;
                    originalDropPosition.Y = vector.Y;
                }

                if (sender is UIElement element)
                {
                    DragDrop.DoDragDrop(element, new DataObject(DataFormats.Serializable, element), DragDropEffects.Move);
                }
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
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                //Point dropPosition = e.GetPosition(canvas);
                Point actualDropPosition = e.GetPosition(bigRectangle);

                if ((actualDropPosition.X >= 0) && (actualDropPosition.Y >= 0) 
                    && ((actualDropPosition.X + element.DesiredSize.Width) <= bigRectangle.Width) 
                    && ((actualDropPosition.Y + element.DesiredSize.Height) <= bigRectangle.Height))
                {
                    //Canvas.SetLeft(element, dropPosition.X);
                    //Canvas.SetTop(element, dropPosition.Y);
                }
                else
                {
                    Canvas.SetLeft(element, originalDropPosition.X);
                    Canvas.SetTop(element, originalDropPosition.Y);
                }

                //if (!canvas.Children.Contains(element))
                //{
                //    canvas.Children.Add(element);
                //}
            }
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
