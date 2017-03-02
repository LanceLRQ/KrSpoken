using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace KrSpoken.Profile
{
    /// <summary>
    /// 试题包安装数据支持模块
    /// </summary>
    public class UserPkLibRuntime
    {
        /// <summary>
        /// 载入试题包列表库文件
        /// </summary>
        /// <param name="_PkLibfile">试题包列表库文件</param>
        /// <param name="UserPkList">试题包列表库</param>
        public static void Infile(string _PkLibfile,out UserPkLib UserPkList)
        {
            try
            {
                if (File.Exists(_PkLibfile) == false)
                {
                    using (FileStream fs = File.Create(_PkLibfile)) { fs.Close(); }
                    UserPkList = new UserPkLib();
                    return;
                }
                using (FileStream fs = new FileStream(_PkLibfile, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length > 0)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        UserPkList = (UserPkLib)bf.Deserialize(fs);
                    }
                    else
                    {
                        UserPkList = new UserPkLib();
                    }
                }
            }
            catch
            {
                throw new Exception("载入试题包列表数据错误！");
            }
        }
        /// <summary>
        /// 保存试题包列表库文件
        /// </summary>
        /// <param name="_PkLibfile">输出的试题包列表库文件</param>
        /// <param name="UserPkList">试题包列表库</param>
        public static void Outfile(string _PkLibfile, UserPkLib UserPkList)
        {
            try
            {
                using (FileStream fs = new FileStream(_PkLibfile, FileMode.Create, FileAccess.Write))
                {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, UserPkList);
                        fs.Close();
                }
            }
            catch
            {
                throw new Exception("输出试题包列表数据错误！");
            }
        }
    }

    /// <summary>
    /// 试题包列表库
    /// </summary>
    [Serializable()]
    public class UserPkLib
    {
        public Hashtable _PackTitle = new Hashtable();
        private Hashtable _fileName=new Hashtable();
        private Hashtable _PackType = new Hashtable();
        private Hashtable _PackDiff = new Hashtable();
        private Hashtable _PackPracticeTime = new Hashtable();
        private Hashtable _PackExamTime = new Hashtable();
        private Hashtable _PkIndex = new Hashtable();
        private Hashtable _PkMix = new Hashtable();
        private Hashtable _IsCanUse = new Hashtable();

        /// <summary>
        /// 安装试题包
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        /// <param name="_fileName_">试题包文件名称</param>
        /// <param name="_PackType_">试题包类型</param>
        /// <param name="_PackDiff_">试题包难度</param>
        /// <param name="PkIndex">试题包解包索引</param>
        /// <param name="PkMix">试题包反混淆引擎</param>
        public void Add(string PackTitle, string _fileName_, string _PackType_, string _PackDiff_, string _PkIndex_, string _PkMix_)
        {
            _PackTitle.Add(_PackTitle.Count+1, PackTitle);
            _fileName.Add(PackTitle, _fileName_);
            _PackType.Add(PackTitle, _PackType_);
            _PackDiff.Add(PackTitle, _PackDiff_);
            _PackPracticeTime.Add(PackTitle, 0);
            _PackExamTime.Add(PackTitle, 0);
            _PkIndex.Add(PackTitle, _PkIndex_);
            _PkMix.Add(PackTitle, _PkMix_);
            if (_PackType_ == "免费")
            {
                _IsCanUse.Add(PackTitle, true);
            }
            else
            {
                _IsCanUse.Add(PackTitle, false);
            }
        }
        /// <summary>
        /// 更新试题包信息
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        /// <param name="_PackPracticeTime_">试题包练习次数</param>
        /// <param name="_PackExamTime_">试题包模拟次数</param>
        public void Update(string PackTitle,int Type, int _PackPracticeTime_, int _PackExamTime_,bool IsCanUse_)
        {
            switch (Type)
            {
                case 1:
                    _PackPracticeTime[PackTitle] = _PackPracticeTime_;
                    break;
                case 2:
                    _PackExamTime[PackTitle] = _PackExamTime_;
                    break;
                case 3:
                    _IsCanUse[PackTitle] = IsCanUse_;
                    break;
            }
        }
        /// <summary>
        /// 卸载试题包
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        public void Delete(string PackTitle,int PkId)
        {
            if (PkId == -1)
            {
                return;
            }
            _PackTitle.Remove(PkId);
            _fileName.Remove(PackTitle);
            _PackType.Remove(PackTitle);
            _PackDiff.Remove(PackTitle);
            _PackPracticeTime.Remove(PackTitle);
            _PackExamTime.Remove(PackTitle);
            _PkIndex.Remove(PackTitle);
            _PkMix.Remove(PackTitle);
            _IsCanUse.Remove(PackTitle);
        }
        /// <summary>
        /// 寻找ID
        /// </summary>
        /// <param name="_PkName">试题包名称</param>
        /// <returns></returns>
        public int FindPackID(string _PkName)
        {
            for (int i = 1; i <= _PackTitle.Count; i++)
            {
                if (_PackTitle[i].ToString() == _PkName)
                {
                    return i;
                }
            }
            return -1;
        }

        #region 输出值
        /// <summary>
        /// 取得文件名
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        public string get_fileName(string PackTitle) { return (string)_fileName[PackTitle]; }
        /// <summary>
        /// 取得试题包类型
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        public string get_PackType(string PackTitle) { return (string)_PackType[PackTitle]; }
        /// <summary>
        /// 取得试题包难易度
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        public string get_PackDiff(string PackTitle) { return (string)_PackDiff[PackTitle]; }
        /// <summary>
        /// 取得试题包练习次数
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        public int get_PackPracticeTime(string PackTitle) { return (int)_PackPracticeTime[PackTitle]; }
        /// <summary>
        /// 取得试题包模拟次数
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        public int get_PackExamTime(string PackTitle) { return (int)_PackExamTime[PackTitle]; }
        /// <summary>
        /// 取得试题包索引
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        public string get_PkIndex(string PackTitle) { return (string)_PkIndex[PackTitle]; }
        /// <summary>
        /// 取得试题包反混淆索引
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        public string get_PkMix(string PackTitle) { return (string)_PkMix[PackTitle]; }
        /// <summary>
        /// 取得试题包是否已能够使用
        /// </summary>
        /// <param name="PackTitle">试题包标题</param>
        public bool get_IsCanUse(string PackTitle) { return (bool)_IsCanUse[PackTitle]; }
#endregion
    }
}
