using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Tile : MonoBehaviour
    {
        
        [SerializeField] private SpriteRenderer circle;

        [SerializeField] private TileAnimation tileAnimationCircle;
        [SerializeField] private TileAnimation tileAnimationFinished;

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

        [Tooltip("backGround sprite color alpha")]
        [Range(0f, 1f)]
        [SerializeField] private float backGroundHighlightAlpha = 0.2f;

        [Tooltip("Boundaries sprite color alpha")]
        [Range(0f, 1f)]
        [SerializeField] private float boundaryAlpha = 0.75f;

        [SerializeField] private SpriteRenderer check;

        [SerializeField] private SpriteRenderer radar;

        //If its an empty tile or not
        private bool empty = false;

        //If its an active tile
        private bool activeTile = false;

        //If its an initialTile
        private bool initialOrEnd = false;

        private Color color;

        //Locks the highLight color to allow 2 colors in the same tile
        private bool lockHighLightColor = false;

        //Walls logic
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
                backGroundHighlight == null || check == null || radar == null || tileAnimationCircle == null || tileAnimationFinished == null) 
            { 
                Debug.LogError("Tile esta mal configurado");
                return;
            }
        }
#endif

        public void shake()
        {
            Vector3 scale = initialOrEnd ? Vector2.one * circleBigSize : Vector2.one * circleSmallSize;
            tileAnimationCircle.setInitialScale(scale);
            tileAnimationCircle.startAnim();
        }

        public void finishedAnim()
        {
            tileAnimationFinished.startAnim();
        }

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

        public void setBoundaryColors(Color color)
        {
            color.a = 1;

            wallUp.color = color;
            wallDown.color = color;
            wallLeft.color = color;
            wallRight.color = color;

            color.a = boundaryAlpha;

            boundaryUp.color = color;
            boundaryDown.color = color;
            boundaryLeft.color = color;
            boundaryRight.color = color;
        }


        public Color getColor()
        {
            return color;
        }

        public void setWall(Logic.Dir dir, bool b = true)
        {
            walls[dir] = b;
            if (empty)
                return;

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

        public void setEmpty()
        {
            empty = true;
            walls[Logic.Dir.Up] = true; wallUp.enabled = false;
            walls[Logic.Dir.Down] = true; wallDown.enabled = false;
            walls[Logic.Dir.Left] = true; wallLeft.enabled = false;
            walls[Logic.Dir.Right] = true; wallRight.enabled = false;

            disableAll();

            boundaryUp.enabled = false;
            boundaryDown.enabled = false;
            boundaryLeft.enabled = false;
            boundaryRight.enabled = false;
        }
        
        public void setHinted()
        {
            check.enabled = true;
        }

        public void enableDirectionSprite(Logic.Dir dir)
        {
            setEnabledDirectionSprite(dir, true);
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

        public void disableAll()
        {
            disableCircle();
            directionUp.enabled = false;
            directionDown.enabled = false;
            directionLeft.enabled = false;
            directionRight.enabled = false;
            backGroundHighlight.enabled = false;
            lockHighLightColor = false;
            check.enabled = false;
            setActiveTile(false);
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

        public bool isEmpty()
        {
            return empty;
        }

        public bool isInitialOrEnd()
        {
            return initialOrEnd;
        }

        private void updateSpritesColor()
        {
            circle.color = color;
            directionUp.color = color;
            directionDown.color = color;
            directionLeft.color = color;
            directionRight.color = color;
            radar.color = color;

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
