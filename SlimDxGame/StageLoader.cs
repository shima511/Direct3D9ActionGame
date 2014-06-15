using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame
{

    class StageLoader
    {
        /// <summary>
        /// ローディングが失敗した場合trueを返します
        /// </summary>
        public bool LoadFailed { get; private set; }

        /// <summary>
        /// 衝突判定が存在する静的なオブジェクトを取得します
        /// </summary>
        public List<Object.ICollisionObject> Collisions { get; private set; }

        /// <summary>
        /// プレイヤーの初期位置を取得します
        /// </summary>
        public SlimDX.Vector3 PlayerPosition { get; private set; }

        public StageLoader()
        {
            Collisions = new List<Object.ICollisionObject>();
        }

        Object.ICollisionObject CreateCollision(string[] values)
        {
            Object.ICollisionObject new_collision = null;
            switch (values[4])
            {
                case "Floor":
                    new_collision = new Object.Floor();
                    break;
                case "Ceiling":
                    new_collision = new Object.Ceiling();
                    break;
                case "RightWall":
                    new_collision = new Object.RightWall();
                    break;
                case "LeftWall":
                    new_collision = new Object.LeftWall();
                    break;
                default:
                    break;
            }
            return new_collision;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">ファイル名</param>
        public void Load(string filename)
        {
            LoadFailed = false;
            try
            {
                using (var sr = new System.IO.StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var values = line.Split(',');
                        var new_collision = CreateCollision(values);
                        Collisions.Add(new_collision);
                    }
                }
            }
            catch(System.Exception){
                LoadFailed = true;
            }
        }
    }
}
