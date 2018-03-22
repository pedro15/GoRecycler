using UnityEngine;
using System.Collections;
using GoRecycler;

namespace GoRecyclerExample
{
    public class BulletScript : MonoBehaviour
    {
        public string ExplosionLabel;
        public float TimeOut;

        private bool started = false;

        IEnumerator Explode()
        {
            yield return new WaitForSeconds(TimeOut);

            RecyclerManager.Spawn(ExplosionLabel, transform.position, Quaternion.identity);
            
            gameObject.Recycle();

            started = false;
            StopCoroutine(Explode());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!started)
            {
                StartCoroutine(Explode());
                started = true;
            }
        }

    }
}