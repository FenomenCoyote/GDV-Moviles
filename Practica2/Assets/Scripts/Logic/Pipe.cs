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

        public void notDraggingAnymore()
        {
            if (checkPipeClosed())
                closed = true;
        }

        public bool provisionalCut(Pipe current)
        {
            int index = isCuttingThisPipe(current);

            if (index < 0)
            {
                restore();
                return false;
            }

            if (closed && provisionalIndex == 1000)
                provisionalIndex = provisionalClosedCut(index);
            else 
                provisionalIndex = index;

            return true;
        }


        public void finallyCut()
        {
            if (provisionalIndex == 1000)
                return;

            positions.RemoveRange(provisionalIndex, positions.Count - provisionalIndex);
            provisionalIndex = 1000;
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


        private int provisionalClosedCut(int index)
        {
            if (index < positions.Count / 2)
            {
                //reverse order
                positions.Reverse();

                index = positions.Count - index - 1;
            }

            return index;
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

        public void addPathFromOrigin(List<Vector2> path)
        {
            int index = isCuttingThisPipe(path);

            if(index < 0)
            {
                positions.AddRange(path);
                return;
            }
            else
            {
                cutMyself(positions[index]);
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
