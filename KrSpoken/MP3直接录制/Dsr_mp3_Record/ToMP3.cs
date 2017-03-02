using System;
using System.Collections.Generic;
using System.Text;
using WaveLib;
using Yeti.MMedia;
using Yeti.MMedia.Mp3;
using System.IO;

namespace Dsr_mp3_Record
{
    class ToMP3
    {
        private Mp3WriterConfig m_Config = null;
        private WaveFormat m_Format = new WaveFormat(44100, 16, 2); // initialize to any format
        public Mp3Writer writer;
        public ToMP3(string fileHref)
        {
            //这下面的暂时不用改的 {
            m_Format.wFormatTag = (short)WaveFormats.Pcm;   // PCM
            m_Format.nSamplesPerSec = 44100;        // 16KHz 录入数据的采样率
            m_Format.wBitsPerSample = 16;              // 16Bit 
            m_Format.nChannels = 1;                    // Mono
            m_Format.nBlockAlign = (short)(m_Format.nChannels * (m_Format.wBitsPerSample / 8));
            m_Format.nAvgBytesPerSec = m_Format.nBlockAlign * m_Format.nSamplesPerSec;
            m_Config = new Mp3WriterConfig(m_Format);
            //  }
            m_Config.Mp3Config.format.mp3.bOriginal  = 96;
            writer = new Mp3Writer(new FileStream(fileHref, FileMode.Create), m_Config);
        }

    }
}
