package es.ucm.gdv.pcohno;

import com.sun.tools.javac.util.Pair;

import java.util.Stack;

import es.ucm.gdv.engine.Graphics;

public class Cell {

    public void render(Graphics graphics, double scale) {
        switch (_state){
            case Unassigned:
                graphics.setColor(0xffeeeeee);
                break;
            case Point:
                graphics.setColor(0xff1cc0e0);
                break;
            case Wall:
                graphics.setColor(0xffff384a);
                break;
        }
        graphics.fillCircle(0, 0, (int)(50 * scale));
        if(_mustWatch > 0){
            graphics.setColor(0xffffffff);
            graphics.drawText(Integer.toString(_mustWatch), 0, 20);
        }

    }

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

    public void nextState(){
        switch (_state){
            case Unassigned:
                _state = State.Point;
                break;
            case Point:
                _state = State.Wall;
                break;
            case Wall:
                _state = State.Unassigned;
                break;
        }
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
