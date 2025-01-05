using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class MessageSearcher
{
    // Функция для поиска сообщений в JSON файле
    public static void SearchMessages(List<string> searchStrings, out List<string> messages, out List<string> checkTypes, out List<string> sections)
    {
        // Инициализация списков
        messages = new List<string>();
        checkTypes = new List<string>();
        sections = new List<string>();

        // Загружаем данные из файла Messages.json
        var jsonData = File.ReadAllText("Messages.json");  // Убедитесь, что файл в правильном месте
        var data = JsonConvert.DeserializeObject<MessagesData>(jsonData);

        // Проходим по всем строкам для поиска
        foreach (var searchString in searchStrings)
        {
            // Поиск по разделу Cautions
            foreach (var caution in data.Cautions)
            {
                if (caution.MessageContent != null && caution.MessageContent.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                {
                    // Добавляем в списки только уникальные значения
                    if (!messages.Contains(caution.MessageContent))
                    {
                        messages.Add(caution.MessageContent);
                        checkTypes.Add(caution.CheckType);
                        sections.Add("Cautions");
                    }
                }
            }

            // Поиск по разделу Warnings
            foreach (var warning in data.Warnings)
            {
                if (warning.MessageContent != null && warning.MessageContent.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                {
                    // Добавляем в списки только уникальные значения
                    if (!messages.Contains(warning.MessageContent))
                    {
                        messages.Add(warning.MessageContent);
                        checkTypes.Add(warning.CheckType);
                        sections.Add("Warnings");
                    }
                }
            }
        }

        // Убираем дубликаты в списках, если они есть
        RemoveDuplicates(ref messages);
        RemoveDuplicates(ref checkTypes);
        RemoveDuplicates(ref sections);
    }

    // Вспомогательная функция для удаления дубликатов
    private static void RemoveDuplicates(ref List<string> list)
    {
        var uniqueList = new List<string>();
        foreach (var item in list)
        {
            if (!uniqueList.Contains(item))
            {
                uniqueList.Add(item);
            }
        }
        list = uniqueList;
    }
}

// Классы для десериализации данных из JSON
public class Message
{
    [JsonProperty("message")]
    public string MessageContent { get; set; }

    [JsonProperty("check_type")]
    public string CheckType { get; set; }
}

public class MessagesData
{
    [JsonProperty("Cautions")]
    public List<Message> Cautions { get; set; }

    [JsonProperty("Warnings")]
    public List<Message> Warnings { get; set; }
}
