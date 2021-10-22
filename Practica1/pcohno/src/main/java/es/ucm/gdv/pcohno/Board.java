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
        _board = new Cell[size + 2][size + 2];

        _size = size;
        this._hint = new Hint(_board);
        for (int i = 0; i < size + 2; ++i){
            for (int j = 0; j < size + 2; ++j){
                _board[i][j] = new Cell();
                if(i == 0 || j == 0 || i == size + 1 || j == size + 1){
                    _board[i][j].setState(Cell.State.Wall);
                    _board[i][j].setLocked(true);
                }
            }
        }

    }

    public void setForGame() {

        _board[2][1].setState(Cell.State.Wall);
        _board[2][1].setLocked(true);

        _board[1][2].setState(Cell.State.Wall);
        _board[1][2].setLocked(true);

        _board[2][3].setState(Cell.State.Point);
        _board[2][3].setMustWatch(2);
        _board[2][3].setLocked(true);

        _board[3][2].setState(Cell.State.Point);
        _board[3][2].setMustWatch(1);
        _board[3][2].setLocked(true);

        _board[4][3].setState(Cell.State.Point);
        _board[4][3].setMustWatch(2);
        _board[4][3].setLocked(true);

        _board[4][4].setState(Cell.State.Point);
        _board[4][4].setMustWatch(4);
        _board[4][4].setLocked(true);
    }

    public void print() {
        System.out.println("=============================");
        for (int i = 1; i < _size + 1; ++i) {
            for (int j = 1; j < _size + 1; ++j) {
                switch (_board[i][j].getState()){
                    case Null:
                        break;
                    case Unassigned:
                        System.out.print("?");
                        break;
                    case Point:
                        if(_board[i][j].getLocked())
                            System.out.print(_board[i][j].getMustWatch());
                        else
                            System.out.print("O");
                        break;
                    case Wall:
                        System.out.print("X");
                        break;
                }
                System.out.print('\t');
            }
            System.out.println("\n");
        }
    }

    /**
     * @return pair of the first wrong cell. If all are correct, returns (-1, -1)
     */
    public Pair<Integer, Integer> wrongCell(){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
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
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                CellHint h = null;
                try {
                    h = _hint.getPositiveHint(i, j);
                } catch (Exception e){
                    e.printStackTrace();
                }
                if(h == null)
                    continue;
                _board[h.pos.fst][h.pos.snd].setState(h.state);
                return;
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
        row = row + dir.fst;
        col = col + dir.snd;

        while(_board[row][col].getState() == Cell.State.Point){
            ++seeing;
            row += dir.fst;
            col += dir.snd;
        }
        return seeing;
    }

    private Cell[][] _board;
    private int _size;
    private Hint _hint;
}