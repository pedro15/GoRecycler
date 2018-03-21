using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoRecycler.Core;

namespace GoRecycler
{
    /// <summary>
    /// Component for Auto Stopping/Playing ParticleSystems when is on a Object Pool
    /// </summary>
    [AddComponentMenu("GoRecycler/ParticleSystem Recycler")]
    public class ParticleSystemRecycler : MonoBehaviour, IPooled
    {
        /// <summary>
        /// Should Autoplay on Spawn ?
        /// </summary>
        [SerializeField]
        private bool AutoPlay;

        ParticleSystem particles;

        private void Awake()
        {
            particles = GetComponent<ParticleSystem>();
        }

        public void OnRecycle(GoRecycleBin RecycleBin)
        {
            if (particles != null)
                particles.Stop();
        }

        public void OnSpawn(GoRecycleBin RecycleBin)
        {
            if (AutoPlay && particles != null)
                particles.Play();
        }
    }
}