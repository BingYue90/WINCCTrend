using System.ComponentModel;

namespace TrendTree.Models
{
    #region 时间轴
    /// <summary>
    /// 时间基数枚举
    /// </summary>
    public enum TimeBaseEnum
    {
        [Description("500毫秒")]
        HalfSecond = 500,
        [Description("秒")]
        Second = 1000,
        [Description("分")]
        Minute = 60000,
        [Description("时")]
        Hour = 3600000,
        [Description("日")]
        Day = 86400000
    }
    /// <summary>
    /// 时间轴
    /// </summary>
    public class TimeAxisModel
    {
        #region 属性
        /// <summary>
        /// 时间区间
        /// </summary>
        public TimeBaseEnum TimeRange { get; set; }
        /// <summary>
        /// 时间系数
        /// </summary>
        public int Factor { get; set; }
        #endregion
    }
    #endregion
}
