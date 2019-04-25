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

        ComboBoxItem selectedItem;

        bool isDown;
        bool isRight;
        bool isDownRectangle = true;

        int speed;
        int speedRectangleValue;


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
            Canvas.SetLeft(rectangle, 0);
            Canvas.SetTop(rectangle, 0);
            rectangle.Height = 50;
            rectangle.Width = 50;
            AnimationCanvas.Children.Add(rectangle);

            Direction.SelectedItem = defaultValue; // значение по умолчанию

        }

        private void StartAnimation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                speed = int.Parse(Speed.Text);
                speedRectangleValue = int.Parse(speedRectangle.Text);

                if ((speed > 15) || (speedRectangleValue > 15) || (speed <= 1) || (speedRectangleValue <= 1))
                {
                    MessageBox.Show("Неверная скорость у одного из элементов!", "Скорость");
                }
                else
                {
                    Speed.IsReadOnly = true;
                    speedRectangle.IsReadOnly = true;
                    StartAnimation.Visibility = Visibility.Hidden; // прячем кнопку "Start" после нажатия

                    var timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 0, 0, 2);
                    timer.Tick += Timer_tick;
                    timer.Start(); // запускаем таймер
                }
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Неверный формат ввода!","Ошибка");
            }

            switch (selectedItem.Tag.ToString())
            {
                case "1":
                    isDown = false;
                    isRight = true;
                    break;
                case "2":
                    isDown = true;
                    isRight = true;
                    break;
                case "3":
                    isDown = false;
                    isRight = false;
                    break;
                case "4":
                    isDown = true;
                    isRight = false;
                    break;
            }   
        }

        private void Timer_tick(object sender, EventArgs e)
        {

           
            if (Canvas.GetTop(rectangle) + rectangle.Height >= AnimationCanvas.ActualHeight) // если достигли нижней границы
                isDownRectangle = false;

            if (Canvas.GetTop(rectangle) + rectangle.Height <= rectangle.Height) // если достигли правой границы
                isDownRectangle = true;



            if (Canvas.GetTop(ellipse) + ellipse.Height >= AnimationCanvas.ActualHeight)
                isDown = false;

            if (Canvas.GetTop(ellipse) + ellipse.Height <= ellipse.Height) // ellipse.Height = 25
                isDown = true;

            if (Canvas.GetLeft(ellipse) + ellipse.Width >= AnimationCanvas.ActualWidth)
                isRight = false;

            if (Canvas.GetLeft(ellipse) + ellipse.Width <= ellipse.Width) // ellipse.Width = 25
                isRight = true;
            

            // Если мяч летит направо
            
            if (isRight)
                Canvas.SetLeft(ellipse, Canvas.GetLeft(ellipse) + speed);
            else
                Canvas.SetLeft(ellipse, Canvas.GetLeft(ellipse) - speed);

            // Если мяч летит вниз
            if (isDown)
                Canvas.SetTop(ellipse, Canvas.GetTop(ellipse) + speed);
            else
                Canvas.SetTop(ellipse, Canvas.GetTop(ellipse) - speed);
            
            // Если прямоугольник летит вниз
            if (isDownRectangle)
                Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) + speedRectangleValue);
            else
                Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) - speedRectangleValue);


        }

        private void Direction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            selectedItem = (ComboBoxItem)comboBox.SelectedItem;
        }
    }
}
