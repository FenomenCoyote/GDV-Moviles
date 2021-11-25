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

        private BoardInput input;



        void Start()
        {
            input = GetComponent<BoardInput>();

#if UNITY_EDITOR
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
#endif
        }


        void Update()
        {
            if (input.justDown())
            { 
                Vector2 t = input.getMouseTilePos();

                if (t != Vector2.negativeInfinity && getTile(t).isActive())
                {
                    lastCursorTilePos = t;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                lastCursorTilePos = Vector2.negativeInfinity;
            }
            else if (input.isPressed() && input.getMouseTilePos() != Vector2.negativeInfinity)
            {
                Vector2 t = input.getMouseTilePos();

                Tile tileActual = getTile(t);

                //I dragged the tile
                if(t != lastCursorTilePos)
                {
                    Tile lastCursorTile = getTile(lastCursorTilePos);

                    Vector2 delta = t - lastCursorTilePos;
                    delta = Vector2.Perpendicular(delta) * -1;
                    Logic.Dir dir = Logic.Direction.GetDirectionFromVector(delta);

                    lastCursorTile.enableDirectionSprite(dir);
                    tileActual.setColor(lastCursorTile.getColor());
                    tileActual.enableDirectionSprite(Logic.Direction.Opposite(dir));

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
