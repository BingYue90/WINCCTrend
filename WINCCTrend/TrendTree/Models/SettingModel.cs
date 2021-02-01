using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using TrendTree.ViewModels;

namespace TrendTree.Models
{
    [Serializable]
    public class SettingModelClass : UiUpdateClass
    {
        #region 属性
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; } = 0;
        /// <summary>
        /// 数值轴
        /// </summary>
        public ValueAxisModel ValueAxis { get; set; } = new ValueAxisModel();
        /// <summary>
        /// 时间轴
        /// </summary>
        public TimeAxisModel TimeAxis { get; set; } = new TimeAxisModel();
        /// <summary>
        /// 图标
        /// </summary>
        public IconModel Icon { get; set; } = new IconModel();
        /// <summary>
        /// 过滤条件
        /// </summary>
        private ObservableCollection<FilterModel> filters = new ObservableCollection<FilterModel>();
        public ObservableCollection<FilterModel> Filters
        {
            get 
            { 
                return filters; 
            }
            set
            {
                filters = value;
                NotifyPropertyChanged("Filters");
            }
        }
        public string FilterStr
        {
            get
            {
                return FilterModel.ToString(Filters);
            }
        }
        /// <summary>
        /// 设置列表
        /// </summary>
        private static Dictionary<int, string> settingList;
        public static Dictionary<int, string> SettingList
        {
            get 
            {
                if (settingList == null)
                {
                    GetList();
                }
                    return settingList;
            }
            set 
            { 
                settingList = value;
            }
        }
        #endregion

        #region 字段
        private const string Folder = @".\setting";
        private const string SettingListFile = @"SettingList.xml";
        #endregion

        public SettingModelClass()
        {
            
        }

        public static SettingModelClass Load(int id)
        {
            SettingModelClass setting = new SettingModelClass();
            setting.Filters.Add(new FilterModel());
            if (id == 0 || SettingList[id] == "")
                return setting;
            if(!SettingList.ContainsKey(id))
            {
                throw new Exception("ID值不存在");
            }
            try
            {
                string file = SettingList[id];
                using (FileStream stream = File.OpenRead(file))
                {
                    var serializer = new XmlSerializer(typeof(SettingModelClass));
                    setting = (SettingModelClass)serializer.Deserialize(stream);
                    if(setting.Filters.Count == 0)
                    {
                        setting.Filters.Add(new FilterModel());
                    }
                    setting.ID = id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                SettingList.Remove(id);
                SaveList();
            }
            
            return setting;
        }

        public void Delete()
        {
            File.Delete(SettingList[ID]);
        }

        public void Save()
        {
            string file = "";
            if (ID == 0)
            {
                int ID = SettingList.Count + 1;
                file = $@"{Folder}\Trend_{ID}.xml";
                SettingList[ID] = file;
                Console.WriteLine(SettingList.Count);
                SaveList();
            }
            else
            {
                file = $@"{Folder}\Trend_{ID}.xml";
            }
            if(filters.Count <= 1)
            {
                if (filters.Last().Content == null)
                {
                    filters.Clear();
                }
            }
            using (FileStream stream = File.Create(file))
            {
                var serializer = new XmlSerializer(typeof(SettingModelClass));
                serializer.Serialize(stream, this);
            }
        }

        private static void SaveList()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            XmlDocument xmldoc = new XmlDocument();
            XmlDeclaration xmldec = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmldoc.AppendChild(xmldec);
            XmlElement xmlSettings = xmldoc.CreateElement("Settings");
            xmldoc.AppendChild(xmlSettings);
            foreach (var item in SettingList)
            {
                XmlElement xmlSetting = xmldoc.CreateElement("Setting"); ;
                xmlSetting.SetAttribute("ID", item.Key.ToString());
                xmlSetting.SetAttribute("FileName", item.Value);
                xmlSettings.AppendChild(xmlSetting);
            }
            using (FileStream stream = File.Create($@"{Folder}\{SettingListFile}"))
            {
                xmldoc.Save(stream);
            }
        }

        private static void GetList()
        {
            try
            {
                if(settingList == null)
                {
                    settingList = new Dictionary<int, string>();
                }
                else
                {
                    settingList.Clear();
                }
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load($@"{Folder}\{SettingListFile}");
                var Settings = xmldoc.SelectSingleNode("Settings").ChildNodes;
                foreach (XmlElement item in Settings)
                {
                    settingList.Add(int.Parse(item.GetAttribute("ID")), item.GetAttribute("FileName"));
                }
            }
            catch (Exception)
            {
                if (!Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);
            }
        }
    }






}
