using TrendTree.ViewModels;

namespace TrendTree.Models
{
    #region 图标
    /// <summary>
    /// 图标
    /// </summary>
    public class IconModel : UiUpdateClass
    {
        #region 属性
        /// <summary>
        /// 展开Icon
        /// </summary>
        protected string openIconPath;
        public string OpenIconPath
        {
            get
            {
                if (string.IsNullOrEmpty(openIconPath))
                    return @"../Images/Open_32x32.png";
                else
                    return openIconPath;
            }
            set
            {
                openIconPath = value;
                NotifyPropertyChanged("OpenIconPath");
            }
        }
        /// <summary>
        /// 折叠Icon
        /// </summary>
        protected string closeIconPath;
        public string CloseIconPath
        {
            get
            {
                if (string.IsNullOrEmpty(closeIconPath))
                    return @"../Images/Close_32x32.png";
                else
                    return closeIconPath;
            }
            set
            {
                closeIconPath = value;
                NotifyPropertyChanged("CloseIconPath");
            }
        }
        /// <summary>
        /// 趋势Icon
        /// </summary>
        protected string trendIconPath;
        public string TrendIconPath
        {
            get
            {
                if (string.IsNullOrEmpty(trendIconPath))
                    return @"../Images/3DLine_32x32.png";
                else
                    return trendIconPath;
            }
            set
            {
                trendIconPath = value;
                NotifyPropertyChanged("TrendIconPath");
            }
        }
        #endregion
    }
    #endregion
}
