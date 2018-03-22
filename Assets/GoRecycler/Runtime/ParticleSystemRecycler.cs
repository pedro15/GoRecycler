using System.Collections;
using UnityEngine;

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
        private bool AutoPlay = true;

        [SerializeField,Tooltip("0 = no TimeOut")]
        public float TimeOut = 0;

        ParticleSystem particles;

        private void Awake()
        {
            particles = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            if (TimeOut > 0)
            {
                StartCoroutine(KillParticles());
            }
        }

        IEnumerator KillParticles()
        {
            yield return new WaitForSeconds(TimeOut);
            gameObject.Recycle();
            StopCoroutine(KillParticles());
        }

        public void OnRecycle()
        {
            if (particles != null)
            {
                particles.Stop();
                ParticleSystem[] childparticles = particles.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < childparticles.Length; i++) childparticles[i].Stop();
            }
        }

        public void OnSpawn()
        {
            if (AutoPlay && particles != null)
            {
                particles.Play();
                ParticleSystem[] childparticles = particles.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < childparticles.Length; i++) childparticles[i].Play();
            }
        }
    }
}