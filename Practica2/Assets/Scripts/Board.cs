using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    public class Board : MonoBehaviour
    {
        private BoardInput input;

        private uint width;
        private uint height;

        [SerializeField] private Tile tile;
        private Tile[,] tiles;

        private Dictionary<Color, Logic.Pipe> pipes;

        private bool draging;
        private Color dragingColor, lastDraggedColor;
        private Logic.Pipe currentPipe;

        private Color lastSolution;
        private int steps;

        private AStar aStar;

        private HashSet<Color> hintsDone;
        private Dictionary<Color, List<Vector2>> hintsSolution;

        [SerializeField]
        private int pathFindingPenalizeOtherColors = 1, pathFindingPenalizeMyEnd = 20;

        [SerializeField]
        Text stepsText;

        [SerializeField]
        Text flowsText;

        [SerializeField]
        Text percentageText;

        [SerializeField]
        InputPointerManager inputPointer;

        [SerializeField]
        LevelManager levelManager;

        BoardScaler scaler;

        public void disableInput()
        {
            if (input)
                input.disable();
        }

        public void enableInput()
        {
            if (input)
                input.enable();
        }

        public int getNPipes()
        {
            return pipes.Count;
        }

        public int getSteps()
        {
            return steps;
        }

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

            if (input.justDown() && input.isInside())
            {
                inputDown();
                updateMap = true;
            }
            else if (currentPipe != null && input.justUp())
            {
                inputUp();
                updateMap = true;
            }
            else if (draging && input.isPressed() && input.isInside())
            {
                inputDrag();
                updateMap = true;
            }

            if (updateMap)
            {
                render();

                percentageText.text = "tuber�a: " + getPercentage() + "%";
                stepsText.text = "pasos: " + steps;

                int completed = getPipesCompleted();
                flowsText.text = "flujos: " + completed + "/" + pipes.Count;

                if(completed == pipes.Count)
                {
                    levelManager.levelDone(steps);
                }
            }
        }

        private void inputDown()
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
            }
            else
            {
                resetMyInfo();
            }
        }

        private void inputUp()
        {
            if (currentPipe != null)
            {
                currentPipe.notDraggingAnymore();
            }

            if (input.isInside())
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
                GameManager.Instance.soundManager.playSound(SoundManager.Sound.Forward);
                getTile(currentPipe.positions[currentPipe.positions.Count - 1]).finishedAnim();
            }
            else
                getTile(currentPipe.positions[currentPipe.positions.Count - 1]).shake();

            inputPointer.enabled = false;

            resetMyInfo();
        }

        private void inputDrag()
        {
            Vector2 pos = input.getMouseTilePos();

            inputPointer.setCorrect();

            if (currentPipe.getOrigin() == pos)
                return;

            if (getTile(pos).isInitialOrEnd() && getTile(pos).getColor() != dragingColor)
            {
                inputPointer.setNotCorrect();
                return;
            }

            AStar(pos);
        }


        private void AStar(Vector2 dest)
        {
            Vector2 origin = currentPipe.getOrigin();

            if (origin == null)
            {
                Debug.LogError("origin cant be null when dragging");
                return;
            }

            if (getTile(dest).isEmpty())
                return;

            List<Vector2> path = aStar.Astrella(origin, dest, dragingColor);
            path.RemoveAt(0);
            currentPipe.addPathFromOrigin(path);

            foreach (KeyValuePair<Color, Logic.Pipe> pipe in pipes)
            {
                if (dragingColor != null && pipe.Key == dragingColor)
                    continue;

                if (pipe.Value.provisionalCut(currentPipe) && pipe.Value.isClosed())
                    GameManager.Instance.soundManager.playSound(SoundManager.Sound.Leak);
            }
        }


        private void clear()
        {
            foreach(Tile t in tiles)
            {
                t.disableAll();
            }
        }

        private void render()
        {
            clear();
            //render each pipe
            foreach (KeyValuePair<Color, Logic.Pipe> pipe in pipes)
            {
                if (dragingColor != null && pipe.Key == dragingColor)
                    continue;

                renderPipe(pipe.Key, pipe.Value, true);
            }

            //render current pipe 
            if (currentPipe != null)
                renderPipe(dragingColor, currentPipe, false);
        }

        private void renderPipe(Color color, Logic.Pipe pipe, bool highLight)
        {
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

        private Logic.Dir getDir(Vector2 dest, Vector2 origen)
        {
            Vector2 delta = dest - origen;
            delta = Vector2.Perpendicular(delta) * -1;
            return Logic.Direction.GetDirectionFromVector(delta);
        }

        private void resetMyInfo()
        {
            draging = false;
            dragingColor = Color.black;
            currentPipe = null;
        }

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


        public void setForGame(Logic.Map map, Color[] colors, Color categoryColor, int record)
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

                //Inicial
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

            flowsText.text = "flujos: 0/" + pipes.Count;
            percentageText.text = "tuber�a: 0%";
            stepsText.text = "pasos: 0";
        }

        public void resetBoard()
        {
            if(steps > 0 || getPercentage() > 0)
                GameManager.Instance.soundManager.playSound(SoundManager.Sound.Leak);

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

            percentageText.text = "tuber�a: " + getPercentage() + "%";
            stepsText.text = "pasos: 0";
            flowsText.text = "flujos: 0/" + pipes.Count;
        }


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

                    percentageText.text = "tuber�a: " + getPercentage() + "%";
                    stepsText.text = "pasos: " + steps;

                    int completed = getPipesCompleted();
                    flowsText.text = "flujos: " + completed + "/" + pipes.Count;

                    if (completed == pipes.Count)
                    {
                        levelManager.levelDone(steps);
                    }

                    return;
                }
            }
        }


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

        private Tile getTile(Vector2 mousePos)
        {
            return tiles[(int)mousePos.x, (int)mousePos.y];
        }
    }
}
