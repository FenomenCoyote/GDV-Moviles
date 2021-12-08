using System.Collections.Generic;
using System;

namespace flow.Logic
{
    public class Map
    {
        public Map()
        {
            levelWidth = 0;
            levelHeight = 0;
            nLevel = 0;
            nPipes = 0;

            bridges = new List<uint>();
            emptyTiles = new List<Tuple<uint, uint>>();
            walls = new List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>>();
            pipes = new List<List<Tuple<uint, uint>>>();
        }

        public void loadLevel(string levelInfo)
        {
            string[] splitedInfo = levelInfo.Split(';');

            //Cabecera
            string[] header = splitedInfo[0].Split(',');

            //tamaño del nivel
            if (header[0].Contains(":"))
            {
                string[] size = header[0].Split(':');
                levelWidth = uint.Parse(size[0]);
                levelHeight = uint.Parse(size[1]);
            }
            else levelWidth = levelHeight = uint.Parse(header[0]);

            //Numero de nivel
            nLevel = uint.Parse(header[2]);

            //Numero tuberias
            nPipes = uint.Parse(header[3]);

            //TODO::Leer campos opcionales

            if(header.Length >= 5) //Puentes
            {
                //Nada
            }
            if (header.Length >= 6) //Huecos
            {
                string[] e = header[5].Split(':');

                if(e[0] != "")
                {
                    for (int i = 0; i < e.Length; ++i)
                    {
                        emptyTiles.Add(getTupleFromNumber(e[i]));
                    }
                }
            }
            if (header.Length >= 7) //Muros
            {
                string[] w = header[6].Split(':');

                if (w[0] != "")
                {
                    for (int i = 0; i < w.Length; ++i)
                    {
                        string[] tiles = w[i].Split('|');

                        walls.Add(new Tuple<Tuple<uint, uint>, Tuple<uint, uint>>(getTupleFromNumber(tiles[0]), getTupleFromNumber(tiles[1])));
                    }
                }
            }

            //Soluciones tuberias
            pipes = new List<List<Tuple<uint, uint>>>(splitedInfo.Length - 1);
            for (int i = 1; i < splitedInfo.Length; ++i)
            {
                string[] pipe = splitedInfo[i].Split(',');
                pipes.Add(new List<Tuple<uint, uint>>(pipe.Length));
                for (int j = 0; j < pipe.Length; ++j)
                {
                    Tuple<uint,uint> tile = getTupleFromNumber(pipe[j]);
                    pipes[i-1].Add(tile);
                }
            }
        }

        private Tuple<uint, uint> getTupleFromNumber(string number)
        {
            return new Tuple<uint, uint>(uint.Parse(number) / levelHeight, uint.Parse(number) % levelHeight);
        }

        public uint getLevelWidth() { return levelWidth; }

        public uint getLevelHeight() { return levelHeight; }

        public uint getNLevel() { return nLevel; }

        public uint getNPipes() { return nPipes; }

        public List<uint> getBridges() { return bridges; }

        public List<Tuple<uint, uint>> getEmptyTiles() { return emptyTiles; }

        public List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>> getWalls() { return walls; }

        public List<List<Tuple<uint, uint>>> getPipes() { return pipes; }

        private uint levelWidth;
        private uint levelHeight;
        private uint nLevel;
        private uint nPipes;

        List<uint> bridges;
        List<Tuple<uint, uint>> emptyTiles;
        List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>> walls;
        List<List<Tuple<uint, uint>>> pipes;
    }
}