using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Board : MonoBehaviour
    {
        private BoardInput input;

        private uint width;
        private uint height;

        [SerializeField] private Tile tile;
        private Tile[,] tiles;

        private Vector2 lastCursorTilePos;

        private Dictionary<Color, Logic.Pipe> pipes;

        private bool draging;
        private Color dragingColor;
        private Logic.Pipe currentPipe;

        private void Awake()
        {
            input = GetComponent<BoardInput>();
            lastCursorTilePos = Vector2.negativeInfinity;
            draging = false;
            pipes = new Dictionary<Color, Logic.Pipe>();
        }

#if UNITY_EDITOR
        void Start()
        {
            if (tile == null)
            {
                Debug.LogError("Prefab of board not setted");
                return;
            }
            if(input == null)
            {
                Debug.LogError("Input board not setted in Board");
                return;
            }
        }
#endif


        void Update()
        {
            input.updateInput();

            if (input.justDown())
            { 
                Vector2 t = input.getMouseTilePos();
                if (getTile(t).isActive())
                {
                    lastCursorTilePos = t;
                    draging = true;
                    dragingColor = getTile(t).getColor();
                    currentPipe = pipes[dragingColor];

                    currentPipe.startDrag(t);
                    currentPipe.cutMyself(t);
                }
                else
                {
                    lastCursorTilePos = Vector2.negativeInfinity;
                    draging = false;
                    dragingColor = Color.black;
                    currentPipe = null;
                }
            }
            else if (input.justUp())
            {
                lastCursorTilePos = Vector2.negativeInfinity;
                draging = false;
                dragingColor = Color.black;

                if(currentPipe != null)
                {
                    currentPipe.notDraggingAnymore();
                    foreach (Logic.Pipe p in pipes.Values)
                        if (currentPipe != p)
                            p.restoreFromProvisionalCut(currentPipe);
                }

                currentPipe = null;
            }

            if (draging && input.isPressed() && input.isInside())
            {
                Vector2 t = input.getMouseTilePos();
                Tile tileActual = getTile(t);

                float distance = Vector2.Distance(t, lastCursorTilePos);
                //I moved
                if (distance >= 0.1f && distance < 1.1f)
                {
                    if (!tileActual.isInitialOrEnd() || tileActual.getColor() == dragingColor)
                    {
                        if (tileActual.isActive() && tileActual.getColor() != dragingColor)
                        {
                            pipes[tileActual.getColor()].provisionalCut(currentPipe, t);
                        }

                        currentPipe.add(t, tileActual);

                        if (currentPipe.pollCutted())
                        {
                            foreach (Logic.Pipe p in pipes.Values)
                                if (currentPipe != p)
                                    p.restoreFromProvisionalCut(currentPipe, true);
                        }

                        if (currentPipe.isClosed())
                        {
                            foreach (Logic.Pipe p in pipes.Values)
                                if (currentPipe != p)
                                    p.restoreFromProvisionalCut(currentPipe);

                            lastCursorTilePos = Vector2.negativeInfinity;
                            draging = false;
                            dragingColor = Color.black;
                            currentPipe.notDraggingAnymore();
                            currentPipe = null;
                        }
                        else
                            lastCursorTilePos = t;
                    }
                }
                else if(distance > 1.1f && tileActual.getColor() == dragingColor)
                {
                    currentPipe.cutMyself(t);
                    lastCursorTilePos = t;
                }
            }

            
        }

        public void setForGame(Logic.Map map, Color[] colors)
        {
            Vector3 pos = transform.position;

            height = map.getLevelHeight();
            width = map.getLevelWidth();

            input.init(width, height);
            
            pos.y = height / 2;
            tiles = new Tile[height, width];
            for (int i = 0; i < height; i++)
            {
                pos.x = -width / 2;
                for (int j = 0; j < width; j++)
                {
                    tiles[i,j] = Instantiate(tile, pos, Quaternion.identity, transform);

                    Vector3 aux = tiles[i, j].transform.localPosition;
                    aux.y = pos.y;
                    aux.x = pos.x;
                    tiles[i, j].transform.localPosition = aux;
                    tiles[i, j].setCircleSmall();
                    pos.x++;
                }
                pos.y--;
            }

            List<List<Tuple<uint,uint>>> _pipes = map.getPipes();
            uint nPipes = map.getNPipes();
            for (int i = 0; i < nPipes; ++i)
            {
                int pipeLength = _pipes[i].Count;
                Color color = colors[i];

                //Inicial
                Tuple<uint,uint> initial = _pipes[i][0];
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
                newPipe.setColor(color);
                newPipe.setInitialAndEndTiles(new Vector2(initial.Item1, initial.Item2), initTile, new Vector2(final.Item1, final.Item2), endTile);
                this.pipes.Add(color, newPipe);
            }

            foreach(Tuple<Tuple<uint, uint>, Tuple<uint, uint>> wall in map.getWalls())
            {
                Vector2 origin = new Vector2(wall.Item1.Item1, wall.Item1.Item2);
                Vector2 dest = new Vector2(wall.Item2.Item1, wall.Item2.Item2);

                Vector2 delta = dest - origin;
                delta = Vector2.Perpendicular(delta) * -1;
                Logic.Dir dir = Logic.Direction.GetDirectionFromVector(delta);

                getTile(origin).setWall(dir);
                getTile(dest).setWall(Logic.Direction.Opposite(dir));
            }
        }

        private Tile getTile(Vector2 mousePos)
        {
            return tiles[(int)mousePos.x, (int)mousePos.y];
        }
    }
}
