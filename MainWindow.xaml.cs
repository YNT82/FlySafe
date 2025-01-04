using System.IO;
using System.Windows;

namespace FlySafe
{
    public partial class MainWindow : Window
    {
        private const string ConfigFilePath = "settings.cfg"; // Путь к файлу настроек

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Загружаем состояние окна "Поверх всех окон" из файла настроек
            LoadWindowState();

            // Обработчик для перетаскивания окна
            this.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
        }

        // Загружаем состояние окна "Поверх всех окон"
        private void LoadWindowState()
        {
            if (File.Exists(ConfigFilePath))
            {
                try
                {
                    // Читаем файл и ищем строку, начинающуюся с "AlwaysOnTop="
                    string[] lines = File.ReadAllLines(ConfigFilePath);
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("AlwaysOnTop="))
                        {
                            // Парсим значение после "AlwaysOnTop="
                            string value = line.Substring("AlwaysOnTop=".Length).Trim();
                            bool isAlwaysOnTop = false;

                            if (bool.TryParse(value, out isAlwaysOnTop))
                            {
                                // Если настройка включена, ставим окно поверх всех
                                this.Topmost = isAlwaysOnTop;
                            }
                            break; // Прерываем цикл после нахождения строки с настройкой
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке настроек: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Файл настроек не найден.");
            }
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
