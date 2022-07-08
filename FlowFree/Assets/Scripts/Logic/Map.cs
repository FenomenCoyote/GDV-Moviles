using System.Collections.Generic;
using System;

namespace flow.Logic
{
    /// <summary>
    /// Class which contains a level info
    /// Gets all info from a level string
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Map()
        {
            levelWidth = 0;
            levelHeight = 0;
            nLevel = 0;
            nPipes = 0;

            emptyTiles = new List<Tuple<uint, uint>>();
            walls = new List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>>();
            pipes = new List<List<Tuple<uint, uint>>>();
        }

        /// <summary>
        /// Retrieves level information from string
        /// </summary>
        public void loadLevel(string levelInfo)
        {
            string[] splitedInfo = levelInfo.Split(';');

            //Splited header
            string[] header = splitedInfo[0].Split(',');

            //Level's size
            if (header[0].Contains(":"))//If level width and height are different
            {
                string[] size = header[0].Split(':');
                levelWidth = uint.Parse(size[0]);

                //For hourglass pack levels
                if (size[1].EndsWith("+B"))
                    size[1] = size[1].Remove(size[1].IndexOf('+'), 2);

                levelHeight = uint.Parse(size[1]);
            }
            //If level width and height are the same
            else levelWidth = levelHeight = uint.Parse(header[0]);

            //Level number
            nLevel = uint.Parse(header[2]);

            //Number of pipes in the level
            nPipes = uint.Parse(header[3]);

            if(header.Length >= 5) //If there are bridges
            {
                //Nothing to do here
            }

            //If level has empty tiles
            if (header.Length >= 6)
            {
                //Each empty tile is separated by ":"
                string[] e = header[5].Split(':');

                if(e[0] != "")
                {
                    for (int i = 0; i < e.Length; ++i)
                    {
                        //Translate nuber to tile position and add to list
                        emptyTiles.Add(getTupleFromNumber(e[i]));
                    }
                }
            }

            //If level has walls
            if (header.Length >= 7)
            {
                //Each wall is separated by ":"
                string[] w = header[6].Split(':');

                if (w[0] != "")
                {
                    for (int i = 0; i < w.Length; ++i)
                    {
                        //Walls separate two tiles separated by "|"
                        string[] tiles = w[i].Split('|');

                        //Translate numbers to tiles position an add to list
                        walls.Add(new Tuple<Tuple<uint, uint>, Tuple<uint, uint>>(getTupleFromNumber(tiles[0]), getTupleFromNumber(tiles[1])));
                    }
                }
            }

            //Pipes solutions
            pipes = new List<List<Tuple<uint, uint>>>(splitedInfo.Length - 1);
            //For every pipe
            for (int i = 1; i < splitedInfo.Length; ++i)
            {
                string[] pipe = splitedInfo[i].Split(',');
                pipes.Add(new List<Tuple<uint, uint>>(pipe.Length));
                //for every tile in the pipe
                for (int j = 0; j < pipe.Length; ++j)
                {
                    //Translate numbers to tiles position an add to list
                    Tuple<uint,uint> tile = getTupleFromNumber(pipe[j]);
                    pipes[i-1].Add(tile);
                }
            }
        }

        /// <summary>
        /// Tranlates a number of tile to row and col
        /// </summary>
        /// <param name="number">Number to translate</param>
        /// <returns>Tuple of row and col</returns>
        private Tuple<uint, uint> getTupleFromNumber(string number)
        {
            if (levelWidth == levelHeight) return new Tuple<uint, uint>(uint.Parse(number) / levelHeight, uint.Parse(number) % levelHeight);
            else
            {
                if (levelWidth < levelHeight) return new Tuple<uint, uint>(uint.Parse(number) / levelWidth, uint.Parse(number) % levelWidth);
                else return new Tuple<uint, uint>(uint.Parse(number) / levelHeight, uint.Parse(number) % levelHeight);
            }
        }

        /// <summary>
        /// Getter for number of columns in the level
        /// </summary>
        /// <returns>level width</returns>
        public uint getLevelWidth() { return levelWidth; }

        /// <summary>
        /// Getter for number of rows in the level
        /// </summary>
        /// <returns>level height</returns>
        public uint getLevelHeight() { return levelHeight; }

        /// <summary>
        /// Getter for level number
        /// </summary>
        /// <returns>level number in the pack</returns>
        public uint getNLevel() { return nLevel; }

        /// <summary>
        /// Getter for number of pipes in the level
        /// </summary>
        /// <returns>number of pipes in the level</returns>
        public uint getNPipes() { return nPipes; }

        /// <summary>
        /// Getter for list of empty tiles
        /// </summary>
        /// <returns>A list of tuples with every empty tile</returns>
        public List<Tuple<uint, uint>> getEmptyTiles() { return emptyTiles; }

        /// <summary>
        /// Getter for list of walls
        /// Every wall is represented by the two tiles being separated
        /// </summary>
        /// <returns>List of walls</returns>
        public List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>> getWalls() { return walls; }

        /// <summary>
        /// Getter for pipes in the level
        /// </summary>
        /// <returns>List of pipes</returns>
        public List<List<Tuple<uint, uint>>> getPipes() { return pipes; }

        private uint levelWidth;
        private uint levelHeight;
        private uint nLevel;
        private uint nPipes;

        List<Tuple<uint, uint>> emptyTiles;
        List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>> walls;
        List<List<Tuple<uint, uint>>> pipes;
    }
}