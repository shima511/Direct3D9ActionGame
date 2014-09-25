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
        public bool IsActive { get; set; }
        /// <summary>
        /// リストへの登録・削除を決定するオブジェクト
        /// </summary>
        public Object.Base.Model CenterObject { get; set; }
        public Collision.Manager CollisionManager { private get; set; }
        /// <summary>
        /// 出現・削除の範囲
        /// </summary>
        readonly float SpawnRange = 50.0f;

        public void Update()
        {
            foreach (var item in this)
            {
                if (item.IsVisible)
                {
                    var length = (item.Position - CenterObject.Position).Length();
                    if (length > SpawnRange)
                    {
                        CollisionManager.Remove(item);
                        item.IsVisible = false;
                        item.IsActive = false;
                    }
                }
                else if(item.Spawnable)
                {
                    var length = (item.Position - CenterObject.Position).Length();
                    if (length < SpawnRange)
                    {
                        CollisionManager.Add(item);
                        item.IsVisible = true;
                        item.IsActive = true;
                    }
                }
            }
        }
    }
}
