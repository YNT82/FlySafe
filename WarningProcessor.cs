﻿// Ищем сообщения среди Warnings,
// ВЫКЛЮЧАЕМ кнопки и лейблы для найденных сообщений.

public class WarningProcessor
{
    // Метод для обработки сообщений и изменения цвета для секции Warnings
    public static void ProcessWarnings(List<string> searchStrings, out List<string> messages, out List<string> checkTypes, out List<string> sections)
    {
        // Вызов функции поиска
        MessageSearcher.SearchMessages(searchStrings, out messages, out checkTypes, out sections, new List<string> { "Warnings" });

        // Проверка наличия секции Warnings и изменение цвета кнопки в зависимости от секции
        // Фактически не требуется, т.к. при Reset вызывается обработка всех оставшихся в ECAM сообщений, в т.ч. перекрашивание кнопок.
        switch (sections.Contains("Warnings"))
        {
            case true:
                ButtonStyleHelper.SetWarningButtonColor("#800000");
                break;
            //case false:
                //ButtonStyleHelper.SetWarningButtonColor("#FF0000");
                //break;
        }

        // Проверка наличия разных типов и изменение цвета лейблов в зависимости от типа
        switch (checkTypes.Contains("Flight plan"))
        {
            case true:
                LabelStyleHelper.SetFuelLabelColor("#806000");
                break;
            //case false:
               // LabelStyleHelper.SetFuelLabelColor("#FF8C00");
                //break;
        }

        switch (checkTypes.Contains("Systems"))
        {
            case true:
                LabelStyleHelper.SetApuLabelColor("#806000");
                break;
            //case false:
                //LabelStyleHelper.SetApuLabelColor("#FF8C00");
                //break;
        }

        switch (checkTypes.Contains("Flight control"))
        {
            case true:
                LabelStyleHelper.SetFlightControlLabelColor("#806000");
                break;
            //case false:
                //LabelStyleHelper.SetFlightControlLabelColor("#FF8C00");
                //break;
        }

        switch (checkTypes.Contains("Weather"))
        {
            case true:
                LabelStyleHelper.SetAntiIceLabelColor("#806000");
                break;
            //case false:
                //LabelStyleHelper.SetAntiIceLabelColor("#FF8C00");
                //break;
        }
    }
}
