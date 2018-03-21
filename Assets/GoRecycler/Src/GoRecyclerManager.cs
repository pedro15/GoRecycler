using System.Collections.Generic;
using UnityEngine;
using GoRecycler.Core;

namespace GoRecycler
{
    [AddComponentMenu("GoRecycler/GoRecycler Manager")]
    public class GoRecyclerManager : MonoBehaviour
    {
        #region Singleton  

        private static GoRecyclerManager _instance;
        
        public static GoRecyclerManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<GoRecyclerManager>();
                }
                if (!_instance)
                {
                    Debug.LogError( "No "  + typeof(GoRecyclerManager).Name + " instance found in the scene");
                }
                return _instance;
            }
        }

        #endregion

        [SerializeField]
        private GoRecycleBin[] Pools;

        private Dictionary<int, int> InstanceidToPoolid = new Dictionary<int, int>();

        private Dictionary<string, int> NameToPoolId = new Dictionary<string, int>();

        private void Awake()
        {
            _instance = null; 
            for (int i = 0; i < Pools.Length; i++ )
            {
                if (!string.IsNullOrEmpty(Pools[i].Label))
                {
                    if (!NameToPoolId.ContainsKey(Pools[i].Label.ToLower()))
                    {

                        NameToPoolId.Add(Pools[i].Label.ToLower(), i);
                        Pools[i].InitPool(this);
                    }
                    else
                    {
                        Debug.LogWarning("[" + typeof(GoRecyclerManager).Name +
                        "] Duplicated label detected (" + Pools[i].Label
                        + "). Please provide diferent label names in your pools, duplicated pool will be ignored.");
                    }
                }else
                {
                    Debug.LogWarning("[" + typeof(GoRecyclerManager).Name +
                      "] Empty label detected, please specify a label for your Pools");
                }
            }
        }

        #region Private API

        /// <summary>
        /// Returns the pool index to given label
        /// </summary>
        /// <param name="label">Label</param>
        private int LabetoPoolid(string label)
        {
            int val = -1;

            if (NameToPoolId.ContainsKey(label.ToLower()))
            {
                NameToPoolId.TryGetValue(label, out val);
            }else
            {
                Debug.LogError("Trying to get a Pool with label: '" + label + "' That doesn't exists!");
                return -1;
            }
            return val;
        }

        /// <summary>
        /// Returns the pool index to given instance id
        /// </summary>
        /// <param name="InstanceId">Instance ID</param>
        /// <returns>Pool Index</returns>
        private int IdinstanceToPoolid(int InstanceId )
        {
            int val;
            return InstanceidToPoolid.TryGetValue(InstanceId, out val) ? val : -1;
        }

        /// <summary>
        /// Returns a GameObject from the Object pool.
        /// </summary>
        private GameObject GetPrefab(string PoolLabel , Vector3 Position , Quaternion Rotation)
        {
            int xid = LabetoPoolid(PoolLabel.ToLower());
            if (xid >= 0 )
            {
                GameObject go = Pools[xid].Spawn(Position, Rotation);

                if (go != null)
                {
                    int instance_id = go.GetInstanceID();
                    if (!InstanceidToPoolid.ContainsKey(instance_id))
                    {
                        InstanceidToPoolid.Add(instance_id, xid);
                    }
                }
                return go;
            }else { return null; }
        }

#endregion

        /// <summary>
        /// Returns a GameObject from the Object pool
        /// </summary>
        /// <param name="Label">Pool label</param>
        /// <param name="Position">Target Position</param>
        /// <param name="Rotation">Target Rotation</param>
        /// <returns>Returns a Gameobject if it is aviable, otherwise returns null</returns>
        public static GameObject Spawn(string Label , Vector3 Position , Quaternion Rotation )
        {
            return Instance.GetPrefab(Label, Position, Rotation);
        }
        
        /// <summary>
        /// Is the instance id asigned to an Object Pool ?
        /// </summary>
        /// <param name="instanceid">GameObject Instanceid</param>
        public bool IsOnPool(int instanceid)
        {
            int poolid = IdinstanceToPoolid(instanceid);
            
            return poolid >= 0 && poolid < Pools.Length;
        }


        /// <summary>
        /// Adds a GameObject to the object pool
        /// </summary>
        /// <param name="obj">GameObject to add</param>
        public void RecycleGameObject(GameObject obj )
        {
            if (!obj)
            {
                Debug.Log("Null gameObject");
                return;
            }

            int _instanceid = obj.GetInstanceID();

            int poolid = IdinstanceToPoolid(_instanceid);

            Debug.Log(poolid);

            if (poolid >= 0 )
            {
                Debug.Log("ok - pool");
                Pools[poolid].Recycle(obj);
            }
        }
        
        /// <summary>
        /// Gets the RecycleBin Assigned from the GameObject
        /// </summary>
        /// <param name="obj">GameObject</param>
        /// <returns>RecycleBin instance</returns>
        public GoRecycleBin GetRecycleBin (GameObject obj)
        {
            if (!obj) return null;

            int _instanceid = obj.GetInstanceID();

            int poolid = IdinstanceToPoolid(_instanceid);

            if (poolid >= 0)
            {
               return Pools[poolid];
            }else
            {
                return null;
            }
        }

        

    }
}