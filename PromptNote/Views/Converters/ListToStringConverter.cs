using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace PromptNote.Views.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<string> list)
            {
                return string.Join(", ", list); // カンマ区切りなど好みに応じて変更
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // 双方向バインディングが不要なら未実装のままでOK
        }
    }
}