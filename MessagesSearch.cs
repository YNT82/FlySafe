//В файле Json ищем сообщения с их типами и классами (секциями), учитываем уникальность всех элементов.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class MessageSearcher
{
    // Функция для поиска сообщений в JSON файле с параметром для указания секций
    public static void SearchMessages(List<string> searchStrings, out List<string> messages, out List<string> checkTypes, out List<string> sections, List<string>? sectionsToSearch = null)
    {
        // Инициализация списков
        messages = new List<string>();
        checkTypes = new List<string>();
        sections = new List<string>();

        // Загружаем данные из файла Messages.json
        var jsonData = File.ReadAllText("Messages.json");  // Убедитесь, что файл в правильном месте
        var data = JsonConvert.DeserializeObject<MessagesData>(jsonData);

        // Проверяем, что объект data не равен null
        if (data != null)
        {
            // Проходим по всем строкам для поиска
            foreach (var searchString in searchStrings)
            {
                // Если секция Cautions указана или секция не указана, ищем в Cautions
                if (sectionsToSearch == null || sectionsToSearch.Contains("Cautions", StringComparer.OrdinalIgnoreCase))
                {
                    if (data.Cautions != null)
                    {
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
                    }
                }

                // Если секция Warnings указана или секция не указана, ищем в Warnings
                if (sectionsToSearch == null || sectionsToSearch.Contains("Warnings", StringComparer.OrdinalIgnoreCase))
                {
                    if (data.Warnings != null)
                    {
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
                }
            }

            // Убираем дубликаты в списках, если они есть
            RemoveDuplicates(ref messages);
            RemoveDuplicates(ref checkTypes);
            RemoveDuplicates(ref sections);
        }
        else
        {
            // Если data равно null, выбрасываем исключение или выполняем соответствующие действия
            throw new InvalidOperationException("Data is null. Unable to search messages.");
        }
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

    // Конструктор для инициализации обязательных полей
    public Message(string messageContent, string checkType)
    {
        MessageContent = messageContent ?? throw new ArgumentNullException(nameof(messageContent));
        CheckType = checkType ?? throw new ArgumentNullException(nameof(checkType));
    }
}

public class MessagesData
{
    [JsonProperty("Cautions")]
    public List<Message> Cautions { get; set; } = new List<Message>();  // Инициализация по умолчанию

    [JsonProperty("Warnings")]
    public List<Message> Warnings { get; set; } = new List<Message>();  // Инициализация по умолчанию

    // Конструктор для инициализации обязательных полей
    public MessagesData()
    {
        Cautions = Cautions ?? new List<Message>();  // Инициализация по умолчанию
        Warnings = Warnings ?? new List<Message>();  // Инициализация по умолчанию
    }
}
