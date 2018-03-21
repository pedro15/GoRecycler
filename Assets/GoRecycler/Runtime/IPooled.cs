using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoRecycler
{
    public interface IPooled
    {
        void OnSpawn(GoRecycleBin RecycleBin);
        void OnRecycle(GoRecycleBin RecycleBin);
    }
}