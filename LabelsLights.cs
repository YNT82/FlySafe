// Изменяем цвет лейблов по типам сообщений

using System.Windows.Media;
using FlySafe;

public static class LabelStyleHelper
{
    /// <summary>
    /// Изменяет цвет текста лейбла FLT_CNT.
    /// </summary>
    /// <param name="colorCode">Код цвета в формате HEX (например, "#FF8C00").</param>
    public static void SetFlightControlLabelColor(string colorCode)
    {
        if (App.Current.MainWindow.FindName("FLT_CNT") is System.Windows.Controls.Label flightControlLabel)
        {
            flightControlLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorCode));
        }
    }

    /// <summary>
    /// Изменяет цвет текста лейбла FUEL.
    /// </summary>
    /// <param name="colorCode">Код цвета в формате HEX (например, "#FF8C00").</param>
    public static void SetFuelLabelColor(string colorCode)
    {
        if (App.Current.MainWindow.FindName("FUEL") is System.Windows.Controls.Label fuelLabel)
        {
            fuelLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorCode));
        }
    }

    /// <summary>
    /// Изменяет цвет текста лейбла ANTI_ICE.
    /// </summary>
    /// <param name="colorCode">Код цвета в формате HEX (например, "#FF8C00").</param>
    public static void SetAntiIceLabelColor(string colorCode)
    {
        if (App.Current.MainWindow.FindName("ANTI_ICE") is System.Windows.Controls.Label antiIceLabel)
        {
            antiIceLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorCode));
        }
    }

    /// <summary>
    /// Изменяет цвет текста лейбла APU.
    /// </summary>
    /// <param name="colorCode">Код цвета в формате HEX (например, "#FF8C00").</param>
    public static void SetApuLabelColor(string colorCode)
    {
        if (App.Current.MainWindow.FindName("APU") is System.Windows.Controls.Label apuLabel)
        {
            apuLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorCode));
        }
    }
}
