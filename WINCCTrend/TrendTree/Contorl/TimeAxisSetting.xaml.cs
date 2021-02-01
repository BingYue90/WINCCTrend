using System.Windows;
using System.Windows.Controls;
using TrendTree.Models;

namespace TrendTree.Contorl
{
    /// <summary>
    /// TimeAxisSetting.xaml 的交互逻辑
    /// </summary>
    public partial class TimeAxisSetting : UserControl
    {
        public TimeAxisModel TimeAxis
        {
            get { return (TimeAxisModel)GetValue(TimeAxisProperty); }
            set { SetValue(TimeAxisProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeAxis.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeAxisProperty =
            DependencyProperty.Register("TimeAxis", typeof(TimeAxisModel), typeof(TimeAxisSetting), null);


        public TimeAxisSetting()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {

        }
    }
}
