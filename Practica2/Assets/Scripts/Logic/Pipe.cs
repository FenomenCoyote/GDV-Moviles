using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow.Logic
{
    public class Pipe 
    {
        private Vector2 startPos, finalPos;
        private Tile startTile, finalTile;

        private List<Vector2> positions;
        private List<Tile> tiles;


        private bool justCutted;
        private Vector2 cuttedPos;

        private bool closed;

        private int provisionalIndex;

        private Color color;


        public Pipe()
        {
            positions = new List<Vector2>();
            tiles = new List<Tile>();

            provisionalIndex = 1000;
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

        private int isCuttingThisPipe(Pipe other)
        {
            int i = 0;
            foreach(Vector2 pos in positions)
            {
                if (other.positions.Contains(pos))
                    return i;

                ++i;
            }

            return -1;
        }

        public bool provisionalCut(Pipe current, Vector2 pos)
        {
            int index = isCuttingThisPipe(current);
            int aux = positions.IndexOf(pos);

            if (index < 0)
                index = aux;

            if (index < 0)
                return false;

            index = Mathf.Min(index, aux);

            if (closed && provisionalIndex == 1000)
                provisionalIndex = provisionalClosedCut(index);
            else 
                provisionalIndex = index;

            tiles[provisionalIndex - 1].disableDestDirectionSprite();

            for (int i = provisionalIndex; i < tiles.Count; ++i)
            {
                if (tiles[i].getColor() != color)
                    continue;

                tiles[i].disableAll();

                tiles[i].disableCircle();

                tiles[i].enableHightLight();
                tiles[i].setHightLock(true);
            }

            for(int i = 0; i < provisionalIndex - 1; ++i)
            {
                join(tiles[i], tiles[i + 1], positions[i], positions[i + 1]);
            }

            return true;

        }

        private int provisionalClosedCut(int index)
        {
            if (index < tiles.Count / 2)
            {
                //reverse order
                positions.Reverse();
                tiles.Reverse();

                index = tiles.Count - index - 1;
                foreach (Tile t in tiles)
                    t.reverse();
            }

            return index;
        }

        public void restoreFromProvisionalCut(Pipe current, bool fromCut = false)
        {
            if (provisionalIndex == 1000)
                return;

            int cut = isCuttingThisPipe(current);

            if(cut > -1)
            {
                if (fromCut)
                {
                    int posCuttedAtCurrent = current.positions.IndexOf(current.cuttedPos);
                    int posProvisionalCutted = current.positions.IndexOf(positions[cut]);

                    if (posCuttedAtCurrent >= posProvisionalCutted)
                        return;
                }

                for (int i = cut ; i < tiles.Count; ++i)
                {
                    tiles[i].setHightLock(false);

                    if (tiles[i].getColor() != color)
                    {
                        continue;
                    }
                    
                    tiles[i].disableHighLight();
                    tiles[i].disableCircle();
                }
                tiles.RemoveRange(cut, tiles.Count - cut);
                positions.RemoveRange(cut, positions.Count - cut);

                closed = false;
            }

            tiles[0].disableAll();
            for (int i = 0; i < tiles.Count - 1; ++i)
            {
                tiles[i + 1].disableAll();
                join(tiles[i], tiles[i + 1], positions[i], positions[i + 1]);
                tiles[i].setColor(color);
                tiles[i].setHightLock(false);
                tiles[i].enableHightLight();
                tiles[i].setActiveTile(true);
            }

            if (tiles.Count > 1)
            {
                tiles[tiles.Count - 1].setActiveTile(true);
                tiles[tiles.Count - 1].enableCircle();
                tiles[tiles.Count - 1].setHightLock(false);
                tiles[tiles.Count - 1].setColor(color);
                tiles[tiles.Count - 1].enableHightLight();
            }

            provisionalIndex = 1000;
        }

        public bool cutMyself(Vector2 pos)
        {  
            if (pos == startPos || pos == finalPos)
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
                justCutted = true;
                cuttedPos = pos;
                return true;
            }
            else
            {
                int index = positions.IndexOf(pos);
                if (index <= 0 || index == positions.Count - 1)
                    return false;

                justCutted = true;
                cuttedPos = pos;

                if (closed)
                {
                    openPipe(index);
                    return true;
                }

                tiles[index].disableDestDirectionSprite();
                ++index;
                while(index < positions.Count)
                {
                    tiles[index].disableAll();
                    tiles[index].disableCircle();
                    tiles[index].setActiveTile(false);
                    tiles.RemoveAt(index);
                    positions.RemoveAt(index);
                }

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
            provisionalIndex = 1000;
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
            t2.disableDestDirectionSprite();

            t1.setColor(color);
            t2.setColor(color);
        }

        private bool canGoTo(Vector2 pos1, Vector2 pos2)
        {
            float dist = Vector2.Distance(pos1, pos2);
            return dist > 0.1f && dist < 1.1f;
        }
    }
}