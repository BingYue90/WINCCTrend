using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace TrendTree.Resources
{
    public class TreeViewLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TreeViewItem item = (TreeViewItem)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }

    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
             System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("必须为bool值");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    #region 枚举
    public class EnumHelper
    {
        public Dictionary<string, Enum> GetNames(Type E)
        {
            var dic = new Dictionary<string, Enum>();
            System.Reflection.FieldInfo[] fields = E.GetFields();
            int i = 1;
            foreach (Enum item in Enum.GetValues(E))
            {
                System.ComponentModel.DescriptionAttribute[] EnumAttributes = (System.ComponentModel.DescriptionAttribute[])fields[i].
                GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (EnumAttributes.Length > 0)
                {
                    dic.Add(EnumAttributes[0].Description, item);

                }
                i++;
            }
            return dic;
        }
    }
    #endregion
}
