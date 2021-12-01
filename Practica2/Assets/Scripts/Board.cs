using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Board : MonoBehaviour
    {
        private BoardInput input;

        private Color[] themeColors;
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
                currentPipe = null;
            }

            if (draging && input.isPressed() && input.isInside())
            {
                Vector2 t = input.getMouseTilePos();
                Tile tileActual = getTile(t);

                //I moved
                if (Vector2.Distance(t, lastCursorTilePos) >= 0.1)
                {
                    //If i moved to an active tile that is not my type, i need to cut that pipe
                    if (tileActual.isActive() && tileActual.getColor() != dragingColor) { //cortar
                        pipes[tileActual.getColor()].cut(t);
                        return;
                    }
                    // If i didnt cut myself and i moved only 1 position 
                    if (!currentPipe.cutMyself(t))
                    {
                        if (Vector2.Distance(t, lastCursorTilePos) <= 1.1f)
                        {
                            Tile lastCursorTile = getTile(lastCursorTilePos);

                            Vector2 delta = t - lastCursorTilePos;
                            delta = Vector2.Perpendicular(delta) * -1;
                            Logic.Dir dir = Logic.Direction.GetDirectionFromVector(delta);

                            //Si he llegado a una final
                            if (tileActual.isInitialOrEnd())
                            {
                                // y del mismo color
                                if (tileActual.getColor() == lastCursorTile.getColor())
                                {
                                    lastCursorTile.enableDestDirectionSprite(dir);
                                    tileActual.enableSourceDirectionSprite(Logic.Direction.Opposite(dir));
                                    lastCursorTilePos = Vector2.negativeInfinity;
                                    draging = false;
                                    currentPipe.close();
                                }
                            }
                            else
                            {
                                lastCursorTile.enableDestDirectionSprite(dir);
                                tileActual.setColor(lastCursorTile.getColor());
                                tileActual.enableSourceDirectionSprite(Logic.Direction.Opposite(dir));
                                tileActual.setActiveTile(true);
                                currentPipe.add(t, tileActual);
                                lastCursorTilePos = t;
                            }
                        }
                    }
                    else
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

                this.pipes.Add(color, new Logic.Pipe());

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
            }
        }

        private Tile getTile(Vector2 mousePos)
        {
            return tiles[(int)mousePos.x, (int)mousePos.y];
        }
    }
}
