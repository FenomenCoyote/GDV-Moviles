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

        private bool disabled = false;

#if PLATFORM_ANDROID
        void Start()
        {
            Input.multiTouchEnabled = false;
        }
#endif

        public void disable()
        {
            disabled = true;
            pressed = false;
        }

        public void enable()
        {
            disabled = false;
        }

        public void updateInput()
        {
#if PLATFORM_ANDROID
            updateAndroid();
#else
            updatePC();
#endif
        }

        private void updateAndroid()
        {
            if (!disabled && Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    //Pressed if user clicked inside the board
                    pressed = calculateMouseTilePos(t.position);
                }
                else if (t.phase == TouchPhase.Ended)
                {
                    //Only stop pressing after mouseUp
                    pressed = false;
                }
                else if (pressed && t.phase == TouchPhase.Moved)
                {
                    calculateMouseTilePos(t.position);
                }
            }
        }

        private void updatePC()
        {
            if (!disabled)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Pressed if user clicked inside the board
                    pressed = calculateMouseTilePos(Input.mousePosition);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    //Only stop pressing after mouseUp
                    pressed = false;
                }
                else if (pressed)
                {
                    calculateMouseTilePos(Input.mousePosition);
                }
            }
        }

        public void init(uint w, uint h)
        {
            width = w;
            height = h;

            disabled = false;

            float auxW = w % 2 == 0 ? w + 0.5f : w;
            float auxH = h % 2 == 0 ? h + 0.5f : h;
            offset = new Vector3(auxW / 2.0f, -auxH / 2.0f);
        }

        public bool justDown()
        {
#if UNITY_ANDROID
            return pressed && Input.GetTouch(0).phase == TouchPhase.Began;
#else
            return pressed && Input.GetMouseButtonDown(0);
#endif
        }

        public bool justUp()
        {
#if UNITY_ANDROID
            return Input.GetTouch(0).phase == TouchPhase.Ended;
#else
            return Input.GetMouseButtonUp(0);
#endif
        }

        public Vector2 getMouseTilePos()
        {
            return mouseTilePos;
        }

        private bool calculateMouseTilePos(Vector3 pos)
        {
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(pos); //raw in tiles

            worldMousePos = (worldMousePos + (offset * transform.localScale)); //adjust offset
            worldMousePos.x += transform.position.x;
            worldMousePos.y -= transform.position.y;
            worldMousePos = Vector2.Perpendicular(worldMousePos);

            worldMousePos /= transform.localScale; //Fit with scale
            worldMousePos = Vector2Int.FloorToInt(worldMousePos); //Floor it

            inside = worldMousePos.x >= 0 && worldMousePos.y >= 0 && worldMousePos.y < width && worldMousePos.x < height;
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