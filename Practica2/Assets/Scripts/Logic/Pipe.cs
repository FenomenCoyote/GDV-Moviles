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
        private List<Vector2> positionsFromInitial;
        private List<Tile> tilesFromInitial;

        private List<Vector2> positionsFromFinal;
        private List<Tile> tilesFromFinal;

        private PipeDir lastAddedFrom;

        private int positionsFromInitialPosIndex, positionsFromEndPosIndex;

        private bool closed;

        private Color color;

        public Pipe()
        {
            positionsFromInitial = new List<Vector2>();
            tilesFromInitial = new List<Tile>();

            positionsFromFinal = new List<Vector2>();
            tilesFromFinal = new List<Tile>();

            positionsFromInitialPosIndex = -1;
            positionsFromEndPosIndex = -1;

            lastAddedFrom = PipeDir.None;
            closed = false;
        }

        public void setInitialAndEndTiles(Vector2 ipos, Tile itile, Vector2 epos, Tile etile)
        {
            positionsFromInitial.Clear();
            tilesFromInitial.Clear();
            positionsFromInitial.Add(ipos);
            tilesFromInitial.Add(itile);


            positionsFromFinal.Clear();
            tilesFromFinal.Clear();
            positionsFromFinal.Add(epos);
            tilesFromFinal.Add(etile);
        }

        public void setColor(Color c)
        {
            color = c;
        }

        public void startDrag(Vector2 pos)
        {
            if (positionsFromInitial.Contains(pos))
                lastAddedFrom = PipeDir.Initial;

            if (positionsFromFinal.Contains(pos))
                lastAddedFrom = PipeDir.Final;
        }

        public bool cutMyself(Vector2 pos)
        {
            if (!checkIfICutMyself(pos))
                return false;

            if (closed) {
                openPipe(pos);
                return true;
            }

            return cutFromDirection(pos, ref positionsFromInitial, ref tilesFromInitial) || cutFromDirection(pos, ref positionsFromFinal, ref tilesFromFinal);
        }

        private bool cutFromDirection(Vector2 pos, ref List<Vector2> positions, ref List<Tile> tiles)
        {
            int it = positions.IndexOf(pos);

            if (it < 0 || positions.Count <= 1)
                return false;

            int i = it + 1;
            Tile removeTile;
            while (i < positions.Count && positions[i] != pos)
            {
                removeTile = tiles[i];
                removeTile.disableAll();
                removeTile.setActiveTile(false);

                positions.RemoveAt(i);
                tiles.RemoveAt(i);
            }

            removeTile = tiles[it];
            removeTile.disableDestDirectionSprite();

            return true;
        }

        private bool checkIfICutMyself(Vector2 pos)
        {
            return positionsFromInitial.Contains(pos) || positionsFromFinal.Contains(pos);
        }

        private bool tryAdd(ref List<Vector2> positions, ref List<Tile> tiles, PipeDir dir, Vector2 pos, Tile t)
        {
            if (canGoTo(positions[positions.Count - 1], pos))
            {

                //join last one from initial to this new one
                join(tiles[tiles.Count - 1], t,
                    positions[positions.Count - 1], pos);

                positions.Add(pos);
                tiles.Add(t);

                t.setColor(color);
                t.setActiveTile(true);

                lastAddedFrom = dir;

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
                cutMyself(pos);
            }


            bool added = false;

            //Si llego desde la pipe desde el principio
            if(lastAddedFrom == PipeDir.None)
            {
                if(tryAdd(ref positionsFromInitial, ref tilesFromInitial, PipeDir.Initial, pos, t))
                {
                    added = true;
                    lastAddedFrom = PipeDir.Initial;
                }
                else if(tryAdd(ref positionsFromFinal, ref tilesFromFinal, PipeDir.Final, pos, t))
                {
                    added = true;
                    lastAddedFrom = PipeDir.Final;
                }
            }
            else
            {
                if (lastAddedFrom == PipeDir.Initial && tryAdd(ref positionsFromInitial, ref tilesFromInitial, PipeDir.Initial, pos, t))
                {
                    added = true;
                }
                if(!added && tryAdd(ref positionsFromFinal, ref tilesFromFinal, PipeDir.Final, pos, t))
                {
                    added = true;
                }
            }

            if (added && checkPipeClosed())
            {
                close();
            }

            return added;
        }

        public void notDraggingAnymore()
        {
            lastAddedFrom = PipeDir.None;
        }

        public bool isClosed()
        {
            return closed;
        }

        private void close()
        {
            closed = true;

            while(positionsFromFinal.Count > 1)
            {
                if (positionsFromInitial[positionsFromInitial.Count - 1] == positionsFromFinal[positionsFromFinal.Count - 1])
                {
                    tilesFromFinal.RemoveAt(tilesFromFinal.Count - 1);
                    positionsFromFinal.RemoveAt(positionsFromFinal.Count - 1);
                    continue;
                }

                tilesFromInitial.Add(tilesFromFinal[tilesFromFinal.Count - 1]);
                positionsFromInitial.Add(positionsFromFinal[positionsFromFinal.Count - 1]);

                tilesFromFinal.RemoveAt(tilesFromFinal.Count - 1);
                positionsFromFinal.RemoveAt(positionsFromFinal.Count - 1);
            }

            for(int i = 0; i < positionsFromInitial.Count-1; ++i)
            {
                join(tilesFromInitial[i],
                    tilesFromInitial[i+1],
                    positionsFromInitial[i],
                    positionsFromInitial[i+1]);
            }

            if (positionsFromInitial[positionsFromInitial.Count - 1] == positionsFromFinal[positionsFromFinal.Count - 1])
            {
                tilesFromInitial.RemoveAt(tilesFromInitial.Count - 1);
                positionsFromInitial.RemoveAt(positionsFromInitial.Count - 1);
            }

            highLight(ref tilesFromInitial, true);
            highLight(ref tilesFromFinal, true);

            tilesFromInitial[0].enableCheck();
            tilesFromFinal[0].enableCheck();

            lastAddedFrom = PipeDir.None;
        }


        private void openPipe(Vector2 pos)
        {
            int it = positionsFromInitial.IndexOf(pos);

            if(it > -1 && it < positionsFromInitial.Count / 2)
            {
                cutFromDirection(pos, ref positionsFromInitial, ref tilesFromInitial);
                tilesFromFinal[0].disableDestDirectionSprite();
                tilesFromFinal[0].disableSourceDirectionSprite();
                lastAddedFrom = PipeDir.Initial;
            }
            else
            {
                while (positionsFromInitial.Count > 1)
                {
                    join(tilesFromFinal[tilesFromFinal.Count - 1],
                        tilesFromInitial[tilesFromInitial.Count - 1],
                        positionsFromFinal[positionsFromFinal.Count - 1],
                        positionsFromInitial[positionsFromInitial.Count - 1]);

                    tilesFromFinal.Add(tilesFromInitial[tilesFromInitial.Count - 1]);
                    positionsFromFinal.Add(positionsFromInitial[positionsFromInitial.Count - 1]);

                    tilesFromInitial.RemoveAt(tilesFromInitial.Count - 1);
                    positionsFromInitial.RemoveAt(positionsFromInitial.Count - 1);
                }

                cutFromDirection(pos, ref positionsFromFinal, ref tilesFromFinal);

                tilesFromInitial[0].disableSourceDirectionSprite();
                tilesFromInitial[0].disableDestDirectionSprite();
                lastAddedFrom = PipeDir.Final;
            }

            tilesFromInitial[0].disableCheck();
            tilesFromFinal[0].disableCheck();

            highLight(ref tilesFromInitial, false);
            highLight(ref tilesFromFinal, false);


            closed = false;
        }

        private void highLight(ref List<Tile> tiles, bool highLight)
        {
            foreach(Tile t in tiles)
            {
                if (highLight)
                    t.enableHightLight();
                else
                    t.disableHighLight();
            }
        }

        private bool checkPipeClosed()
        {
            return positionsFromInitial[positionsFromInitial.Count - 1] == positionsFromFinal[positionsFromFinal.Count - 1];
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