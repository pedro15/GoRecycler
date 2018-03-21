using UnityEngine;

namespace GoRecycler
{
    /// <summary>
    /// Extention methods for gameObject for easy use API
    /// </summary>
    public static class ObjectPoolingExtentions
    {
        /// <summary>
        /// Recycle the gameObject to the Object Pool if available
        /// </summary>
        public static void Recycle(this GameObject go)
        {
            GoRecyclerManager.Instance.RecycleGameObject(go);
        }
        
        /// <summary>
        /// Is the gameObject asigned on an Object Pool ?
        /// </summary>
        public static bool IsInObjectPool( this GameObject go)
        {
            return GoRecyclerManager.Instance.IsOnPool(go.GetInstanceID());
        }

        /// <summary>
        /// Returns the RecycleBin instance for the gameObject
        /// </summary>
        /// <returns>RcycleBin instance or null if gameObject is not asigned to any object pool</returns>
        public static GoRecycleBin GetRecycleBin(this GameObject go)
        {
            return GoRecyclerManager.Instance.GetRecycleBin(go);
        }
    }
}