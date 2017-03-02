using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.IO;

namespace KrSpoken
{
    namespace PlayListLib
    {

        #region 实现二进制列表的读入
        public class KrPlayListRunTime
        {
            /// <summary>
            /// 实现二进制列表的读入 
            /// </summary>
            /// <param name="_ListHref">系列化的二进制数据储存位置</param>
            /// <param name="List">列表数据</param>
            public static void GetPlayList(string _ListHref, out KrPlayList List)
            {
                try
                {
                    FileStream fs = new FileStream(_ListHref, FileMode.OpenOrCreate, FileAccess.Read);
                    BinaryFormatter bf = new BinaryFormatter();
                    List = (KrPlayList)bf.Deserialize(fs);
                    fs.Close();
                }
                catch
                {
                    throw new Exception("反系列化过程发生严重错误！");
                }
            }
            /// <summary>
            /// 实现二进制列表的读入 
            /// </summary>
            /// <param name="_ListStream">系列化的二进制数据流</param>
            /// <param name="List">列表数据</param>
            public static void GetPlayList(Stream _ListStream, out KrPlayList List)
            {
                try
                {
                    Stream fs = _ListStream;
                    BinaryFormatter bf = new BinaryFormatter();
                    List = (KrPlayList)bf.Deserialize(fs);
                    fs.Close();
                }
                catch
                {
                    throw new Exception("反系列化过程发生严重错误！");
                }
            }
            /// <summary>
            /// 实现二进制列表的写入
            /// </summary>
            /// <param name="_ListHref">系列化的二进制数据储存位置</param>
            /// <param name="List">列表二进制数据</param>
            public static void SetPlayListToFile(string _ListHref, KrPlayList List)
            {
                try
                {
                    FileStream fs = new FileStream(_ListHref, FileMode.Create, FileAccess.Write);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, List);
                    fs.Close();
                }
                catch
                {
                    throw new Exception("系列化过程发生严重错误！");
                }
            }
        }
        #endregion

        #region 播放列表
        [Serializable()]
        public class KrPlayList
        {
            public KrPlayList()
            {
                MyList = new Hashtable();
            }
            /// <summary>
            /// 列表标题
            /// </summary>
            public string ListTitle { get; set; }
            /// <summary>
            /// 列表版本
            /// </summary>
            public string ListVersion { get; set; }
            /// <summary>
            /// 列表数据
            /// </summary>
            public Hashtable MyList { get; set; }
        }
        #endregion

        #region 播放列表单个拆包
        public class KrPlayListData
        {
            private object[] GetData;
            public KrPlayListData(object[] Data)
            {
                try
                {
                    if (Data.Length == 7)
                    {
                        GetData = Data;
                    }
                    else
                    {
                        throw new Exception("播放列表严重损坏");
                    }
                }
                catch
                {
                    throw new Exception("播放列表读取失败 ");
                }
            }
            /// <summary>
            /// 所属大题
            /// </summary>
            public int ItemID
            {
                get { return (int)GetData[0]; }
            }
            /// <summary>
            /// 所属小题
            /// </summary>
            public  int SubItemID
            {
                get { return (int)GetData[1]; }
            }
            /// <summary>
            /// 文件模式：0==学生录音文件 1==文本文件 3==音频文件 4==视频文件 5==准备时间
            /// </summary>
            public  int FileType
            {
                get { return (int)GetData[2]; }
            }
            /// <summary>
            /// 文件名称
            /// </summary>
            public  string FilePath
            {
                get { return (string)GetData[3]; }
            }
            /// <summary>
            /// 倒计时时间
            /// </summary>
            public  int PlayTime
            {
                get { return (int)GetData[4]; }
            }
            /// <summary>
            /// 播放时静音
            /// </summary>
            public  int PlayFlag
            {
                get { return (int)GetData[5]; }
            }
            /// <summary>
            /// 播放标题
            /// </summary>
            public  string PlayHint
            {
                get { return (string)GetData[6]; }
            }

        }
        #endregion
    }
}
