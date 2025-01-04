using System.Windows;

namespace FlySafe
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Устанавливаем начальную позицию окна в центре экрана
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        // Обработчик для перетаскивания окна
        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();  // Перетаскивание окна
            }
        }

        // Обработчик правой кнопки мыши на Label ECAM MESSAGE TEST
        private void ECAMLabel_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Создаем и показываем новое окно
            NewWindow newWindow = new NewWindow();

            // Устанавливаем владельца окна, чтобы оно отображалось на переднем плане
            newWindow.Owner = this;

            // Получаем позицию лейбла относительно главного окна
            Point labelPosition = ECAMLabel.TransformToAncestor(this).Transform(new Point(0, 0));

            // Устанавливаем окно точно на позиции лейбла
            newWindow.Left = this.Left + labelPosition.X;
            newWindow.Top = this.Top + labelPosition.Y;

            // Показываем окно как модальное
            newWindow.ShowDialog();
        }
    }
}