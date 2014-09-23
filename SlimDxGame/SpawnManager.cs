using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame
{
    /// <summary>
    /// オブジェクトの出現・削除管理クラス
    /// </summary>
    class SpawnManager : List<Object.IFieldObject>, Component.IUpdateObject
    {
        /// <summary>
        /// リストへの登録・削除を決定するオブジェクト
        /// </summary>
        public Object.Base.Model CenterObject { get; set; }
        public List<Component.IUpdateObject> UpdateList { private get; set; }
        public List<List<Component.IDrawableObject>> Layers { private get; set; }
        public Collision.Manager CollisionManager { private get; set; }

        public void Update()
        {
            foreach (var item in this)
            {
                if (item.IsVisible)
                {
                    /*
                    UpdateList.Remove(item);
                    Layers[0].Remove(item);
                    CollisionManager.Remove(item);
                    **/
                }
                else
                {

                }
            }
        }
    }
}
