using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class InputPointerManager : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sprite;

        [SerializeField]
        [Range(0, 1)] private float alphaCorrect, alphaNotCorrect;

        public void setColor(Color c)
        {
            sprite.enabled = true;
            float a = sprite.color.a;
            sprite.color = c;
            Color tempColor = sprite.color;
            tempColor.a = a;
            sprite.color = tempColor;
        }

        private void OnEnable()
        {
            sprite.enabled = true;
        }

        private void OnDisable()
        {
            sprite.enabled = false;
        }

        public void setCorrect()
        {
            Color c = sprite.color;
            c.a = alphaCorrect;
            sprite.color = c;
        }

        public void setNotCorrect()
        {
            Color c = sprite.color;
            c.a = alphaNotCorrect;
            sprite.color = c;
        }

        void Update()
        {

#if PLATFORM_ANDROID
            if(Input.touchCount > 0)         
                transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
#else
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif
        }
    }
}