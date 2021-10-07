package es.ucm.gdv.pcohno;

import com.sun.tools.javac.util.Pair;

static Pair<Integer, Integer>[] _dirs = { (-1,0),(1,0),(0,-1),(0,1) };
static enum Dirs {Up, Down, Left, Right}

public class Board {

    public Board(int size){
        _board = new Cell[size][size];
        _size = size;
        this._hint = new Hint(_board);
        for (int i = 0; i < size; ++i){
            for (int j = 0; j < size; ++j){
                _board[i][j] = new Cell(i, j, size);
            }
        }
    }

    /**
     * @return pair of the first wrong cell. If all are correct, returns (-1, -1)
     */
    public Pair<Integer, Integer> wrongCell(){
        for (int i = 0; i < _size; ++i){
            for (int j = 0; j < _size; ++j){
                if(!_board[i][j].isRight(_board))
                    return new Pair<Integer, Integer>(i, j);
            }
        }
        return null;
    }

    public void getHint() {
        for (int i = 0; i < _size; ++i){
            for (int j = 0; j < _size; ++j){
                Cell.State s = _hint.getCellState(i, j);
                if(s == Cell.State.Null)
                    continue;
                _board[i][j].setState
            }
        }
    }

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
        Pair<Integer,Integer> dir = _dirs[direction];

        int row = _pos.fst;
        int col = _pos.snd;
        int seeing = 0;
        while(row >= 0 && row < _boardSize && col >= 0 && col < _boardSize){
            if(board[row][col].isPoint()) ++seeing;
                row += dir.fst; col += dir.snd;
            else break;
        }
        return seeing;
    }

    public int getSize(){ return _size;}

    private Cell[][] _board;
    private int _size;
    private Hint _hint;
}