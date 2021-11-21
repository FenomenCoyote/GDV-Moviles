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
            pos.y = 2f;
            tiles = new Tile[map.getLevelHeight(), map.getLevelWidth()];
            for (int i = 0; i < map.getLevelHeight(); i++)
            {
                pos.x = -2f;
                for (int j = 0; j < map.getLevelWidth(); j++)
                {
                    tiles[i,j] = Instantiate<Tile>(tile, pos, Quaternion.identity, transform);
                    pos.x++;
                }
                pos.y--;
            }

            List<List<uint>> pipes = map.getPipes();
            uint nPipes = map.getNPipes();

            uint boardWidth = map.getLevelWidth();
            uint boardHeight = map.getLevelHeight();

            for (int i=0; i<nPipes; ++i)
            {
                int pipeLength = pipes[i].Count;
                uint color = Logic.Colors.ClassicColors[i];
                
                //Inicial
                uint initial = pipes[i][0];
                int row = (int)(initial / boardHeight);
                int col = (int)(initial % boardWidth);
                Tile initTile = tiles[row, col];
                initTile.setColor(color);
                initTile.setCircleBig();

                //Final
                uint final = pipes[i][pipeLength-1];
                Tile endTile = tiles[final / boardHeight, final % boardWidth];
                endTile.setColor(color);
                endTile.setCircleBig();
            }
        }
    }
}
