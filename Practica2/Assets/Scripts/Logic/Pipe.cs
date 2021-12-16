using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow.Logic
{
    public class Pipe 
    {
        private Vector2 startPos, finalPos;

        public List<Vector2> positions { get; private set; }

        public int provisionalIndex { get; private set; }

        private bool closed;

        private bool changedMySolution;

        private List<Vector2> solutionPositions;


        public Pipe()
        {
            positions = new List<Vector2>();

            solutionPositions = new List<Vector2>();

            provisionalIndex = 1000;
            closed = false;
            changedMySolution = true;
        }

        public void reset()
        {
            closed = false;
            changedMySolution = false;
            provisionalIndex = 1000;

            solutionPositions.Clear();
            positions.Clear();
        }

        public void setInitialAndEndTiles(Vector2 ipos, Vector2 epos)
        {
            positions.Clear();
            solutionPositions.Clear();
            startPos = ipos;
            finalPos = epos;
        }


        public void startDrag(Vector2 pos)
        {
            if (pos == startPos || pos == finalPos)
            {
                positions.Clear();

                closed = false;

                positions.Add(pos);
            }
            else
            {
                if(cutMyself(pos))
                    closed = false;
            }
        }

        public Vector2 getOrigin()
        {
            if(checkPipeClosed())
                positions.RemoveAt(positions.Count - 1);

            return positions[positions.Count - 1];
        }

        public Vector2 getStartPos()
        {
            return startPos;
        }

        public Vector2 getFinalPos()
        {
            return finalPos;
        }

        public void notDraggingAnymore()
        {
            if (checkPipeClosed())
            {
                bool changed = false;
                foreach (Vector2 p in positions)
                {
                    if (!solutionPositions.Contains(p))
                    {
                        changed = true;
                        break;
                    }
                }

                changedMySolution = changed;

                if (changedMySolution)
                {
                    solutionPositions.Clear();
                    foreach (Vector2 p in positions)
                        solutionPositions.Add(p);
                } 

                closed = true;
            }
        }

        public bool provisionalCut(Pipe current)
        {
            int index = isCuttingThisPipe(current);

            if (index < 0)
            {
                restore();
                return false;
            }

            bool a = provisionalIndex == 1000;

            if (index != provisionalIndex)
                provisionalIndex = index;

            return a;
        }

        public Vector2 getLastPosProvisional()
        {
            if (provisionalIndex == 1000)
                return positions[0];
            return positions[provisionalIndex - 1];
        }


        public void finallyCut()
        {
            if (provisionalIndex == 1000)
                return;

            positions.RemoveRange(provisionalIndex, positions.Count - provisionalIndex);
            provisionalIndex = 1000;
            closed = checkPipeClosed();

            solutionPositions.Clear();
            changedMySolution = true;
        }


        private int isCuttingThisPipe(Pipe other)
        {
            int i = 0;
            foreach (Vector2 pos in positions)
            {
                if (other.positions.Contains(pos))
                    return i;

                ++i;
            }

            return -1;
        }

        private int isCuttingThisPipe(List<Vector2> path)
        {
            int i = 0;
            foreach (Vector2 pos in positions)
            {
                if (path.Contains(pos))
                    return i;

                ++i;
            }

            return -1;
        }

        public void restore()
        {
            provisionalIndex = 1000;
        }


        public bool cutMyself(Vector2 pos)
        {  
            int index = positions.IndexOf(pos);

            if(index == 0 && positions.Count > 1)
            {
                positions.Clear();
                positions.Add(pos);
                return true;
            }

            if (index < 0 || index == positions.Count - 1)
                return false;

            if (closed)
            {
                openPipe(index);
                return true;
            }
               
            ++index;
            while(index < positions.Count)
            {
                positions.RemoveAt(index);
            }

            return true;
        }

        public bool addPathFromOrigin(List<Vector2> path)
        {
            int index = isCuttingThisPipe(path);

            if(index < 0)
            {
                positions.AddRange(path);

                return true;
            }
            else
            {
                cutMyself(positions[index]);
                return false;
            }
        }

        public bool isClosed()
        {
            return closed;
        }

        public bool changedSolution()
        {
            return changedMySolution;
        }

        public void setSolution(in List<Vector2> solution)
        {
            positions.Clear();
            solutionPositions.Clear();

            foreach(Vector2 p in solution)
            {
                positions.Add(p);
                solutionPositions.Add(p);
            }

            closed = true;
        }

        private void openPipe(int index)
        {
            if(index < positions.Count / 2)
            {
                while (index + 1 < positions.Count)
                {
                    positions.RemoveAt(index + 1);
                }
            } 
            else
            {
                //reverse order
                positions.Reverse();

                while (index + 1 < positions.Count)
                {
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
    }
}
