package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Pair;

import static es.ucm.gdv.pcohno.Board.Dirs;
import static es.ucm.gdv.pcohno.Board._dirs;

/**
 * Class which manages hints
 */
public class Hint {

    static enum Type{
        First, Second, Third, Fourth, Fifth, Sixth, Seventh
    }

    /**
     * Constructor
     * @param board: board where the hint will be applied on
     */
    public Hint(Cell[][] board){
        this._board = board;
        this._utils = new BoardUtils();
    }

    /**
     * Returns the max number of positions a cell can watch according to walls
     * @param seeing: points given cell is already watching
     * @param mustWatch: points given cell must watch
     * @param posRow: row of cell
     * @param posCol: column of cell
     * @return an array where the index represents a direction and has the max number
     *  of cells watchable in that direction
     */
    private int[] maxWatchablePositions(int seeing, int mustWatch, int posRow, int posCol){
        int[] maxWatchablePoss = {0, 0, 0, 0};
        //For every direction
        for(Board.Dirs dir : Board.Dirs.values()){
            int i = 0;
            int row = posRow + _dirs[dir.ordinal()].fst;
            int col = posCol + _dirs[dir.ordinal()].snd;
            //Count watchable positions
            while(seeing < mustWatch && _board[row][col].getState() != Cell.State.Wall){
                ++i;
                //Next position according to direction
                row += _dirs[dir.ordinal()].fst;
                col += _dirs[dir.ordinal()].snd;
            }
            maxWatchablePoss[dir.ordinal()] = i;
        }
        return maxWatchablePoss;
    }

    /**
     * Applies first hint
     * @param seeing: points given cell is already watching
     * @param locked: locked state of cell
     * @param mustWatch: points given cell must watch
     * @param posRow: row of cell
     * @param posCol: column of cell
     * @return a CellHint if hint can be applied, null in other case
     */
    //Si un número tiene ya visibles el número de celdas que dice, entonces se puede cerrar
    private CellHint hint1(int seeing, boolean locked, int mustWatch, int posRow, int posCol){
        if(locked && seeing == mustWatch){
            for(Dirs dir : Dirs.values()){
                int row = posRow + _dirs[dir.ordinal()].fst;
                int col = posCol + _dirs[dir.ordinal()].snd;
                boolean emptyPos = false;
                while(!emptyPos){
                    if (_board[row][col].getState() == Cell.State.Unassigned){
                        emptyPos = true;
                        break;
                    }
                    if (_board[row][col].getState() == Cell.State.Wall){
                        break;
                    }
                    row += _dirs[dir.ordinal()].fst;
                    col += _dirs[dir.ordinal()].snd;
                }
                if(emptyPos)
                    return new CellHint(Cell.State.Wall, row, col, Type.First);
            }
        }
        return null;
    }

    /**
     * Applies second hint
     * @param seeing: points given cell is already watching
     * @param locked: locked state of cell
     * @param mustWatch: points given cell must watch
     * @param posRow: row of cell
     * @param posCol: column of cell
     * @return a CellHint if hint can be applied, null in other case
     */
    /*Si pusiéramos un punto azul en una celda vacía, superaríamos el número de visibles
       del número, y por tanto, debe ser una pared*/
    private CellHint hint2(int seeing, boolean locked, int mustWatch, int posRow, int posCol){
        if(locked && seeing < mustWatch){
            for(Dirs dir : Dirs.values()) {
                int row = posRow + _dirs[dir.ordinal()].fst;
                int col = posCol + _dirs[dir.ordinal()].snd;
                while(_board[row][col].getState() == Cell.State.Point) {
                    row += _dirs[dir.ordinal()].fst;
                    col += _dirs[dir.ordinal()].snd;
                }
                if(_board[row][col].getState() == Cell.State.Unassigned &&
                        seeing + 1 + -_utils.lookDirection(dir, row, col, _board) > mustWatch)
                {
                    return new CellHint(Cell.State.Wall, row, col, Type.Second);
                }
            }
        }
        return null;
    }

    /**
     * Applies third hint
     * @param seeing: points given cell is already watching
     * @param locked: locked state of cell
     * @param mustWatch: points given cell must watch
     * @param posRow: row of cell
     * @param posCol: column of cell
     * @return a CellHint if hint can be applied, null in other case
     */
    /*Si no ponemos un punto en alguna celda vacía, entonces es imposible alcanzar el
       número*/
    private CellHint hint3(int seeing, boolean locked, int mustWatch, int posRow, int posCol){
        if(locked && seeing < mustWatch){
            int[] maxWatchablePoss = maxWatchablePositions(seeing, mustWatch, posRow, posCol);
            for(Dirs dir : Dirs.values()){
                int maxOthers = maxWatchablePoss[(dir.ordinal() +1) % 4] +
                        maxWatchablePoss[(dir.ordinal() +2) % 4] +
                        maxWatchablePoss[(dir.ordinal() +3) % 4];
                if (maxOthers < mustWatch){ //Minus 1 because i count myself in mustWatch
                    int n = _utils.lookDirection(dir, posRow, posCol, _board);
                    if(maxOthers + n < mustWatch ){
                        Pair _dir = _dirs[dir.ordinal()];
                        int posiblePosRow = posRow + _dir.fst * (n + 1);
                        int posiblePosCol = posCol + _dir.snd * (n + 1);

                        if(_board[posiblePosRow][posiblePosCol].getState() == Cell.State.Unassigned)
                            return new CellHint(Cell.State.Point, posiblePosRow, posiblePosCol, Type.Third);
                    }
                }
            }
        }
        return null;
    }

    /**
     * Applies fourth hint
     * @param seeing: points given cell is already watching
     * @param locked: locked state of cell
     * @param mustWatch: points given cell must watch
     * @param posRow: row of cell
     * @param posCol: column of cell
     * @return a CellHint if hint can be applied, null in other case
     */
    private CellHint hint4(int seeing, boolean locked, int mustWatch, int posRow, int posCol){
        if(_board[posRow][posCol].getState() == Cell.State.Point) {
            if (locked && seeing > mustWatch)
                return new CellHint(Cell.State.Null, posRow, posCol, Type.Fourth);
        }
        return null;
    }

    /**
     * Applies fifth hint
     * @param seeing: points given cell is already watching
     * @param locked: locked state of cell
     * @param mustWatch: points given cell must watch
     * @param posRow: row of cell
     * @param posCol: column of cell
     * @return a CellHint if hint can be applied, null in other case
     */
    private CellHint hint5(int seeing, boolean locked, int mustWatch, int posRow, int posCol) {
        if(_board[posRow][posCol].getState() == Cell.State.Point) {
            if (locked && seeing < mustWatch) {
                int[] maxWatchablePoss = maxWatchablePositions(seeing, mustWatch, posRow, posCol);
                int sum = 0;
                for (int value : maxWatchablePoss)
                    sum += value;
                if(sum < mustWatch)
                    return new CellHint(Cell.State.Null, posRow, posCol, Type.Fifth);
            }
        }
        return null;
    }

    /**
     * Applies sixth hint
     * @param seeing: points given cell is already watching
     * @param locked: locked state of cell
     * @param mustWatch: points given cell must watch
     * @param posRow: row of cell
     * @param posCol: column of cell
     * @return a CellHint if hint can be applied, null in other case
     */
    private CellHint hint6(int seeing, boolean locked, int mustWatch, int posRow, int posCol) {
        if (_board[posRow][posCol].getState() == Cell.State.Unassigned){
            if(     (_board[posRow - 1][posCol].getState() == Cell.State.Wall) &&
                    (_board[posRow + 1][posCol].getState() == Cell.State.Wall) &&
                    ( _board[posRow][posCol - 1].getState() == Cell.State.Wall) &&
                    (_board[posRow][posCol + 1].getState() == Cell.State.Wall))
            {
                return new CellHint(Cell.State.Wall, posRow, posCol, Type.Sixth);
            }
        }
        return null;
    }


    /**
     * Applies seventh hint
     * @param seeing: points given cell is already watching
     * @param locked: locked state of cell
     * @param mustWatch: points given cell must watch
     * @param posRow: row of cell
     * @param posCol: column of cell
     * @return a CellHint if hint can be applied, null in other case
     */
    private CellHint hint7(int seeing, boolean locked, int mustWatch, int posRow, int posCol) {
        if (_board[posRow][posCol].getState() == Cell.State.Point && !locked && seeing == 0){
            if(     ( _board[posRow - 1][posCol].getState() == Cell.State.Wall) &&
                    (_board[posRow + 1][posCol].getState() == Cell.State.Wall) &&
                    (_board[posRow][posCol - 1].getState() == Cell.State.Wall) &&
                    (_board[posRow][posCol + 1].getState() == Cell.State.Wall))
            {
                return new CellHint(Cell.State.Null, posRow, posCol, Type.Seventh);
            }
        }
        return null;
    }

    /**
     * Tries to apply every positive hint to a cell in ascending order
     * @param row: cell's row on board
     * @param col: cell's column on board
     * @return a CellHint if any positive hint can be applied, null in other case
     */
    public CellHint getPositiveHint(int row, int col){
        int seeing = _utils.lookDirections(row, col, _board);
        boolean locked = _board[row][col].getLocked();
        int mustWatch = _board[row][col].getMustWatch();

        CellHint c = null;

        c = hint1(seeing, locked, mustWatch, row, col);
        if(c != null) return c;

        c = hint2(seeing, locked, mustWatch, row, col);
        if(c != null) return c;

        c = hint3(seeing, locked, mustWatch, row, col);
        if(c != null) return c;

        c = hint6(seeing, locked, mustWatch, row, col);
        return c;
    }

    /**
     * Tries to apply every negative hint to a cell in ascending order
     * @param row: cell's row on board
     * @param col: cell's column on board
     * @return a CellHint if any negative hint can be applied, null in other case
     */
    public CellHint getNegativeHint(int row, int col){
        int seeing = _utils.lookDirections(row, col, _board);
        boolean locked = _board[row][col].getLocked();
        int mustWatch = _board[row][col].getMustWatch();

        CellHint c = null;

        c = hint4(seeing, locked, mustWatch, row, col);
        if(c != null) return c;

        c = hint5(seeing, locked, mustWatch, row, col);
        if(c != null) return c;

        c = hint7(seeing, locked, mustWatch, row, col);
        return c;
    }

    /**
     * Board representation to apply hints
     */
    private Cell[][] _board;

    /**
     * Object for cell's calculation
     */
    private final BoardUtils _utils;
}