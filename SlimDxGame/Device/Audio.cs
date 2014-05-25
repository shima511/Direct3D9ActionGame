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
            XAudioDevice = new XAudio2();
            xaMasterVoice = new MasteringVoice(XAudioDevice);
        }

        public void Terminate()
        {
            xaMasterVoice.Dispose();
            XAudioDevice.Dispose();
        }
    }
}
