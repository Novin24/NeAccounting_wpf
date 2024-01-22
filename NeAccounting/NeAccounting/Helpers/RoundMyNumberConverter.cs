// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Globalization;
using System.Windows.Data;

namespace NeAccounting.Helpers
{
    internal class RoundMyNumberConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object paramater, CultureInfo culture)
        {
            return value;
        }

        public object Convert(object obj, Type targetType, object parameter, CultureInfo culture)
        {
            long num = 0;

            if (obj is not string str)
                return obj;

            if (long.TryParse(str, out num))
            {
                return num.ToString("N0");
            }

            return str;//str.ToString("N0");
        }
    }
}
