using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrendTree.ViewModels;

namespace TrendTree.Models
{
    #region 过滤
    /// <summary>
    /// 路径枚举
    /// </summary>
    public enum PathEnum
    {
        [Description("目录")]
        Folder,
        [Description("变量名")]
        Tagname
    }
    public enum FilterTypeEnum
    {
        [Description("包含")]
        Include,
        [Description("不包含")]
        Exclude,
        [Description("等于")]
        Equal,
        [Description("不等于")]
        Unequal
    }
    public enum OperatorEnum
    {
        [Description("")]
        Null,
        [Description("和")]
        Or,
        [Description("或")]
        And
    }
    public class FilterModel : UiUpdateClass
    {
        #region 事件委托
        public delegate void FilterDelegate();
        public static event FilterDelegate OnFilterChange;
        #endregion

        #region 属性
        /// <summary>
        /// 路径选择
        /// </summary>
        private PathEnum pathSelect = PathEnum.Folder;
        public PathEnum PathSelect
        {
            get { return pathSelect; }
            set
            {
                pathSelect = value;
                NotifyPropertyChanged("PathSelect");
                OnFilterChange?.Invoke();
            }
        }
        /// <summary>
        /// 运算符
        /// </summary>
        private FilterTypeEnum filterType = FilterTypeEnum.Equal;
        public FilterTypeEnum FilterType
        {
            get { return filterType; }
            set
            {
                filterType = value;
                NotifyPropertyChanged("FilterType");
                OnFilterChange?.Invoke();
            }
        }
        /// <summary>
        /// 设置
        /// </summary>
        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                NotifyPropertyChanged("Content");
                OnFilterChange?.Invoke();
            }
        }
        /// <summary>
        /// 操作符
        /// </summary>
        private OperatorEnum operatorSelect;
        public OperatorEnum OperatorSelect
        {
            get { return operatorSelect; }
            set
            {
                operatorSelect = value;
                NotifyPropertyChanged("OperatorSelect");
                OnFilterChange?.Invoke();
            }
        }
        #endregion

        public FilterModel()
        {
            PathSelect = PathEnum.Folder;
            FilterType = FilterTypeEnum.Include;
            OperatorSelect = OperatorEnum.Null;
        }

        public string ToString(bool last = false)
        {
            if (Content == null)
            {
                return "";
            }
            string str = "ValueName ";
            string pahtStr = PathSelect == PathEnum.Tagname ? @"%\" : "";
            string folderStr = PathSelect == PathEnum.Folder ? @"\%" : "";
            switch (FilterType)
            {
                case FilterTypeEnum.Equal:
                    str += $" Like '{pahtStr}{Content}{folderStr}'";
                    break;
                case FilterTypeEnum.Unequal:
                    str += $"Not Like '{pahtStr}{Content}{folderStr}'";
                    break;
                case FilterTypeEnum.Include:
                    str += $"Like '{pahtStr}%{Content}%{folderStr}'";
                    break;
                case FilterTypeEnum.Exclude:
                    str += $"Not Like '{pahtStr}%{Content}%{folderStr}'";
                    break;
            }
            str += OperatorSelect == OperatorEnum.Null || last ? "" : OperatorSelect.ToString();
            str += " ";
            return str;
        }

        public static string ToString(ObservableCollection<FilterModel> Filters)
        {
            string str = "";
            if (Filters != null && (Filters.Count > 0 && !string.IsNullOrEmpty(Filters[0].Content)))
            {
                str += "Where ";
            } //判断Filter是否为空
            else
            {
                return str;
            }
            foreach (var item in Filters)
            {
                if (item.Equals(Filters.Last()))
                {
                    str += item.ToString(true);
                }
                else
                {
                    str += item.ToString();
                }
            }
            return str;
        }

    }
    #endregion
}
