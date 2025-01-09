//Изменяем цвет кнопок по классам сообщений

using System.Windows.Media;
using FlySafe;

public static class ButtonStyleHelper
{
    /// <summary>
    /// Изменяет цвет текста кнопки WarningBtn.
    /// </summary>
    /// <param name="colorCode">Код цвета в формате HEX (например, "#FF0000").</param>
    public static void SetWarningButtonColor(string colorCode)
    {
        if (App.Current.MainWindow.FindName("WarningBtn") is System.Windows.Controls.Button warningButton)
        {
            warningButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorCode));
        }
    }

    /// <summary>
    /// Изменяет цвет текста кнопки CautionBtn.
    /// </summary>
    /// <param name="colorCode">Код цвета в формате HEX (например, "#FF8C00").</param>
    public static void SetCautionButtonColor(string colorCode)
    {
        if (App.Current.MainWindow.FindName("CautionBtn") is System.Windows.Controls.Button cautionButton)
        {
            cautionButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorCode));
        }
    }
}
