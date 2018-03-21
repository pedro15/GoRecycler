using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoRecycler;

namespace GoRecyclerExample
{
    public class Shooter : MonoBehaviour
    {
        public string BulletLabel;

        public Transform ShootPoint;

        public float LookSpeed;

        public float ShootForce;

        private void Update()
        {
            LookAt();
            Shoot();
        }
        
        private void LookAt()
        {
            Plane p = new Plane(Vector3.up, transform.position);
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

            float raydist;

            if (p.Raycast(r, out raydist))
            {
                Vector3 Point = r.GetPoint(raydist);

                Vector3 dir = Point - transform.position;

                if (dir != Vector3.zero)
                {
                    Quaternion targetrot = Quaternion.LookRotation(dir);

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetrot, LookSpeed * Time.deltaTime);
                    
                }


            }
        }

        private void Shoot()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject bullet = GoRecyclerManager.Spawn(BulletLabel, ShootPoint.position, ShootPoint.rotation);

                if (bullet != null)
                {
                    Rigidbody rb = bullet.GetComponent<Rigidbody>();
                    if (!rb) rb = bullet.AddComponent<Rigidbody>();

                    rb.AddForce(ShootPoint.TransformDirection(Vector3.forward * ShootForce), ForceMode.Impulse);

                }

            }
        }

    }
}