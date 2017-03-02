Imports System.IO
Imports System.Threading

Namespace KrSpoken.VBAudioPlayer
    ''' <summary>
    ''' 声音播放模块播放完成回调托管
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub KrAudioPlayerComplete()

    ''' <summary>
    ''' 声音播放模块
    ''' </summary>
    Public Class KrAudioPlayer
        ''' <summary>
        ''' 声音播放完成的回调
        ''' </summary>
        Public Event PlayComplete As KrAudioPlayerComplete
        Private WaveStream As Stream
        Private aThread As Threading.Thread
        ''' <summary>
        ''' 播放一个声音
        ''' </summary>
        ''' <param name="_WaveStream">声音的流数据,Wav或PCM</param>
        ''' <remarks></remarks>
        Public Sub PlayWave(ByVal _WaveStream As Stream)
            WaveStream = _WaveStream
            aThread = New Thread(AddressOf PlayWaveInThread)
            aThread.IsBackground = True
            aThread.Start()
        End Sub
        Public Sub StopPlay()
            aThread.Abort()
        End Sub
        Private Sub PlayWaveInThread()
            My.Computer.Audio.Play(WaveStream, AudioPlayMode.WaitToComplete)
            RaiseEvent PlayComplete()
        End Sub
    End Class
End Namespace

