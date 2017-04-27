using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoRecycler;

namespace GoRecyclerExample
{
    public class CarScript : MonoBehaviour
    {
        public float Speed;
        public float MaxTime;
        float timer;

        private void OnEnable()
        {
            timer = 0f;
        }

        private void Update()
        {
            if (timer < MaxTime)
            {
                timer += Time.deltaTime;
                transform.position += transform.forward * Speed * Time.deltaTime;
            }
            else
            {
                gameObject.Recycle();
            }
        }
    }
}