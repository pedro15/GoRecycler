using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoRecycler
{
    /// <summary>
    /// Interface to use on Scripts that are in a GameObject associated on a Object Pool
    /// </summary>
    public interface IPooled
    {
        void OnSpawn(GoRecycleBin RecycleBin);
        void OnRecycle(GoRecycleBin RecycleBin);
    }
}