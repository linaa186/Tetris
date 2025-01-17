using System;
using System.Globalization;
using System.Windows.Data;

namespace Tetris;

public class PositionToCanvasConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return System.Convert.ToInt32(value) * 20;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return System.Convert.ToInt32(value) / 20;
    }
}
