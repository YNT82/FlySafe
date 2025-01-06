public class CautionProcessor
{
    // Метод для обработки сообщений и изменения цвета для секции Cautions
    public static void ProcessCautions(List<string> searchStrings, out List<string> messages, out List<string> checkTypes, out List<string> sections)
    {
        // Вызов функции поиска
        MessageSearcher.SearchMessages(searchStrings, out messages, out checkTypes, out sections, new List<string> { "Cautions"});

        // Проверка наличия секции Cautions и изменение цвета кнопки в зависимости от секции
        switch (sections.Contains("Cautions"))
        {
            case true:
                ButtonStyleHelper.SetCautionButtonColor("#806000");
                break;
            case false:
                ButtonStyleHelper.SetCautionButtonColor("#FF8C00");
                break;
        }

        // Проверка наличия разных типов и изменение цвета лейблов в зависимости от типа
        switch (checkTypes.Contains("Flight plan"))
        {
            case true:
                LabelStyleHelper.SetFuelLabelColor("#806000");
                break;
            case false:
                LabelStyleHelper.SetFuelLabelColor("#FF8C00");
                break;
        }

        switch (checkTypes.Contains("Systems"))
        {
            case true:
                LabelStyleHelper.SetApuLabelColor("#806000");
                break;
            case false:
                LabelStyleHelper.SetApuLabelColor("#FF8C00");
                break;
        }

        switch (checkTypes.Contains("Flight control"))
        {
            case true:
                LabelStyleHelper.SetFlightControlLabelColor("#806000");
                break;
            case false:
                LabelStyleHelper.SetFlightControlLabelColor("#FF8C00");
                break;
        }

        switch (checkTypes.Contains("Weather"))
        {
            case true:
                LabelStyleHelper.SetAntiIceLabelColor("#806000");
                break;
            case false:
                LabelStyleHelper.SetAntiIceLabelColor("#FF8C00");
                break;
        }
    }
}