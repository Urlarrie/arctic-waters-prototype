using ArcticWaters.Core.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcticWaters.Core
{
    public class CameraController : MonoBehaviour
    {
        Camera cam;
        LevelManager manager;

        void Awake()
        {
            cam = GetComponent<Camera>();
            manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

            SetCameraSize();
        }

        private void SetCameraSize()
        {
            cam.orthographicSize = manager.RoomHeight / 2;
        }
    }

}