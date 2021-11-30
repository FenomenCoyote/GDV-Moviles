using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class BoardInput : MonoBehaviour
    {
        private uint width, height;

        private bool pressed = false;

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
                Debug.Log(mouseTilePos);
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
        }

        public bool justDown()
        {
#if UNITY_ANDROID
             return Input.GetTouch(0);
#endif
            return Input.GetMouseButtonDown(0);
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
            Vector2 offset = new Vector3((-width) / 2.0f, (height) / 2.0f);
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 cursorPos = (offset - worldMousePos) + new Vector2(0.2f, -0.2f);
            cursorPos = new Vector2(cursorPos.x - 0.1f, cursorPos.y + 0.1f);

            bool inside = cursorPos.x < 0 && cursorPos.y > 0 && cursorPos.y < height && cursorPos.x > -width;
            if (inside)
                mouseTilePos = new Vector2(Mathf.Abs((int)cursorPos.y), Mathf.Abs((int)cursorPos.x));
            return inside;
        }

        public bool isPressed()
        {
            return pressed;
        }
    }
}