using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Tile : MonoBehaviour
    {
        
        [SerializeField] private SpriteRenderer circle;

        [Tooltip("Scale used when this tile is an initial or ending pipe")]
        [Range(0f, 1f)]
        [SerializeField] private float circleBigSize = 0.8f;

        [Tooltip("Scale used when this tile is in a half pipe")]
        [Range(0f, 1f)]
        [SerializeField] private float circleSmallSize = 0.4f;

        [SerializeField] private SpriteRenderer up;
        [SerializeField] private SpriteRenderer down;
        [SerializeField] private SpriteRenderer left;
        [SerializeField] private SpriteRenderer right;

        [SerializeField] private SpriteRenderer backGroundHighlight;

        [Range(0f, 1f)]
        [SerializeField] private float backGroundHighlightAlpha = 0.2f;

        [SerializeField] private SpriteRenderer check;

        private bool activeTile = false;
        private bool initialOrEnd = false;
        private Color color;

        private Logic.Dir origen, dest;

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


        //TODO: Algo para hacer el shake


        public void setColor(Color c)
        {
            color = c;
            updateSpritesColor();
        }

        public Color getColor()
        {
            return color;
        }

        public void enableDestDirectionSprite(Logic.Dir dir)
        {
            setEnabledDirectionSprite(dir, true);
            dest = dir;
        }

        public void enableSourceDirectionSprite(Logic.Dir dir)
        {
            setEnabledDirectionSprite(dir, true);
            origen = dir;
        }

        public void enableDirectionSprite(Logic.Dir dir)
        {
            setEnabledDirectionSprite(dir, true);
        }

        public void disableDestDirectionSprite()
        {
            setEnabledDirectionSprite(dest, false);
            dest = Logic.Dir.None;
        }

        public void disableSourceDirectionSprite()
        {
            setEnabledDirectionSprite(origen, true);
            origen = Logic.Dir.None;
        }

        public void disableDirectionSprite(Logic.Dir dir)
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

        public void setCircleBig()
        {
            circle.transform.localScale = Vector2.one * circleBigSize;
            initialOrEnd = true;
        }

        public void setCircleSmall()
        {
            circle.transform.localScale = Vector2.one * circleSmallSize;
            initialOrEnd = false;
        }

        public void setActiveTile(bool b)
        {
            activeTile = b;
        }

        public bool isActive()
        {
            return activeTile;
        }

        public bool isInitialOrEnd()
        {
            return initialOrEnd;
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

        private void setEnabledDirectionSprite(Logic.Dir dir, bool e)
        {
            switch (dir)
            {
                case Logic.Dir.Up:
                    up.enabled = e;
                    break;
                case Logic.Dir.Down:
                    down.enabled = e;
                    break;
                case Logic.Dir.Left:
                    left.enabled = e;
                    break;
                case Logic.Dir.Right:
                    right.enabled = e;
                    break;
                default:
                    Debug.LogError("Direction imposible en tile");
                    break;
            }
        }

    }

}
