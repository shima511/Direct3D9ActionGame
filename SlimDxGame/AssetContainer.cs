using System;
using System.Collections.Generic;


namespace SlimDxGame
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
