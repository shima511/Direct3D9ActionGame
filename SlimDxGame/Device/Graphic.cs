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
            }catch(Direct3D9Exception ex){
                System.Diagnostics.Debug.Assert(false, ex.Message + "初期化失敗");
            }
        }

        public void Terminate()
        {
            D3DSprite.Dispose();
            D3DDevice.Dispose();
            direct3D.Dispose();
        }
    }
}
