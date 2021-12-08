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

        [SerializeField] private SpriteRenderer directionUp;
        [SerializeField] private SpriteRenderer directionDown;
        [SerializeField] private SpriteRenderer directionLeft;
        [SerializeField] private SpriteRenderer directionRight;

        [SerializeField] private SpriteRenderer boundaryUp;
        [SerializeField] private SpriteRenderer boundaryDown;
        [SerializeField] private SpriteRenderer boundaryLeft;
        [SerializeField] private SpriteRenderer boundaryRight;

        [SerializeField] private SpriteRenderer wallUp;
        [SerializeField] private SpriteRenderer wallDown;
        [SerializeField] private SpriteRenderer wallLeft;
        [SerializeField] private SpriteRenderer wallRight;

        [SerializeField] private SpriteRenderer backGroundHighlight;

        [Range(0f, 1f)]
        [SerializeField] private float backGroundHighlightAlpha = 0.2f;

        [SerializeField] private SpriteRenderer check;

        private bool activeTile = false;
        private bool initialOrEnd = false;
        private Color color;

        private Logic.Dir origen = Logic.Dir.None, dest = Logic.Dir.None;

        private bool lockHighLightColor = false;

        private Dictionary<Logic.Dir, bool> walls;

        private void Awake()
        {
            walls = new Dictionary<Logic.Dir, bool>();
            walls.Add(Logic.Dir.Up, false);
            walls.Add(Logic.Dir.Down, false);
            walls.Add(Logic.Dir.Left, false);
            walls.Add(Logic.Dir.Right, false);
            walls.Add(Logic.Dir.None, false);
        }

#if UNITY_EDITOR
        void Start()
        {
            if (circle == null || 
                directionUp == null || directionDown == null || directionLeft == null || directionRight == null ||
                boundaryUp == null || boundaryDown == null || boundaryLeft == null || boundaryRight == null || 
                wallUp == null || wallDown == null || wallLeft == null || wallRight == null ||
                backGroundHighlight == null ||
                check == null ) 
            { 
                Debug.LogError("Tile no tiene circle asociado");
                return;
            }
        }
#endif


        //TODO: Algo para hacer el shake

        public void setHightLock(bool b)
        {
            lockHighLightColor = b;

            if(!lockHighLightColor) updateSpritesColor();
        }

        public void setColor(Color c)
        {
            color = c;
            updateSpritesColor();
        }

        public Color getColor()
        {
            return color;
        }

        public void setWall(Logic.Dir dir, bool b = true)
        {
            walls[dir] = b;

            switch (dir)
            {
                case Logic.Dir.Up:
                    wallUp.enabled = b;
                    break;
                case Logic.Dir.Down:
                    wallDown.enabled = b;
                    break;
                case Logic.Dir.Left:
                    wallLeft.enabled = b;
                    break;
                case Logic.Dir.Right:
                    wallRight.enabled = b;
                    break;
            }
        }

        public bool hasWall(Logic.Dir dir)
        {
            return walls[dir];
        }

        public bool hasNoDir()
        {
            return origen == Logic.Dir.None && dest == Logic.Dir.None;
        }

        public void enableDestDirectionSprite()
        {
            setEnabledDirectionSprite(dest, true);
        }

        public void enableSourceDirectionSprite()
        {
            setEnabledDirectionSprite(origen, true);
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
        }

        public void disableSourceDirectionSprite()
        {
            setEnabledDirectionSprite(origen, false);
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
            if(!initialOrEnd)
                circle.enabled = false;
        }

        public void enableCheck()
        {
            check.enabled = true;
        }

        public void enableHightLight()
        {
            backGroundHighlight.enabled = true;
        }

        public void disableHighLight()
        {
            backGroundHighlight.enabled = false;
        }

        public void disableCheck()
        {
            check.enabled = false;
        }

        public void disableAll()
        {
            //circle.enabled = false;
            directionUp.enabled = false;
            directionDown.enabled = false;
            directionLeft.enabled = false;
            directionRight.enabled = false;
            backGroundHighlight.enabled = false;
            check.enabled = false;
            origen = Logic.Dir.None;
            dest = Logic.Dir.None;
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
            if (initialOrEnd && !b) 
                return;
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

        public void reverse()
        {
            Logic.Dir aux = origen;
            origen = dest;
            dest = aux;
        }

        private void updateSpritesColor()
        {
            circle.color = color;
            directionUp.color = color;
            directionDown.color = color;
            directionLeft.color = color;
            directionRight.color = color;

            if (!lockHighLightColor)
            {
                float colorAlpha = color.a;
                color.a = backGroundHighlightAlpha;
                backGroundHighlight.color = color;
                color.a = colorAlpha;
            }
        }

        private void setEnabledDirectionSprite(Logic.Dir dir, bool e)
        {
            switch (dir)
            {
                case Logic.Dir.Up:
                    directionUp.enabled = e;
                    break;
                case Logic.Dir.Down:
                    directionDown.enabled = e;
                    break;
                case Logic.Dir.Left:
                    directionLeft.enabled = e;
                    break;
                case Logic.Dir.Right:
                    directionRight.enabled = e;
                    break;
            }
        }

    }

}
