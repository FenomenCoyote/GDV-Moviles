package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Pair;

import static es.ucm.gdv.pcohno.Board.Dirs;
import static es.ucm.gdv.pcohno.Board._dirs;

public class Hint {


    static enum Type{
        First, Second, Third, Fourth, Fifth, Sixth, Seventh
    }

    public Hint(Cell[][] board){
        this._board = board;
        this._utils = new BoardUtils();
    }

    private int[] maxWatchablePositions(int seeing, int mustWatch, int posRow, int posCol){
        int[] maxWatchablePoss = {0, 0, 0, 0};
        for(Board.Dirs dir : Board.Dirs.values()){
            int i = 0;
            int row = posRow + _dirs[dir.ordinal()].fst;
            int col = posCol + _dirs[dir.ordinal()].snd;
            while(seeing < mustWatch && _board[row][col].getState() != Cell.State.Wall){
                ++i;
                row += _dirs[dir.ordinal()].fst;
                col += _dirs[dir.ordinal()].snd;
            }
            maxWatchablePoss[dir.ordinal()] = i;
        }
        return maxWatchablePoss;
    }

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

    private CellHint hint4(int seeing, boolean locked, int mustWatch, int posRow, int posCol){
        if(_board[posRow][posCol].getState() == Cell.State.Point) {
            if (locked && seeing > mustWatch)
                return new CellHint(Cell.State.Null, posRow, posCol, Type.Fourth);
        }
        return null;
    }

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

    private Cell[][] _board;
    private final BoardUtils _utils;
}