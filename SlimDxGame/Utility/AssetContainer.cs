using System;
using System.Collections.Generic;


namespace SlimDxGame.Utility
{
    class AssetContainer<TYPE> : Dictionary<string, TYPE>
        where TYPE : IDisposable
    {
        public void DeleteAllObject()
        {
            foreach (var obj in this.Values)
            {
                if (obj != null)
                {
                    obj.Dispose();
                }
            }
            this.Clear();
        }

        /// <summary>
        /// マップとして保存した名前からオブジェクトを探します。
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>アセットオブジェクト</returns>
        public TYPE GetValue(string name)
        {
            TYPE new_obj = default(TYPE);
            this.TryGetValue(name, out new_obj);
            return new_obj;
        }

        /// <summary>
        /// 不正なオブジェクトが含まれていた場合、trueを返します。
        /// </summary>
        /// <param name="object_names">不正なオブジェクトの名前のリスト</param>
        /// <returns>不正なオブジェクトが含まれているかどうか</returns>
        public bool IncludeInvalidObject(ref List<string> object_names)
        {
            bool include_invalid_object = false;
            foreach(var obj in this){
                if (obj.Value == null)
                {
                    object_names.Add(obj.Key + "\n");
                    include_invalid_object = true;
                }
            }
            return include_invalid_object;
        }
    }

}
