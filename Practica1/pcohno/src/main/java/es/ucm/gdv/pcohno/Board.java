package es.ucm.gdv.pcohno;

import com.sun.tools.javac.util.Pair;

import java.util.Random;

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

       /* _board[2][1].setState(Cell.State.Wall);
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
        _board[4][4].setLocked(true);*/

        Cell[][] puzzle = new Cell[_size + 2][_size + 2];

        for (int i = 0; i < _size + 2; ++i){
            for (int j = 0; j < _size + 2; ++j){
                puzzle[i][j] = new Cell(_board[i][j]);
            }
        }

        //Hasta que haya un puzzle que tenga solucion
        while(true) {
            clean(puzzle);
            randomize(puzzle);
            if(!wrongInitialBoard(puzzle)) {
                //print(puzzle);
                copyToBoard(puzzle);
                if (resolve(puzzle)) { //Si se puede resolver
                    //print(puzzle);
                    return;
                }
            }
        }
    }

    public void setForGame2(){
        Cell[][] puzzle = new Cell[_size + 2][_size + 2];

        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size +1; ++j){
                puzzle[i][j] = new Cell(_board[i][j]);
            }
        }

        Random rng = new Random();
        do{
            clean(puzzle);
            int points = 0;
            int walls = 0;
            int numWalls = (_size*_size)/8 + rng.nextInt((_size*_size)/3);
            int maxPoints = (_size*_size)/4 + rng.nextInt((_size*_size)/2);

            print(puzzle);

            for(int i=1; i< _size+1; i++){
                for(int j=1; j<_size+1; j++){
                    Cell c = puzzle[i][j];
                    if(rng.nextFloat() <= 0.8f || points >= maxPoints){
                        c.setState(Cell.State.Wall);
                        walls++;
                    }
                    else{
                        c.setState(Cell.State.Point);
                        c.setMustWatch(1 + rng.nextInt((_size - 1) * 2));
                        points++;
                    }
                    c.setLocked(true);
                }
            }
            print(puzzle);
            if(walls < numWalls || wrongInitialBoard(puzzle)){
                continue;
            }

            while(walls > numWalls){
                int posX = 1 + rng.nextInt(_size);
                int posY = 1 + rng.nextInt(_size);
                if(puzzle[posX][posY].getState() == Cell.State.Wall) {
                    puzzle[posX][posY].setLocked(false);
                    puzzle[posX][posY].setState(Cell.State.Unassigned);
                    walls--;
                }
                print(puzzle);
            }

            print(puzzle);
        } while(!resolve(puzzle));
    }

    public void print() {
        print(_board);
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

    public boolean wrongInitialBoard(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                Cell c = puzzle[i][j];
                if(c.getState() == Cell.State.Point
                        && lookDirections(i, j) > c.getMustWatch())
                    return true;
            }
        }
        return false;
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

    private void clean(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                Cell c = puzzle[i][j];
                c.setLocked(false);
                c.setMustWatch(-1);
                c.setState(Cell.State.Unassigned);
            }
        }
    }

    private void randomize(Cell[][] puzzle){
        Random rng = new Random();
        int randomCells = _size + rng.nextInt(_size/2);
        for(int i = 0; i < randomCells; ++i){
            int posX, posY;
            posX = 1 + rng.nextInt(_size);
            posY = 1 + rng.nextInt(_size);
            Cell c = puzzle[posX][posY];
            if(c.getLocked()){
                --i;
                continue;
            }
            if(rng.nextInt(randomCells) == 0){
                c.setState(Cell.State.Wall);
            }
            else {
                c.setState(Cell.State.Point);
                c.setMustWatch(1 + rng.nextInt((_size - 1) * 2));
            }
            c.setLocked(true);
        }
    }

    private boolean resolve(Cell[][] puzzle){
        boolean changed = true;
        Hint puzzleHint = new Hint(puzzle);
        while(changed){
            changed = false;
            for (int i = 1; i < _size + 1; ++i){
                for (int j = 1; j < _size + 1; ++j){
                    CellHint h = null;
                    try {
                        h = puzzleHint.getPositiveHint(i, j);
                    } catch (Exception e){
                        e.printStackTrace();
                    }
                    if(h == null)
                        continue;
                    puzzle[h.pos.fst][h.pos.snd].setState(h.state);
                    changed = true;
                }
            }
        }
        //print(puzzle);
        return wrongCell() == null;
    }

    private void copyToBoard(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j) {
                _board[i][j].setState(puzzle[i][j].getState());
                _board[i][j].setMustWatch(puzzle[i][j].getMustWatch());
                _board[i][j].setLocked(puzzle[i][j].getLocked());
            }
        }
    }

    private void print(Cell[][] board) {
        System.out.println("=============================");
        for (int i = 1; i < _size + 1; ++i) {
            for (int j = 1; j < _size + 1; ++j) {
                switch (board[i][j].getState()){
                    case Null:
                        break;
                    case Unassigned:
                        System.out.print("?");
                        break;
                    case Point:
                        if(board[i][j].getLocked())
                            System.out.print(board[i][j].getMustWatch());
                        else
                            System.out.print("o");
                        break;
                    case Wall:
                        System.out.print("x");
                        break;
                }
                System.out.print('\t');
            }
            System.out.println("\n");
        }
    }

    private Cell[][] _board;
    private int _size;
    private Hint _hint;
}