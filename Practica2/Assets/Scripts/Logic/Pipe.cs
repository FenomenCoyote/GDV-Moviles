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

        public bool cutMyself(Vector2 pos)
        {
            if (!checkIfICutMyself(pos))
                return false;

            int it = positionsFromInitial.IndexOf(pos);

            if(it >= 0)
            {
                int i = it + 1;
                Tile removeTile;
                while (i < positionsFromInitial.Count && positionsFromInitial[i] != pos)
                {
                    removeTile = tilesFromInitial[i];
                    removeTile.disableAll();
                    removeTile.setActiveTile(false);

                    positionsFromInitial.RemoveAt(i);
                    tilesFromInitial.RemoveAt(i);
                }

                removeTile = tilesFromInitial[it];
                removeTile.disableDestDirectionSprite();

                return true;
            }
            else { 
                it = positionsFromFinal.IndexOf(pos);
                if (it < 0)
                    return false;

                int i = it + 1;
                Tile removeTile;
                while (i < positionsFromFinal.Count && positionsFromFinal[i] != pos)
                {
                    removeTile = tilesFromFinal[i];
                    removeTile.disableAll();
                    removeTile.setActiveTile(false);

                    positionsFromFinal.RemoveAt(i);
                    tilesFromFinal.RemoveAt(i);
                }

                removeTile = tilesFromFinal[it];
                removeTile.disableSourceDirectionSprite();

                return true;
            }
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
                    positions[positions.Count - 1], pos, dir);

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
                if(tryAdd(ref positionsFromInitial, ref tilesFromInitial, PipeDir.Initial, pos, t) || tryAdd(ref positionsFromFinal, ref tilesFromFinal, PipeDir.Final, pos, t))
                {
                    added = true;
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

            if(added && checkPipeClosed())
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
        }

        private bool checkPipeClosed()
        {
            return positionsFromInitial[positionsFromInitial.Count - 1] == positionsFromFinal[positionsFromFinal.Count - 1];
        }

        private void join(Tile t1, Tile t2, Vector2 p1, Vector2 p2, PipeDir fromInital)
        {
            Vector2 delta = p2 - p1;
            delta = Vector2.Perpendicular(delta) * -1;
            Dir dir = Direction.GetDirectionFromVector(delta);

            if (fromInital == PipeDir.Initial)
            {
                t1.enableDestDirectionSprite(dir);
                t2.enableSourceDirectionSprite(Direction.Opposite(dir));
            }
            else
            {
                t1.enableSourceDirectionSprite(dir);
                t2.enableDestDirectionSprite(Direction.Opposite(dir));
            }
        }

        private bool canGoTo(Vector2 pos1, Vector2 pos2)
        {
            float dist = Vector2.Distance(pos1, pos2);
            return dist > 0.1f && dist < 1.1f;
        }
    }
}