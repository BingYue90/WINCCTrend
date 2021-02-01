using CCHMIRTGRAPHICS;
using CCHMIRUNTIME;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace TrendTree.Models
{
    public class TrendModelClass
    {
        #region 静态字段
        /// <summary>
        /// 设置
        /// </summary>
        public static SettingModelClass setting;
        /// <summary>
        /// 控件对象
        /// </summary>
        public static ObjectClass obj = new ObjectClass();
        #endregion
        #region 属性
        /// <summary>
        /// 当前索引号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 趋势名
        /// </summary>
        public string TrendName { get => Name.Split('\\')[1]; }
        /// <summary>
        /// 上限值
        /// </summary>
        public float HighLimit
        {
            get
            {
                if (string.IsNullOrEmpty(setting.ValueAxis.HighString))
                    return LowLimit;
                else
                    return Convert.ToSingle(obj.hmi.Tags[$"{TrendName.Split('.')[0]}.{setting.ValueAxis.HighString}"].Read());
            }
        }
        /// <summary>
        /// 下限值
        /// </summary>
        public float LowLimit
        {
            get
            {
                if (string.IsNullOrEmpty(setting.ValueAxis.LowString))
                    return 0.0f;
                else
                    return Convert.ToSingle(obj.hmi.Tags[$"{TrendName.Split('.')[0]}.{setting.ValueAxis.LowString}"].Read());
            }
        }
        #endregion
        #region 构造函数
        public TrendModelClass()
        {
        }
        public TrendModelClass(string name) : this()
        {
            Name = name;
            AddTrend();
        }
        #endregion
        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            
            var trend = obj.trend;
            if (trend == null)
                return;
            #region 初始化数值轴
            for (int i = trend.ValueAxisCount - 1; i >= (setting.ValueAxis.NewValueAxis ? 0 : 1); i--)
            {
                trend.ValueAxisIndex = i;
                trend.ValueAxisRemove = trend.ValueAxisName;
            }
            
            if (!setting.ValueAxis.NewValueAxis)
            {
                trend.ValueAxisIndex = 0;
                trend.ValueAxisRename = "数值轴";
                trend.ValueAxisAutoRange = setting.ValueAxis.AutoValueAxis == ValueAxisRangeEnum.Auto;
                trend.ValueAxisEndValue = setting.ValueAxis.HighLimit;
                trend.ValueAxisBeginValue = setting.ValueAxis.LowLimit;
            }
            #endregion

            #region 初始化时间轴
            for (int i = trend.TimeAxisCount - 1; i >= 1; i--)
            {
                trend.TimeAxisIndex = i;
                trend.TimeAxisRemove = trend.TimeAxisName;
            }
            trend.TimeAxisTimeRangeFactor = setting.TimeAxis.Factor;
            trend.TimeAxisTimeRangeBase = (int)setting.TimeAxis.TimeRange;
            trend.TimeAxisRename = "时间轴";
            #endregion

            #region 初始化趋势
            for (int i = trend.TrendCount - 1; i >= 0; i--)
            {
                trend.TrendIndex = i;
                trend.TrendRemove = trend.TrendRename;
            }
            #endregion
        }
        /// <summary>
        /// 添加趋势
        /// </summary>
        public void AddTrend()
        {
            var trend = obj.trend;
            if (trend == null)
                return;
            ID = trend.TrendCount;
            for (int i = 0; i < ID; i++)
            {
                trend.TrendIndex = i;
                if (Name == trend.TrendTagName)
                {
                    return;
                }
            }

            int[] ColourValues = new int[] {
        0xFF0000, 0x00FF00, 0x0000FF, 0xFFFF00, 0xFF00FF, 0x00FFFF, 0x000000,
        0x800000, 0x008000, 0x000080, 0x808000, 0x800080, 0x008080, 0x808080,
        0xC00000, 0x00C000, 0x0000C0, 0xC0C000, 0xC000C0, 0x00C0C0, 0xC0C0C0,
        0x400000, 0x004000, 0x000040, 0x404000, 0x400040, 0x004040, 0x404040,
        0x200000, 0x002000, 0x000020, 0x202000, 0x200020, 0x002020, 0x202020,
        0x600000, 0x006000, 0x000060, 0x606000, 0x600060, 0x006060, 0x606060,
        0xA00000, 0x00A000, 0x0000A0, 0xA0A000, 0xA000A0, 0x00A0A0, 0xA0A0A0,
        0xE00000, 0x00E000, 0x0000E0, 0xE0E000, 0xE000E0, 0x00E0E0, 0xE0E0E0,
                };

            trend.TrendAdd = TrendName;
            trend.TrendIndex = trend.TrendCount;
            trend.TrendTagName = Name;
            trend.TrendColor = ColourValues[trend.TrendCount - 1];
            trend.TrendPointStyle = 0;
            trend.TrendTrendWindow = trend.TrendWindowName;
            if (setting.ValueAxis.NewValueAxis)
            {
                AddValueAxis();
            }
            trend.TrendValueAxis = trend.ValueAxisName;
            trend.TrendTimeAxis = trend.TimeAxisName;

            trend.TimeAxisTimeRangeBase = 3600000;
        }
        /// <summary>
        /// 添加数值轴
        /// </summary>
        private void AddValueAxis()
        {
            var trend = obj.trend;
            if (trend == null)
                return;
            trend.ValueAxisRemove = TrendName;
            trend.ValueAxisAdd = TrendName;
            trend.ValueAxisIndex = trend.ValueAxisCount;
            switch (setting.ValueAxis.AutoValueAxis)
            {
                case ValueAxisRangeEnum.Auto:
                    trend.ValueAxisAutoRange = true;
                    break;
                case ValueAxisRangeEnum.Dynamic:
                    trend.ValueAxisAutoRange = false;
                    var pre = TrendName.Split('.')[0];
                    trend.ValueAxisBeginValue = obj.hmi.Tags[$"{pre}.{setting.ValueAxis.LowString}"].Read();
                    trend.ValueAxisEndValue = obj.hmi.Tags[$"{pre}.{setting.ValueAxis.HighString}"].Read();
                    break;
                case ValueAxisRangeEnum.Fixed:
                    trend.ValueAxisAutoRange = false;
                    trend.ValueAxisBeginValue = setting.ValueAxis.LowLimit;
                    trend.ValueAxisEndValue = setting.ValueAxis.HighLimit;
                    break;
            }
            trend.ValueAxisTrendWindow = trend.TrendWindowName;
        }
        /// <summary>
        /// 删除趋势
        /// </summary>
        public void Remove()
        {
            var trend = obj.trend;
            if (trend == null)
                return;
            trend.TrendRemove = TrendName;
            if (trend.TrendCount < 1)
            {
                trend.ValueAxisRename = "数值轴1";
            }
            else
            {
                trend.ValueAxisRemove = TrendName;
            }
        }
        /// <summary>
        /// 清空所有趋势
        /// </summary>
        public static void ClearTrends()
        {
        }
        #endregion
    }

    public class ObjectClass
    {
        #region 私有字段
        public HMIRuntime hmi;
        public dynamic trend;
        #endregion
        #region 构造函数
        public ObjectClass()
        {
            hmi = new HMIRuntime();
            trend = GetObject();
        }
        #endregion
        #region 方法
        /// <summary>
        /// 读取数据库
        /// </summary>
        /// <returns>变量记录列表</returns>
        public List<string> GetList()
        {
            List<string> trends = new List<string>();
            string database = hmi.Tags["@DatasourceNameRT"].Read(HMIReadType.hmiReadCache).ToString();
            string connStr = $@"Data Source = 127.0.0.1\WINCC; Integrated Security = SSPI; Initial Catalog = {database};";
            SqlConnection con = new SqlConnection();
            con = new SqlConnection(connStr);
            con.Open();
            var setting = TrendModelClass.setting;

            string cmdStr = $@"select ValueName from Archive {setting.FilterStr} Order by ValueName;";
            SqlCommand command = new SqlCommand(cmdStr, con);
            
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                trends.Add(reader[0].ToString());
            }
            command.Dispose();
            con.Close();
            con.Dispose();
            return trends;
        }
        /// <summary>
        /// 获取趋势控件对象
        /// </summary>
        /// <returns>趋势控件对象</returns>
        public dynamic GetObject()
        {
            string objName = string.Empty;

            foreach (HMIScreenItem item in hmi.ActiveScreen.ScreenItems)
            {
                if (item.Type == "CCAxOnlineTrendControl.AxOnlineTrendControl")
                {
                    objName = item.ObjectName;
                }
            }
            if (objName == string.Empty)
            {
                throw new Exception("没有找到趋势控件");
            }
            dynamic trend = hmi.ActiveScreen.ScreenItems[objName];
            return trend;
        }
        /// <summary>
        /// 获取控件名称
        /// </summary>
        /// <returns></returns>
        public string GetObjectName()
        {
            return hmi.ActiveScreen.ActiveScreenItem.ObjectName;
        }
        /// <summary>
        /// 获取画面名称
        /// </summary>
        /// <returns></returns>
        public string GetScreenName()
        {
            return hmi.ActiveScreen.ObjectName;
        }

        
        #endregion
    }
}