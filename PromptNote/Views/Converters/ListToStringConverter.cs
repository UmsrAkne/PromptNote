using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using PromptNote.Models;

namespace PromptNote.Views.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<Tag> list)
            {
                return string.Join(", ", list); // カンマ区切りなど好みに応じて変更
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string input)
            {
                return input.Split(new[] { ", ", }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => new Tag() { Value = s, })
                    .ToList();
            }

            return new List<string>();
        }
    }
}