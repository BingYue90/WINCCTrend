using CCHMIRUNTIME;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TrendTree.Models;
using TrendTree.ViewModels;

namespace TrendTree
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class TrendTreeControl : UserControl
    {
        #region 依赖属性
        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set 
            {
                SetValue(IDProperty, value);
                Init();
            }
        }

        // Using a DependencyProperty as the backing store for ID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID", typeof(int), typeof(TrendTreeControl), null);

        public bool Edit
        {
            get { return (bool)GetValue(EditProperty); }
            set { SetValue(EditProperty, value); EditAction(); }
        }

        // Using a DependencyProperty as the backing store for Edit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditProperty =
            DependencyProperty.Register("Edit", typeof(bool), typeof(TrendTreeControl), null);

        #endregion

        public TrendTreeControl()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            tree.ItemsSource = null;
            try
            {
                TrendModelClass.setting = SettingModelClass.Load(ID);
                TrendModelClass.Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Source}   {ex.Message}");
            }
            
            new Thread(Update).Start();
        }

        private void Update()
        {
            try
            {
                List<Node> source = new Node().Init();
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    tree.ItemsSource = source;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Source}   {ex.Message}");
            }
        }

        private void EditAction()
        {
            EditWindow editWindow = new EditWindow();
            editWindow.ShowInTaskbar = false;
            editWindow.ShowDialog();
            if(editWindow.Update)
            {
                ID = editWindow.Setting.ID;
                //MessageBox.Show($"{ID}    {editWindow.Setting.ID}");
            }
        }
    }
}
