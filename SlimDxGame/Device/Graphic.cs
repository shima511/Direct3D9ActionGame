using System;
using SlimDX.Direct3D9;

namespace SlimDxGame.Device
{
    class Graphic : Device
    {
        private SlimDX.Direct3D9.Direct3D direct3D;
        public SlimDX.Direct3D9.Device D3DDevice { get; private set; }
        public SlimDX.Direct3D9.Sprite D3DSprite { get; private set; }

        private System.Windows.Forms.Form form;

        public Graphic(System.Windows.Forms.Form f)
        {
            form = f;
        }

        public void Initialize()
        {
            direct3D = new Direct3D();
            try
            {
                D3DDevice = new SlimDX.Direct3D9.Device(direct3D, 0, DeviceType.Hardware, form.Handle, CreateFlags.HardwareVertexProcessing,
                    new SlimDX.Direct3D9.PresentParameters
                    {
                        BackBufferFormat = Format.X8R8G8B8,
                        BackBufferCount = 1,
                        BackBufferWidth = form.Width,
                        BackBufferHeight = form.Height,
                        Multisample = MultisampleType.None,
                        SwapEffect = SwapEffect.Discard,
                        EnableAutoDepthStencil = true,
                        AutoDepthStencilFormat = Format.D16,
                        PresentFlags = PresentFlags.DiscardDepthStencil,
                        PresentationInterval = PresentInterval.Default,
                        Windowed = true,
                        DeviceWindowHandle = form.Handle
                    });
                D3DSprite = new SlimDX.Direct3D9.Sprite(D3DDevice);
            }catch(Direct3D9Exception){
                throw new Core.InitializeException("Direct3Dの初期化に失敗しました。");
            }
        }

        public void Terminate()
        {
            if(D3DSprite != null) D3DSprite.Dispose();
            if(D3DDevice != null) D3DDevice.Dispose();
            if(direct3D != null) direct3D.Dispose();
        }
    }
}
