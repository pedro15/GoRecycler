using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoRecycler;

namespace GoRecyclerExample
{
    public class TrafficSpawner : MonoBehaviour
    {
        public Transform[] TrafficPoints;

        public float SpawnTime;

        public string[] PoolLabels;

        int lastindex = 0;

        private IEnumerator Start()
        {
            while (Application.isPlaying)
            {
                int i = Random.Range(0, TrafficPoints.Length);
                if (i != lastindex)
                    GoRecyclerUtility.GetgameObject(PoolLabels[Random.Range(0,PoolLabels.Length)], TrafficPoints[i].position, TrafficPoints[i].rotation);
                lastindex = i;
                yield return new WaitForSeconds(Time.deltaTime + SpawnTime);
            }
        }
    }
}