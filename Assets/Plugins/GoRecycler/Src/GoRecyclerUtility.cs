using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoRecycler
{
    [AddComponentMenu("GoRecycler/GoRecycler Utility")]
    public class GoRecyclerUtility : MonoBehaviour
    {
        #region Singleton  

        private static GoRecyclerUtility _instance;

        public static GoRecyclerUtility Instance
        {
            get
            {
                if (!_instance)
                {
                    GoRecyclerUtility[] Oplist = FindObjectsOfType<GoRecyclerUtility>();
                    if (Oplist.Length > 0 )
                    {
                        if (Oplist.Length > 1 )
                        {
                            Debug.LogWarning("[" + typeof(GoRecyclerUtility).Name + 
                           "] Duplicated instance, please keep only one " + 
                           typeof(GoRecyclerUtility).Name + " instace in the scene");
                        }
                        _instance = Oplist[0];
                        return _instance;
                    }
                }
                if (!_instance)
                {
                    Debug.LogError("[" + typeof(GoRecyclerUtility).Name + "] No " 
                        + typeof(GoRecyclerUtility).Name + " found in the scene");
                }
                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// Objects pools array
        /// </summary>
        [SerializeField,HideInInspector]
        private GoRecycleBin[] Pools;

        /// <summary>
        /// Dictionary to handle the instances asociated to an Object Pool
        /// </summary>
        private Dictionary<int, int> InstanceidtoPoolid = new Dictionary<int, int>();

        /// <summary>
        /// Dictionary to handle the labels of the object pools
        /// </summary>
        private Dictionary<string, int> NameToPoolId = new Dictionary<string, int>();

        void Awake()
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
                        Debug.LogWarning("[" + typeof(GoRecyclerUtility).Name +
                        "] Duplicated label detected (" + Pools[i].Label
                        + "). Please provide diferent label names in your pools, duplicated pool will be ignored.");
                    }
                }else
                {
                    Debug.LogWarning("[" + typeof(GoRecyclerUtility).Name +
                      "] Empty label detected, please specify a label for your Pools");
                }
            }
        }

        private int LabetoPoolid(string label)
        {
            int val = -1;

            if (NameToPoolId.ContainsKey(label))
            {
                NameToPoolId.TryGetValue(label, out val);
            }else
            {
                Debug.LogError("Trying to get a Pool with label: '" + label + "' That doesn't exists!");
                return -1;
            }
            return val;
        }

        private int IdinstanceToPoolid(int InstanceId)
        {
            int val = -1;
            
            InstanceidtoPoolid.TryGetValue(InstanceId, out val);

            return val;
        }
        

        private GameObject GetGameObject(string PoolLabel , Vector3 Position , Quaternion Rotation)
        {
            int xid = LabetoPoolid(PoolLabel.ToLower());
            if (xid >= 0 )
            {
                GameObject go = Pools[xid].GetGameObject(Position, Rotation);

                if (go != null)
                {
                    int instance_id = go.GetInstanceID();
                    if (!InstanceidtoPoolid.ContainsKey(instance_id))
                    {
                        InstanceidtoPoolid.Add(instance_id, xid);
                    }
                }
                return go;
            }else { return null; }
        }

        public static GameObject GetgameObject(string Label , Vector3 Position , Quaternion Rotation )
        {
            return Instance.GetGameObject(Label, Position, Rotation);
        }
        
        public bool IsOnPool(int instanceid)
        {
            int poolid = IdinstanceToPoolid(instanceid);

            return poolid >= 0 && InstanceidtoPoolid.ContainsKey(instanceid);
        }

        public void RecycleGameObject(GameObject obj )
        {
            int _instanceid = obj.GetInstanceID();

            int poolid = IdinstanceToPoolid(_instanceid);
            
            if (poolid >= 0 )
            {
                Pools[poolid].Recycle(obj);
            }
        }
        
        public GoRecycleBin GetRecycleBin (GameObject obj)
        {
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