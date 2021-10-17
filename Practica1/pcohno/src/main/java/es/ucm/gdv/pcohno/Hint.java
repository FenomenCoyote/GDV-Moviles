package es.ucm.gdv.pcohno;


import com.sun.tools.javac.util.Pair;

import static es.ucm.gdv.pcohno.Board.Dirs;
import static es.ucm.gdv.pcohno.Board._dirs;

public class Hint {

    public Hint(Cell[][] board){
        this._board = board;
        this._boardSize = board.length;
    }

    //Devuelve un array de 4 con el numero de puntos que ve en cada direccion :)
    private int[] getView(int row, int col) {
        int[] view = new int[4];
        view[0] = lookDirection(Dirs.Up, row, col);
        view[1] = lookDirection(Dirs.Down, row, col);
        view[2] = lookDirection(Dirs.Left, row, col);
        view[3] = lookDirection(Dirs.Right, row, col);
        return view;
    }

    private int lookDirections(int row, int col){
        int[] view = getView(row, col);
        return 1 + view[0] + view[1] + view[2] + view[3];
    }

    private int lookDirection(Dirs direction, int posRow, int posCol){
        Pair<Integer,Integer> dir = _dirs[direction.ordinal()];

        int row = posRow + dir.fst;
        int col = posCol + dir.snd;
        int seeing = 0;
        while(row >= 0 && row < _boardSize && col >= 0 && col < _boardSize){
            if(_board[row][col].getState() == Cell.State.Point) {
                ++seeing;
                row += dir.fst;
                col += dir.snd;
            }
            else break;
        }
        return seeing;
    }


    private int[] maxWatchablePositions(int seeing, int mustWatch, int posRow, int posCol){
        int[] maxWatchablePoss = {0, 0, 0, 0};
        for(Board.Dirs dir : Board.Dirs.values()){
            int i = 0;
            int row = posRow + _dirs[dir.ordinal()].fst;
            int col = posCol + _dirs[dir.ordinal()].snd;
            while(row >= 0 && row < _boardSize && col >= 0 && col < _boardSize &&
                    seeing + i < mustWatch){
                if(_board[row][col].getState() == Cell.State.Wall)
                    break;
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
                while(row >= 0 && row < _boardSize && col >= 0 && col < _boardSize && !emptyPos){
                    if (_board[row][col].getState() == Cell.State.Unassigned)
                        emptyPos = true;
                    row += _dirs[dir.ordinal()].fst;
                    col += _dirs[dir.ordinal()].snd;
                }
                if(emptyPos)
                    return new CellHint(Cell.State.Wall, row, col);
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
                while(row >= 0 && row < _boardSize && col >= 0 && col < _boardSize
                        && _board[row][col].getState() != Cell.State.Wall){
                    if(_board[row][col].getState() == Cell.State.Point)
                        break;

                    if(lookDirections(row, col) > mustWatch)
                        return new CellHint(Cell.State.Wall, row, col);
                    else {
                        _board[row][col].setState(Cell.State.Unassigned);
                    }
                    row += _dirs[dir.ordinal()].fst;
                    col += _dirs[dir.ordinal()].snd;
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
                if (maxOthers < mustWatch - 1){ //Minus 1 because i count myself in mustWatch
                    int n = 1 + lookDirection(dir, posRow, posCol);
                    if(maxOthers + n < mustWatch ){
                        Pair<Integer, Integer> _dir = _dirs[dir.ordinal()];
                        int posiblePosRow = posRow + _dir.fst * n;
                        int posiblePosCol = posCol + _dir.snd * n;

                        if(     posiblePosRow >= 0 && posiblePosRow < _boardSize &&
                                posiblePosCol >= 0 && posiblePosCol < _boardSize &&
                                _board[posiblePosRow][posiblePosCol].getState() == Cell.State.Unassigned)
                            return new CellHint(Cell.State.Point,
                                    posiblePosRow,
                                    posiblePosCol);
                    }
                }
            }
        }
        return null;
    }

    private CellHint hint4(int seeing, boolean locked, int mustWatch, int posRow, int posCol){
        if(_board[posRow][posCol].getState() == Cell.State.Point) {
            if (locked && seeing > mustWatch)
                return new CellHint(Cell.State.Null, posRow, posCol);
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
                    return new CellHint(Cell.State.Null, posRow, posCol);
            }
        }
        return null;
    }

    private CellHint hint6(int seeing, boolean locked, int mustWatch, int posRow, int posCol) {
        if (_board[posRow][posCol].getState() == Cell.State.Unassigned){
            if(     ((posRow - 1) < 0 || _board[posRow - 1][posCol].getState() == Cell.State.Wall) &&
                    ((posRow + 1) >= _boardSize || _board[posRow + 1][posCol].getState() == Cell.State.Wall) &&
                    ((posCol - 1) < 0 || _board[posRow][posCol - 1].getState() == Cell.State.Wall) &&
                    ((posCol + 1) >= _boardSize || _board[posRow][posCol + 1].getState() == Cell.State.Wall)){
                return new CellHint(Cell.State.Null, posRow, posCol);
            }
        }
        return null;
    }


    private CellHint hint7(int seeing, boolean locked, int mustWatch, int posRow, int posCol) {
        if (_board[posRow][posCol].getState() == Cell.State.Point && !locked && seeing == 0){
            if(     ((posRow - 1) < 0 || _board[posRow - 1][posCol].getState() == Cell.State.Wall) &&
                    ((posRow + 1) >= _boardSize || _board[posRow + 1][posCol].getState() == Cell.State.Wall) &&
                    ((posCol - 1) < 0 || _board[posRow][posCol - 1].getState() == Cell.State.Wall) &&
                    ((posCol + 1) >= _boardSize || _board[posRow][posCol + 1].getState() == Cell.State.Wall)){
                return new CellHint(Cell.State.Null, posRow, posCol);
            }
        }
        return null;
    }

    public CellHint getPositiveHint(int row, int col){
        int seeing = lookDirections(row, col);
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
        int seeing = lookDirections(row, col);
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
    private int _boardSize;
}