/*
  Практическая работа №2 по Прикладному программированию
  Вариант 1-8.  

  Пользователь выбирает начальную скорость мяча, направление движения мяча (на бильярдном столе),  а программа 
  показывает замедленный «мультфильм» того, как мяч летит с учетом упрогого столкновения с границами стола, 
  а также с движущимся параллелепипедом (указывается левый верхний и правый нижний угол параллелепипеда, 
  его скорость). Параллелепипед движется сверху вниз, а, после достижения границы стола, снизу вверх и так далее.
  
  Apr. 2019.
 */



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
        private const byte maxSpeed = 15; // максимальная скорость любого объекта на поле
        private const byte minSpeed = 1; // минимальная скорость любого объекта на поле

        Ellipse ellipse = new Ellipse(); // эллипс
        Rectangle rectangle = new Rectangle(); // прямоугольник
        ComboBoxItem selectedItem; // текущий выбранный элемент ComboBox. По умолчанию = "Правый верхний угол"

        bool isDown; // летит ли мяч вниз
        bool isRight; // летит ли мяч вправо
        bool isDownRectangle = true; // летит ли прямоугольник вниз

        int speed; // скорость мяча
        int speedRectangleValue; // скорость прямоугольника
        int weightEllipse; // масса мяча
        int weightRectangle; // масса прямоугольника
        delegate void DelegateSet(UIElement element, double length); // для определения Canvas.SetLeft / Canvas.SetTop
        delegate double DelegateGet(UIElement element); // для определения Canvas.GetLeft / Canvas.GetTop


        public MainWindow()
        {
            InitializeComponent();

            // создаем эллипс
            ellipse.Width = 25;
            ellipse.Height = 25;
            Canvas.SetLeft(ellipse, 358);
            Canvas.SetTop(ellipse, 160);
            ellipse.Fill = Brushes.AntiqueWhite;
            ellipse.Stroke = Brushes.AntiqueWhite;
            AnimationCanvas.Children.Add(ellipse);

            Direction.SelectedItem = defaultValue; // значение по умолчанию для ComboBox

        }

        /// <summary>
        /// Кнопка "Start"
        /// </summary>
        private void StartAnimation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                speed = int.Parse(Speed.Text);
                speedRectangleValue = int.Parse(speedRectangle.Text);
                weightEllipse = int.Parse(WeightEllipse.Text);
                weightRectangle= int.Parse(WeightRectangle.Text);

                var X1 = int.Parse(x1.Text);
                var Y1 = int.Parse(y1.Text);
                var X2 = int.Parse(x2.Text);
                var Y2 = int.Parse(y2.Text);
            
                if ((speed > maxSpeed) || (speedRectangleValue > maxSpeed) || (speed < minSpeed) || (speedRectangleValue < minSpeed))
                    MessageBox.Show("Неверная скорость у одного из элементов!", "Скорость");

                else if ((X1 < AnimationCanvas.MinWidth) || (Y1 < AnimationCanvas.MinHeight) || (X2 < AnimationCanvas.MinWidth) || (Y2 < AnimationCanvas.MinHeight) || 
                        (X2 > AnimationCanvas.Width) || (Y2 > AnimationCanvas.Height) || (X1 > AnimationCanvas.Width) || (Y1 > AnimationCanvas.Height))
                    MessageBox.Show("Неверные координаты прямоугольника!", "Координаты");                

                else
                {
                    rectangle.Stroke = Brushes.Tomato;
                    rectangle.Fill = Brushes.Tomato;
                    Canvas.SetLeft(rectangle, X1);
                    Canvas.SetTop(rectangle, Y1);
                    rectangle.Width = X2 - X1;
                    rectangle.Height = Y2 - Y1;
                    AnimationCanvas.Children.Add(rectangle);

                    HideTheTips(); // прячем подсказки
                    // Все элементы TextBox делаем неизменяемыми
                    x1.IsReadOnly = true; 
                    y1.IsReadOnly = true;
                    x2.IsReadOnly = true;
                    y2.IsReadOnly = true;
                    Speed.IsReadOnly = true;
                    speedRectangle.IsReadOnly = true;
                    StartAnimation.Visibility = Visibility.Hidden; // прячем кнопку "Start" после нажатия

                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                    timer.Tick += Timer_tick;
                    timer.Start(); // запускаем таймер
                }
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Неверный формат ввода!"
                    + Environment.NewLine  
                    + Environment.NewLine
                    + "Пожалуйста, заполните все поля и"
                    + Environment.NewLine
                    + "проверьте корректность данных!"
                    ,"Ошибка");
            }

            switch (selectedItem.Tag.ToString())
            {
                case "1": // Правый верхний угол
                    isDown = false;
                    isRight = true;
                    break;
                     
                case "2": // Правый нижний угол
                    isDown = true;
                    isRight = true;
                    break;

                case "3": // Левый верхний угол
                    isDown = false;
                    isRight = false;
                    break;

                case "4": // Левый нижний угол
                    isDown = true;
                    isRight = false;
                    break;
            }
        }

        /// <summary>
        /// Момент времени
        /// </summary>
        private void Timer_tick(object sender, EventArgs e)
        {
            if (Canvas.GetTop(rectangle) + rectangle.Height >= AnimationCanvas.ActualHeight) // если прямоугольник достиг нижней границы
                isDownRectangle = false;

            if (Canvas.GetTop(rectangle) + rectangle.Height <= rectangle.Height) // если прямоугольник достиг верхней границы
                isDownRectangle = true;


            if (Canvas.GetTop(ellipse) + ellipse.Height >= AnimationCanvas.ActualHeight) // если эллипс достиг нижней границы
                isDown = false;

            if (Canvas.GetTop(ellipse) + ellipse.Height <= ellipse.Height) // ellipse.Height = 25. Если эллипс достиг верхней границы
                isDown = true;

            if (Canvas.GetLeft(ellipse) + ellipse.Width >= AnimationCanvas.ActualWidth) // если эллипс достиг правой границы
                isRight = false;

            if (Canvas.GetLeft(ellipse) + ellipse.Width <= ellipse.Width) // ellipse.Width = 25. Если эллипс достиг левой границы
                isRight = true;


            /// Столкновения с прямоугольником


            // Снизу
            if ((Canvas.GetTop(ellipse) <= Canvas.GetTop(rectangle) + rectangle.Height) &&
                    (Canvas.GetTop(ellipse) >= Canvas.GetTop(rectangle) - ellipse.Height + rectangle.Height) &&
                    (Canvas.GetLeft(ellipse) >= Canvas.GetLeft(rectangle) - (ellipse.Width / 2)) &&
                    (Canvas.GetLeft(ellipse) + ellipse.Width <= Canvas.GetLeft(rectangle) + rectangle.ActualWidth + ellipse.Width))
            {
                isDown = true;
                MessageBox.Show("Снизу");
            }


            // Сверху
            if (Canvas.GetTop(ellipse) + ellipse.Width >= Canvas.GetTop(rectangle) &&
                    (Canvas.GetTop(ellipse) <= Canvas.GetTop(rectangle)) &&
                    (Canvas.GetLeft(ellipse) >= Canvas.GetLeft(rectangle) - (ellipse.Width / 2)) &&
                    (Canvas.GetLeft(ellipse) + ellipse.Width <= Canvas.GetLeft(rectangle) + rectangle.ActualWidth + ellipse.Width))
            {
          
                isDown = false;
                MessageBox.Show("Сверху");
            }


            // Слева
            if ((Canvas.GetLeft(ellipse) + ellipse.Width >= (Canvas.GetLeft(rectangle))) &&
                    (Canvas.GetLeft(ellipse) + ellipse.Width <= Canvas.GetLeft(rectangle) + ellipse.Width) &&
                    (Canvas.GetTop(ellipse) + (ellipse.Height / 2) >= Canvas.GetTop(rectangle)) &&
                    (Canvas.GetTop(ellipse) <= Canvas.GetTop(rectangle) + rectangle.ActualHeight))
            {
                isRight = false;
                MessageBox.Show("Слева");
            }


            // Справа
            if ((Canvas.GetLeft(ellipse) <= (Canvas.GetLeft(rectangle) + rectangle.ActualWidth)) &&
                    (Canvas.GetLeft(ellipse) > (Canvas.GetLeft(rectangle) - ellipse.Width + rectangle.ActualWidth)) &&
                    (Canvas.GetTop(ellipse) + ellipse.Width >= Canvas.GetTop(rectangle)) &&
                    (Canvas.GetTop(ellipse) + ellipse.Height <= Canvas.GetTop(rectangle) + rectangle.ActualHeight + ellipse.Height))
            {
                isRight = true;
                MessageBox.Show("Справа");
            }
               



            DelegateSet delegateSetLeft = Canvas.SetLeft;
            DelegateSet delegateSetTop = Canvas.SetTop;

            DelegateGet delegateGetLeft = Canvas.GetLeft;
            DelegateGet delegateGetTop = Canvas.GetTop;

            // Если мяч летит вправо
            isOnDirection(isRight, speed, delegateSetLeft, delegateGetLeft, ellipse);

            // Если мяч летит вниз
            isOnDirection(isDown, speed, delegateSetTop, delegateGetTop, ellipse);

            // Если прямоугольник летит вниз
            if (isDownRectangle)
                Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) + speedRectangleValue);
            else
                Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) - speedRectangleValue);
        }



        /// <summary>
        /// Изменяет направление мяча в зависимости от "isDirection"
        /// </summary>
        /// <param name="isDirection">Направление мяча</param>
        /// <param name="speed">Скорость мяча</param>
        /// <param name="delegateSetLeftTop">Определяем SetLeft или SetTop</param>
        /// <param name="delegateGetLeftTop">Определяем GetLeft или GetTop</param>
        /// <param name="ellipse">ellipse</param>
        private void isOnDirection(bool isDirection, int speed, DelegateSet delegateSetLeftTop, DelegateGet delegateGetLeftTop, Ellipse ellipse)
        {
            if (isDirection)
                delegateSetLeftTop(ellipse, delegateGetLeftTop(ellipse) + speed);
            else
                delegateSetLeftTop(ellipse, delegateGetLeftTop(ellipse) - speed);
        }


        /// <summary>
        /// Убираем подсказки
        /// </summary>
        private void HideTheTips()
        {
            Tip1.Visibility = Visibility.Hidden;
            Tip2.Visibility = Visibility.Hidden;
            Tip3.Visibility = Visibility.Hidden;
            Tip4.Visibility = Visibility.Hidden;
            Tip5.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Элемент ComboBox
        /// </summary>
        private void Direction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            selectedItem = (ComboBoxItem)comboBox.SelectedItem;
        }
    }
}
