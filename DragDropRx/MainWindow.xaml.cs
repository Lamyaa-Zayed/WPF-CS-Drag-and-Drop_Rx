using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Reactive;
using System.Reactive.Subjects;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace DragDropRx
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // Stream of mousedown events with special condition (must drag on the small rectangle)
            var mouseDownOnRect = from mouseEvt in Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseDown")
                                  let pos = mouseEvt.EventArgs.GetPosition(smallRect)
                                  where pos.X >= 0 && pos.Y >= 0 &&
                                        pos.X <= smallRect.Width && pos.Y <= smallRect.Height
                                  select mouseEvt;

            // Stream mouseup events
            var mouseUp = Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseUp");

            // Stream of mouse postion on mousemove
            var mouseMovePos = from md in Observable.FromEventPattern<MouseEventArgs>(this, "MouseMove")
                               select md.EventArgs.GetPosition(this);


            // Drag:
            var onDrag = from start in mouseDownOnRect
                         from moves in mouseMovePos.TakeUntil(mouseUp)
                               select moves;

            // Subscription and modify the postion of the smallRect
            onDrag.Subscribe(val => {
                Canvas.SetTop(smallRect, val.Y);
                Canvas.SetLeft(smallRect, val.X);
            });

            // Drop: 
            //int smallRectWidth = 100;
            //int smallRectHight = 100;
            
            mouseUp.Subscribe(val => { 
                var actualDropPosition = val.EventArgs.GetPosition(bigRect);
                var pos = val.EventArgs.GetPosition(smallRect);

                //if only mouse over small rectangle
                //if (pos.X >= 0 && pos.Y >= 0 && pos.X <= smallRect.Width && pos.Y <= smallRect.Height)
                //{   // drop on big rectangle
                if ((actualDropPosition.X >= 0) && (actualDropPosition.Y >= 0) 
                && ((actualDropPosition.X + smallRect.DesiredSize.Width) <= bigRect.Width)
                && ((actualDropPosition.Y + smallRect.DesiredSize.Height) <= bigRect.Height))
                {
                    canvas.Children.Remove(smallRect);
                    if (!canvas.Children.Contains(smallRect))
                    {
                        canvas.Children.Add(smallRect);
                    }
                }
                else //return to its old position
                {
                    Canvas.SetLeft(smallRect, 100);
                    Canvas.SetTop(smallRect, 100);
                }
                //}
            });




        }

    }
}