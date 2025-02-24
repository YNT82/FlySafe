﻿//Принимаем сообщения, ищем их секции (классы) и типы.
//Окрашиваем кнопки и лейблы типов для найденных сообщений.
//Окрашиваем сообщения в зависимости от класса и добавляем признак класса (секции).

public class MessageProcessor
{
    // Метод для обработки сообщений и изменения цвета
    public static void ProcessMessages(List<string> searchStrings, out List<string> messages, out List<string> checkTypes, out List<string> sections)
    {
        // Вызов функции поиска
        MessageSearcher.SearchMessages(searchStrings, out messages, out checkTypes, out sections, new List<string> { "Warnings", "Cautions" });

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
                //ButtonStyleHelper.SetCautionButtonColor("#FF8C00");
                ButtonStyleHelper.SetCautionButtonColor("#cccd3f");
                break;
            case false:
                //ButtonStyleHelper.SetCautionButtonColor("#806000");
                ButtonStyleHelper.SetCautionButtonColor("#9d9d87");
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

        // Окрашиваем сообщения в зависимости от секции
        for (int i = 0; i < messages.Count; i++)
        {
            string message = messages[i];
            string foundSection = "Unknown";

            // Используем MessageSearcher для поиска секции для текущего сообщения
            MessageSearcher.SearchMessages(new List<string> { message }, out var tempMessages, out var tempCheckTypes, out var tempSections, new List<string> { "Warnings", "Cautions" });

            // Определяем секцию, если она найдена
            if (tempSections.Count > 0)
            {
                foundSection = tempSections[0]; // Берем первую найденную секцию
            }

            // Обновляем сообщение с найденной секцией
            messages[i] = $"{foundSection}: {message}";
        }
    }
}
