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

        public TYPE GetValue(string name)
        {
            TYPE new_obj = default(TYPE);
            this.TryGetValue(name, out new_obj);
            return new_obj;
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
