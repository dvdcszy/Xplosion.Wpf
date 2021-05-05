using System;
using System.Globalization;

using System.Windows;

namespace Xplosion.Wpf
{
    class BooleanToVisibilityConverter : BaseValueConverter<BooleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)parameter == "true")
            {
                return (bool)value == true ? Visibility.Visible : Visibility.Collapsed;
            } 
            else
            {
                // Reverse logic, when IsEditButtonVisible = false, the EditControls should be visible.
                return (bool)value == true ? Visibility.Collapsed : Visibility.Visible;
            }

        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
