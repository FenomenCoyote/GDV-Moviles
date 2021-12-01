using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow.Logic
{
    public class Pipe 
    {
        private List<Vector2> currentActivePos;
        private List<Tile> currentActiveTiles;

        private int leftIndex, rightIndex;

        private bool closed;

        public Pipe()
        {
            currentActivePos = new List<Vector2>();
            currentActiveTiles = new List<Tile>();
        }

        public bool cut(Vector2 pos)
        {
            if (!currentActivePos.Contains(pos))
                return false;

            if (closed)
            {
                int it = 0;
                while (currentActivePos[it] != pos)
                {
                    ++it;
                }

                if (it < currentActivePos.Count / 2) //He cortado por el principio
                {
                    currentActiveTiles[it].disableDestDirectionSprite();

                    for (int i = 0; i < it; ++i)
                    {
                        Tile t = currentActiveTiles[i];
                        t.disableAll();
                        t.setActiveTile(false);
                    }
                    leftIndex = it;
                }
                else //He cortado por el final
                {
                    for (int i = it; i < currentActivePos.Count; ++i)
                    {
                        Tile t = currentActiveTiles[it];
                        t.disableAll();
                        t.setActiveTile(false);
                    }
                    rightIndex = it;
                }

                currentActiveTiles[it].disableAll();
            }

            

            return true;
        }

        public bool cutMyself(Vector2 pos)
        {
            if (!currentActivePos.Contains(pos))
                return false;

 
            while (currentActivePos[currentActivePos.Count - 1] != pos)
            {
                Tile t = currentActiveTiles[currentActiveTiles.Count - 1];
                t.disableAll();
                t.setActiveTile(false);
                currentActivePos.RemoveAt(currentActivePos.Count - 1);
                currentActiveTiles.RemoveAt(currentActiveTiles.Count - 1);
            }

            currentActiveTiles[currentActivePos.Count - 1].disableDestDirectionSprite();

            return true;
        }

        public void restore()
        {

        }

        public void add(Vector2 pos, Tile t)
        {
            currentActivePos.Add(pos);
            currentActiveTiles.Add(t);
        }

        public void close()
        {
            closed = true;
        }
    }
}