﻿using System;
using System.IO;
using System.Threading;
// 对DirectSound的支持
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;


namespace KrSpoken.KrMp3Recoder
{
    public delegate void Mp3RecorderErrorExcepton(string ErrMsg);
    public class KrMp3Recoder
    {
        public const int cNotifyNum = 16;       // 缓冲队列的数目
        private int mNextCaptureOffset = 0;      // 该次录音缓冲区的起始点
        private int mSampleCount = 0;            // 录制的样本数目
        private int mNotifySize = 0;             // 每次通知大小
        private int mBufferSize = 0;             // 缓冲队列大小
        private string mFileName = string.Empty;     // 文件名
        private FileStream mWaveFile = null;         // 文件流
        private BinaryWriter mWriter = null;         // 写文件
        private Capture mCapDev = null;              // 音频捕捉设备
        private CaptureBuffer mRecBuffer = null;     // 缓冲区对象
        private Notify mNotify = null;               // 消息通知对象
        private WaveFormat mWavFormat;                       // 录音的格式
        private Thread mNotifyThread = null;                 // 处理缓冲区消息的线程
        private AutoResetEvent mNotificationEvent = null;    // 通知事件
        private WavToMP3 ToAMP3;
        private bool IsAllowRecode = false; //是否允许开始录音

        //创建一个通知事件
        /// <summary>
        /// 当发生异常时发生的通知事件
        /// </summary>
        public event Mp3RecorderErrorExcepton ErrorExcepton;

        /// <summary>
        /// 设定录音结束后保存的文件,包括路径
        /// </summary>
        /// <param name="filename">保存wav文件的路径名</param>

        public void SetFileName(string filename)
        {
            mFileName = filename;
        }

        public KrMp3Recoder()
        {
            // 初始化音频捕捉设备
            if (InitCaptureDevice() == true) IsAllowRecode = true;
            // 设定录音格式
            mWavFormat = CreateWaveFormat();
        }

        /// <summary>
        /// 开始录音
        /// </summary>
        public void RecStart()
        {
            try
            {
               

                if (IsAllowRecode == false)
                {
                    return;
                }
                // 创建录音文件
                // CreateSoundFile();
                ToAMP3 = new WavToMP3(mFileName);
                // 创建一个录音缓冲区，并开始录音
                CreateCaptureBuffer();
                // 建立通知消息,当缓冲区满的时候处理方法
                InitNotifications();
                mRecBuffer.Start(true);
            }
            catch
            {
                ErrorExcepton("录音初始化失败");
                return;
            }
        }
        /// <summary>
        /// 停止录音
        /// </summary>
        public void RecStop()
        {
            try
            {
                // 关闭通知消息
                if (null != mNotificationEvent)
                    mNotificationEvent.Set();
                // 停止录音
                mRecBuffer.Stop();
                // 写入缓冲区最后的数据
                RecordCapturedData();

                ToAMP3.writer.Close();
                ToAMP3 = null;
                mNotifyThread.Abort();
                mNotifyThread = null;
            }
            catch
            {
                ErrorExcepton("录音初始化失败");
                return;
            }
        }
        /// <summary>
        /// 初始化录音设备,此处使用主录音设备.
        /// </summary>
        /// <returns>调用成功返回true,否则返回false</returns>
        private bool InitCaptureDevice()
        {
            // 获取默认音频捕捉设备
            CaptureDevicesCollection devices = new CaptureDevicesCollection(); // 枚举音频捕捉设备
            Guid deviceGuid = Guid.Empty;                                       // 音频捕捉设备的ID
            if (devices.Count > 0)
                deviceGuid = devices[0].DriverGuid;
            else
            {
               // ErrorExcepton("系统中没有音频捕捉设备");
                return false;
            }
            // 用指定的捕捉设备创建Capture对象
            try
            {
                mCapDev = new Capture(deviceGuid);
            }
            catch (DirectXException e)
            {
               // ErrorExcepton(e.ToString());
                return false;
            }
            return true;
        }
        /// <summary>
        /// 创建录音格式,此处使用16bit,16KHz,Mono的录音格式
        /// </summary>
        /// <returns>WaveFormat结构体</returns>
        private WaveFormat CreateWaveFormat()
        {
            WaveFormat format = new WaveFormat();
            format.FormatTag = WaveFormatTag.Pcm;   // PCM
            format.SamplesPerSecond = 22050;        // 16KHz
            format.BitsPerSample = 16;              // 16Bit
            format.Channels = 1;                    // Mono

            format.BlockAlign = (short)(format.Channels * (format.BitsPerSample / 8));
            format.AverageBytesPerSecond = format.BlockAlign * format.SamplesPerSecond;
            return format;
        }
        /// <summary>
        /// 创建录音使用的缓冲区
        /// </summary>
        private void CreateCaptureBuffer()
        {
            // 缓冲区的描述对象
            CaptureBufferDescription bufferdescription = new CaptureBufferDescription();
            if (null != mNotify)
            {
                mNotify.Dispose();
                mNotify = null;
            }
            if (null != mRecBuffer)
            {
                mRecBuffer.Dispose();
                mRecBuffer = null;
            }
            // 设定通知的大小,默认为1s钟
            mNotifySize = (1024 > mWavFormat.AverageBytesPerSecond / 8) ? 1024 : (mWavFormat.AverageBytesPerSecond / 8);
            mNotifySize -= mNotifySize % mWavFormat.BlockAlign;
            // 设定缓冲区大小
            mBufferSize = mNotifySize * cNotifyNum;
            // 创建缓冲区描述           
            bufferdescription.BufferBytes = mBufferSize;
            bufferdescription.Format = mWavFormat;           // 录音格式
            // 创建缓冲区
            mRecBuffer = new CaptureBuffer(bufferdescription, mCapDev);
            mNextCaptureOffset = 0;
        }
        /// <summary>
        /// 初始化通知事件,将原缓冲区分成16个缓冲队列,在每个缓冲队列的结束点设定通知点.
        /// </summary>
        /// <returns>是否成功</returns>
        private bool InitNotifications()
        {
            if (null == mRecBuffer)
            {
                ErrorExcepton("未创建录音缓冲区");
                return false;
            }
            // 创建一个通知事件,当缓冲队列满了就激发该事件.
            mNotificationEvent = new AutoResetEvent(false);
            // 创建一个线程管理缓冲区事件
            if (null == mNotifyThread)
            {
                mNotifyThread = new Thread(new ThreadStart(WaitThread));
                mNotifyThread.Start();
            }
            // 设定通知的位置
            BufferPositionNotify[] PositionNotify = new BufferPositionNotify[cNotifyNum + 1];
            for (int i = 0; i < cNotifyNum; i++)
            {
                PositionNotify[i].Offset = (mNotifySize * i) + mNotifySize - 1;
                PositionNotify[i].EventNotifyHandle = mNotificationEvent.Handle;
            }
            mNotify = new Notify(mRecBuffer);
            mNotify.SetNotificationPositions(PositionNotify, cNotifyNum);
            return true;
        }
        /// <summary>
        /// 将录制的数据写入wav文件
        /// </summary>
        private void RecordCapturedData()
        {
            byte[] CaptureData = null;
            int ReadPos;
            int CapturePos;
            int LockSize;
            mRecBuffer.GetCurrentPosition(out CapturePos, out ReadPos);
            LockSize = ReadPos - mNextCaptureOffset;
            if (LockSize < 0)
                LockSize += mBufferSize;
            // 对齐缓冲区边界,实际上由于开始设定完整,这个操作是多余的.
            LockSize -= (LockSize % mNotifySize);
            if (0 == LockSize)
                return;
            // 读取缓冲区内的数据
            CaptureData = (byte[])mRecBuffer.Read(mNextCaptureOffset, typeof(byte), LockFlag.None, LockSize);
            // 写入Mp3文件
            ToAMP3.writer.Write(CaptureData, 0, CaptureData.Length);
            // 更新已经录制的数据长度.
            mSampleCount += CaptureData.Length;
            // 移动录制数据的起始点,通知消息只负责指示产生消息的位置,并不记录上次录制的位置
            mNextCaptureOffset += CaptureData.Length;
            mNextCaptureOffset %= mBufferSize; // Circular buffer
        }
        /// <summary>
        /// 接收缓冲区满消息的处理线程
        /// </summary>
        private void WaitThread()
        {
            while (true)
            {
                // 等待缓冲区的通知消息
                mNotificationEvent.WaitOne(Timeout.Infinite, true);
                // 录制数据
                RecordCapturedData();
            }
        }
    }
}
