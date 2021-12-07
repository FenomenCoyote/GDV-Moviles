using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow.Logic
{
    public class Pipe 
    {
        private enum PipeDir
        {
            None, Initial, Final
        }
        private List<Vector2> positions;
        private List<Tile> tiles;

        private Vector2 startPos, finalPos;
        private Tile startTile, finalTile;

        private int provisionalIndex;
        private bool provisionalCutting, justCutted;

        private bool closed;



        private Color color;

        public Pipe()
        {
            positions = new List<Vector2>();
            tiles = new List<Tile>();

            provisionalIndex = -1;
            provisionalCutting = false;

            closed = false;
        }

        public void setInitialAndEndTiles(Vector2 ipos, Tile itile, Vector2 epos, Tile etile)
        {
            positions.Clear();
            tiles.Clear();

            startPos = ipos;
            startTile = itile;
            finalPos = epos;
            finalTile = etile;
        }

        public void setColor(Color c)
        {
            color = c;
        }

        public void startDrag(Vector2 pos)
        {
            foreach(Tile t in tiles)
            {
                t.disableHighLight();
            }

            if(tiles.Count > 0)
            {
                if(!tiles[tiles.Count - 1].isInitialOrEnd()) 
                    tiles[tiles.Count - 1].disableCircle();
            }
        }

        public void notDraggingAnymore()
        {
            foreach (Tile t in tiles)
            {
                t.enableHightLight();
            }
            if (tiles.Count > 0)
            {
                if (!tiles[tiles.Count - 1].isInitialOrEnd())
                    tiles[tiles.Count - 1].enableCircle();
            }
        }

        public bool pollCutted()
        {
            bool b = justCutted;
            justCutted = false;
            return b;
        }

        public bool isCuttingThisPipe(Pipe other)
        {
            foreach(Vector2 pos in positions)
            {
                if (other.positions.Contains(pos))
                    return true;
            }

            return false;
        }

        public bool provisionalCut(Vector2 pos)
        {
            int index = positions.IndexOf(pos);
            if (index < 0)
                return false;

            provisionalCutting = true;

            if (closed)
            {
                provisionalClosedCut(index);
                return true;
            }
            tiles[index - 1].disableDestDirectionSprite();
            int it = index;
            while (it < tiles.Count)
            {
                tiles[it].disableAll();
                tiles[it].disableCircle();
                tiles[it].enableHightLight();
                ++it;
                //tiles.RemoveAt(index);
                //positions.RemoveAt(index);
            }
            tiles[0].setActiveTile(true);

            provisionalIndex = Mathf.Min(index, provisionalIndex);

            return true;

        }

        private void provisionalClosedCut(int index)
        {  
            if (index < positions.Count / 2)
            {
                //reverse order
                positions.Reverse();
                tiles.Reverse();

                index = positions.Count - index - 1;
                foreach (Tile t in tiles)
                    t.reverse();

                tiles[index - 1].disableDestDirectionSprite();

                int it = index;

                while (it < positions.Count)
                {
                    tiles[it].disableAll();
                    tiles[it].enableHightLight();
                    tiles[it].setHightLock(true);
                    it++;
                }

                tiles[0].setActiveTile(true);

                if (provisionalIndex < 0)
                    provisionalIndex = index;
                else
                    provisionalIndex = Mathf.Min(index, provisionalIndex);
            }
            else
            {
                
                tiles[index - 1].disableDestDirectionSprite();

                int it = index;

                while (it < positions.Count)
                {
                    tiles[it].disableAll();
                    tiles[it].enableHightLight();
                    tiles[it].setHightLock(true);

                    it++;
                }
                tiles[0].setActiveTile(true);

                if (provisionalIndex < 0)
                    provisionalIndex = index;
                else
                    provisionalIndex = Mathf.Min(index, provisionalIndex);
            }
        }

        public void restoreFromProvisionalCut()
        {
            if (!provisionalCutting)
                return;

            provisionalCutting = false;

            bool restore = true;
            int joinNext = 1000;
            for (int i = 0; i < tiles.Count; ++i) { 

                Tile t = tiles[i];
                if(t.getColor() != color)
                {
                    if (!t.hasNoDir())
                    {
                        restore = false;
                        closed = false;
                        tiles[i - 1].disableDestDirectionSprite();
                        tiles[i - 1].enableCircle();
                    }
                    else
                    {
                        join(tiles[i - 1], tiles[i], positions[i - 1], positions[i]);
                        joinNext = 1;
                    }
                }

                if (restore)
                {
                    if (joinNext == 0)
                    {
                        join(tiles[i - 1], tiles[i], positions[i - 1], positions[i]);
                        joinNext = 1000;
                    }
                    else
                    {
                        --joinNext;
                    }
                    t.setHightLock(false);
                    t.setColor(color);
                    t.setActiveTile(true);
                    t.enableHightLight();
                    t.enableDestDirectionSprite();
                    t.enableSourceDirectionSprite();
                }
                else 
                {
                    if (t.getColor() != color)
                    {
                        t.setHightLock(false);
                    }
                    else
                    {
                        t.disableAll();
                        t.setActiveTile(false);
                    }
                    tiles.RemoveAt(i);
                    positions.RemoveAt(i);
                    --i;
                }
            }


        }

        public bool cutMyself(Vector2 pos)
        {
            justCutted = true;
            if(pos == startPos || pos == finalPos)
            {
                foreach (Tile t in tiles)
                {
                    t.disableAll();
                    t.setActiveTile(false);
                }

                positions.Clear();
                tiles.Clear();

                startTile.disableSourceDirectionSprite();
                startTile.disableDestDirectionSprite();
                startTile.setActiveTile(true);
                finalTile.disableSourceDirectionSprite();
                finalTile.disableDestDirectionSprite();
                finalTile.setActiveTile(true);

                closed = false;
                return true;
            }
            else
            {
                int index = positions.IndexOf(pos) + 1;
                if (index <= 0)
                    return false;

                if (closed)
                {
                    openPipe(index - 1);
                    return true;
                }

                tiles[index - 1].disableDestDirectionSprite();
                while(index < positions.Count)
                {
                    tiles[index].disableAll();
                    tiles[index].setActiveTile(false);
                    tiles.RemoveAt(index);
                    positions.RemoveAt(index);
                }

                tiles[0].setActiveTile(true);

                return true;
            }
        }

        private bool checkIfICutMyself(Vector2 pos)
        {
            return positions.Contains(pos);
        }

        private bool tryAdd(Vector2 pos, Tile t)
        {
            if(positions.Count == 0)
            {
                if (canGoTo(startPos, pos))
                {
                    //join last one from initial to this new one
                    join(startTile, t, startPos, pos);

                    positions.Add(startPos);
                    tiles.Add(startTile);

                    positions.Add(pos);
                    tiles.Add(t);

                    t.setColor(color);
                    t.setActiveTile(true);

                    return true;
                }
                else if (canGoTo(finalPos, pos))
                {
                    //join last one from initial to this new one
                    join(finalTile, t, finalPos, pos);

                    positions.Add(finalPos);
                    tiles.Add(finalTile);

                    positions.Add(pos);
                    tiles.Add(t);

                    t.setColor(color);
                    t.setActiveTile(true);

                    return true;
                }
                else return false;
            }

            else if (canGoTo(positions[positions.Count - 1], pos))
            {
                //join last one from initial to this new one
                join(tiles[tiles.Count - 1], t,
                    positions[positions.Count - 1], pos);

                positions.Add(pos);
                tiles.Add(t);

                t.setColor(color);
                t.setActiveTile(true);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds tile to this pipe
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="t"></param>
        /// <returns>true if it could be added, false otherwise</returns>
        public bool add(Vector2 pos, Tile t)
        {
            if (t.isInitialOrEnd())
            {
                if (t.getColor() != color)
                    return false;   
            }

            if (checkIfICutMyself(pos))
            {
                if (cutMyself(pos))
                    return false;
            }

            bool added = tryAdd(pos, t);

            if (added && checkPipeClosed())
            {
                close();
            }

            return added;
        }

        public bool isClosed()
        {
            return closed;
        }

        private void close()
        {
            closed = true;
        }


        private void openPipe(int index)
        {
            if(index < positions.Count / 2)
            {
                tiles[index].disableDestDirectionSprite();
                while (index + 1 < positions.Count)
                {
                    tiles[index + 1].disableAll();
                    tiles[index + 1].setActiveTile(false);
                    tiles.RemoveAt(index + 1);
                    positions.RemoveAt(index + 1);
                }
            } 
            else
            {
                //reverse order
                positions.Reverse();
                tiles.Reverse();

                index = positions.Count - index - 1;
                foreach (Tile t in tiles)
                    t.reverse();

                tiles[index].disableDestDirectionSprite();
                while (index + 1 < positions.Count)
                {
                    tiles[index + 1].disableAll();
                    tiles[index + 1].setActiveTile(false);
                    tiles.RemoveAt(index + 1);
                    positions.RemoveAt(index + 1);
                }

            }

            closed = false;
        }

        private bool checkPipeClosed()
        {
            return positions[0] == startPos && positions[positions.Count - 1] == finalPos ||
                positions[positions.Count - 1] == startPos && positions[0] == finalPos;
        }


        private void join(Tile t1, Tile t2, Vector2 p1, Vector2 p2)
        {
            Vector2 delta = p2 - p1;
            delta = Vector2.Perpendicular(delta) * -1;
            Dir dir = Direction.GetDirectionFromVector(delta);

            t1.enableDestDirectionSprite(dir);
            t2.enableSourceDirectionSprite(Direction.Opposite(dir));         
        }

        private bool canGoTo(Vector2 pos1, Vector2 pos2)
        {
            float dist = Vector2.Distance(pos1, pos2);
            return dist > 0.1f && dist < 1.1f;
        }
    }
}