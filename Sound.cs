// Обработка звуков

using System;
using System.Media;
using System.Threading.Tasks;
using System.Windows;

namespace FlySafe.Helpers
{
    public static class SoundPlayerHelper
    {
        // Асинхронный метод для воспроизведения звука
        public static async Task PlaySoundAsync(string soundFilePath)
        {
            // Используем Application.GetResourceStream для доступа к ресурсу
            var uri = new Uri(soundFilePath, UriKind.RelativeOrAbsolute);  // Путь к звуковому файлу
            var stream = Application.GetResourceStream(uri)?.Stream;

            if (stream == null)
            {
                // Обработка ошибки, если звук не найден
                Console.WriteLine("Sound file not found.");
                return;
            }

            // Создаем объект SoundPlayer и передаем ему поток звука
            SoundPlayer player = new SoundPlayer(stream);

            // Воспроизведение звука в отдельном потоке, чтобы не блокировать UI
            await Task.Run(() => player.Play()); // PlaySync - синхронный метод, но выполняется в другом потоке
        }
    }
}