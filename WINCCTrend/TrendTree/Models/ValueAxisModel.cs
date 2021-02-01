using System.ComponentModel;

namespace TrendTree.Models
{
    #region 数值轴
    public enum ValueAxisRangeEnum
    {
        [Description("自动")]
        Auto,
        [Description("动态")]
        Dynamic,
        [Description("固定")]
        Fixed
    }
    /// <summary>
    /// 数值轴
    /// </summary>
    public class ValueAxisModel
    {
        #region 事件委托
        public delegate void RangeDelegate(ValueAxisRangeEnum range);
        public static event RangeDelegate OnRangeChange;
        #endregion
        #region 属性
        /// <summary>
        /// 数值轴区间设定
        /// </summary>
        public ValueAxisRangeEnum AutoValueAxis
        {
            get => autoValueAxis;
            set
            {
                autoValueAxis = value;
                OnRangeChange?.Invoke(value);
            }
        }
        private ValueAxisRangeEnum autoValueAxis = ValueAxisRangeEnum.Auto;
        #region 动态上下限
        /// <summary>
        /// 是否新增数值轴
        /// </summary>
        public bool NewValueAxis { get; set; }
        /// <summary>
        /// 上限后缀
        /// </summary>
        public string HighString { get; set; }
        /// <summary>
        /// 下限后缀
        /// </summary>
        public string LowString { get; set; }
        #endregion
        #region 固定上下限
        /// <summary>
        /// 数值轴上限
        /// </summary>
        public double HighLimit { get; set; }
        /// <summary>
        /// 数值轴下限
        /// </summary>
        public double LowLimit { get; set; }
        #endregion
        #endregion

        #region 方法
        public override string ToString()
        {
            string str = $"Range = {AutoValueAxis}, NewAxis = {NewValueAxis}, HighString = {HighString}, " +
                $"LowString = {LowString}, HighLimit = {HighLimit}, LowLimit = {LowLimit}";
            return str;
        }
        #endregion
    }
    #endregion
}
