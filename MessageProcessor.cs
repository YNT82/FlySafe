//Не  используется
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

        // Окрашиваем сообщения в зависимости от секции
        for (int i = 0; i < messages.Count; i++)
        {
            string section = sections.Count > i ? sections[i] : ""; // Безопасно получаем секцию для сообщения
            string message = messages[i];

            // Применяем цвета в зависимости от секции
            if (section == "Warnings")
            {
                messages[i] = $"Warning: {message}"; // Мы добавляем текст для понимания, что это Warning
            }
            else if (section == "Cautions")
            {
                messages[i] = $"Caution: {message}"; // Аналогично для Caution
            }
            else
            {
                messages[i] = $"Info: {message}";  // Для других сообщений
            }
        }
    }
}
