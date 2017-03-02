using System;
using System.Collections;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace KrSpoken
{
    namespace KrPacker
    {
        #region 实现二进制文件装箱功能(最大文件为2GB)
        /// <summary>
        /// 实现二进制文件装箱功能(最大文件为2GB)
        /// </summary>
        public class FileInBox
        {
            /// <summary>
            /// 缓冲长度
            /// </summary>
            public int BufferLength { get; set; }
            // 二进制流数据（合并）
            private Stream InBoxStream = new MemoryStream();
            // 文件目录
            private String DirPath;
            // 设置返回一个索引文件的位置，如果文件存在将自动覆盖
            private String OutIndexFile;
            // 设置输出的合并过的数据流，如果文件存在将自动覆盖
            private String OutStreamFile;
            // 是否已经初始化
            private bool IsInit = false;
            private KrIndexFile IndexFile = new KrIndexFile();
            // 获取的文件列表
            private string[] FilesList;
            // 获取的文件列表中的文件名
            private string[] FilesListName;
            // 程序生成的文件指针
            private string[] FilesInBoxIndexStr;
            /// <summary>
            /// 返回当前流数据的长度
            /// </summary>
            public long Length { get { return InBoxStream.Length; } }

            /// <summary>
            /// 实现二进制文件装箱功能(最大文件为2GB)
            /// </summary>
            public FileInBox()
            {
                BufferLength = 8192;
            }
            /// <summary>
            /// 实现二进制文件装箱功能(最大文件为2GB)
            /// </summary>
            /// <param name="bufferLength">文件读入写出所使用的缓冲大小，较多大文件请使用较高的缓冲，较多小文件，请使用较低的缓冲。在适当的条件下，缓冲将使文件读入读出加速。</param>
            public FileInBox(int bufferLength)
            {
                BufferLength = bufferLength;
            }

            /// <summary>
            /// 初始化文件装箱类
            /// </summary>
            /// <param name="datDirPath">设置被装箱文件目录，如果目录不存在或数据为空，将返回错误消息</param>
            /// <param name="datOutIndexFile">设置返回一个索引文件的位置，如果文件存在将自动覆盖</param>
            /// <param name="datOutStreamFile">设置输出的合并过的数据流，如果文件存在将自动覆盖</param>
            public int Init(string datDirPath, string datOutIndexFile, string datOutStreamFile)
            {
                if (Directory.Exists(datDirPath) == false)
                {
                    return 1;
                }
                string[] tmpFilesList = Directory.GetFiles(datDirPath);
                if (tmpFilesList.Length < 1)
                {
                    return 2;
                }
                datDirPath = datDirPath.Replace("/", @"\");
                if (datDirPath.Substring(datDirPath.Length - 1, 1) != @"\")
                {
                    datDirPath += @"\";
                }
                FilesList = new string[tmpFilesList.Length];
                FilesListName = new string[tmpFilesList.Length];
                FilesInBoxIndexStr = new string[tmpFilesList.Length];
                for (int i = 0; i < tmpFilesList.Length; i++)
                {
                    string[] tempdata = tmpFilesList[i].Split(Convert.ToChar(@"\"));
                    FilesListName[i] = tempdata[tempdata.Length - 1];
                }
                DirPath = datDirPath;
                FilesList = tmpFilesList;
                OutIndexFile = datOutIndexFile;
                OutStreamFile = datOutStreamFile;
                IndexFile.Header = "[KrSpoken Pack Index]";
                IndexFile.Version = "__KrSpokenPackV1.00";
                IsInit = true;
                return 0;
            }
            /// <summary>
            /// 读取文件
            /// </summary>
            /// <param name="IgnoreError">忽略过程中的错误，否则遇到错误即终止</param>
            /// <returns></returns>
            public int ReadFileToMemory(bool IgnoreError = false)
            {
                if (IsInit == false)
                {
                    return -1;
                }
                //设置初始位置
                long NowPos = 0;
                for (int i = 0; i < FilesList.Length; i++)
                {
                    try
                    {
                        FileStream FStm = new FileStream(FilesList[i], FileMode.Open);
                        byte[] ByteFileStream;
                        long[] DATA = new long[2];
                        DATA[0] =NowPos;

                        long maxLength = FStm.Length;

                        if (maxLength <= 4096)
                        {
                            ByteFileStream = new byte[maxLength];
                            FStm.Read(ByteFileStream, 0, (Int32)maxLength);
                            InBoxStream.Write(ByteFileStream, 0, (Int32)maxLength);
                            NowPos += maxLength;
                        }
                        else
                        {
                            long FileLengthPos = 0;
                            while (FileLengthPos < maxLength)
                            {
                                if (maxLength - FileLengthPos >= 4096)
                                {
                                    byte[] buffer = new byte[4096];
                                    FStm.Read(buffer, 0, 4096);
                                    InBoxStream.Write(buffer, 0, 4096);
                                    NowPos += 4096;
                                    FileLengthPos += 4096;
                                }
                                else
                                {
                                    byte[] buffer = new byte[maxLength - FileLengthPos];
                                    FStm.Read(buffer, 0, buffer.Length);
                                    InBoxStream.Write(buffer, 0, buffer.Length);
                                    NowPos += maxLength - FileLengthPos;
                                    FileLengthPos += (maxLength - FileLengthPos);
                                }
                            }
                        }
                        DATA[1] =maxLength;
                        IndexFile.FileList.Add(FilesListName[i], DATA);
                    }
                    catch
                    {
                        if (IgnoreError == true)
                        {
                            return 4;
                        }
                    }

                }

                try
                {
                    InBoxStream.Position = 0;
                    //保存文件
                    FileStream outFile = new FileStream(OutStreamFile, FileMode.Create, FileAccess.Write);
                    if (InBoxStream.Length <= 4096)
                    {
                        byte[] buffer = new byte[InBoxStream.Length];
                        InBoxStream.Read(buffer, 0, buffer.Length);
                        outFile.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        long li = InBoxStream.Length;
                        long nowI = 0;
                        while (nowI < li)
                        {
                            if (li - nowI >= 4096)
                            {
                                byte[] buffer = new byte[4096];
                                InBoxStream.Read(buffer, 0, 4096);
                                outFile.Write(buffer, 0, 4096);
                                nowI += 4096;
                            }
                            else
                            {
                                byte[] buffer = new byte[li - nowI];
                                InBoxStream.Read(buffer, 0, buffer.Length);
                                outFile.Write(buffer, 0, buffer.Length);
                                nowI = li;
                            }
                        }
                        outFile.Close();
                    }
                    outFile = null;
                    //计算文件的哈希值SHA1
                    SHA1Managed SHA1Runtime = new SHA1Managed();
                    FileStream outFileA = new FileStream(OutStreamFile, FileMode.Open, FileAccess.Read);
                    string FileOutHash = BitConverter.ToString(SHA1Runtime.ComputeHash(outFileA));
                    //输出文件列表

                    FileStream Swrt = new FileStream(OutIndexFile, FileMode.Create, FileAccess.Write);
                    IndexFile.Hash = FileOutHash;
                    IndexFile.HashType = "SHA1";
                    IndexFile.Length =  outFileA.Length;
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(Swrt, IndexFile);
                    Swrt.Close();
                    outFileA.Close();
                    Swrt = null;
                    outFileA = null;
                }
                catch
                {
                    if (IgnoreError == true)
                    {
                        return 5;
                    }
                }
                return 3;
            }

        }
        #endregion

        #region 文件拆箱功能实现类
        /// <summary>
        /// 文件拆箱功能实现类
        /// </summary>
        public class FileOutBox
        {
            // 二进制流数据
            private MemoryStream InBoxStream = new MemoryStream();
            // 文件列表（返回查询使用的ID）
            private KrIndexFile IndexFile;
            /// <summary>
            /// 缓冲长度
            /// </summary>
            public int BufferLength { get; set; }
            // 是否已经初始化
            private bool IsInit = false;

            /// <summary>
            /// 文件拆箱功能实现类
            /// </summary>
            public FileOutBox()
            {
                BufferLength = 8192;
            }
            /// <summary>
            /// 文件拆箱功能实现类
            /// </summary>
            /// <param name="Bufferlength">文件读入写出所使用的缓冲大小，较多大文件请使用较高的缓冲，较多小文件，请使用较低的缓冲。在适当的条件下，缓冲将使文件读入读出加速。</param>
            public FileOutBox(int Bufferlength)
            {
                BufferLength = Bufferlength;
            }

            /// <summary>
            /// 初始化功能
            /// </summary>
            /// <param name="datInBoxStream">装箱文件的流数据</param>
            /// <param name="datIndexStream">装箱文件的索引的流数据</param>
            /// <returns></returns>
            public int Init(MemoryStream datInBoxStream, MemoryStream datIndexStream)
            {
                if (datInBoxStream.Length < 1)
                {
                    return 9;
                }
                if (datIndexStream.Length < 1)
                {
                    return 12;
                }
                BinaryFormatter bf = new BinaryFormatter();
                IndexFile = (KrIndexFile)bf.Deserialize(datIndexStream);
                datIndexStream.Close();
                if (IndexFile.Version != "__KrSpokenPackV1.00")
                {
                    return 13;
                }
                InBoxStream = datInBoxStream;
                datInBoxStream.Position = 0;
                SHA1Managed SHA1Cryp = new SHA1Managed();
                if (BitConverter.ToString(SHA1Cryp.ComputeHash(InBoxStream)) != IndexFile.Hash)
                {
                    //debugstr = BoxfileHash + "\n" + BitConverter.ToString(SHA1Cryp.ComputeHash(InBoxStream));
                    return 14;
                }

                if (IndexFile.FileList.Count < 1)
                {
                    return 15;
                }
                IsInit = true;
                return 8;
            }

            /// <summary>
            /// 初始化功能
            /// </summary>
            /// <param name="datInBoxFile">装箱文件的存放位置</param>
            /// <param name="datIndexFile">装箱文件的索引的存放位置</param>
            /// <returns></returns>
            public int Init(string datInBoxFile, string datIndexFile)
            {
                if (File.Exists(datInBoxFile) == false)
                {
                    return 10;
                }
                if (File.Exists(datIndexFile) == false)
                {
                    return 11;
                }

                FileStream stmIndexFile = new FileStream(datIndexFile, FileMode.OpenOrCreate, FileAccess.Read);
                if (stmIndexFile.Length < 1)
                {
                    return 12;
                }
                BinaryFormatter bf = new BinaryFormatter();
                IndexFile = (KrIndexFile)bf.Deserialize(stmIndexFile);
                stmIndexFile.Close();
                if (IndexFile.Version != "__KrSpokenPackV1.00")
                {
                    return 13;
                }
                using (FileStream aa = new FileStream(datInBoxFile, FileMode.Open, FileAccess.Read))
                {
                    byte[] buf = new byte[aa.Length];
                    aa.Read(buf, 0, buf.Length);
                    InBoxStream.Write(buf, 0, buf.Length);
                    InBoxStream.Position = 0;
                }
                SHA1Managed SHA1Cryp = new SHA1Managed();
                if (BitConverter.ToString(SHA1Cryp.ComputeHash(InBoxStream)) != IndexFile.Hash)
                {
                    // debugstr = BoxfileHash + "\n" + BitConverter.ToString(SHA1Cryp.ComputeHash(InBoxStream));
                    return 14;
                }
                if (IndexFile.FileList.Count < 1)
                {
                    return 15;
                }
                IsInit = true;
                return 8;
            }

            /// <summary>
            /// 初始化功能
            /// </summary>
            /// <param name="datInBoxFile">装箱文件的存放位置</param>
            /// <param name="datIndexStream">装箱文件的索引的流数据</param>
            /// <returns></returns>
            public int Init(string datInBoxFile, MemoryStream datIndexStream)
            {
                if (File.Exists(datInBoxFile) == false)
                {
                    return 10;
                }
                if (datIndexStream.Length < 1)
                {
                    return 12;
                }
                BinaryFormatter bf = new BinaryFormatter();
                IndexFile = (KrIndexFile)bf.Deserialize(datIndexStream);
                datIndexStream.Close();
                if (IndexFile.Version != "__KrSpokenPackV1.00")
                {
                    return 13;
                }
                using (FileStream aa = new FileStream(datInBoxFile, FileMode.Open, FileAccess.Read))
                {
                    byte[] buf = new byte[aa.Length];
                    aa.Read(buf, 0, buf.Length);
                    InBoxStream.Write(buf, 0, buf.Length);
                    InBoxStream.Position = 0;
                }
                SHA1Managed SHA1Cryp = new SHA1Managed();
                if (BitConverter.ToString(SHA1Cryp.ComputeHash(InBoxStream)) != IndexFile.Hash)
                {
                    // debugstr = BoxfileHash + "\n" + BitConverter.ToString(SHA1Cryp.ComputeHash(InBoxStream));
                    return 14;
                }
                if (IndexFile.FileList.Count < 1)
                {
                    return 15;
                }
                IsInit = true;
                return 8;
            }

            /// <summary>
            /// 返回指定文件名的文件流，如果不存在文件，则该流为空。
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public Stream GetFileStream(string fileName)
            {
                // fileName = fileName.ToLower();
                Stream FStm = new MemoryStream();
                if (IsInit == false)
                {
                    return FStm;
                }
                if (IndexFile.FileList.Contains(fileName) == false)
                {
                    return FStm;
                }
                else
                {
                    try
                    {
                        long[] tmpfileIndex = (long[])IndexFile.FileList[fileName];
                        long startIndex, fLength;
                        startIndex = tmpfileIndex[0];
                        fLength = tmpfileIndex[1];

                        if (fLength <= BufferLength)
                        {
                            byte[] buffer = new byte[fLength];
                            InBoxStream.Position = startIndex;
                            InBoxStream.Read(buffer, 0, buffer.Length);
                            FStm.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            InBoxStream.Position = startIndex;
                            while (fLength > 0)
                            {
                                if (fLength - BufferLength >= 0)
                                {
                                    byte[] buffer = new byte[BufferLength];
                                    InBoxStream.Read(buffer, 0, BufferLength);
                                    FStm.Write(buffer, 0, BufferLength);
                                    fLength -= BufferLength;
                                }
                                else
                                {
                                    byte[] buffer = new byte[fLength];
                                    InBoxStream.Read(buffer, 0, buffer.Length);
                                    FStm.Write(buffer, 0, buffer.Length);
                                    fLength = 0;
                                }
                            }
                        }

                    }
                    catch
                    {
                        return FStm;
                    }
                }
                FStm.Position = 0;
                return FStm;
            }

            /// <summary>
            /// 输入一个流，返回文件的文本
            /// </summary>
            /// <param name="encoding">文件编码</param>
            /// <param name="datFileStream">文件流</param>
            /// <returns></returns>
            public string GetFileText(Stream datFileStream, Encoding _encoding)
            {
                if (IsInit == false)
                {
                    return "";
                }
                if (datFileStream.Length < 1)
                {
                    return "";
                }
                StreamReader TextStreamReader = new StreamReader(datFileStream, _encoding);
                return TextStreamReader.ReadToEnd();
            }
            /// <summary>
            /// 输入一个文件名，返回文件的文本，如果不存在文件则为空
            /// </summary>
            /// <param name="_encoding">文件编码</param>
            /// <param name="datFilename">文件名</param>
            /// <returns></returns>
            public string GetFileText(string datFilename, Encoding _encoding)
            {

                if (IsInit == false)
                {
                    return "";
                }
                Stream datFileStream = GetFileStream(datFilename);
                if (datFileStream.Length < 1)
                {
                    return "";
                }
                StreamReader TextStreamReader = new StreamReader(datFileStream, _encoding);
                return TextStreamReader.ReadToEnd();
            }

            /// <summary>
            /// 保存流数据到文件，根据文件名搜索流数据
            /// </summary>
            /// <param name="datFilename">文件名（箱文件内）</param>
            /// <param name="datFileFullPath">完整的保存位置</param>
            /// <returns></returns>
            public int SaveToFile(string datFilename, string datFileFullPath)
            {
                Stream datFileStream = GetFileStream(datFilename);
                if (datFileStream.Length < 1)
                {
                    return 15;
                }
                FileStream Fstm = new FileStream(datFileFullPath, FileMode.Create, FileAccess.Write);
                long dFStmLength = datFileStream.Length;
                if (dFStmLength <= BufferLength)
                {
                    byte[] buffer = new byte[dFStmLength];
                    datFileStream.Read(buffer, 0, (Int32)dFStmLength);
                    Fstm.Write(buffer, 0, (Int32)dFStmLength);
                    Fstm.Close();
                }
                else
                {
                    while (dFStmLength > 0)
                    {
                        if (dFStmLength >= BufferLength)
                        {
                            byte[] buffer = new byte[BufferLength];
                            datFileStream.Read(buffer, 0, BufferLength);
                            Fstm.Write(buffer, 0, BufferLength);
                            dFStmLength -= BufferLength;
                        }
                        else
                        {
                            byte[] buffer = new byte[dFStmLength];
                            datFileStream.Read(buffer, 0, buffer.Length);
                            Fstm.Write(buffer, 0, buffer.Length);
                            dFStmLength -= buffer.Length;
                            Fstm.Close();
                        }
                    }
                }
                return 20;
            }
            /// <summary>
            /// 保存流数据到文件，根据输入的流
            /// </summary>
            /// <param name="datFileStream">输入的数据流</param>
            /// <param name="datFileFullPath">完整的保存位置</param>
            /// <returns></returns>
            public int SaveToFile(Stream datFileStream, string datFileFullPath)
            {
                if (datFileStream.Length < 1)
                {
                    return 15;
                }
                FileStream Fstm = new FileStream(datFileFullPath, FileMode.Create, FileAccess.Write);
                long dFStmLength = datFileStream.Length;
                if (dFStmLength <= BufferLength)
                {
                    byte[] buffer = new byte[dFStmLength];
                    datFileStream.Read(buffer, 0, (Int32)dFStmLength);
                    Fstm.Write(buffer, 0, (Int32)dFStmLength);
                    Fstm.Close();
                }
                else
                {
                    while (dFStmLength > 0)
                    {
                        if (dFStmLength >= BufferLength)
                        {
                            byte[] buffer = new byte[BufferLength];
                            datFileStream.Read(buffer, 0, BufferLength);
                            Fstm.Write(buffer, 0, BufferLength);
                            dFStmLength -= BufferLength;
                        }
                        else
                        {
                            byte[] buffer = new byte[dFStmLength];
                            datFileStream.Read(buffer, 0, buffer.Length);
                            Fstm.Write(buffer, 0, buffer.Length);
                            dFStmLength -= buffer.Length;
                            Fstm.Close();
                        }
                    }
                }
                return 20;
            }
            /// <summary>
            /// 判断文件是否存在
            /// </summary>
            /// <param name="fileName">文件名称</param>
            public bool FileExist(string fileName)
            {
                if (IsInit == false)
                {
                    return false;
                }
                return IndexFile.FileList.Contains(fileName);
            }
        }
        #endregion

        #region 索引文件集合类
        /// <summary>
        /// 索引文件集合类
        /// </summary>
        [Serializable]
        public class KrIndexFile
        {
            /// <summary>
            /// 文件列表
            /// </summary>
            public Hashtable FileList =new Hashtable();
            /// <summary>
            /// 头数据
            /// </summary>
            public string Header;
            /// <summary>
            /// 版本数据
            /// </summary>
            public string Version;
            /// <summary>
            /// 被索引文件的哈希值
            /// </summary>
            public string Hash;
            /// <summary>
            /// 被索引文件长度
            /// </summary>
            public long Length;
            /// <summary>
            /// 哈希值算法
            /// </summary>
            public string HashType;
        }
        #endregion

        #region 装箱类消息类型
        /// <summary>
        /// 装箱类消息类型
        /// </summary>
        public class FileInOutBoxMessage
        {
            /// <summary>
            /// 功能没有初始化
            /// </summary>
            public const int NoInit= -1;

            /// <summary>
            /// 装箱类初始化完毕
            /// </summary>
            public const int In_InitSuccess= 0;
            /// <summary>
            /// 指定被装箱文件目录不存在
            /// </summary>
            public const int In_NoPath=1;
            /// <summary>
            /// 指定被装箱文件目录不存在文件
            /// </summary>
            public const int In_NoFileIn=2;
            /// <summary>
            /// 装箱处理完毕
            /// </summary>
            public int In_InBoxSuccess=3;
            /// <summary>
            /// 读取文件错误
            /// </summary>
            public const int In_ErrByReadFile=4;
            /// <summary>
            /// 保存文件错误
            /// </summary>
            public const int In_ErrBySaveFile=5;

            /// <summary>
            /// 拆箱类初始化
            /// </summary>
            public const int Out_InitSuccess=8;
            /// <summary>
            /// 错误的箱文件流
            /// </summary>
            public const int Out_NoInboxStream=9;
            /// <summary>
            /// 未找到箱文件
            /// </summary>
            public const int Out_NoInboxFile=10;
            /// <summary>
            /// 未找到箱的索引文件
            /// </summary>
            public const int Out_NoIndexFile=11;
            /// <summary>
            /// 错误的箱文件索引
            /// </summary>
            public const int Out_ErrIndexFile=12;
            /// <summary>
            /// 未知的索引版本
            /// </summary>
            public const int Out_ErrIndex=13;
            /// <summary>
            /// 文件哈希值校验失败，可能该索引不是由这个文件生成的
            /// </summary>
            public const int Out_ErrInBoxFileHash = 14;
            /// <summary>
            /// 索引不存在文件
            /// </summary>
            public const int Out_ErrIndexFiles=15;
        }
        #endregion

        #region 包裹混淆模块
        /// <summary>
        /// 包裹混淆模块
        /// </summary>
        public class MixThePackUp
        {
            private bool IsInit = false;
            private FileStream Fs;
            private FileStream MixupData; //混合后的数据
            private FileStream MixupDataIndex; //混合后的数据
            private MixupIndexFile Mf=new MixupIndexFile();
            private int bufferLength = 16384; 
            /// <summary>
            /// 初始化类
            /// </summary>
            /// <param name="_PackFileHref">被混淆文件的位置</param>
            /// <param name="_IndexFileHref">被混淆文件的索引放置位置</param>
            /// <param name="_MxPackFileHref">混淆文件的位置</param>
            /// <returns></returns>
            public int Init(string _PackFileHref, string _IndexFileHref, string _MxPackFileHref)
            {
                if (File.Exists(_PackFileHref) == false)
                {
                    return 1;
                }
                Fs = new FileStream(_PackFileHref, FileMode.Open, FileAccess.Read);
                if(Fs.Length<10240){
                    return 2;
                }
                MixupDataIndex = new FileStream(_IndexFileHref, FileMode.Create, FileAccess.Write);
                MixupData = new FileStream(_MxPackFileHref, FileMode.Create, FileAccess.Write);
                IsInit = true;
                return 0;
            }

            public int OutTheFile()
            {
                if (IsInit == false)
                {
                    return -1;
                }
                try
                {
                    GetIndexFile(Fs.Length);
                    for (int i = 0; i < Mf.encdoeOrder.Length; i++)
                    {
                        long[] data = (long[])Mf.IndexTable[Mf.encdoeOrder[i]];
                        Fs.Position = data[0];
                        long part_length = data[1];
                        Mf.dIndexTable.Add(i, data);
                        long n_part_length = 0;
                        while (n_part_length < part_length)
                        {
                            if (part_length - n_part_length >= bufferLength)
                            {
                                byte[] buffer = new byte[bufferLength];
                                Fs.Read(buffer, 0, bufferLength);
                                MixupData.Write(buffer, 0, bufferLength);
                                n_part_length += bufferLength;
                            }
                            else
                            {
                                byte[] buffer = new byte[(int)(part_length - n_part_length)];
                                Fs.Read(buffer, 0, (int)(part_length - n_part_length));
                                MixupData.Write(buffer, 0, (int)(part_length - n_part_length));
                                n_part_length = part_length;
                            }
                        }
                    }
                    MixupData.Close();
                    Fs.Close();
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(MixupDataIndex, Mf);//系列化索引数据
                    MixupDataIndex.Close();
                    return 3;
                }
                catch
                {
                    return 4;
                }

            }

            // 生成索引
            private void GetIndexFile(long FileLengh)
            {
                long n_FileLong=0;
                int i = 0;
                long a = 0;
                a = FileLengh / 10 - 640;
                Random r = new Random();
                while (n_FileLong < FileLengh)
                {  
                    long[] Data = new long[2];
                    Data[0] = n_FileLong;
                    long readlen=(long)(a + r.Next(1280));
                    if (n_FileLong + readlen > FileLengh)
                    {
                        Data[1] = FileLengh - n_FileLong;
                        n_FileLong = FileLengh;
                    }
                    else
                    {
                        n_FileLong += readlen;
                        Data[1] = readlen;
                    }
                    Mf.IndexTable.Add(i, Data);
                    i++;
                }
                //创建排序
                int[] eOrder = new int[Mf.IndexTable.Count];
                int[] dOrder = new int[Mf.IndexTable.Count];
                for (i = 0; i < Mf.IndexTable.Count; i++)
                {
                    eOrder[i] = i;
                }
                int l=0,v=0,m=0;
                for (i = 0; i < 1000; i++)
                {
                    v = r.Next(eOrder.Length);
                    m = r.Next(eOrder.Length);
                    l = eOrder[v];
                    eOrder[v] = eOrder[m];
                    eOrder[m] = l;
                }
                for (i = 0; i < dOrder.Length; i++)
                {
                    dOrder[eOrder[i]] =i ;
                }
                Mf.encdoeOrder = eOrder;
                Mf.decdoeOrder = dOrder;
                return;
            }            
        }
        #endregion

        #region 包裹反混淆模块


        /// <summary>
        /// 包裹反混淆模块
        /// </summary>
        public class ReBuiltThePack
        {
            private bool IsInit = false;
            private MemoryStream Fs=new MemoryStream(); //输出文件
            private FileStream ReBuiltData; //混合后的数据
            private MemoryStream MixupDataIndex; //混合后的数据索引文件
            private MixupIndexFile Mf = new MixupIndexFile();
            private int bufferLength = 16384; 
            /// <summary>
            /// 初始化类
            /// </summary>
            /// <param name="_PackFileHref">输出数据位置</param>
            /// <param name="_IndexFileHref">混淆文件的索引放置位置</param>
            /// <param name="_MxPackFileHref">混淆文件的位置</param>
            /// <returns></returns>
            public int Init(MemoryStream _IndexFile, FileStream _MxPackFile)
            {
                if (_IndexFile.Length < 1 || _MxPackFile.Length < 2)
                {
                    return 1;
                }
                MixupDataIndex = _IndexFile;
                ReBuiltData = _MxPackFile;
                IsInit = true;
                return 5;
            }

            public int OutTheFile(out MemoryStream _OutPackStream)
            {
                if (IsInit == false)
                {
                    _OutPackStream = Fs;
                    return -1;
                }
                try
                {
                    //反系列化
                    MixupDataIndex.Position = 0;
                    BinaryFormatter bf = new BinaryFormatter();
                    MixupIndexFile Mf = (MixupIndexFile)bf.Deserialize(MixupDataIndex);
                    MixupDataIndex.Close();
                    for (int i = 0; i < Mf.decdoeOrder.Length; i++)
                    {
                        long[] data = (long[])Mf.dIndexTable[i];
                        Fs.Position = data[0];
                        long part_length = data[1];
                        long n_part_length = 0;
                        while (n_part_length < part_length)
                        {
                            if (part_length - n_part_length >= bufferLength)
                            {
                                byte[] buffer = new byte[bufferLength];
                                ReBuiltData.Read(buffer, 0, bufferLength);
                                Fs.Write(buffer, 0, bufferLength);
                                n_part_length += bufferLength;
                            }
                            else
                            {
                                byte[] buffer = new byte[(int)(part_length - n_part_length)];
                                ReBuiltData.Read(buffer, 0, (int)(part_length - n_part_length));
                                Fs.Write(buffer, 0, (int)(part_length - n_part_length));
                                n_part_length = part_length;
                            }
                        }
                    }
                    ReBuiltData.Close();
                    _OutPackStream = Fs;
                    return 6;
                }
                catch
                {
                    _OutPackStream =Fs;
                    return 7;
                }
            }
        }


        #endregion

        #region 包裹混淆索引文件，系列化用类
        /// <summary>
        /// 裹混淆索引文件
        /// </summary>
         [Serializable]
        public class MixupIndexFile{
             /// <summary>
             /// 加密模块数据
             /// </summary>
             public Hashtable IndexTable=new Hashtable();
             /// <summary>
             /// 解密模块数据
             /// </summary>
             public Hashtable dIndexTable = new Hashtable();
             /// <summary>
             /// 加密排序表
             /// </summary>
             public int[] decdoeOrder;
             /// <summary>
             /// 解密排序表
             /// </summary>
             public int[] encdoeOrder;
        }
        #endregion

        #region 包裹混淆模块消息类型
        /// <summary>
        /// 包裹混淆模块消息类型
        /// </summary>
        public class MixUpMessage
        {
            /// <summary>
            /// 功能没有初始化
            /// </summary>
            public const int NoInit = -1;
            /// <summary>
            /// 包裹混淆模块初始化完毕
            /// </summary>
            public const int Mixup_InitSuccess = 0;
            /// <summary>
            /// 文件未找到！
            /// </summary>
            public const int Mixup_FileNotExists = 1;
            /// <summary>
            /// 文件太小
            /// </summary>
            public const int Mixup_FileTooSmall = 2;
            /// <summary>
            /// 混淆完成！
            /// </summary>
            public const int Mixup_Success = 3;
            /// <summary>
            /// 混淆过程中发生错误 
            /// </summary>
            public const int Mixup_Error = 4;
            /// <summary>
            /// 包裹反混淆模块初始化完毕
            /// </summary>
            public const int Rebuilt_InitSuccess = 5;
            /// <summary>
            /// 反混淆完成！
            /// </summary>
            public const int Rebuilt_Success = 6;
            /// <summary>
            /// 反混淆过程中发生错误 
            /// </summary>
            public const int Rebuilt_Error = 7;

        }
        #endregion
    }
}