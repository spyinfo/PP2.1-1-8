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
using System.Windows.Threading;

namespace PP2._1_1_8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ellipse ellipse = new Ellipse();
        Rectangle rectangle = new Rectangle();

        bool isDown = true;
        bool isRight = true;

        bool isDownRectangle = true;

        public MainWindow()
        {
            InitializeComponent();

            ellipse.Width = 25;
            ellipse.Height = 25;
            Canvas.SetLeft(ellipse, 343);
            Canvas.SetTop(ellipse, 156);
            ellipse.Fill = Brushes.AntiqueWhite;
            ellipse.Stroke = Brushes.AntiqueWhite;
            AnimationCanvas.Children.Add(ellipse);


            rectangle.Stroke = Brushes.Blue;
            rectangle.Fill = Brushes.Blue;
            Canvas.SetLeft(rectangle, 634);
            Canvas.SetTop(rectangle, 10);
            rectangle.Height = 50;
            rectangle.Width = 50;
            AnimationCanvas.Children.Add(rectangle);

        }

        private void StartAnimation_Click(object sender, RoutedEventArgs e)
        {

            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            timer.Tick += Timer_tick;
            timer.Start();
        }

        private void Timer_tick(object sender, EventArgs e)
        {
            int speed = int.Parse(Speed.Text);
            Speed.IsReadOnly = true;
            if (Canvas.GetTop(rectangle) + rectangle.Height >= AnimationCanvas.ActualHeight)
                isDownRectangle = false;

            if (Canvas.GetTop(rectangle) + rectangle.Height <= rectangle.Height)
                isDownRectangle = true;



            if ((Canvas.GetTop(ellipse) + ellipse.Height >= AnimationCanvas.ActualHeight))
                isDown = false;

            if (Canvas.GetTop(ellipse) + ellipse.Height <= ellipse.Height) // ellipse.Height = 25
                isDown = true;

            if (Canvas.GetLeft(ellipse) + ellipse.Width >= AnimationCanvas.ActualWidth)
                isRight = false;

            if (Canvas.GetLeft(ellipse) + ellipse.Width <= ellipse.Width) // ellipse.Width = 25
                isRight = true;



            // Если шар летит направо
            if (isRight)
                Canvas.SetLeft(ellipse, Canvas.GetLeft(ellipse) + speed);
            else
                Canvas.SetLeft(ellipse, Canvas.GetLeft(ellipse) - speed);

            // Если шар летит вниз
            if (isDown)
                Canvas.SetTop(ellipse, Canvas.GetTop(ellipse) + speed);
            else
                Canvas.SetTop(ellipse, Canvas.GetTop(ellipse) - speed);
            
            // Если прямоугольник летит вниз
            if (isDownRectangle)
                Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) + 3);
            else
                Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) - 3);


        }
    }
}
