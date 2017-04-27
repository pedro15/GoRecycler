using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoRecycler;

namespace GoRecyclerExample
{
    public class Spawner : MonoBehaviour
    {
        public string PoolLabel;

        public float SpawnRepeat;

        Vector3 GetRandomPos(float radius)
        {
            return transform.position + (Random.insideUnitSphere * radius);
        }

        private IEnumerator Start()
        {
            while (Application.isPlaying)
            {
                GoRecyclerUtility.GetgameObject(PoolLabel, GetRandomPos(15f), Quaternion.identity);

                yield return new WaitForSeconds(Time.deltaTime + SpawnRepeat);
            }
        }

    }
}