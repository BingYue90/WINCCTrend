using CCHMIRUNTIME;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrendTree.Models;

namespace TrendTree.ViewModels
{
    public class Node : UiUpdateClass
    {
        #region 构造函数
        public Node()
        {
            this.isChecked = false;
        }
        public Node(Node parent, string nodeName) : this()
        {
            Parent = parent;
            DisplayName = nodeName;
        }
        public Node(Node parent, string nodeName, Boolean bChecked)
            : this(parent, nodeName)
        {
            this.isChecked = bChecked;
        }
        #endregion

        #region 图片存储
        const string OpenPath = @"../Images/Open_32x32.png";
        const string ClosePath = @"../Images/Close_32x32.png";
        const string IconPath = @"../Images/3DLine_32x32.png";
        #endregion

        #region 属性
        /// <summary>
        /// 子节点
        /// </summary>
        public ObservableCollection<Node> Children { get; set; }
        /// <summary>
        /// 选中列表
        /// </summary>
        private static List<TrendModelClass> SelectedList { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        private Node Parent { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        private string icon;
        public string Icon
        {
            get => icon;
            set
            {
                icon = value;
                NotifyPropertyChanged("Icon");
            }
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool? isChecked;
        public bool? IsChecked
        {
            get { return this.isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnCheckChange?.Invoke(DisplayName);
                    NotifyPropertyChanged("IsChecked");
                    if (isChecked == true) // 如果节点被选中
                    {
                        if (Children != null && Children.Count > 0)
                        {
                            foreach (var dt in Children)
                            {
                                dt.IsChecked = true;
                            }
                        }
                        if (Parent != null)
                        {
                            SelectedList.Add(new TrendModelClass($@"{Parent.DisplayName}\{DisplayName}"));
                            bool bExistUncheckedChildren = false;
                            foreach (var dt in Parent.Children)
                            {
                                if (dt.IsChecked != true)
                                {
                                    bExistUncheckedChildren = true;
                                    break;
                                }
                            }
                            if (bExistUncheckedChildren)
                                Parent.IsChecked = null;
                            else
                                Parent.IsChecked = true;
                        }
                    }
                    else if (isChecked == false)   // 如果节点未选中
                    {
                        if (Children != null && Children.Count > 0)
                        {
                            foreach (var dt in Children)
                            {
                                dt.IsChecked = false;
                            }
                        }
                        if (Parent != null)
                        {
                            var item = SelectedList.Find(t => t.Name == $@"{Parent.DisplayName}\{DisplayName}");
                            item.Remove();
                            SelectedList.Remove(item);
                            bool bExistCheckedChildren = false;
                            foreach (var dt in Parent.Children)
                            {
                                if (dt.IsChecked != false)
                                {
                                    bExistCheckedChildren = true;
                                    break;
                                }
                            }
                            if (bExistCheckedChildren)
                                Parent.IsChecked = null;
                            else
                                Parent.IsChecked = false;
                        }
                    }
                    else
                    {
                        if (Parent != null)
                            Parent.IsChecked = null;
                    }
                }
            }
        }
        /// <summary>
        /// 展示内容
        /// </summary>
        public string TipContents { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        private bool isExpanded;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                Console.WriteLine($"isExpanded = {value}");
                isExpanded = value;
                Icon = value ? OpenPath : ClosePath;
            }
        }
        #endregion

        #region 点击事件委托
        public delegate void CheckDelegate(string Name);
        public static event CheckDelegate OnCheckChange;
        #endregion

        /// <summary>
        /// 获取初始化列表
        /// </summary>
        /// <returns></returns>
        public List<Node> Init()
        {
            if (SelectedList == null)
            {
                SelectedList = new List<TrendModelClass>();
            }
            else
            {
                SelectedList.Clear();
            }
            var items = new List<Node>();
            string folder, name;
            foreach (var str in TrendModelClass.obj.GetList())
            {
                folder = str.Split('\\')[0];
                name = str.Split('\\')[1];
                var item = items.Find(t => t.DisplayName.Equals(folder));
                if (item == null)
                {
                    items.Add(new Node()
                    {
                        DisplayName = folder,
                        Icon = ClosePath,
                        Children = new ObservableCollection<Node>()
                    });
                }
                else
                {
                    item.Children.Add(new Node()
                    {
                        Parent = item,
                        DisplayName = name,
                        Icon = IconPath
                    });
                }
            }
            return items;
        }
    }
}
