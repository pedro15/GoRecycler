using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoRecycler
{
    public static class ObjectPoolingExtentions
    {
        public static void Recycle(this GameObject go)
        {
           GoRecyclerUtility.Instance.RecycleGameObject(go);
        }
        
        public static bool IsInObjectPool( this GameObject go)
        {
            return  GoRecyclerUtility.Instance.IsOnPool(go.GetInstanceID());
        }

        public static GoRecycleBin GetRecycleBin(this GameObject go)
        {
            return GoRecyclerUtility.Instance.GetRecycleBin(go);
        }
    }
}