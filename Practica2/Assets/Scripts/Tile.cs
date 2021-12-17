using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Tile : MonoBehaviour
    {
        //Circle image of the tile
        [SerializeField] private SpriteRenderer circle;

        //Animation components 
        [SerializeField] private TileAnimation tileAnimationCircle;
        [SerializeField] private TileAnimation tileAnimationFinished;

        [Tooltip("Scale used when this tile is an initial or ending pipe")]
        [Range(0f, 1f)]
        [SerializeField] private float circleBigSize = 0.8f;

        [Tooltip("Scale used when this tile is in a half pipe")]
        [Range(0f, 1f)]
        [SerializeField] private float circleSmallSize = 0.4f;

        //Directions images of the tile
        [SerializeField] private SpriteRenderer directionUp;
        [SerializeField] private SpriteRenderer directionDown;
        [SerializeField] private SpriteRenderer directionLeft;
        [SerializeField] private SpriteRenderer directionRight;

        //Boundary images of the tile
        [SerializeField] private SpriteRenderer boundaryUp;
        [SerializeField] private SpriteRenderer boundaryDown;
        [SerializeField] private SpriteRenderer boundaryLeft;
        [SerializeField] private SpriteRenderer boundaryRight;

        //Wall images of th tile
        [SerializeField] private SpriteRenderer wallUp;
        [SerializeField] private SpriteRenderer wallDown;
        [SerializeField] private SpriteRenderer wallLeft;
        [SerializeField] private SpriteRenderer wallRight;

        //Background image of the tile
        [SerializeField] private SpriteRenderer backGroundHighlight;

        [Tooltip("backGround sprite color alpha")]
        [Range(0f, 1f)]
        [SerializeField] private float backGroundHighlightAlpha = 0.2f;

        [Tooltip("Boundaries sprite color alpha")]
        [Range(0f, 1f)]
        [SerializeField] private float boundaryAlpha = 0.75f;

        //Check image of the tile
        [SerializeField] private SpriteRenderer check;

        //Radar image of the tile
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
            //Initialization of the walls dictionary
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
        /// <summary>
        /// Performs a shake animation of the tile
        /// </summary>
        public void shake()
        {
            Vector3 scale = initialOrEnd ? Vector2.one * circleBigSize : Vector2.one * circleSmallSize;
            tileAnimationCircle.setInitialScale(scale);
            tileAnimationCircle.startAnim();
        }

        /// <summary>
        /// Performs the tile animation when a pipe is connected
        /// </summary>
        public void finishedAnim()
        {
            tileAnimationFinished.startAnim();
        }

        /// <summary>
        /// Updates the sprites color and sets the lockHighLightColor variable that avoids having two
        /// highLight color in a tile
        /// </summary>
        /// <param name="b">If the highLightColor is locked or not</param>
        public void setHightLock(bool b)
        {
            lockHighLightColor = b;

            if(!lockHighLightColor) updateSpritesColor();
        }

        /// <summary>
        /// Sets the color of the tile
        /// </summary>
        /// <param name="c">New color</param>
        public void setColor(Color c)
        {
            color = c;
            updateSpritesColor();
        }

        /// <summary>
        /// Sets the boundaries and walls color of the tile
        /// </summary>
        /// <param name="color">New color</param>
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

        /// <summary>
        /// Returns the color of the sprite
        /// </summary>
        /// <returns></returns>
        public Color getColor()
        {
            return color;
        }

        /// <summary>
        /// Enables or disables the walls images of the tile
        /// </summary>
        /// <param name="dir">Direction</param>
        /// <param name="b">If it is enbaled or disabled</param>
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

        /// <summary>
        /// Return if the tile has a wall in a direction
        /// </summary>
        /// <param name="dir">Direction</param>
        /// <returns></returns>
        public bool hasWall(Logic.Dir dir)
        {
            return walls[dir];
        }

        /// <summary>
        /// Sets the tile to empty by disabling th walls,the boundaries,
        /// the tile directions, the tile images and the color
        /// </summary>
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
        
        /// <summary>
        /// Enables the check image of the tile
        /// </summary>
        public void setHinted()
        {
            check.enabled = true;
        }

        /// <summary>
        /// Enables the direction image
        /// </summary>
        /// <param name="dir">Direction</param>
        public void enableDirectionSprite(Logic.Dir dir)
        {
            setEnabledDirectionSprite(dir, true);
        }

        /// <summary>
        /// Disables the direction image
        /// </summary>
        /// <param name="dir">Direction</param>
        public void disableDirectionSprite(Logic.Dir dir)
        {
            setEnabledDirectionSprite(dir, false);
        }

        /// <summary>
        ///Enables the circle image of the tile
        /// </summary>
        public void enableCircle()
        {
            circle.enabled = true;
        }

        /// <summary>
        /// Disables the circle image if it is not the inital or end image
        /// </summary>
        public void disableCircle()
        {
            if(!initialOrEnd)
                circle.enabled = false;
        }

        /// <summary>
        /// Enables the background image of the tile
        /// </summary>
        public void enableHightLight()
        {
            backGroundHighlight.enabled = true;
        }

        /// <summary>
        /// Disables all the images of the tile
        /// </summary>
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

        /// <summary>
        /// Makes bigger the scale of the circle image
        /// </summary>
        public void setCircleBig()
        {
            circle.transform.localScale = Vector2.one * circleBigSize;
            initialOrEnd = true;  //This tile is an initial or end tile
        }

        /// <summary>
        /// Makes smaller the scale of the circle image 
        /// </summary>
        public void setCircleSmall()
        {
            circle.transform.localScale = Vector2.one * circleSmallSize;
            initialOrEnd = false;   //This tile is not an initial or end tile
        }

        /// <summary>
        /// Actives the tile
        /// </summary>
        /// <param name="b"></param>
        public void setActiveTile(bool b)
        {
            if (initialOrEnd && !b) 
                return;
            activeTile = b;
        }

        /// <summary>
        /// Returns if the tile is active
        /// </summary>
        /// <returns></returns>
        public bool isActive()
        {
            return activeTile;
        }

        /// <summary>
        /// Returns if the tile is empty
        /// </summary>
        /// <returns></returns>
        public bool isEmpty()
        {
            return empty;
        }

        /// <summary>
        /// Returns if the tile is an intial or end tile
        /// </summary>
        /// <returns></returns>
        public bool isInitialOrEnd()
        {
            return initialOrEnd;
        }

        /// <summary>
        /// Sets the color of all images of the tile
        /// </summary>
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

        /// <summary>
        /// Enables or disables the direction image
        /// </summary>
        /// <param name="dir">Direction</param>
        /// <param name="e">If it is enbaled or disabled</param>
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
