//Обработчик строк, переданных в ECAM: очищаем ECAM, обрабатываем сообщения,
//убираем признак класса сообщения, добавляем возвращенные строки с переданным цветом,
//показываем или скрываем признак скролла.

using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

public class ECAMHandler
{
    public void HandleMessages(List<string> searchStrings, TextBlock ECAM, Image Arrows)
    {
        // Списки для хранения результатов
        List<string> messages;
        List<string> checkTypes;
        List<string> sections;

        // Вызов метода обработки сообщений
        MessageProcessor.ProcessMessages(searchStrings, out messages, out checkTypes, out sections);

        // Очищаем текущие элементы в ECAM
        ECAM.Inlines.Clear();

        // Убедиться, что нужно очищать строки: если нужно добавлять сообщения к существующим,
        // то очищать и добавлять пустую строку не нужно.
        // Возможно, будет обработано на уровне передачи результатов проверок в ECAM.

        // Добавляем пустую строку в начало текста
        ECAM.Inlines.Add(new LineBreak());

        // Добавляем строки из messages в TextBlock с настройкой цвета
        foreach (var message in messages)
        {
            // Создаем новый Run с текстом сообщения
            Run newRun;

            // Устанавливаем цвет в зависимости от типа сообщения
            if (message.StartsWith("Warnings"))
            {
                // Удаляем признак "Warning" из сообщения
                string cleanMessage = message.Substring("Warnings".Length).TrimStart(':', ' ');
                newRun = new Run(cleanMessage);
                newRun.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000")); // Красный для предупреждений
            }
            else if (message.StartsWith("Cautions"))
            {
                // Удаляем признак "Caution" из сообщения
                string cleanMessage = message.Substring("Cautions".Length).TrimStart(':', ' ');
                newRun = new Run(cleanMessage);
                newRun.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8C00")); // Оранжевый для предостережений
            }
            else
            {
                // Оставляем сообщение без изменений для информационных сообщений
                newRun = new Run(message);
                newRun.Foreground = new SolidColorBrush(Colors.White); // Белый для информационных сообщений
            }

            // Добавляем Run в TextBlock для отображения
            ECAM.Inlines.Add(newRun);
            ECAM.Inlines.Add(new LineBreak()); // Добавляем разрыв строки

            // Показать/скрыть картинку в зависимости от количества сообщений
            if (messages.Count > 5)
            {
                // Если сообщений больше 5, картинка будет видимой
                Arrows.Visibility = Visibility.Visible;
            }
            else
            {
                // Если сообщений 5 или меньше, картинка будет скрыта
                Arrows.Visibility = Visibility.Collapsed;
            }
        }

        // Отображение результатов (пример)
        MessageBox.Show($"Messages: {string.Join(", ", messages)}\n" +
                        $"Check Types: {string.Join(", ", checkTypes)}\n" +
                        $"Sections: {string.Join(", ", sections)}",
                        "Process Messages Result");
    }
}
