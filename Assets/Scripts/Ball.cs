using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout
{
    public class Ball : MonoBehaviour
    {
        // Speed that the ball travels
        public float speed = 5f;

        // Velocity of the ball
        private Vector3 velocity;
        private bool[] reflected;
        private Vector3[] normals = new Vector3[] 
        {
            Vector3.right, 
            Vector3.left,
            Vector3.up,
            Vector3.down
        };

        void Start()
        {
            reflected = new bool[4];
        }

        // Fire off ball in a given direction
        public void Fire(Vector3 direction)
        {
            // Calculate velocity
            velocity = direction * speed;
        }
        
        void Reflect(Vector3 normal)
        {
            // Calculate the reflection point of the ball
            Vector3 reflect = Vector3.Reflect(velocity, normal);
            // Maintail the same velocity but in the direction (reflect)
            velocity = reflect.normalized * speed;
        }

        /// <summary>
        /// Checks if the ball hit an edge and reflects in direction that corresponds the index
        /// </summary>
        /// <param name="index">0 = Right, 1 = Left, 2 = Up, 3 = Down</param>
        void CheckEdge(bool hitEdge, int index)
        {
            if(hitEdge && !reflected[index])
            {
                Reflect(normals[index]);
                // Reflected!
                reflected[index] = true;
            }
            else
            {
                reflected[index] = false;
            }
        }

        void BounceOffScreen()
        { 
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            CheckEdge(pos.x < 0.0f, 0);
            CheckEdge(pos.x > 1.0f, 1);
            CheckEdge(pos.y < 0.0f, 2);
            CheckEdge(pos.y > 1.0f, 3);
        }
        
        void FixedUpdate()
        {
            BounceOffScreen();

            transform.position += velocity * Time.deltaTime;
        }
        
        // Detect collision
        void OnCollisionEnter2D(Collision2D other)
        {
            Racket racket = other.gameObject.GetComponent<Racket>();
            if(racket != null)
            {
                float x = racket.HitFactor(transform.position);
                Vector3 reflect = new Vector2(x, 1).normalized;
                velocity = reflect * speed;
            }
        }
    }
}