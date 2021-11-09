package es.ucm.gdv.pcohno;

import java.util.Random;
import java.util.Stack;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Pair;
import es.ucm.gdv.engine.Image;

public class Board {

    static Pair[] _dirs = new Pair[]{
            new Pair(-1, 0),
            new Pair(1, 0),
            new Pair(0, -1),
            new Pair(0, 1),
    };

    public Pair[] getDirs(){
        return _dirs;
    }

    static enum Dirs {Up, Down, Left, Right}

    public Board(int size, Image lockImg){
        _board = new Cell[size + 2][size + 2];
        _totalUnassignedCells=1;
        _size = size;
        this._hint = new Hint(_board);
        _actions = new Stack<CellHint>();
        highlightedCircle = false;
        highlightedRow = highlightedCol = 0;

        imgLock = lockImg;
        showLocks = false;
        biggerCell = new Pair(-1, -1);
        biggerCellScale = 1;

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

    public void render(Graphics graphics) {
        graphics.save();

        graphics.translate(10, 100);

        int spacing = 105;
        float aux = 380 / (float)(spacing * _size);

        graphics.scale(aux, aux);
        graphics.translate((int)(spacing * 0.5), spacing/2);

        for(int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; j++) {
                if(biggerCell.fst == i && biggerCell.snd == j){
                    _board[i][j].render(graphics, biggerCellScale);
                }
                else _board[i][j].render(graphics, 1);

                if(showLocks && _board[i][j].getState() == Cell.State.Wall && _board[i][j].getLocked()){
                    graphics.scale(0.75f, 0.75f);
                    graphics.translate(-imgLock.getWidth()/2, -imgLock.getHeight()/2);
                    graphics.drawImage(imgLock, 0, 0, 0.2f);
                    graphics.translate(imgLock.getWidth()/2, imgLock.getHeight()/2);
                    graphics.scale(1/0.75f, 1/0.75f);
                }
                if (highlightedCircle && i == highlightedRow && j == highlightedCol){
                    graphics.setColor(0xff000000);
                    graphics.drawCircle(0,0,50, 3);
                }
                graphics.translate(spacing, 0);
            }
            graphics.translate(-spacing * _size, spacing);
        }

        graphics.restore();
    }

    public boolean isOnMe(int x, int y){

        y -= 100;
        x -= 10;

        int spacing = 105;
        float aux = 380 / (float)(spacing * _size);
        float mx = x, my = y;

        mx /= aux;
        my /= aux;

        mx = mx / spacing;
        my = my / spacing;

        int columna;
        int fila;

        if(mx >= 0 && mx <= _size && my >= 0 && my <= _size){
            fila = (int)++my;
            columna = (int)++mx;
            Cell c = _board[fila][columna];
            if(!c.getLocked()) {
                _actions.push(new CellHint(c.getState(), fila, columna));
                c.nextState();
            }
            else{
                biggerCell.fst = fila;
                biggerCell.snd = columna;
                return false;
            }
        }
        return true;
    }


    public void print() {
        print(_board);
    }

    /**
     * @return pair of the first wrong cell. If all are correct, returns (-1, -1)
     */
    public Pair wrongCell() {
        return wrongCell(_board);
    }

    public Pair wrongCell(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                if(!isCellRight(i, j, puzzle))
                    return new Pair(i, j);
            }
        }
        return null;
    }

    private boolean isCellRight(int row, int col, Cell[][] puzzle){
        Cell c = puzzle[row][col];
        if(c.getLocked() && c.getState() == Cell.State.Point)
            return c.getMustWatch() == lookDirections(row, col, puzzle);
        else if(c.getState() == Cell.State.Point)
            return lookDirections(row, col, puzzle) > 0;
        else
            return c.getState() == Cell.State.Wall;
    }

    //Devolver nueva celda puesta o una cela mal muesta
    public CellHint getHint() {
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                CellHint h = null;
                try {
                    h = _hint.getPositiveHint(i, j);
                } catch (Exception e){
                    e.printStackTrace();
                }
                if(h == null){
                    try {
                        h = _hint.getNegativeHint(i, j);
                    } catch (Exception e){
                        e.printStackTrace();
                    }
                }
                if(h == null) continue;

                h.pos = new Pair(i, j);
                return h;
            }
        }
        return null;
    }

    public int getPercentage()
    {
        int unassignedCells = countUnassignedCells();

        int percentage = 100-((100*unassignedCells)/_totalUnassignedCells);

        return percentage;
    }

    public CellHint undo(){
        if(!_actions.empty()){
            CellHint last = _actions.pop();
            _board[last.pos.fst][last.pos.snd].setState(last.state);
            return last;
        }
        return null;
    }

    public void highlightCircle(int row, int col, boolean enable){
        highlightedRow = row;
        highlightedCol = col;
        highlightedCircle = enable;
    }

    public void setShowLocks(boolean enable){
        showLocks = enable;
    }

    public boolean getShowLocks(){
        return showLocks;
    }

    public void noBiggerCell(){
        biggerCell.fst = -1;
        biggerCell.snd = -1;
    }

    public void setBiggerCellScale(double scale) { biggerCellScale = scale; }

    public int lookDirections(int row, int col, Cell[][] puzzle){
        int seeing = 0;

        //Look up
        seeing += lookDirection(Dirs.Up, row, col, puzzle);
        //Look down
        seeing+= lookDirection(Dirs.Down, row, col, puzzle);
        //Look left
        seeing += lookDirection(Dirs.Left, row, col, puzzle);
        //Look right
        seeing += lookDirection(Dirs.Right, row, col, puzzle);

        return seeing;
    }

    public int posibleSeeingDirection(Cell[][] puzzle, Dirs direction, int row, int col){
        Pair dir = _dirs[direction.ordinal()];

        int seeing = 0;
        row = row + dir.fst;
        col = col + dir.snd;

        while(puzzle[row][col].getState() != Cell.State.Wall){
            if(puzzle[row][col].getState() == Cell.State.Unassigned)
                ++seeing;
            row += dir.fst;
            col += dir.snd;
        }
        return seeing;
    }

    private int lookDirection(Dirs direction, int row, int col){
        return lookDirection(direction, row, col, _board);
    }

    public int lookDirection(Dirs direction, int row, int col, Cell[][] board){
        Pair dir = _dirs[direction.ordinal()];

        int seeing = 0;
        row = row + dir.fst;
        col = col + dir.snd;

        while(board[row][col].getState() == Cell.State.Point){
            ++seeing;
            row += dir.fst;
            col += dir.snd;
        }
        return seeing;
    }

    public void copyToBoard(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j) {
                _board[i][j].setLocked(false);
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

    public int countUnassignedCells()
    {
        int unassignedCells=0;
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                if(_board[i][j].getState()== Cell.State.Unassigned && !_board[i][j].getLocked())
                {
                    unassignedCells++;
                }
            }
        }

        return unassignedCells;
    }

    public void setFinished(){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                if(_board[i][j].getState() == Cell.State.Point && !_board[i][j].getLocked())
                {
                    _board[i][j].setMustWatch(lookDirections(i, j, _board));
                }
            }
        }
    }

    public void setUnassignedCells(int numberUnassigned){
        _totalUnassignedCells = numberUnassigned;
    }

    public Cell getCell(int row, int col){ return _board[row][col]; }

    private Cell[][] _board;
    private int _size;
    private Hint _hint;
    private int _totalUnassignedCells;
    private Stack<CellHint> _actions;

    private boolean highlightedCircle;
    private int highlightedRow;
    private int highlightedCol;

    private boolean showLocks;
    private Image imgLock;
    Pair biggerCell;
    double biggerCellScale;
}