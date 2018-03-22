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
            RecyclerManager.Instance.RecycleGameObject(go);
        }
        
        /// <summary>
        /// Is the gameObject asigned on an Object Pool ?
        /// </summary>
        public static bool IsOnPool( this GameObject go)
        {
            return RecyclerManager.Instance.IsOnPool(go.GetInstanceID());
        }
    }
}