using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrendTree.Models;

namespace TrendTree.Contorl
{
    /// <summary>
    /// IConSetting.xaml 的交互逻辑
    /// </summary>
    public partial class IConSetting : UserControl
    {

        #region 依赖属性
        public IconModel Icon
        {
            get { return (IconModel)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(IConSetting), null);
        #endregion

        #region Command
        public static readonly RoutedCommand SelectCommand =
            new RoutedCommand("Select", typeof(IConSetting));
        #endregion

        public IConSetting()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            CommandBindings.Add(new CommandBinding(SelectCommand,
                delegate (object sender, ExecutedRoutedEventArgs e) {
                    Select(Convert.ToInt32(e.Parameter));
                }));
        }

        private void Select(int index)
        {
            List<string> titles = new List<string>()
            {
                "展开",
                "折叠",
                "趋势"
            };
            string title = $"请选择{titles[index]}的图标";
            title += "图标";
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Title = title;
            openFileDialog.Filter = "图片文件|*.png;*.jpg;*.ico";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string fileName = openFileDialog.FileName;
            switch (index)
            {
                case 0:
                    Icon.OpenIconPath = fileName;
                    break;
                case 1:
                    Icon.CloseIconPath = fileName;
                    break;
                case 2:
                    Icon.TrendIconPath = fileName;
                    
                    break;
                default:
                    throw new Exception("选择图标参数错误");
            }
        }
    }
}
