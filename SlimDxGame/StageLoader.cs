using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace SlimDxGame
{
    class StageLoader
    {
        /// <summary>
        /// ローディングが失敗した場合trueを返します
        /// </summary>
        public bool LoadFailed { get; private set; }

        /// <summary>
        /// 衝突判定が存在する地形オブジェクトを取得します
        /// </summary>
        public List<Object.Ground.Base> GroundCollisions { get; private set; }

        /// <summary>
        /// プレイヤーの初期位置を取得します
        /// </summary>
        public SlimDX.Vector3 PlayerPosition { get; private set; }

        public StageLoader()
        {
            GroundCollisions = new List<Object.Ground.Base>();
        }

        void AttachGroundProperty(string str, out Object.Ground.Base ground)
        {
            switch (str)
            {
                case "Floor":
                    ground = new Object.Ground.Floor();
                    break;
                case "Ceiling":
                    ground = new Object.Ground.Ceiling();
                    break;
                case "RightWall":
                    ground = new Object.Ground.RightWall();
                    break;
                case "LeftWall":
                    ground = new Object.Ground.LeftWall();
                    break;
                default:
                    ground = null;
                    break;
            }
        }

        void SetGroundCollisionLine(string[] values, Object.Ground.Base ground)
        {
            var start_point = new SlimDX.Vector2(float.Parse(values[0]), float.Parse(values[1]));
            var terminal_point = new SlimDX.Vector2(float.Parse(values[2]), float.Parse(values[3]));
            ground.CollisionLine.StartingPoint = start_point;
            ground.CollisionLine.TerminalPoint = terminal_point;
        }

        Object.Ground.Base CreateGroundCollision(string[] values)
        {
            Object.Ground.Base new_ground;
            AttachGroundProperty(values[4], out new_ground);
            SetGroundCollisionLine(values, new_ground);
            return new_ground;
        }

        /// <summary>
        /// ステージを読み込みます
        /// </summary>
        /// <param name="filename">ファイル名</param>
        public void Load(string filename)
        {
            LoadFailed = false;
            try
            {
                using (var parser = new TextFieldParser(filename, System.Text.Encoding.GetEncoding("Shift_JIS")))
                {
                    parser.TextFieldType = FieldType.Delimited;

                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        string[] row = parser.ReadFields();
                        var new_ground = CreateGroundCollision(row);
                        GroundCollisions.Add(new_ground);
                    }
                }
            }
            catch(System.Exception e){
                LoadFailed = true;
#if DEBUG
                System.Windows.Forms.MessageBox.Show(e.Message);
#endif
            }
        }
    }
}
