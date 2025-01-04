using System.IO;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace FlySafe
{
    public partial class Options : Window
    {
        private const string ConfigFilePath = "Settings.cfg"; // Путь к файлу настроек

        public Options()
        {
            InitializeComponent();
            UpdateLabelWithVersion();
            this.Loaded += Options_Loaded;
        }

        private void Options_Loaded(object sender, RoutedEventArgs e)
        {
            // Загружаем состояние чекбокса AlwaysOnTop
            LoadCheckboxState();
        }

        // Обработчик для левого клика мыши по окну (для закрытия окна)
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Закрытие окна при клике
            this.Close();
        }

        // Обработчик для события "Чекбокс AlwaysOnTop отмечен"
        private void AlwaysOnTopCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Показываем галочку
            var checkMark = (System.Windows.Shapes.Path)AlwaysOnTopCheckBox.Template.FindName("CheckMark", AlwaysOnTopCheckBox);
            if (checkMark != null)
            {
                checkMark.Opacity = 1;  // Показываем галочку
            }

            // Сохраняем состояние чекбокса AlwaysOnTop
            SaveCheckboxState(true);
        }

        // Обработчик для события "Чекбокс AlwaysOnTop снят"
        private void AlwaysOnTopCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Скрываем галочку
            var checkMark = (System.Windows.Shapes.Path)AlwaysOnTopCheckBox.Template.FindName("CheckMark", AlwaysOnTopCheckBox);
            if (checkMark != null)
            {
                checkMark.Opacity = 0;  // Скрываем галочку
            }

            // Сохраняем состояние чекбокса AlwaysOnTop
            SaveCheckboxState(false);
        }

        // Сохранение состояния чекбокса AlwaysOnTop в файл
        private void SaveCheckboxState(bool isChecked)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ConfigFilePath))
                {
                    writer.WriteLine($"AlwaysOnTop={isChecked}");
                }

                // Сообщаем главному окну об изменении состояния
                if (Owner is MainWindow mainWindow)
                {
                    mainWindow.Topmost = isChecked;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении настроек: " + ex.Message);
            }
        }

        // Загрузка состояния чекбокса AlwaysOnTop из файла
        private void LoadCheckboxState()
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
                            bool isChecked = false;

                            if (bool.TryParse(value, out isChecked))
                            {
                                // Устанавливаем состояние чекбокса AlwaysOnTop
                                AlwaysOnTopCheckBox.IsChecked = isChecked;

                                // После того как шаблон был загружен, ищем элемент "CheckMark"
                                var checkMark = (System.Windows.Shapes.Path)AlwaysOnTopCheckBox.Template.FindName("CheckMark", AlwaysOnTopCheckBox);
                                if (checkMark != null)
                                {
                                    checkMark.Opacity = isChecked ? 1 : 0; // Отображаем или скрываем галочку
                                }
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

        // Загрузка версии из файла
        private void UpdateLabelWithVersion()
        {
            string versionFile = "Version.txt";
            string fileContent = string.Empty;

            // Проверка существования файла и чтение содержимого
            if (File.Exists(versionFile))
            {
                fileContent = File.ReadAllText(versionFile);
            }
            else
            {
                fileContent = "Not found";
            }

            // Просто объединяем строку
            string combinedText = "Version: " + fileContent;

            // Обновляем Label
            VersionLabel.Content = combinedText;

            // При необходимости можно обновить визуальное отображение
            VersionLabel.InvalidateVisual();
        }

    }
}
