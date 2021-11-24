using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Tile tile;
        private Tile[,] tiles;

#if UNITY_EDITOR
        void Start()
        {
            if(tile == null)
            {
                Debug.LogError("Prefab of board not setted");
                return;
            }

            Logic.Map map = new Logic.Map();
            map.loadLevel("5,0,1,5;18,17,12;21,16,11,6;3,4,9;0,1,2,7,8,13,14,19,24,23,22;20,15,10,5");
            setForGame(map);
        }
#endif
        public void setForGame(Logic.Map map)
        {
            Vector3 pos = transform.position;
            uint height = map.getLevelHeight(), width = map.getLevelWidth();

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
                uint color = Logic.Colors.ClassicColors[i];

                //Inicial
                Tuple<uint,uint> initial = pipes[i][0];
                Tile initTile = tiles[initial.Item1, initial.Item2];
                initTile.setColor(color);
                initTile.setCircleBig();
                initTile.enableCircle();

                //Final
                Tuple<uint, uint> final = pipes[i][pipeLength - 1];
                Tile endTile = tiles[final.Item1, final.Item2];
                endTile.setColor(color);
                endTile.setCircleBig();
                endTile.enableCircle();
            }
        }
    }
}
