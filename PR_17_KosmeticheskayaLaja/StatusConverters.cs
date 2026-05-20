using System;
using System.Windows.Data;

namespace PR_17_KosmeticheskayaLaja
{
    public class BooleanToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool b) return b ? "Выдан (Закрыт)" : "Активен (В обработке)";
            return "Активен (В обработке)";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => throw new NotImplementedException();
    }

    public class BooleanToFreezeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool b) return b ? "Заморожен" : "Продаётся (Активен)";
            return "Продаётся (Активен)";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => throw new NotImplementedException();
    }

    public class BooleanToCancelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool b) return b ? "Отменена" : "Ожидается";
            return "Ожидается";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => throw new NotImplementedException();
    }
}