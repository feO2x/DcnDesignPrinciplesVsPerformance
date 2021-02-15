using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Light.GuardClauses;

namespace SyncVsAsync.WpfClient
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        private Visibility _visibilityWhenFalse = Visibility.Hidden;

        public Visibility VisibilityWhenFalse
        {
            get => _visibilityWhenFalse;
            set => _visibilityWhenFalse = value.MustBeValidEnumValue();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ReSharper disable once PossibleNullReferenceException
            return (bool) value ? Visibility.Visible : _visibilityWhenFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}