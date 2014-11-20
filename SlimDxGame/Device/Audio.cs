using System;
using SlimDX.XAudio2;
using SlimDX.Multimedia;

namespace SlimDxGame.Device
{
    class Audio : Device
    {
        public XAudio2 XAudioDevice { get; private set; }
        private MasteringVoice xaMasterVoice;

        public void Initialize()
        {
            try
            {
                XAudioDevice = new XAudio2();
                xaMasterVoice = new MasteringVoice(XAudioDevice);
            }catch(XAudio2Exception){
                throw new Core.InitializeException("XAudio2の初期化に失敗しました。");
            }
        }

        public void Terminate()
        {
            if(xaMasterVoice != null) xaMasterVoice.Dispose();
            if(XAudioDevice != null) XAudioDevice.Dispose();
        }
    }
}
