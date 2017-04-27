using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoRecycler;

namespace GoRecyclerExample
{
    public class Killer : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            bool isinpool = gameObject.IsInObjectPool();

            if (collision.gameObject.CompareTag("Ground"))
            {
                if (isinpool)
                    gameObject.Recycle();
                else
                    Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
           
        }

    }
}