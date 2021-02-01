using System.Windows;
using System.Windows.Controls;
using TrendTree.Models;

namespace TrendTree.Contorl
{
    /// <summary>
    /// ValueAxisSetting.xaml 的交互逻辑
    /// </summary>
    public partial class ValueAxisSetting : UserControl
    {

        public ValueAxisModel ValueAxis
        {
            get { return (ValueAxisModel)GetValue(ValueAxisProperty); }
            set { SetValue(ValueAxisProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueAxis.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueAxisProperty =
            DependencyProperty.Register("ValueAxis", typeof(ValueAxisModel), typeof(ValueAxisSetting), null);

        public ValueAxisSetting()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            ValueAxisModel.OnRangeChange += ChangeRange;
        }

        private void ChangeRange(ValueAxisRangeEnum range)
        {
            switch(range)
            {
                case ValueAxisRangeEnum.Auto:
                    FixedExpander.Visibility = Visibility.Collapsed;
                    DynamicExpander.Visibility = Visibility.Collapsed;
                    break;
                case ValueAxisRangeEnum.Dynamic:
                    if (ValueAxis.NewValueAxis)
                    {
                        FixedExpander.Visibility = Visibility.Collapsed;
                        DynamicExpander.Visibility = Visibility.Visible;
                    }
                    else 
                    {
                        MessageBox.Show("请同时选择新增轴");
                    }
                    break;
                case ValueAxisRangeEnum.Fixed:
                    FixedExpander.Visibility = Visibility.Visible;
                    DynamicExpander.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}
