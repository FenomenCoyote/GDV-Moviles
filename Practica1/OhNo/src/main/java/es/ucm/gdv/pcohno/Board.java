package es.ucm.gdv.pcohno;

import java.util.Stack;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Pair;

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

     /**
     * Creates board of size*size Cells (size+2*size+2 in logic)
     * @param size
     * @param lockImg
     * @param showLocksTime
     * @param pool
     */
    public Board(int size, Image lockImg, double showLocksTime, CellPool pool){

        this._utils = new BoardUtils();
        this._board = new Cell[size + 2][size + 2];
        this._size = size;
        this._hint = new Hint(_board);
        this._totalUnassignedCells = 1;
        this._actions = new Stack<CellHint>();

        this._highlightedCircle = false;
        this._highlightedRow = this._highlightedCol = 0;

        this._showLocksTime = this._initialShowLocksTime = showLocksTime;
        this._showLocks = false;
        this._imgLock = lockImg;
        this._biggerCell = new Pair(-1, -1);
        this._biggerCellScale = 1;

        for (int i = 0; i < size + 2; ++i){
            for (int j = 0; j < size + 2; ++j){
                Cell c = pool.retrieveCell();
                if(i == 0 || j == 0 || i == size + 1 || j == size + 1){
                    c.setLocked(false);
                    c.setMustWatch(-1);
                    c.setState(Cell.State.Wall);
                    c.setLocked(true);
                }
                else {
                    c.setLocked(false);
                    c.setMustWatch(-1);
                    c.setState(Cell.State.Unassigned);
                }
                _board[i][j] = c;
            }
        }
    }

    /**
     * Must be called in order to release the cells to the CellPool
     * @param pool
     */
    public void release(CellPool pool) {
        pool.releaseCell(_board);
    }

    /**
     * Used for animations
     * @param elapsedTime
     */
    public void update(double elapsedTime){
        //alpha management for the locks
        if(_showLocks){
            _showLocksTime -= elapsedTime;
            double s = _showLocksTime / _initialShowLocksTime;
            s = 1 + (s * 0.2);
            _biggerCellScale = s;
            if(_showLocksTime <= 0){
                _showLocks = false;
                _biggerCell.fst = -1; _biggerCell.snd = -1;
                _showLocksTime = _initialShowLocksTime;
            }
        }

        //alpha management for each cell state
        for(int i=1; i<_size+1; i++){
            for(int j=1; j<_size+1; j++){
                _board[i][j].update(elapsedTime);
            }
        }
    }

    /**
     * Renders the board at the center. Also renders the locks if needed
     * @param graphics
     * @param alphaTransition
     */
    public void render(Graphics graphics, float alphaTransition) {
        graphics.save();

        graphics.translate(10, 100);

        int spacing = 105;
        float aux = 380 / (float)(spacing * _size);

        graphics.scale(aux, aux);
        graphics.translate((int)(spacing * 0.5), spacing/2);

        for(int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; j++) {
                if(_biggerCell.fst == i && _biggerCell.snd == j){
                    _board[i][j].render(graphics, _biggerCellScale, alphaTransition);
                }
                else _board[i][j].render(graphics, 1, alphaTransition);

                if(_showLocks && _board[i][j].getState() == Cell.State.Wall && _board[i][j].getLocked()){
                    graphics.scale(0.75f, 0.75f);
                    graphics.translate(-_imgLock.getWidth()/2, -_imgLock.getHeight()/2);
                    graphics.drawImage(_imgLock, 0, 0, 0.2f * alphaTransition);
                    graphics.translate(_imgLock.getWidth()/2, _imgLock.getHeight()/2);
                    graphics.scale(1/0.75f, 1/0.75f);
                }
                if (_highlightedCircle && i == _highlightedRow && j == _highlightedCol){
                    graphics.setColor(((int)(alphaTransition * 255) << 24) | 0xff333333);
                    graphics.drawCircle(0,0,50, 6);
                }
                graphics.translate(spacing, 0);
            }
            graphics.translate(-spacing * _size, spacing);
        }

        graphics.restore();
    }

    /**
     * Called from Playing to check if input is on some cell
     * @param x
     * @param y
     */
    public void isOnMe(int x, int y){

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
                c.nextStateTransition();
            }
            else{
                _biggerCell.fst = fila;
                _biggerCell.snd = columna;
                if(_showLocks){
                    _showLocksTime = _initialShowLocksTime;
                }
                else _showLocks = true;
            }
        }
    }

    /**
     * Prints current board state (ignoring exterior walls)
     */
    public void print() {
        print(_board);
    }

    /**
     * @return pair of the first wrong cell. If all are correct, returns null
     */
    public Pair wrongCell() {
        return wrongCell(_board);
    }

    /**
     *
     * @param puzzle
     * @return pair of the first wrong cell. If all are correct, returns null
     */
    public Pair wrongCell(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                if(!isCellRight(i, j, puzzle))
                    return new Pair(i, j);
            }
        }
        return null;
    }

    /**
     * Checks if cell at pos row, col is placed correctly
     * @param row
     * @param col
     * @param puzzle
     * @return
     */
    private boolean isCellRight(int row, int col, Cell[][] puzzle){
        Cell c = puzzle[row][col];
        if(c.getLocked() && c.getState() == Cell.State.Point)
            return c.getMustWatch() == _utils.lookDirections(row, col, puzzle);
        else if(c.getState() == Cell.State.Point)
            return _utils.lookDirections(row, col, puzzle) > 0;
        else
            return c.getState() == Cell.State.Wall;
    }


    /**
     * Tries to get a hint, either positive or negative, from the board
     * @return null if no hint was found
     */
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

    /**
     * Calculates and returns current percentage
     * @return
     */
    public int getPercentage()
    {
        int unassignedCells = countUnassignedCells();
        return 100-((100*unassignedCells)/_totalUnassignedCells);
    }

    /**
     * Undoes last move
     * @return
     */
    public CellHint undo(){
        if(!_actions.empty()){
            CellHint last = _actions.pop();
            _board[last.pos.fst][last.pos.snd].setState(last.state);
            return last;
        }
        return null;
    }

    /**
     * Highlights row, col cell
     * @param row
     * @param col
     * @param enable
     */
    public void highlightCircle(int row, int col, boolean enable){
        _highlightedRow = row;
        _highlightedCol = col;
        _highlightedCircle = enable;
    }

    /**
     * Copies puzzle to board
     * @param puzzle
     */
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

    /**
     * Prints board
     * @param board
     */
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

    /**
     * Counts how many unassigned cell there are
     * @return
     */
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

    /**
     * Sets all cells mustWatch to what they see
     */
    public void setFinished(){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                if(_board[i][j].getState() == Cell.State.Point && !_board[i][j].getLocked())
                {
                    _board[i][j].setMustWatch(_utils.lookDirections(i, j, _board));
                }
            }
        }
    }

    /**
     * Called to set how many unassigned cells the are on the board
     * @param numberUnassigned
     */
    public void setUnassignedCells(int numberUnassigned){
        _totalUnassignedCells = numberUnassigned;
    }

    /**
     * Gets cell at row, col
     * @param row
     * @param col
     * @return
     */
    public Cell getCell(int row, int col){ return _board[row][col]; }

    private BoardUtils _utils;
    private Cell[][] _board;
    private int _size;
    private Hint _hint;
    private int _totalUnassignedCells;
    private Stack<CellHint> _actions;

    private boolean _highlightedCircle;
    private int _highlightedRow;
    private int _highlightedCol;

    private double _showLocksTime;
    private double _initialShowLocksTime;
    private boolean _showLocks;
    private Image _imgLock;
    private Pair _biggerCell;
    private double _biggerCellScale;
}