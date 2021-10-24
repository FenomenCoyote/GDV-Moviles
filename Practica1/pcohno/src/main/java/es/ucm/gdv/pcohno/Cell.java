package es.ucm.gdv.pcohno;

public class Cell {

    public enum State {Null, Unassigned, Point, Wall}

    public Cell(){
        this._locked = false;
        this._mustWatch = -1;
        this._state = State.Unassigned;
    }

    public Cell(boolean locked, int mustWatch, State state){
        this._locked = locked;
        this._mustWatch = mustWatch;
        this._state = state;
    }

    public Cell(Cell cell){
        this._locked = cell._locked;
        this._mustWatch = cell._mustWatch;
        this._state = cell._state;
    }

    public boolean setState(State state) {
        if(!_locked){
            _state = state;
            return true;
        }
        return false;
    }

    public State getState() { return _state;  }
    public int getMustWatch(){ return _mustWatch; }
    public boolean getLocked(){ return _locked; }

    public void setMustWatch(int mustWatch){ _mustWatch = mustWatch; }
    public void setLocked(boolean locked){ _locked = locked; }

    private boolean _locked;
    private int _mustWatch;
    private State _state;
}
