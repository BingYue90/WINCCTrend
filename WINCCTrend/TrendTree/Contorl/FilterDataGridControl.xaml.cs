using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrendTree.Models;

namespace TrendTree.Contorl
{
    /// <summary>
    /// FilterDataGridControl.xaml 的交互逻辑
    /// </summary>
    public partial class FilterDataGridControl : UserControl
    {
        #region 依赖属性
        public ObservableCollection<FilterModel> Filters
        {
            get { return (ObservableCollection<FilterModel>)GetValue(FiltersProperty); }
            set { SetValue(FiltersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FiltersProperty =
            DependencyProperty.Register("Filters", typeof(ObservableCollection<FilterModel>), typeof(FilterDataGridControl), null);
        #endregion

        #region Command
        public static readonly RoutedCommand AddCommand =
            new RoutedCommand("Add", typeof(FilterDataGridControl));

        public static readonly RoutedCommand DeleteCommand =
            new RoutedCommand("Delete", typeof(FilterDataGridControl));

        public static readonly RoutedCommand UpdateCommand =
            new RoutedCommand("Update", typeof(FilterDataGridControl));
        #endregion

        public FilterDataGridControl()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //FilterModel.OnFilterChange += () => { FilterText.Text = FilterModel.ToString(Filters); };

            #region Command绑定
            CommandBindings.Add(new CommandBinding(AddCommand,
               delegate (object sender, ExecutedRoutedEventArgs e)
               {
                   Add();
               }));

            CommandBindings.Add(new CommandBinding(DeleteCommand,
                delegate (object sender, ExecutedRoutedEventArgs e)
                {
                    Delete();
                }));

            CommandBindings.Add(new CommandBinding(UpdateCommand,
                delegate (object sender, ExecutedRoutedEventArgs e)
                {
                    FilterText.Text = FilterModel.ToString(Filters);
                }));
            #endregion
        }

        private void Add()
        {
            if (Filters == null)
            {
                Filters = new ObservableCollection<FilterModel>();
            }
            if (dataGrid.SelectedIndex + 1 > Filters.Count)
            {
                Filters.Add(new FilterModel());
            }
            else
            {
                Filters.Insert(dataGrid.SelectedIndex + 1, new FilterModel());
            }
        }

        private void Delete()
        {
            if (Filters.Count > 1)
            {
                Filters.Remove(dataGrid.SelectedItem as FilterModel);
            }
            else
            {
                Filters[0].Content = "";
            }
        }
    }
}
