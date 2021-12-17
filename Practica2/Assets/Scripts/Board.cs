using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    public class Board : MonoBehaviour
    {
        //Manages input for me
        private BoardInput input;

        //Size 
        private uint width;
        private uint height;

        [Tooltip("Prefab to instance")]
        [SerializeField] private Tile tile;

        //Tile array
        private Tile[,] tiles;

        //Pipes (or flows) 
        private Dictionary<Color, Logic.Pipe> pipes;

        //To manage logic and render
        private bool draging;
        private Color dragingColor, lastDraggedColor;
        private Logic.Pipe currentPipe;

        //Manages each step the player did
        private Color lastSolution;
        private int steps;

        //Pathfinding algorithm
        private AStar aStar;

        //Manages hints
        private HashSet<Color> hintsDone;
        private Dictionary<Color, List<Vector2>> hintsSolution;

        [Tooltip("How AStar algorithim penalizes some tiles")]
        [SerializeField]
        private int pathFindingPenalizeOtherColors = 1, pathFindingPenalizeMyEnd = 20;

        [Tooltip("Text to update how many steps have been done")]
        [SerializeField]
        private Text stepsText;

        [Tooltip("Text to update how many flows are completed")]
        [SerializeField]
        private Text flowsText;

        [Tooltip("Text to update how many tiles are filled with a flow")]
        [SerializeField]
        private Text percentageText;

        [Tooltip("Reference to an Input Pointer")]
        [SerializeField]
        private InputPointerManager inputPointer;

        [Tooltip("Reference to the Level Manager")]
        [SerializeField]
        private LevelManager levelManager;

        //Scales the board in relation to the screen
        private BoardScaler scaler;

        /// <summary>
        /// Disables input on board
        /// </summary>
        public void disableInput()
        {
            if (input)
                input.disable();
        }

        /// <summary>
        /// Enables input on board
        /// </summary>
        public void enableInput()
        {
            if (input)
                input.enable();
        }

        /// <summary>
        /// Gets number of pipes
        /// </summary>
        /// <returns></returns>
        public int getNPipes()
        {
            return pipes.Count;
        }

        /// <summary>
        /// Gets number of steps
        /// </summary>
        /// <returns></returns>
        public int getSteps()
        {
            return steps;
        }

        /// <summary>
        /// Initializes some variables
        /// </summary>
        private void Awake()
        {
            input = GetComponent<BoardInput>();
            scaler = GetComponent<BoardScaler>();
            draging = false;
            pipes = new Dictionary<Color, Logic.Pipe>();
            steps = 0;
            aStar = new AStar();

            inputPointer.enabled = false;

            hintsDone = new HashSet<Color>();
            hintsSolution = new Dictionary<Color, List<Vector2>>();
        }

        /// <summary>
        /// Serialized variables check in debug
        /// </summary>
#if UNITY_EDITOR
        void Start()
        {
            if (tile == null)
            {
                Debug.LogError("Prefab of board not setted");
                return;
            }
            if (input == null)
            {
                Debug.LogError("Input board not setted in Board");
                return;
            }
            if (stepsText == null)
            {
                Debug.LogError("stepsText not setted in Board");
                return;
            }
            if (flowsText == null)
            {
                Debug.LogError("flowsText not setted in Board");
                return;
            }
            if (percentageText == null)
            {
                Debug.LogError("percentageText not setted in Board");
                return;
            }
            if (inputPointer == null)
            {
                Debug.LogError("inputPointerSprite not setted in Board");
                return;
            }
            if (levelManager == null)
            {
                Debug.LogError("levelManager not setted in Board");
                return;
            }
        }
#endif

        void Update()
        {
            //Needs to be called it here to make the update call order right
            input.updateInput();

            bool updateMap = false;

            if (input.justDown() && input.isInside()) //start a drag
            {          
                updateMap = inputDown();
            }
            else if (currentPipe != null && input.justUp()) //end a drag
            {        
                updateMap = inputUp();
            }
            else if (draging && input.isPressed() && input.isInside()) //drag
            {
                updateMap = inputDrag();
            }

            //if player interacted, render current state and check if im done
            if (updateMap)
            {
                render();

                percentageText.text = "pipe: " + getPercentage() + "%";
                stepsText.text = "moves: " + steps;

                int completed = getPipesCompleted();
                flowsText.text = "flows: " + completed + "/" + pipes.Count;

                if(completed == pipes.Count)
                {
                    levelManager.levelDone(steps);
                }
            }
        }

        /// <summary>
        /// Called when input was just down
        /// </summary>
        /// <returns>True if the player clicked an active tile, false otherwise</returns>
        private bool inputDown()
        {
            Vector2 t = input.getMouseTilePos();

            if (getTile(t).isActive())
            {
                draging = true;
                dragingColor = getTile(t).getColor();

                //pointer things
                inputPointer.enabled = true;
                inputPointer.setColor(dragingColor);
                inputPointer.setCorrect();

                currentPipe = pipes[dragingColor];

                currentPipe.startDrag(t);

                getTile(currentPipe.getStartPos()).shake();
                getTile(currentPipe.getFinalPos()).shake();

                lastDraggedColor = dragingColor;

                return true;
            }
            else
            {
                resetMyInfo();
                return false;
            }
        }

        /// <summary>
        /// Called when the player releases the input (finger or mouse)
        /// </summary>
        /// <returns>True</returns>
        private bool inputUp()
        {
            if (currentPipe != null)
            {
                currentPipe.notDraggingAnymore();
            }

            if (input.isInside() && ! (getTile(input.getMouseTilePos()).isInitialOrEnd() && getTile(input.getMouseTilePos()).getColor() != dragingColor))
                AStar(input.getMouseTilePos());

            foreach (KeyValuePair<Color, Logic.Pipe> pipe in pipes)
            {
                if (dragingColor != null && pipe.Key == dragingColor)
                    continue;

                pipe.Value.finallyCut();
            }

            if(lastSolution != dragingColor && currentPipe.changedSolution())
            {
                steps++;
                lastSolution = dragingColor;
            }

            if (currentPipe.isClosed())
            {
                SoundManager.Instance.playSound(SoundManager.Sound.Forward);
                getTile(currentPipe.positions[currentPipe.positions.Count - 1]).finishedAnim();
            }
            else
                getTile(currentPipe.positions[currentPipe.positions.Count - 1]).shake();

            inputPointer.enabled = false;

            resetMyInfo();

            return true;
        }

        /// <summary>
        /// Called when the player is dragging
        /// </summary>
        /// <returns>True if player moved somewhere, false otherwise</returns>
        private bool inputDrag()
        {
            Vector2 pos = input.getMouseTilePos();

            if (input.isInside())
                inputPointer.setCorrect();
            else
                inputPointer.setNotCorrect();

            if (currentPipe.getOrigin() == pos)
                return false;

            if (getTile(pos).isInitialOrEnd() && getTile(pos).getColor() != dragingColor)
            {
                inputPointer.setNotCorrect();
                return false;
            }

            return AStar(pos);
        }

        /// <summary>
        /// Pathfind towards 'dest'
        /// </summary>
        /// <param name="dest"></param>
        /// <returns>True if a path tried, false if i tried to go to an empty tile</returns>
        private bool AStar(Vector2 dest)
        {
            Vector2 origin = currentPipe.getOrigin();

            if (origin == null)
            {
                Debug.LogError("origin cant be null when dragging");
                return false;
            }

            if (getTile(dest).isEmpty())
                return false;

            List<Vector2> path = aStar.Astrella(origin, dest, dragingColor);
            path.RemoveAt(0);

            currentPipe.addPathFromOrigin(path);

            foreach (KeyValuePair<Color, Logic.Pipe> pipe in pipes)
            {
                if (dragingColor != null && pipe.Key == dragingColor)
                    continue;

                if (pipe.Value.provisionalCut(currentPipe) && pipe.Value.isClosed())
                {
                    getTile(pipe.Value.getLastPosProvisional()).finishedAnim();
                    SoundManager.Instance.playSound(SoundManager.Sound.Leak);
                }
            }
            return true;
        }

        /// <summary>
        /// Disables every tile sprite renderer
        /// </summary>
        private void clear()
        {
            foreach(Tile t in tiles)
            {
                t.disableAll();
            }
        }

        /// <summary>
        /// Renders current board state
        /// </summary>
        private void render()
        {
            clear();

            //render each pipe
            foreach (KeyValuePair<Color, Logic.Pipe> pipe in pipes)
            {
                //I skip current render pipe
                if (dragingColor != null && pipe.Key == dragingColor)
                    continue;

                renderPipe(pipe.Key, pipe.Value, true);
            }

            //render current pipe last
            if (currentPipe != null)
                renderPipe(dragingColor, currentPipe, false);
        }

        /// <summary>
        /// Renders a pipe in color and highlighted if needed
        /// </summary>
        /// <param name="color"></param>
        /// <param name="pipe"></param>
        /// <param name="highLight"></param>
        private void renderPipe(Color color, Logic.Pipe pipe, bool highLight)
        {
            //Check if im being provisionally cutted
            if(pipe.provisionalIndex == 1000)
            {
                for (int i = 0; i < pipe.positions.Count; ++i)
                {
                    Tile t1 = getTile(pipe.positions[i]);
                    t1.setColor(color);

                    t1.setActiveTile(true);

                    if (color == lastDraggedColor && i == pipe.positions.Count - 1)
                        t1.enableCircle();

                    if (highLight && pipe.positions.Count > 1)
                        t1.enableHightLight();

                    if (i == pipe.positions.Count - 1)
                        break;

                    Tile t2 = getTile(pipe.positions[i + 1]);

                    Logic.Dir dir = getDir(pipe.positions[i + 1], pipe.positions[i]);

                    t1.enableDirectionSprite(dir);
                    t2.enableDirectionSprite(Logic.Direction.Opposite(dir));
                }
            }
            else
            {
                for (int i = 0; i < pipe.provisionalIndex - 1; ++i)
                {
                    Tile t1 = getTile(pipe.positions[i]);
                    t1.setColor(color);

                    t1.setActiveTile(true);

                    if (highLight && pipe.positions.Count > 1)
                        t1.enableHightLight();

                    if (i == pipe.positions.Count - 1)
                        break;

                    Tile t2 = getTile(pipe.positions[i + 1]);
                    t2.setColor(color);
                    if (highLight && pipe.positions.Count > 1)
                        t2.enableHightLight();

                    Logic.Dir dir = getDir(pipe.positions[i + 1], pipe.positions[i]);

                    t1.enableDirectionSprite(dir);
                    t2.enableDirectionSprite(Logic.Direction.Opposite(dir));
                }

                for (int i = pipe.provisionalIndex; i < pipe.positions.Count; ++i)
                {
                    Tile t1 = getTile(pipe.positions[i]);
                    t1.setColor(color);

                    if (highLight && pipe.positions.Count > 1)
                    {
                        t1.setHightLock(false);
                        t1.enableHightLight();
                        t1.setHightLock(true);
                        continue;
                    }
                }
            }

            //If pipe was hinted...
            if (pipe.isClosed() && hintsDone.Contains(color) && hintsSolution[color].Count == pipe.positions.Count)
            {
                bool isHintSolution = true;

                foreach(Vector2 p in hintsSolution[color])
                {
                    if (!pipe.positions.Contains(p))
                    {
                        isHintSolution = false;
                        break;
                    }
                }

                if (isHintSolution)
                {
                    getTile(pipe.positions[0]).setHinted();
                    getTile(pipe.positions[pipe.positions.Count - 1]).setHinted();
                }
            }
        }

        /// <summary>
        /// Gets dir from origin towards dest
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="origen"></param>
        /// <returns></returns>
        private Logic.Dir getDir(Vector2 dest, Vector2 origen)
        {
            Vector2 delta = dest - origen;
            delta = Vector2.Perpendicular(delta) * -1;
            return Logic.Direction.GetDirectionFromVector(delta);
        }

        /// <summary>
        /// Rests meta info that keeps track of the drag
        /// </summary>
        private void resetMyInfo()
        {
            draging = false;
            dragingColor = Color.black;
            currentPipe = null;
        }

        /// <summary>
        /// Gets the number of pipes that are closed
        /// </summary>
        /// <returns></returns>
        private int getPipesCompleted()
        {
            int completed = 0;
            foreach (Logic.Pipe p in pipes.Values)
            {
                if (p.isClosed())
                {
                    ++completed;
                }
            }
            return completed;
        }

        /// <summary>
        /// Gets a number in [0, 100] that represent the percentage of tiles filled
        /// </summary>
        /// <returns></returns>
        private int getPercentage()
        {
            int active = 0;
            int empties = 0;
            foreach (Tile t in tiles)
            {
                if (t.isActive()) ++active;
                if (t.isEmpty()) ++empties;
            }

            active -= pipes.Count * 2;

            active += getPipesCompleted() * 2;

            float done = (float)active / (float)(width * height - empties);

            return (int)(done * 100.0f);
        }

        /// <summary>
        /// Initializes board using Map info
        /// </summary>
        /// <param name="map"></param>
        /// <param name="colors"></param>
        /// <param name="categoryColor"></param>
        /// <param name="record"></param>
        public void setForGame(Logic.Map map, Color[] colors, Color categoryColor)
        {
            Vector3 pos = transform.position;

            height = map.getLevelHeight();
            width = map.getLevelWidth();

            scaler.fitInScreen((int)width, (int)height);

            input.init(width, height);

            pos.y = height / 2;
            if (height % 2 == 0)
            {
                pos.y -= 0.5f;
            }
            tiles = new Tile[height, width];
            for (int i = 0; i < height; i++)
            {
                pos.x = -width / 2;
                if (width % 2 == 0)
                {
                    pos.x += 0.5f;
                }
                for (int j = 0; j < width; j++)
                {
                    tiles[i, j] = Instantiate(tile, pos, Quaternion.identity, transform);

                    Vector3 aux = tiles[i, j].transform.localPosition;
                    aux.y = pos.y;
                    aux.x = pos.x;
                    tiles[i, j].transform.localPosition = aux;
                    tiles[i, j].setCircleSmall();
                    tiles[i, j].setBoundaryColors(categoryColor);
                    pos.x++;
                }
                pos.y--;
            }

            hintsDone.Clear();
            hintsSolution.Clear();

            List<List<Tuple<uint, uint>>> _pipes = map.getPipes();
            uint nPipes = map.getNPipes();
            for (int i = 0; i < nPipes; ++i)
            {
                int pipeLength = _pipes[i].Count;
                Color color = colors[i];

                //Initial
                Tuple<uint, uint> initial = _pipes[i][0];
                Tile initTile = tiles[initial.Item1, initial.Item2];
                initTile.setColor(color);
                initTile.setCircleBig();
                initTile.setActiveTile(true);
                initTile.enableCircle();

                //Final
                Tuple<uint, uint> final = _pipes[i][pipeLength - 1];
                Tile endTile = tiles[final.Item1, final.Item2];
                endTile.setColor(color);
                endTile.setCircleBig();
                endTile.setActiveTile(true);
                endTile.enableCircle();

                Logic.Pipe newPipe = new Logic.Pipe();

                newPipe.setInitialAndEndTiles(new Vector2(initial.Item1, initial.Item2), new Vector2(final.Item1, final.Item2));

                this.pipes.Add(color, newPipe);

                hintsSolution.Add(color, new List<Vector2>());
                foreach (Tuple<uint, uint> p in _pipes[i])
                    hintsSolution[color].Add(new Vector2(p.Item1, p.Item2));
             
            }

            bool addBounds = false;

            foreach (Tuple<Tuple<uint, uint>, Tuple<uint, uint>> wall in map.getWalls())
            {
                Vector2 origin = new Vector2(wall.Item1.Item1, wall.Item1.Item2);
                Vector2 dest = new Vector2(wall.Item2.Item1, wall.Item2.Item2);

                Vector2 delta = dest - origin;
                delta = Vector2.Perpendicular(delta) * -1;
                Logic.Dir dir = Logic.Direction.GetDirectionFromVector(delta);

                getTile(origin).setWall(dir);
                getTile(dest).setWall(Logic.Direction.Opposite(dir));

                addBounds = true;
            }

            foreach (Tuple<uint, uint> empty in map.getEmptyTiles())
            {
                Vector2 t = new Vector2(empty.Item1, empty.Item2);
                Tile tile = getTile(t);

                tile.setEmpty();

                if (t.x > 0) 
                    getTile(t + Vector2.left).setWall(Logic.Dir.Down);
                if (t.y > 0) 
                    getTile(t + Vector2.down).setWall(Logic.Dir.Right);
                if (t.x < height - 1) 
                    getTile(t + Vector2.right).setWall(Logic.Dir.Up);
                if (t.y < width - 1)
                    getTile(t + Vector2.up).setWall(Logic.Dir.Left);

                addBounds = true;
            }

            if (addBounds)
                borderAll();

            aStar.RecibeLaberinto(tiles, pathFindingPenalizeOtherColors, pathFindingPenalizeMyEnd);

            flowsText.text = "flows: 0/" + pipes.Count;
            percentageText.text = "pipe: 0%";
            stepsText.text = "moves: 0";
        }

        /// <summary>
        /// Resets board but keeps it's initial state
        /// </summary>
        public void resetBoard()
        {
            if(steps > 0 || getPercentage() > 0)
                SoundManager.Instance.playSound(SoundManager.Sound.Leak);

            foreach (Logic.Pipe p in pipes.Values)
            {
                p.reset();
            }
            steps = 0;
            lastSolution = Color.black;
            resetMyInfo();

            input.enable();

            foreach (Tile t in tiles)
            {
                t.disableAll();
                t.disableCircle();
                t.setActiveTile(false);
            }

            percentageText.text = "pipes: " + getPercentage() + "%";
            stepsText.text = "moves: 0";
            flowsText.text = "flows: 0/" + pipes.Count;
        }

        /// <summary>
        /// Gives next hint
        /// </summary>
        public void nextHint()
        {
            foreach(KeyValuePair<Color, Logic.Pipe> pipe in pipes)
            {
                //If it's not already done
                if(!hintsDone.Contains(pipe.Key) && !pipe.Value.isClosed())
                {
                    hintsDone.Add(pipe.Key);

                    steps++;

                    pipe.Value.setSolution(hintsSolution[pipe.Key]);

                    foreach (KeyValuePair<Color, Logic.Pipe> otherPipes in pipes)
                    {
                        if (pipe.Key == otherPipes.Key)
                            continue;

                        otherPipes.Value.provisionalCut(pipe.Value);
                        otherPipes.Value.finallyCut();
                    }

                    render();

                    percentageText.text = "pipe: " + getPercentage() + "%";
                    stepsText.text = "moves: " + steps;

                    int completed = getPipesCompleted();
                    flowsText.text = "flows: " + completed + "/" + pipes.Count;

                    if (completed == pipes.Count)
                    {
                        levelManager.levelDone(steps);
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// Borders every exterior tile
        /// </summary>
        private void borderAll()
        {
            for(int i = 0; i < height; ++i)
            {
                tiles[i, 0].setWall(Logic.Dir.Left);
                tiles[i, width - 1].setWall(Logic.Dir.Right);
            }
            for (int i = 0; i < width; ++i)
            {
                tiles[0, i].setWall(Logic.Dir.Up);
                tiles[height - 1, i].setWall(Logic.Dir.Down);
            }
        }

        /// <summary>
        /// Gets tile from position
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private Tile getTile(Vector2 pos)
        {
            return tiles[(int)pos.x, (int)pos.y];
        }
    }
}
