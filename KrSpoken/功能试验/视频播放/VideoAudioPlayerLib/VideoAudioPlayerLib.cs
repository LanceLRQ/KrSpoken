using System;
using System.Text;
using DirectShowLib;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace KrSpoken.VideoAudioPlayerLib
{
    /// <summary>
    /// 听雨口语视频播放模块 => 视频播放完成回调 
    /// </summary>
    public delegate void KrVideoPlayerPlayCompletedHandle();
    /// <summary>
    /// 听雨口语视频播放模块
    /// </summary>
    public class VideoPlayer
    {
        /// <summary>
        /// 视频播放完成回调事件
        /// </summary>
        public event KrVideoPlayerPlayCompletedHandle PlayCompleted;

        #region PlayWnd部分
        internal enum PlayState
        {
            Stopped,
            Paused,
            Running,
            Init
        };

        internal enum MediaType
        {
            Audio,
            Video
        }

        private Panel PlayerPanel;
        private const int WMGraphNotify = 0x0400 + 13;
        private const int VolumeFull = 0;
        private const int VolumeSilence = -10000;

        private Thread Listener;

        private IGraphBuilder graphBuilder = null;
        private IMediaControl mediaControl = null;
        private IMediaEventEx mediaEventEx = null;
        private IVideoWindow videoWindow = null;
        private IBasicAudio basicAudio = null;
        private IBasicVideo basicVideo = null;
        private IMediaSeeking mediaSeeking = null;
        private IMediaPosition mediaPosition = null;
        private IVideoFrameStep frameStep = null;

        public string filename = string.Empty;
        public bool isAudioOnly = false;
        public bool isFullScreen = false;
        public int currentVolume = VolumeFull;
        private PlayState currentState = PlayState.Stopped;
        public double currentPlaybackRate = 1.0;
        private IntPtr hDrain = IntPtr.Zero;

#if DEBUG
        private DsROTEntry rot = null;
#endif

        /// <summary>
        /// 获取播放总时间(String == 00:00)
        /// </summary>
        /// <returns></returns>
        public string getDurationTime(){
            if (mediaPosition != null)
            {
                double a = 0;
                mediaPosition.get_Duration(out a);
                
                int s = (int)a;
                int h = s / 3600;
                int m = (s - (h * 3600)) / 60;
                s = s - (h * 3600 + m * 60);
                return String.Format("{0:D2}:{1:D2}", m, s);
            }
            else
            {
                return "00:00:00";
            }
        }

        /// <summary>
        /// 设置播放位置(double:秒)
        /// </summary>
        /// <param name="p">设置时间</param>
        public void setPosition(double p)
        {
          DsLong dl=  DsLong.FromInt64((Int64)(p*10000000));
          mediaSeeking.SetPositions(dl, AMSeekingSeekingFlags.AbsolutePositioning, null, AMSeekingSeekingFlags.NoPositioning);
        }

        /// <summary>
        /// 获取播放位置总长(double:秒)
        /// </summary>
        /// <returns></returns>
        public double getPosition()
        {
            long a = 0, b = 0;
            mediaSeeking.GetPositions(out a, out b);
            return (double)b / (double)10000000;
        }

        /// <summary>
        /// 获取播放位置(double:秒)
        /// </summary>
        /// <returns></returns>
        public double getCurretPosition()
        {
            long a = 0, b = 0;
            mediaSeeking.GetPositions(out a, out b);
            return (double)a / (double)10000000;
        }

        /// <summary>
        /// 获取播放总时间(Int)
        /// </summary>
        /// <returns></returns>
        public int getDurationTimeInt()
        {
            if (mediaPosition != null)
            {
                double a = 0;
                mediaPosition.get_Duration(out a);
                return (int)a;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取播放总时间(Double)
        /// </summary>
        /// <returns></returns>
        public double getDurationTimeDouble()
        {
            if (mediaPosition != null)
            {
                double a = 0;
                mediaPosition.get_Duration(out a);
                return a;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取当前播放时间(String == 00:00)
        /// </summary>
        /// <returns></returns>
        public string getCurrentPositionTime()
        {
            if (mediaPosition != null)
            {
                double a = 0;
                mediaPosition.get_CurrentPosition(out a);
                int s = (int)a;
                int h = s / 3600;
                int m = (s - (h * 3600)) / 60;
                s = s - (h * 3600 + m * 60);
                return String.Format("{0:D2}:{1:D2}", m, s);
            }
            else
            {
                return "00:00:00";
            }
        }

        /// <summary>
        /// 获取当前播放时间(Int)
        /// </summary>
        /// <returns></returns>
        public int getCurrentPositionTimeInt()
        {
            if (mediaPosition != null)
            {
                double a = 0;
                mediaPosition.get_CurrentPosition(out a);
                return (int)a;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取当前播放时间(Double)
        /// </summary>
        /// <returns></returns>
        public double getCurrentPositionTimeDouble()
        {
            if (mediaPosition != null)
            {
                double a = 0;
                mediaPosition.get_CurrentPosition(out a);
                return a;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 新建一个播放
        /// </summary>
        /// <param name="fileName">wmv文件位置</param>
        /// <param name="_PlayerPanel">播放承载的panel</param>
        public void OpenClip(string fileName,Panel _PlayerPanel)
        {
            try
            {
                // If no filename specified by command line, show file open dialog
                if (this.filename == string.Empty)
                {
                    this.filename = fileName;
                    if (this.filename == string.Empty)  return;
                }
                // Reset status variables
                this.currentState = PlayState.Stopped;
                this.currentVolume = VolumeFull;
                this.PlayerPanel = _PlayerPanel;
                // Start playing the media file
                PlayMovieInWindow(this.filename);
                //启动监听事件线程 
                Listener = new Thread(HandleGraphEvent);
                Listener.IsBackground = true;
                Listener.Start();
            }
            catch
            {
                CloseClip();
            }
        }

        //初始化播放器
        private void PlayMovieInWindow(string filename)
        {
            int hr = 0;
            if (filename == string.Empty)   return;
            this.graphBuilder = (IGraphBuilder)new FilterGraph();
            // Have the graph builder construct its the appropriate graph automatically
            hr = this.graphBuilder.RenderFile(filename, null);
            DsError.ThrowExceptionForHR(hr);
            // QueryInterface for DirectShow interfaces
            this.mediaControl = (IMediaControl)this.graphBuilder;
            this.mediaEventEx = (IMediaEventEx)this.graphBuilder;
            this.mediaSeeking = (IMediaSeeking)this.graphBuilder;
            this.mediaPosition = (IMediaPosition)this.graphBuilder;
            // Query for video interfaces, which may not be relevant for audio files
            this.videoWindow = this.graphBuilder as IVideoWindow;
            this.basicVideo = this.graphBuilder as IBasicVideo;
            // Query for audio interfaces, which may not be relevant for video-only files
            this.basicAudio = this.graphBuilder as IBasicAudio;
            // Is this an audio-only file (no video component)?
            CheckVisibility();
            // Have the graph signal event via window callbacks for performance
            hr = this.mediaEventEx.SetNotifyWindow(PlayerPanel.Handle, WMGraphNotify, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);
            if (!this.isAudioOnly)
            {
                // Setup the video window
                hr = this.videoWindow.put_Owner(PlayerPanel.Handle);
                DsError.ThrowExceptionForHR(hr);
                hr = this.videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipSiblings | WindowStyle.ClipChildren);
                DsError.ThrowExceptionForHR(hr);
                hr = InitVideoWindow(1, 1);
                DsError.ThrowExceptionForHR(hr);
                GetFrameStepInterface();
            }
            else
            {
                // Initialize the default player size and enable playback menu items
                hr = InitPlayerWindow();
                DsError.ThrowExceptionForHR(hr);
            }
            this.isFullScreen = false;
            this.currentPlaybackRate = 1.0;
#if DEBUG
            rot = new DsROTEntry(this.graphBuilder);
#endif
            // Run the graph to play the media file
            hr = this.mediaControl.Run();
            DsError.ThrowExceptionForHR(hr);
            this.currentState = PlayState.Running;
        }

        /// <summary>
        /// 关闭播放
        /// </summary>
        public void CloseClip()
        {
            int hr = 0;
            // Stop media playback
            if (this.mediaControl != null)
                hr = this.mediaControl.Stop();
            // Clear global flags
            this.currentState = PlayState.Stopped;
            this.isAudioOnly = true;
            this.isFullScreen = false;
            // Free DirectShow interfaces
            CloseInterfaces();
            // Clear file name to allow selection of new file with open dialog
            this.filename = string.Empty;
            // No current media state
           this.currentState = PlayState.Init;
            InitPlayerWindow();
        }

        //初始化视频播放(内部函数)
        private int InitVideoWindow(int nMultiplier, int nDivider)
        {
            int hr = 0;
            int lHeight, lWidth;
            if (this.basicVideo == null)
                return 0;
            // Read the default video size
            hr = this.basicVideo.GetVideoSize(out lWidth, out lHeight);
            if (hr == DsResults.E_NoInterface)            return 0;
            // Account for requests of normal, half, or double size
            lWidth = lWidth * nMultiplier / nDivider;
            lHeight = lHeight * nMultiplier / nDivider;
            hr = this.videoWindow.SetWindowPosition(0, 0, lWidth, lHeight);
            return hr;
        }

        //移动窗口(内部函数)
        private void MoveVideoWindow()
        {
            int hr = 0;
            // Track the movement of the container window and resize as needed
            if (this.videoWindow != null)
            {
                hr = this.videoWindow.SetWindowPosition(0,0,640,480);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        //(内部函数)
        private void CheckVisibility()
        {
            int hr = 0;
            OABool lVisible;

            if ((this.videoWindow == null) || (this.basicVideo == null))
            {
                // Audio-only files have no video interfaces.  This might also
                // be a file whose video component uses an unknown video codec.
                this.isAudioOnly = true;
                return;
            }
            else
            {
                // Clear the global flag
                this.isAudioOnly = false;
            }
            hr = this.videoWindow.get_Visible(out lVisible);
            if (hr < 0)
            {
                // If this is an audio-only clip, get_Visible() won't work.
                // Also, if this video is encoded with an unsupported codec,
                // we won't see any video, although the audio will work if it is
                // of a supported format.
                if (hr == unchecked((int)0x80004002)) //E_NOINTERFACE
                {
                    this.isAudioOnly = true;
                }
                else
                    DsError.ThrowExceptionForHR(hr);
            }
        }

        /*
        // Some video renderers support stepping media frame by frame with the
        // IVideoFrameStep interface.  See the interface documentation for more
        // details on frame stepping.
        */

        //获取视频帧(内部函数)
        private bool GetFrameStepInterface()
        {
            int hr = 0;
            IVideoFrameStep frameStepTest = null;
            // Get the frame step interface, if supported
            frameStepTest = (IVideoFrameStep)this.graphBuilder;
            // Check if this decoder can step
            hr = frameStepTest.CanStep(0, null);
            if (hr == 0)
            {
                this.frameStep = frameStepTest;
                return true;
            }
            else
            {
                // BUG 1560263 found by husakm (thanks)...
                // Marshal.ReleaseComObject(frameStepTest);
                this.frameStep = null;
                return false;
            }
        }

        //关闭视频(内部函数)
        private void CloseInterfaces()
        {
            int hr = 0;
            try
            {
                lock (this)
                {
                    // Relinquish ownership (IMPORTANT!) after hiding video window
                    if (!this.isAudioOnly)
                    {
                        hr = this.videoWindow.put_Visible(OABool.False);
                        DsError.ThrowExceptionForHR(hr);
                        hr = this.videoWindow.put_Owner(IntPtr.Zero);
                        DsError.ThrowExceptionForHR(hr);
                    }
                    if (this.mediaEventEx != null)
                    {
                        hr = this.mediaEventEx.SetNotifyWindow(IntPtr.Zero, 0, IntPtr.Zero);
                        DsError.ThrowExceptionForHR(hr);
                    }
#if DEBUG
                    if (rot != null)
                    {
                        rot.Dispose();
                        rot = null;
                    }
#endif
                    // Release and zero DirectShow interfaces
                    if (this.mediaEventEx != null)
                        this.mediaEventEx = null;
                    if (this.mediaSeeking != null)
                        this.mediaSeeking = null;
                    if (this.mediaPosition != null)
                        this.mediaPosition = null;
                    if (this.mediaControl != null)
                        this.mediaControl = null;
                    if (this.basicAudio != null)
                        this.basicAudio = null;
                    if (this.basicVideo != null)
                        this.basicVideo = null;
                    if (this.videoWindow != null)
                        this.videoWindow = null;
                    if (this.frameStep != null)
                        this.frameStep = null;
                    if (this.graphBuilder != null)
                        Marshal.ReleaseComObject(this.graphBuilder); this.graphBuilder = null;
                    GC.Collect(); //回收内存
                }
            }
            catch
            {
            }
        }

        /*
         * Media Related methods
         */

        /// <summary>
        /// 暂停播放
        /// </summary>
        public void PauseClip()
        {
            if (this.mediaControl == null)
                return;
            // Toggle play/pause behavior
            if ((this.currentState == PlayState.Paused) || (this.currentState == PlayState.Stopped))
            {
                if (this.mediaControl.Run() >= 0)
                    this.currentState = PlayState.Running;
            }
            else
            {
                if (this.mediaControl.Pause() >= 0)
                    this.currentState = PlayState.Paused;
            }
        }
        /// <summary>
        /// 停止播放
        /// </summary>
        public void StopClip()
        {
            int hr = 0;
            DsLong pos = new DsLong(0);
            if ((this.mediaControl == null) || (this.mediaSeeking == null))
                return;
            // Stop and reset postion to beginning
            if ((this.currentState == PlayState.Paused) || (this.currentState == PlayState.Running))
            {
                hr = this.mediaControl.Stop();
                this.currentState = PlayState.Stopped;
                // Seek to the beginning
                hr = this.mediaSeeking.SetPositions(pos, AMSeekingSeekingFlags.AbsolutePositioning, null, AMSeekingSeekingFlags.NoPositioning);
                // Display the first frame to indicate the reset condition
                hr = this.mediaControl.Pause();
            }
        }

        /// <summary>
        /// 音量设置
        /// </summary>
        public int ToggleMute(bool IsSilence)
        {
            int hr = 0;
            if ((this.graphBuilder == null) || (this.basicAudio == null))
                return 0;
            // Read current volume
            hr = this.basicAudio.get_Volume(out this.currentVolume);
            if (hr == -1) //E_NOTIMPL
            {
                // Fail quietly if this is a video-only media file
                return 0;
            }
            else if (hr < 0)
            {
                return hr;
            }
            if (IsSilence == true)
                this.currentVolume = VolumeSilence;
            else
                this.currentVolume = VolumeFull;
            // Set new volume
            hr = this.basicAudio.put_Volume(this.currentVolume);
            //UpdateMainTitle();
            return hr;
        }

        //设置全屏(暂时无用)
        private int ToggleFullScreen()
        {
            int hr = 0;
            OABool lMode;
            // Don't bother with full-screen for audio-only files
            if ((this.isAudioOnly) || (this.videoWindow == null))
                return 0;
            // Read current state
            hr = this.videoWindow.get_FullScreenMode(out lMode);
            DsError.ThrowExceptionForHR(hr);
            if (lMode == OABool.False)
            {
                // Save current message drain
                hr = this.videoWindow.get_MessageDrain(out hDrain);
                DsError.ThrowExceptionForHR(hr);
                // Set message drain to application main window
                hr = this.videoWindow.put_MessageDrain(this.PlayerPanel.Handle);
                DsError.ThrowExceptionForHR(hr);
                // Switch to full-screen mode
                lMode = OABool.True;
                hr = this.videoWindow.put_FullScreenMode(lMode);
                DsError.ThrowExceptionForHR(hr);
                this.isFullScreen = true;
            }
            else
            {
                // Switch back to windowed mode
                lMode = OABool.False;
                hr = this.videoWindow.put_FullScreenMode(lMode);
                DsError.ThrowExceptionForHR(hr);
                // Undo change of message drain
                hr = this.videoWindow.put_MessageDrain(hDrain);
                DsError.ThrowExceptionForHR(hr);
                // Reset video window
                hr = this.videoWindow.SetWindowForeground(OABool.True);
                DsError.ThrowExceptionForHR(hr);
                // Reclaim keyboard focus for player application
                //this.Focus();
                this.isFullScreen = false;
            }
            return hr;
        }

        //视频播放状态监听线程
        private void HandleGraphEvent()
        {
            try
            {
                while (true)
                {
                    int hr = 0;
                    EventCode evCode;
                    IntPtr evParam1, evParam2;
                    // Make sure that we don't access the media event interface
                    // after it has already been released.
                    if (this.mediaEventEx == null)
                        return;
                    // Process all queued events
                    while (this.mediaEventEx.GetEvent(out evCode, out evParam1, out evParam2, 0) == 0)
                    {
                        // Free memory associated with callback, since we're not using it
                        hr = this.mediaEventEx.FreeEventParams(evCode, evParam1, evParam2);
                        // If this is the end of the clip, reset to beginning
                        if (evCode == EventCode.Complete)
                        {
                            //这里插入播放完成后的事件
                            PlayCompleted();
                            Listener.Abort();
                        }
                    }
                    Thread.Sleep(500);
                }
            }
            catch
            {
            }
        }

        /*
         * WinForm Related methods
         */
        //初始化窗口位置(失效)
        private int InitPlayerWindow()
        {
            return 0;
        }

        #endregion
    }

}
