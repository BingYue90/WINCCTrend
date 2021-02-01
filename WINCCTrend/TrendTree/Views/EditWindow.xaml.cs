using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;
using TrendTree.Models;

namespace TrendTree
{
    /// <summary>
    /// EditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditWindow : Window
    {

        #region 依赖属性
        public Dictionary<int, string> Settings
        {
            get { return (Dictionary<int, string>)GetValue(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for keyValuePairs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingsProperty =
            DependencyProperty.Register("Settings", typeof(Dictionary<int, string>), typeof(EditWindow), null);

        public SettingModelClass Setting
        {
            get { return (SettingModelClass)GetValue(SettingProperty); }
            set { SetValue(SettingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Setting.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingProperty =
            DependencyProperty.Register("Setting", typeof(SettingModelClass), typeof(EditWindow), null);

        public bool Update
        {
            get { return (bool)GetValue(UpdateProperty); }
            set { SetValue(UpdateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Update.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateProperty =
            DependencyProperty.Register("Update", typeof(bool), typeof(SettingModelClass), null);


        #endregion

        public EditWindow()
        {
            InitializeComponent();
            test();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Setting.Save();
        }

        private void test()
        {
            Settings = SettingModelClass.SettingList;
            if(Settings.Count == 0)
            {
                Setting = new SettingModelClass();
                Setting.Filters.Add(new FilterModel());
            }
            SelectedSetting.SelectedIndex = 1;
        }

        private void Text_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{Setting.ValueAxis.HighString}\r\n{Setting.ValueAxis.LowString}\r\n{Setting.Filters}");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Setting.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Update = true;
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SelectedSetting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selected = SelectedSetting.SelectedItem;
            Setting = SettingModelClass.Load(selected.Key);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (null != Setting && Setting.ID == 0)
                {
                    Setting.Save(); 
                }
                Settings.Add(Settings.Last().Key + 1, "");
                SelectedSetting.SelectedIndex = SelectedSetting.Items.Count - 1;
                Setting = new SettingModelClass();
                Setting.Filters.Add(new FilterModel());
                SelectedSetting.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var ID = ((dynamic)SelectedSetting.SelectedItem).Key;
            Setting.Delete();
            Settings.Remove(ID);
            SelectedSetting.Items.Refresh();
        }
    }
}
