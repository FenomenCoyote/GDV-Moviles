package es.ucm.gdv.pcohno;



public class Hint {

    public Hint(Cell[][] board){
        this._board = board;
        this._boardSize = board.length();
    }

    //Devuelve un array de 4 con el numero de puntos que ve en cada direccion :)
    private int[] getView() {
        int[] view = new int[4];
        view[0] = lookDirection(Dirs.Up);
        view[1] = lookDirection(Dirs.Down);
        view[2] = lookDirection(Dirs.Left);
        view[3] = lookDirection(Dirs.Right);
        return view;
    }

    private int lookDirections(){
        int[] view = getView();
        return view[0] + view[1] + view[2] + view[3];
    }

    private int lookDirection(Dirs direction, int posRow, int posCol){
        Pair<Integer,Integer> dir = _dirs[direction];

        int row = posRow;
        int col = posCol;
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
        for(Dirs dir : Dirs.values()){
            int i = 0;
            int row = posRow + _dirs[dir].fst;
            int col = posCol + _dirs[dir].snd;
            while(row >= 0 && row < _boardSize && col >= 0 && col < _boardSize &&
                    seeing + i < mustWatch){
                if(_board[row][col].getState() == Cell.State.Wall)
                    break;
                ++i;
                row += _dirs[dir].fst;
                col += _dirs[dir].snd;
            }
            maxWatchablePoss[dir] = i;
        }
        return maxWatchablePoss;
    }

    //Si un número tiene ya visibles el número de celdas que dice, entonces se puede cerrar
    private Cell hint1(int seeing, boolean locked, int mustWatch, int posRow, int posCol){
        if(locked && seeing == mustWatch){
            for(Dirs dir : Dirs.values()){
                int row = posRow + _dirs[dir].fst;
                int col = posCol + _dirs[dir].snd;
                boolean emptyPos = false;
                while(row >= 0 && row < _boardSize && col >= 0 && col < _boardSize && !emptyPos){
                    if (board[row][col].getState() == Cell.State.Unassigned)
                        emptyPos = true;
                    row += _dirs[dir].fst;
                    col += _dirs[dir].snd;
                }
                if(emptyPos)
                    return new Cell(false, -1, Cell.State.Wall);
            }
        }
        return null;
    }

    /*Si pusiéramos un punto azul en una celda vacía, superaríamos el número de visibles
       del número, y por tanto, debe ser una pared*/
    private Cell hint2(int seeing, boolean locked, int mustWatch, int posRow, int posCol){
        if(locked && seeing < mustWatch){
            for(Dirs dir : Dirs.values()) {
                int row = posRow + _dirs[dir].fst;
                int col = posCol + _dirs[dir].snd;
                while(row >= 0 && row < _boardSize && col >= 0 && col < _boardSize
                        && board[row][col].getState() != Cell.State.Wall){
                    if(board[row][col].getState() == Cell.State.Point)
                        continue;

                    board[row][col].getState() = Cell.State.Point;
                    if(lookDirections(board) > mustWatch)
                        return new Cell(false, -1, Cell.State.Wall);
                    else {
                        board[row][col].getState() = Cell.State.Unassigned;
                    }
                    row += _dirs[dir].fst; col += _dirs[dir].snd;
                }
            }
        }
        return null;
    }

    /*Si no ponemos un punto en alguna celda vacía, entonces es imposible alcanzar el
       número*/
    private Cell hint3(int seeing, boolean locked, int mustWatch, int posRow, int posCol){
        if(locked && seeing < mustWatch){
            int[] maxWatchablePoss = maxWatchablePositions(board);
            for(Dirs dir : Dirs.values()){
                int maxOthers = maxWatchablePoss[(dir +1) % 4] +
                        maxWatchablePoss[(dir +2) % 4] +
                        maxWatchablePoss[(dir +3) % 4];
                if (maxOthers < mustWatch){
                    int n = lookDirection(_dirs[dir], board);
                    if(maxOthers + n < mustWatch){
                        return new Cell(false, -1, Cell.State.Null);
                    }
                }
            }
        }
        return Cell.State.Null;
    }

    public Cell getCellState(int row, int col){
        int seeing = lookDirections(row, col);
        boolean locked = _board[row][col].getLocked();
        int mustWatch = _board[row][col].getMustWatch();

        Cell c = null;

        c = hint1(seeing, locked, mustWatch, row, col);
        if(c != null) return c;

        c = hint2(seeing, locked, mustWatch, row, col);
        if(c != null) return c;

        c = hint3(seeing, locked, mustWatch, row, col);
        if(c != null) return c;

        //Si no he podido usar ninguna pista, devuelvo un null state
        return null;
    }

    private Cell[][] _board;
    private int _boardSize;
}