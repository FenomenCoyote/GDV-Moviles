using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class BoardInput : MonoBehaviour
    {
        private uint width, height;

        private bool pressed = false, inside = false;

        private Vector2 offset;
        private Vector2 mouseTilePos;

#if UNITY_EDITOR
        void Start()
        {

        }
#endif

        public void updateInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Pressed if user clicked inside the board
                pressed = calculateMouseTilePos();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //Only stop pressing after mouseUp
                pressed = false;
            }
            else if (pressed)
            {
                calculateMouseTilePos();
            }
        }

        public void init(uint w, uint h)
        {
            width = w;
            height = h;

            offset = new Vector3((width) / 2.0f, (-height) / 2.0f);
        }

        public bool justDown()
        {
#if UNITY_ANDROID
             return Input.GetTouch(0);
#endif
            return pressed && Input.GetMouseButtonDown(0);
        }

        public bool justUp()
        {
#if UNITY_ANDROID
             return Input.GetTouch(0);
#endif
            return Input.GetMouseButtonUp(0);
        }

        public Vector2 getMouseTilePos()
        {
            return mouseTilePos;
        }

        private bool calculateMouseTilePos()
        {
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //raw in tiles

            worldMousePos = (worldMousePos + (offset * transform.localScale)); //adjust offset
            worldMousePos = Vector2.Perpendicular(worldMousePos);

            worldMousePos /= transform.localScale; //Fit with scale
            worldMousePos = Vector2Int.FloorToInt(worldMousePos); //Floor it

            inside = worldMousePos.x >= 0 && worldMousePos.y >= 0 && worldMousePos.y < height && worldMousePos.x < width;
            if (inside)
                mouseTilePos = worldMousePos;
            return inside;
        }

        public bool isPressed()
        {
            return pressed;
        }

        public bool isInside()
        {
            return inside;
        }
    }
}