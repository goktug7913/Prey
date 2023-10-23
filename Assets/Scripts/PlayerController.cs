using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        public Camera cam;

        public float speed = 25f;
        public float zoomSensitivty = 100f;
        
        private void Start()
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            // WASD movement
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            
            var movement = new Vector2(x, y);
            
            transform.position += (Vector3) movement * (speed * Time.deltaTime);
        }

        private void HandleZoom()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            
            cam.orthographicSize += scroll * (zoomSensitivty * Time.deltaTime);
        }
    }
}