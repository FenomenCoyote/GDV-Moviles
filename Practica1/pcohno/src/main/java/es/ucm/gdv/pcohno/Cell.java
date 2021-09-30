package es.ucm.gdv.pcohno;

import com.sun.tools.javac.util.Pair;

public class Cell {

    private enum State {Unassigned, Point, Wall}
    private enum Dirs {Up, Down, Left, Right}

    public Cell(int posX, int posY){
        this._locked = false;
        this._mustWatch = -1;
        this._pos = new Pair<Integer, Integer>(posX, posY);
        this._state = State.Unassigned;
    }

    public Cell(boolean locked, int mustWatch, int posX, int posY, State state){
        this._locked = locked;
        this._mustWatch = mustWatch;
        this._pos = new Pair<Integer, Integer>(posX, posY);
        this._state = state;
    }

    public boolean setState(State state) {
        if(!_locked){
            _state = state;
            return true;
        }
        return false;
    }

    public void setMustWatch(int mustWatch){ _mustWatch = mustWatch; }

    public void setLocked(boolean locked){ _locked = locked; }

    public boolean isPoint(){
        return _state == State.Point;
    }

   /** devuelve un array de 4 con el numero de puntos que ve en cada direccion :)
    * public int[] getView() {

    }
    */

    public boolean isRight(Cell[][] board, int size){
        if(_state == State.Unassigned)
            return false;
        else if(_state == State.Wall)
            return true;

        //Look for mistakes
        int seeing = 0;

        //Look up
        int row = _pos.fst;
        while(seeing <= _mustWatch && row >= 0){
            if(board[row][_pos.snd].isPoint()) {
                ++seeing;
                --row;
            } else break;
        }

        //Look down
        row = _pos.fst;
        while(seeing <= _mustWatch && row < size){
            if(board[row][_pos.snd].isPoint()){
                ++seeing;
                ++row;
            } else break;
        }

        //Look left
        int col = _pos.snd;
        while(seeing <= _mustWatch && col >= 0){
            if(board[_pos.fst][col].isPoint()){
                ++seeing;
                --col;
            } else break;
        }

        //Look right
        col = _pos.snd;
        while(seeing <= _mustWatch && col  < size){
            if(board[_pos.fst][col].isPoint()){
                ++seeing;
                ++col;
            } else break;
        }

        return seeing == _mustWatch;
    }

    private boolean _locked;
    private int _mustWatch;
    private Pair<Integer, Integer> _pos;
    private State _state;
}
