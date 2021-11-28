using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Board : MonoBehaviour
    {
        private Color[] themeColors;
        private uint width;
        private uint height;

        [SerializeField] private Tile tile;
        private Tile[,] tiles;

        private Vector2 lastCursorTilePos;
        private List<Vector2> path;

        private BoardInput input;

        private bool draging;

        private void Awake()
        {
            input = GetComponent<BoardInput>();
            lastCursorTilePos = Vector2.negativeInfinity;
            draging = false;
            path = new List<Vector2>();
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
            if (input.justDown())
            { 
                Vector2 t = input.getMouseTilePos();
                path.Clear();
                if (getTile(t).isActive())
                {
                    lastCursorTilePos = t;
                    draging = true;
                }
                else
                {
                    lastCursorTilePos = Vector2.negativeInfinity;
                    draging = false;
                }
            }
            else if (input.justUp())
            {
                draging = false;
                lastCursorTilePos = Vector2.negativeInfinity;
                path.Clear();
            }

            if (draging && input.isPressed())
            {
                Vector2 t = input.getMouseTilePos();
                Tile tileActual = getTile(t);

                //I dragged the tile
                if(t != lastCursorTilePos)
                {
                    if (path.Contains(t))
                    {
                        while(path[path.Count - 1] != t)
                        {
                            Tile tile = getTile(path[path.Count - 1]);
                            tile.disableAll();
                            path.RemoveAt(path.Count - 1);
                        }
                        getTile(lastCursorTilePos).disableAll();
                        lastCursorTilePos = t;
                        path.RemoveAt(path.Count - 1);
                    }
                    else
                    {
                        Tile lastCursorTile = getTile(lastCursorTilePos);

                        Vector2 delta = t - lastCursorTilePos;
                        delta = Vector2.Perpendicular(delta) * -1;
                        Logic.Dir dir = Logic.Direction.GetDirectionFromVector(delta);

                        lastCursorTile.enableDirectionSprite(dir);
                        tileActual.setColor(lastCursorTile.getColor());
                        tileActual.enableDirectionSprite(Logic.Direction.Opposite(dir));

                        path.Add(lastCursorTilePos);
                        lastCursorTilePos = t;
                    }
                   
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

            List<List<Tuple<uint,uint>>> pipes = map.getPipes();
            uint nPipes = map.getNPipes();
            for (int i = 0; i < nPipes; ++i)
            {
                int pipeLength = pipes[i].Count;
                Color color = colors[i];

                //Inicial
                Tuple<uint,uint> initial = pipes[i][0];
                Tile initTile = tiles[initial.Item1, initial.Item2];
                initTile.setColor(color);
                initTile.setCircleBig();
                initTile.setActiveTile(true);
                initTile.enableCircle();

                //Final
                Tuple<uint, uint> final = pipes[i][pipeLength - 1];
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
