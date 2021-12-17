using UnityEngine;

namespace flow
{
    public class InputPointerManager : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sprite; //Pointer sprite 

        [SerializeField]
        [Range(0, 1)] private float alphaCorrect, alphaNotCorrect; //Alpha of the pointer

        /// <summary>
        /// Sets the color of the pointer 
        /// </summary>
        /// <param name="c"></param>
        public void setColor(Color c)
        {
            sprite.enabled = true;
            float a = sprite.color.a;
            sprite.color = c;
            Color tempColor = sprite.color;
            tempColor.a = a;
            sprite.color = tempColor;
        }

        /// <summary>
        /// Enanbles the pointer
        /// </summary>
        private void OnEnable()
        {
            sprite.enabled = true;
        }

        /// <summary>
        /// Disables the pointer
        /// </summary>
        private void OnDisable()
        {
            sprite.enabled = false;
        }

        /// <summary>
        /// Sets the correct alpha to the pointer color
        /// </summary>
        public void setCorrect()
        {
            Color c = sprite.color;
            c.a = alphaCorrect;
            sprite.color = c;

            updatePos();
        }

        /// <summary>
        /// Sets the incorrect alpha to the pointer color
        /// </summary>
        public void setNotCorrect()
        {
            Color c = sprite.color;
            c.a = alphaNotCorrect;
            sprite.color = c;
        }

        /// <summary>
        /// Updates the position of the pointer
        /// </summary>
        void Update()
        {
            if(sprite.enabled)
                updatePos();
        }

        void updatePos()
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