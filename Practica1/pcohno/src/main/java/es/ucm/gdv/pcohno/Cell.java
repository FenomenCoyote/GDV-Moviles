package es.ucm.gdv.pcohno;

import com.sun.tools.javac.util.Pair;

public class Cell {

    private static Pair<Integer, Integer>[] _dirs = { (-1,0),(1,0),(0,-1),(0,1) };

    private enum State {Unassigned, Point, Wall}
    private enum Dirs {Up, Down, Left, Right}

    public Cell(int posX, int posY, int boardSize){
        this._locked = false;
        this._mustWatch = -1;
        this._pos = new Pair<Integer, Integer>(posX, posY);
        this._boardSize = boardSize;
        this._state = State.Unassigned;
    }

    public Cell(boolean locked, int mustWatch, int posX, int posY, int boardSize, State state){
        this._locked = locked;
        this._mustWatch = mustWatch;
        this._pos = new Pair<Integer, Integer>(posX, posY);
        this._boardSize = boardSize;
        this._state = state;
    }

    public boolean setState(State state) {
        if(!_locked){
            _state = state;
            return true;
        }
        return false;
    }

    public Pair<Integer, Integer> getPos() { return _pos; }

    public void setMustWatch(int mustWatch){ _mustWatch = mustWatch; }

    public void setLocked(boolean locked){ _locked = locked; }

    public boolean isPoint(){
        return _state == State.Point;
    }

    public boolean isRight(Cell[][] board){
        if(_state == State.Unassigned)
            return false;
        else if(_state == State.Wall)
            return true;
        else if (_locked){
            int seeing = lookDirections(board);
            return seeing == _mustWatch;
        }
        else {
            int seeing = lookDirections(board);
            return seeing > 0;
        }
    }

    //Devuelve un array de 4 con el numero de puntos que ve en cada direccion :)
    private int[] getView(Cell[][] board) {
        int[] view = new int[4];
        view[0] = lookDirection(Dirs.Up, board);
        view[1] = lookDirection(Dirs.Down, board);
        view[2] = lookDirection(Dirs.Left, board);
        view[3] = lookDirection(Dirs.Right, board);
        return view;
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

    public Cell getHint(Cell[][] board){
        //Probamos con pista 1
        //Si un número tiene ya visibles el número de celdas que dice, entonces se puede cerrar
        int seeing = lookDirections(board);
        if(_locked && seeing == _mustWatch){
            for(Dirs dir : Dirs.values()){
                int row = _pos.fst + _dirs[dir].fst;
                int col = _pos.snd + _dirs[dir].snd;
                boolean emptyPos = false;
                while(row >= 0 && row < _boardSize && col >= 0 && col < _boardSize && !emptyPos){
                    if (board[row][col]._state == State.Unassigned) emptyPos = true;
                    row += _dirs[dir].fst; col += _dirs[dir].snd;
                }
                if(emptyPos) return new Cell(false, -1, row, col, _boardSize, State.Wall);
            }
        }

        //Probamos con pista 2
        /*Si pusiéramos un punto azul en una celda vacía, superaríamos el número de visibles
        del número, y por tanto, debe ser una pared*/
        if(_locked && seeing < _mustWatch){
            for(Dirs dir : Dirs.values()) {
                int row = _pos.fst + _dirs[dir].fst;
                int col = _pos.snd + _dirs[dir].snd;
                while(row >= 0 && row < _boardSize && col >= 0 && col < _boardSize
                    && board[row][col]._state != State.Wall){
                    if(board[row][col]._state == State.Point)
                        continue;

                    board[row][col]._state = State.Point;
                    if(lookDirections(board) > _mustWatch)
                        return new Cell(false, -1, row, col, _boardSize, State.Wall);
                    else {
                        board[row][col]._state = State.Unassigned;
                    }
                    row += _dirs[dir].fst; col += _dirs[dir].snd;
                }
            }
        }

        //Probamos con pista 3
        /*Si no ponemos un punto en alguna celda vacía, entonces es imposible alcanzar el
        número*/
        if(_locked && seeing < _mustWatch){
            int[] freePositions = unassignedPositions(board);
            int numAvailableDirs = 0;
            Dirs availableDir = Dirs.Up;
            for(Dirs dir : Dirs.values()){
                if(freePositions[dir] >= _mustWatch - seeing){
                    numAvailableDirs++;
                    availableDir = dir;
                }
            }
            if(numAvailableDirs == 1){

            }
        }

        private int[] canWatchPositions(Cell[][] board){
            
        }

        //Si no he podido usar ninguna pista, devuelvo una cell con posicion invalida
        return new Cell (false, -1, -1, -1, _boardSize, State.Point);
    }



    private boolean _locked;
    private int _mustWatch;
    private Pair<Integer, Integer> _pos;
    private State _state;
    private int _boardSize;
}
