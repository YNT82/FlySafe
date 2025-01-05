using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Timers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media.Media3D;

namespace FlySafe
{
    public partial class MainWindow : Window
    {
        private const string ConfigFilePath = "Settings.cfg"; // Путь к файлу настроек
        private System.Timers.Timer initialDelayTimer;
        private System.Timers.Timer connectionTimer;

        // Импортируем функцию для открытия SimConnect
        [DllImport("SimConnect.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int SimConnect_Open(ref IntPtr hSimConnect, string szName, IntPtr hwnd, uint dwNotifyFlags, uint dwCallback, uint dwUserData);

        // Импортируем функцию для закрытия SimConnect
        [DllImport("SimConnect.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SimConnect_Close(IntPtr hSimConnect);

        private IntPtr hSimConnect = IntPtr.Zero;

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
            Options Options = new Options();

            // Устанавливаем владельца окна, чтобы оно отображалось на переднем плане
            Options.Owner = this;

            // Получаем позицию лейбла относительно главного окна
            Point labelPosition = ECAMLabel.TransformToAncestor(this).Transform(new Point(0, 0));

            // Устанавливаем окно точно на позиции лейбла
            Options.Left = this.Left + labelPosition.X;
            Options.Top = this.Top + labelPosition.Y;

            // Показываем окно как модальное
            Options.ShowDialog();
        }

        // Таймер начальной задержки
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            initialDelayTimer = new System.Timers.Timer(5000); // Задержка в 5 секунд
            initialDelayTimer.Elapsed += StartConnectionTimer;
            initialDelayTimer.AutoReset = false; // Однократное срабатывание
            initialDelayTimer.Start();
        }
        // Первая попытка подключения

        private void StartConnectionTimer(object sender, ElapsedEventArgs e)
        {
            // Остановка таймера начальной задержки
            initialDelayTimer.Stop();
            initialDelayTimer.Dispose();

            // Настройка основного таймера на попытки подключения каждые 10 секунд
            connectionTimer = new System.Timers.Timer(10000);
            connectionTimer.Elapsed += AttemptConnection;
            connectionTimer.AutoReset = true;
            connectionTimer.Start();

            // Первая попытка подключения
            AttemptConnection(this, null);
        }

        // Повторные попытки подключения
        private void AttemptConnection(object sender, ElapsedEventArgs? e)
        {
            Dispatcher.Invoke(() =>
            {
                ConnectToSim();

                // Если соединение успешно, останавливаем основной таймер
                if (hSimConnect != IntPtr.Zero)
                {
                    connectionTimer.Stop();
                    connectionTimer.Dispose();
                }
            });
        }

        // Проверка состояния соединения
        // Восстановление соединения

        // Функция для подключения к симулятору
        private void ConnectToSim()
        {
            if (hSimConnect != IntPtr.Zero)
            {
                ConnectLabel.Content = "Ready";
                ConnectLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00C800"));
                return;
            }

            try
            {
                // Пытаемся установить соединение с SimConnect
                int result = SimConnect_Open(ref hSimConnect, "WPF SimConnect", IntPtr.Zero, 0, 0, 0);
                if (result == 0) // 0 — это успешный код подключения
                {
                    ConnectLabel.Content = "Ready";
                    ConnectLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00C800"));
                }
                else
                {
                    ConnectLabel.Content = $"Waiting for Simulator"; //{result}
                    ConnectLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8C00"));
                }
            }
            catch (Exception ex)
            {
                ConnectLabel.Content = $"Exception: {ex.Message}";
                ConnectLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
            }
        }

        // Функция закрытия соединения
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (hSimConnect != IntPtr.Zero)
            {
                SimConnect_Close(hSimConnect);
                hSimConnect = IntPtr.Zero;
            }
        }

        private void CautionBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WarningBtn_Click(object sender, RoutedEventArgs e)
        {
            // Строки для поиска
            List<string> searchStrings = new List<string>
            {
                "Wind shear",
                "Landing lights",
                "Spoilers not armed",
                "Simbrief type does not match flight type"
            };

            // Списки для результатов
            List<string> messages;
            List<string> checkTypes;
            List<string> sections;

            // Вызов функции поиска
            MessageSearcher.SearchMessages(searchStrings, out messages, out checkTypes, out sections);

            // Путь к файлу для сохранения результатов
            string resultFilePath = "result.txt";

            // Открытие/создание файла и запись результата
            using (StreamWriter writer = new StreamWriter(resultFilePath, false))
            {
                writer.WriteLine("Messages:");
                foreach (var message in messages)
                {
                    writer.WriteLine(message);
                }

                writer.WriteLine("\nCheck Types:");
                foreach (var checkType in checkTypes)
                {
                    writer.WriteLine(checkType);
                }

                writer.WriteLine("\nSections:");
                foreach (var section in sections)
                {
                    writer.WriteLine(section);
                }
            }
            // Проверка наличия разных секций и изменение цвета кнопки в зависимости от секции
            switch (sections.Contains("Warnings"))
            {
                case true:
                    ButtonStyleHelper.SetWarningButtonColor("#FF0000");
                    break;
                case false:
                    ButtonStyleHelper.SetWarningButtonColor("#800000");
                    break;
            }

            switch (sections.Contains("Cautions"))
            {
                case true:
                    ButtonStyleHelper.SetCautionButtonColor("#FF8C00");
                    break;
                case false:
                    ButtonStyleHelper.SetCautionButtonColor("#806000");
                    break;
            }

            // Проверка наличия разных типов и изменение цвета лейблов в зависимости от типа
            switch (checkTypes.Contains("Flight plan"))
            {
                case true:
                    LabelStyleHelper.SetFuelLabelColor("#FF8C00");
                    break;
                case false:
                    LabelStyleHelper.SetFuelLabelColor("#806000");
                    break;
            }

            switch (checkTypes.Contains("Systems"))
            {
                case true:
                    LabelStyleHelper.SetApuLabelColor("#FF8C00");
                    break;
                case false:
                    LabelStyleHelper.SetApuLabelColor("#806000");
                    break;
            }

            switch (checkTypes.Contains("Flight control"))
            {
                case true:
                    LabelStyleHelper.SetFlightControlLabelColor("#FF8C00");
                    break;
                case false:
                    LabelStyleHelper.SetFlightControlLabelColor("#806000");
                    break;
            }

            switch (checkTypes.Contains("Weather"))
            {
                case true:
                    LabelStyleHelper.SetAntiIceLabelColor("#FF8C00");
                    break;
                case false:
                    LabelStyleHelper.SetAntiIceLabelColor("#806000");
                    break;
            }





            // Информация об успешном завершении
            MessageBox.Show("Результаты сохранены в файл result.txt");
        }
    }
}
