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
        if(c.getLocked() && c.getState() == Cell.State.Point)
            return c.getMustWatch() == lookDirections(row, col);
        else if(c.getState() == Cell.State.Point)
            return lookDirections(row, col) > 0;
        else
            return c.getState() == Cell.State.Wall;
    }

    //Devolver nueva celda puesta o una cela mal muesta
    public void getHint() {
        for (int i = 0; i < _size; ++i){
            for (int j = 0; j < _size; ++j){
                CellHint h = _hint.getPositiveHint(i, j);
                if(h == null)
                    continue;
                _board[h.pos.fst][h.pos.snd].setState(h.state);
            }
        }
    }


    private int lookDirections(int row, int col){
        int seeing = 0;

        //Look up
        seeing += lookDirection(Dirs.Up, row, col);
        //Look down
        seeing+= lookDirection(Dirs.Down, row, col);
        //Look left
        seeing += lookDirection(Dirs.Left, row, col);
        //Look right
        seeing += lookDirection(Dirs.Right, row, col);

        return seeing;
    }

    private int lookDirection(Dirs direction, int row, int col){
        Pair<Integer,Integer> dir = _dirs[direction.ordinal()];

        int seeing = 0;
        while(row >= 0 && row < _size && col >= 0 && col < _size){
            if(_board[row][col].getState() == Cell.State.Point) {
                ++seeing;
                row += dir.fst;
                col += dir.snd;
            }
            else
                break;

        }
        return seeing;
    }

    public int getSize(){ return _size;}

    private Cell[][] _board;
    private int _size;
    private Hint _hint;
}