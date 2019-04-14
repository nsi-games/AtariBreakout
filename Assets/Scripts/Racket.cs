using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout
{
    public class Racket : MonoBehaviour
    {
        public float movementSpeed = 20f;
        public Ball ball;
        // Directions the ball travels at the start
        public Vector2[] directions = new Vector2[]
        {
            new Vector2(-1f, 1f),
            new Vector2(1f, 1f)
        };
        private Rigidbody2D rigid;
        private SpriteRenderer rend;

        void Start()
        {
            rigid = GetComponent<Rigidbody2D>();
            rend = GetComponent<SpriteRenderer>();
            // Parent to racket before the game begins
            ball.transform.SetParent(transform);
        }

        void BeginGame()
        {
            // Detatch ball from parent
            ball.transform.SetParent(null);
            // Generate random direction from list of directions
            int randIndex = Random.Range(0, directions.Length);
            Vector3 randDir = directions[randIndex];
            // Fire off the ball in that direction
            ball.Fire(randDir);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BeginGame();
            }
        }
        
        void Movement()
        {
            float inputH = Input.GetAxis("Horizontal");
            Vector3 right = transform.right;
            rigid.velocity = right * inputH * movementSpeed;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Movement();
        }

        public float HitFactor(Vector3 ballPos)
        {
            Vector3 racketPos = transform.position;
            float racketWidth = rend.bounds.size.x;
            return (ballPos.x - racketPos.x) / racketWidth;
        }
    }
}