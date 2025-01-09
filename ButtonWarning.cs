//По клику забираем строки из ECAM, 
//
//Удаляем найденные Cautions, добавляем обратно оставшиеся строки,
//Обрабатываем строки, приводим цвета кнопок, строк, лейблов в соответствие с классами и типами.

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

public class ButtonWarning
{
    private TextBlock ECAM;
    private Image Arrows;

    public ButtonWarning(TextBlock ecam, Image arrows)
    {
        ECAM = ecam;
        Arrows = arrows;
    }

    public void HandleWarningButtonClick()
    {
        // Строки для поиска
        // Получаем все элементы в TextBlock (они являются Run-ами)
        var runs = ECAM.Inlines.OfType<Run>().ToList();

        // Составляем список строк, извлекая текст из каждого элемента
        List<string> searchStrings = new List<string>();
        foreach (var run in runs)
        {
            searchStrings.Add(run.Text);
        }

        // Списки для результатов
        List<string> messages;
        List<string> checkTypes;
        List<string> sections;

        // Вызов функции обработки сообщений и изменения цветов
        WarningProcessor.ProcessWarnings(searchStrings, out messages, out checkTypes, out sections);

        // Фильтруем строки, чтобы удалить те, которые содержатся в messages
        var filteredRuns = runs
            .Where(run => !messages.Any(message =>
                message.Trim().Equals(run.Text.Trim(), StringComparison.OrdinalIgnoreCase))) // Убираем лишние пробелы и игнорируем регистр
            .ToList();

        // Очищаем текущие элементы в ECAM
        ECAM.Inlines.Clear();

        // Добавляем пустую строку в начало текста
        ECAM.Inlines.Add(new LineBreak());

        // Добавляем оставшиеся строки обратно в TextBlock с сохранением цвета
        foreach (var run in filteredRuns)
        {
            // Создаем новый Run для каждой строки с тем же цветом и текстом
            var newRun = new Run(run.Text) { Foreground = run.Foreground };
            ECAM.Inlines.Add(newRun);
            ECAM.Inlines.Add(new LineBreak());  // Добавляем разрыв строки
        }

        // Заново получаем строки из ECAM
        var updatedRuns = ECAM.Inlines.OfType<Run>().ToList();
        List<string> updatedSearchStrings = new List<string>();
        foreach (var run in updatedRuns)
        {
            updatedSearchStrings.Add(run.Text);
        }

        // Вызов MessageProcessor для обработки сообщений с новыми строками
        MessageProcessor.ProcessMessages(updatedSearchStrings, out messages, out checkTypes, out sections);

        // Путь к файлу для сохранения результатов
        string resultFilePath = "WarningsResult.txt";

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
        // Информация об успешном завершении
        MessageBox.Show("Результаты сохранены в файл WarningsResult.txt");

        // Проверка количества непустых строк
        int nonEmptyLinesCount = filteredRuns.Count(run => !string.IsNullOrWhiteSpace(run.Text));

        // Скрыть/показать картинку в зависимости от количества строк
        if (nonEmptyLinesCount > 5)
        {
            Arrows.Visibility = Visibility.Visible;  // Сделать картинку видимой
        }
        else
        {
            Arrows.Visibility = Visibility.Collapsed;  // Сделать картинку невидимой
        }
    }
}