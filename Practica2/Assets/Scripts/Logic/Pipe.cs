using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow.Logic
{
    /// <summary>
    /// Manages a single pipe (or flow) positions
    /// </summary>
    public class Pipe 
    {
        private Vector2 startPos, finalPos;

        //List of positions 
        public List<Vector2> positions { get; private set; }

        //is 1000 when is not being provisionally cutted
        //otherwise, it points the provisionall cut position
        public int provisionalIndex { get; private set; }

        private bool closed;

        private bool changedMySolution;

        private List<Vector2> solutionPositions;

        /// <summary>
        /// Constructor
        /// </summary>
        public Pipe()
        {
            positions = new List<Vector2>();

            solutionPositions = new List<Vector2>();

            provisionalIndex = 1000;
            closed = false;
            changedMySolution = true;
        }

        /// <summary>
        /// Reset pipe's info
        /// </summary>
        public void reset()
        {
            closed = false;
            changedMySolution = false;
            provisionalIndex = 1000;

            solutionPositions.Clear();
            positions.Clear();
        }

        /// <summary>
        /// Sets start and final pos
        /// </summary>
        /// <param name="ipos"></param>
        /// <param name="epos"></param>
        public void setInitialAndEndTiles(Vector2 ipos, Vector2 epos)
        {
            positions.Clear();
            solutionPositions.Clear();
            startPos = ipos;
            finalPos = epos;
        }

        /// <summary>
        /// Starts a drag and cuts if it's needed
        /// </summary>
        /// <param name="pos"></param>
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

        /// <summary>
        /// Gets the tip of the pipe
        /// </summary>
        /// <returns>Position of tip of the pipe</returns>
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

        /// <summary>
        /// If this pipe reached both ends, it means is now closed
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <returns>False if it was not cutted anymore and restores, true if it's it could be cutted</returns>
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

        /// <summary>
        /// Returns theorical last pos if it was finally cutted
        /// </summary>
        /// <returns></returns>
        public Vector2 getLastPosProvisional()
        {
            if (provisionalIndex == 1000)
                return positions[0];
            return positions[provisionalIndex - 1];
        }

        /// <summary>
        /// Finally cuts the provisional cut
        /// </summary>
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

        /// <summary>
        /// Index of the cut the other pipes makes to this one
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Index of the cut the path makes to the pipe
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        /// <summary>
        /// No more provisional cut
        /// </summary>
        public void restore()
        {
            provisionalIndex = 1000;
        }

        /// <summary>
        /// Cuts myself 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>True if it was cutted, False otherwise</returns>
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

        /// <summary>
        /// Continues this pipe 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool addPathFromOrigin(List<Vector2> path)
        {
            int index = isCuttingThisPipe(path);

            if(index < 0)
            {
                foreach(Vector2 p in path)
                {
                    positions.Add(p);
                    if (p == startPos || p == finalPos)
                        break;
                }

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

        /// <summary>
        /// Sets the positions to match a solution
        /// </summary>
        /// <param name="solution"></param>
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

        /// <summary>
        /// Opens pipe at index. 
        /// Cuts itself and keeps the longest path
        /// </summary>
        /// <param name="index"></param>
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
