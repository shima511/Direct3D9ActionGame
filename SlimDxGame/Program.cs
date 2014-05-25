using System;

namespace SlimDxGame
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Core.Game game = new Core.Game(new Scene.Stage()))
            {
                game.Run();
            }
        }
    }
}
