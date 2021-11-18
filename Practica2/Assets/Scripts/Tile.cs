using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer circle;
        [SerializeField] private SpriteRenderer up;
        [SerializeField] private SpriteRenderer down;
        [SerializeField] private SpriteRenderer left;
        [SerializeField] private SpriteRenderer right;

        [SerializeField] private SpriteRenderer backGroundHighlight;

        [Range(0f, 1f)]
        [SerializeField] private float backGroundHighlightAlpha = 0.2f;

        [SerializeField] private SpriteRenderer check;

        private Color color;

        #if UNITY_EDITOR
        void Start()
        {
            if (circle == null || up == null || down == null || 
                left == null || right == null || backGroundHighlight == null ||
                check == null) 
            { 
                Debug.LogError("Tile no tiene circle asociado");
                return;
            }
        }
        #endif

        public void setColor(Color c)
        {
            color = c;
            updateSpritesColor();
        }

        public Color getColor()
        {
            return color;
        }

        public void enableDirectionSprite(Logic.Direction dir)
        {
            setEnabledDirectionSprite(dir, true);
        }

        public void disableDirectionSprite(Logic.Direction dir)
        {
            setEnabledDirectionSprite(dir, false);
        }

        public void enableCircle()
        {
            circle.enabled = true;
        }

        public void disableCircle()
        {
            circle.enabled = false;
        }

        public void enableCheck()
        {
            check.enabled = true;
        }

        public void disableCheck()
        {
            check.enabled = false;
        }

        public void disableAll()
        {
            circle.enabled = false;
            up.enabled = false;
            down.enabled = false;
            left.enabled = false;
            right.enabled = false;
            backGroundHighlight.enabled = false;
            check.enabled = false;
        }


        private void updateSpritesColor()
        {
            circle.color = color;
            up.color = color;
            down.color = color;
            left.color = color;
            right.color = color;

            float colorAlpha = color.a;
            color.a = backGroundHighlightAlpha;
            backGroundHighlight.color = color;
            color.a = colorAlpha;
        }

        private void setEnabledDirectionSprite(Logic.Direction dir, bool e)
        {
            switch (dir)
            {
                case Logic.Direction.Up:
                    up.enabled = e;
                    break;
                case Logic.Direction.Down:
                    down.enabled = e;
                    break;
                case Logic.Direction.Left:
                    left.enabled = e;
                    break;
                case Logic.Direction.Right:
                    right.enabled = e;
                    break;
                default:
                    Debug.LogError("Direction imposible en tile");
                    break;
            }
        }

    }

}
