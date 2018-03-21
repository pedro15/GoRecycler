using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoRecycler
{
    [System.Serializable]
    public class GoRecycleBin : System.Object
    {
        #region Fields
        
        public string Label { get { return label; } }

        /// <summary>
        /// Object pool label
        /// </summary>
        [SerializeField]
        private string label;


        /// <summary>
        /// Object pool Max Items
        /// </summary>
        [SerializeField]
        private int MaxItems;

        /// <summary>
        /// Object Pool Prefab
        /// </summary>
        [SerializeField]
        private GameObject Prefab;

        /// <summary>
        /// Object Pool Parent
        /// </summary>
        [SerializeField]
        private Transform PoolParent;
        
        /// <summary>
        /// How much elements must be allocated on initialization ?
        /// </summary>
        [SerializeField]
        private int PreAllocateCount;
        
        private Stack<GameObject> PooledObjects = new Stack<GameObject>();
        
        #endregion

        #region Public API 

        private int mCount = 0;

        /// <summary>
        /// Returns the object pool size
        /// </summary>
        public int ObjectCount
        {
            get
            {
                return mCount;
            }
        }

        /// <summary>
        /// Initializes the ObjectPool
        /// </summary>
        public void InitPool(MonoBehaviour component)
        {
            if (PreAllocateCount > 0)
            {
                component.StartCoroutine(initPool(component));
            }
        }
        
        /// <summary>
        /// Recycles an object to the object pool
        /// </summary>
        /// <param name="InstanceId">Instance id of the object</param>
        public void Recycle(GameObject go )
        {
            if (!PooledObjects.Contains(go) && PooledObjects.Count < MaxItems )
            {
                IPooled ip = go.GetComponent<IPooled>();
                if (ip != null)
                {
                    ip.OnRecycle(this);
                }
                go.SetActive(false);
                go.transform.position = Vector3.zero;
                PooledObjects.Push(go);
            }
        }
        
        /// <summary>
        /// Returns a GameObject from the Object pool
        /// </summary>
        /// <param name="Position">Target Position</param>
        /// <param name="Rotation">Target Rotation</param>
        /// <returns>Returns a Gameobject if it is aviable, otherwise returns null</returns>
        public GameObject Spawn(Vector3 Position, Quaternion Rotation)
        {
            if (MaxItems > 0 )
            {
                if ( PooledObjects.Count <= 0 )
                    RegisterPrefab();
                GameObject curr = GetFromPool(Position , Rotation);
                return curr;
            }
            return null; 
        }

        /// <summary>
        /// Clears the entrie recycle bin
        /// </summary>
        /// <param name="destroyObjects"></param>
        public void ClearRecycleBin(bool destroyObjects = false)
        {
            while (PooledObjects.Count > 0 )
            {
                GameObject obj = PooledObjects.Pop();
                if (destroyObjects)
                {
                    Object.Destroy(obj);
                    mCount--;
                }
            }
        }

        #endregion

        #region Private API 
        
        /// <summary>
        /// Iterator for initialize the object pool
        /// </summary>
        /// <returns></returns>
        private IEnumerator initPool(MonoBehaviour component)
        {
            for (int i = 0; i < PreAllocateCount; i++)
            {
                RegisterPrefab();
                yield return new WaitForEndOfFrame();
            }
            component.StopCoroutine("initPool");
        }


        /// <summary>
        /// Returns a GameObject from object pool and enables it
        /// </summary>
        /// <param name="pos">New Position</param>
        /// <param name="rot">New Rotation</param>
        private GameObject GetFromPool(Vector3 pos , Quaternion rot )
        {
            if (PooledObjects.Count > 0 )
            {
                GameObject g = PooledObjects.Pop();
                g.transform.position = pos;
                g.transform.rotation = rot;
                g.SetActive(true);
                

                IPooled ip = g.GetComponent<IPooled>();
                if (ip != null)
                {
                    ip.OnSpawn(this);
                }
                return g;
            }
            return null; 
        }

        /// <summary>
        /// Creates the prefab clone and adds it to the Object Pool
        /// </summary>
        /// <returns>Prefab clone</returns>
        private GameObject RegisterPrefab()
        {
            if (ObjectCount < MaxItems )
            {
                if (Prefab != null)
                {
                    GameObject Clone = Object.Instantiate(Prefab, Vector3.zero, Quaternion.identity);
                    
                    if (PoolParent != null && PoolParent.gameObject.scene.IsValid())
                    {
                        Clone.transform.parent = PoolParent;
                    }

                    Recycle(Clone);

                    mCount++;

                    return Clone;
                }
            }
            return null;
        }
        #endregion
    }
}