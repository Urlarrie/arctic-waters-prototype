using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcticWaters.Core.Managers
{
    //gizmos to show tiles
    //gizmos to show where borders will be
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private float _playerBorderChangeBuffer;
        [SerializeField] private float _roomHeight;
        [SerializeField] private float _roomWidth;

        [SerializeField] private GameObject cameraSpawn;
        [SerializeField] private GameObject roomBorder;
        [SerializeField] private Transform borderParent;
        [SerializeField] private Camera mainCamera;

        private bool roomShift = false;

        private GameObject[] borders = new GameObject[4];
        private GameObject topBorder;
        private GameObject rightBorder;
        private GameObject bottomBorder;
        private GameObject leftBorder;

        public float RoomHeight
        { 
            get => _roomHeight;

            private set
            {
                if (value <= 0) throw new ArgumentException($"Invalid room height of {value}");
            }
        }

        public float RoomWidth
        {
            get => _roomWidth;

            private set
            {
                if (value <= 0) throw new ArgumentException($"Invalid room width of {value}");
            }
        }

        void Awake()
        {
            SetCameraPosition(cameraSpawn);
            GenerateRoomBorders();
        }

        private void GenerateRoomBorders()
        {
            Vector2 topBorderPos = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, 0));
            Vector2 rightBorderPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));
            Vector2 bottomBorderPos = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, 0));
            Vector2 leftBorderPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));

            //Northern
            topBorder = GenerateBorder(topBorderPos, 1, RoomWidth);

            //Eastern
            rightBorder = GenerateBorder(rightBorderPos, RoomHeight, 1);

            //Southern
            bottomBorder = GenerateBorder(bottomBorderPos, 1, RoomWidth);

            //Western
            leftBorder = GenerateBorder(leftBorderPos, RoomHeight, 1);

            borders[0] = topBorder;
            borders[1] = rightBorder;
            borders[2] = bottomBorder;
            borders[3] = leftBorder;
        }

        private GameObject GenerateBorder(Vector2 pos, float height, float width)
        {
            GameObject newborder = roomBorder;

            BoxCollider2D newBorderCollider = newborder.GetComponent<BoxCollider2D>();

            newBorderCollider.size = new Vector2(width, height);

            return Instantiate(newborder, pos, Quaternion.identity, borderParent);
        }

        private void SetCameraPosition(GameObject cameraSpawn)
        {
            mainCamera.transform.position = cameraSpawn.transform.position;
        }

        public void ChangeRoom(GameObject player, GameObject borderHit)
        {
            //Should probably abstract movement and collision
            Player playerScript = player.GetComponent<Player>();
            playerScript.enabled = false;

            if (borderHit == topBorder)
            {
                var positionChange = new Vector3(0, RoomHeight);

                mainCamera.transform.position += positionChange;

                foreach(GameObject border in borders)
                {
                    border.transform.position += positionChange;
                }

                player.transform.position += new Vector3(0, _playerBorderChangeBuffer);

                playerScript.enabled = true;
            }
            else if (borderHit == rightBorder)
            {
                var positionChange = new Vector3(RoomWidth, 0);

                mainCamera.transform.position += positionChange;

                foreach (GameObject border in borders)
                {
                    border.transform.position += positionChange;
                }

                player.transform.position += new Vector3(0, _playerBorderChangeBuffer);

                playerScript.enabled = true;
            }
            else if (borderHit == bottomBorder)
            {
                var positionChange = new Vector3(0, -RoomHeight);

                mainCamera.transform.position += positionChange;

                foreach (GameObject border in borders)
                {
                    border.transform.position += positionChange;
                }

                player.transform.position += new Vector3(0, -_playerBorderChangeBuffer);

                playerScript.enabled = true;
            }
            else if (borderHit == leftBorder)
            {
                var positionChange = new Vector3(-RoomWidth, 0);

                mainCamera.transform.position += positionChange;

                foreach (GameObject border in borders)
                {
                    border.transform.position += positionChange;
                }

                player.transform.position += new Vector3(-_playerBorderChangeBuffer, 0);

                playerScript.enabled = true;
            }          
        }
    }
}

