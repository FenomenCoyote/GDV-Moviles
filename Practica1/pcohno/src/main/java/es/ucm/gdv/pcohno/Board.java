package es.ucm.gdv.pcohno;

import com.sun.tools.javac.util.Pair;

public class Board {

    static Pair<Integer, Integer>[] _dirs = new Pair[]{
            new Pair(-1, 0),
            new Pair(1, 0),
            new Pair(0, -1),
            new Pair(0, 1),
    };

    static enum Dirs {Up, Down, Left, Right}

    public Board(int size){
        _board = new Cell[size][size];

        _size = size;
        this._hint = new Hint(_board);
        for (int i = 0; i < size; ++i){
            for (int j = 0; j < size; ++j){
                _board[i][j] = new Cell();
            }
        }
    }

    /**
     * @return pair of the first wrong cell. If all are correct, returns (-1, -1)
     */
    public Pair<Integer, Integer> wrongCell(){
        for (int i = 0; i < _size; ++i){
            for (int j = 0; j < _size; ++j){
                if(!isCellRight(i, j))
                    return new Pair<Integer, Integer>(i, j);
            }
        }
        return null;
    }

    private boolean isCellRight(int row, int col){
        Cell c = _board[row][col];
        /*if(c.getLocked())
            return c.getMustWatch() == lookDirections(row, col);
        else //Necesita estar definida a wall o a point
            return c.getState() != Cell.State.Unassigned;*/
        return false;
    }

    public void getHint() {
        for (int i = 0; i < _size; ++i){
            for (int j = 0; j < _size; ++j){
                //Devolver posicion
                /* Cell.State s = _hint.getCellState(i, j);
                if(s == Cell.State.Null)
                    continue;
                _board[i][j].setState(whatever);*/
            }
        }
    }

/*
    private int lookDirections(Cell[][] board){
        int seeing = 0;

        //Look up
        seeing += lookDirection(Dirs.Up, board);
        //Look down
        seeing+= lookDirection(Dirs.Down, board);
        //Look left
        seeing += lookDirection(Dirs.Left, board);
        //Look right
        seeing += lookDirection(Dirs.Right, board);

        return seeing;
    }

    private int lookDirection(Dirs direction, Cell[][] board){
        Pair<Integer,Integer> dir = _dirs[direction.ordinal()];

        int row = _pos.fst;
        int col = _pos.snd;
        int seeing = 0;
        while(row >= 0 && row < _size && col >= 0 && col < _size){
            if(board[row][col].isPoint()) ++seeing;
                row += dir.fst; col += dir.snd;
            else break;
        }
        return seeing;
    }
 */

    public int getSize(){ return _size;}

    private Cell[][] _board;
    private int _size;
    private Hint _hint;
}