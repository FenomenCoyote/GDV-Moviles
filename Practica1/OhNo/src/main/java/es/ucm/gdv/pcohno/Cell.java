package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;

public class Cell {

    public void render(Graphics graphics, double scale, double alpha) {
        int actualAlpha = (int)((alpha * this._alpha) * 255f);
        switch (_state){
            case Unassigned:
                graphics.setColor((actualAlpha << 24) | 0x00eeeeee);
                break;
            case Point:
                graphics.setColor((actualAlpha << 24) | 0x001cc0e0);
                break;
            case Wall:
                graphics.setColor((actualAlpha << 24) | 0x00ff384a);
                break;
        }
        graphics.fillCircle(0, 0, (int)(50 * scale));
        if(_mustWatch > 0){
            graphics.setColor((actualAlpha << 24) | 0x00ffffff);
            graphics.drawText(Integer.toString(_mustWatch), 0, 20);
        }

    }

    public void update(double elapsedTime){
        if(_fading){
            _alpha = Math.max(0f, Math.min(1f, _alpha + (float)elapsedTime * _fadeSpeed));
            if(_alpha <= 0f){
                _fadeSpeed = -_fadeSpeed;
                nextState();
            }
            else if(_alpha >= 1f){
                _fading = false;
                _fadeSpeed = -_fadeSpeed;
            }
        }
    }

    public enum State {Null, Unassigned, Point, Wall}

    public Cell(){
        this._locked = false;
        this._mustWatch = -1;
        this._state = State.Unassigned;

        this._alpha = 1;
        this._fadeSpeed = -10;
        this._fading = false;
    }

    public Cell(boolean locked, int mustWatch, State state){
        this._locked = locked;
        this._mustWatch = mustWatch;
        this._state = state;

        this._alpha = 1;
        this._fadeSpeed = -10;
        this._fading = false;
    }

    public Cell(Cell cell){
        this._locked = cell._locked;
        this._mustWatch = cell._mustWatch;
        this._state = cell._state;

        this._alpha = 1;
        this._fadeSpeed = -10;
        this._fading = false;
    }

    public void init(Cell cell) {
        this._locked = cell._locked;
        this._mustWatch = cell._mustWatch;
        this._state = cell._state;

        this._alpha = 1;
        this._fadeSpeed = -10;
        this._fading = false;
    }

    public boolean setState(State state) {
        if(!_locked){
            _state = state;
            return true;
        }
        return false;
    }

    private void nextState(){
        switch (_state) {
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

    public void nextStateTransition(){
        if(!_fading){
            _fading = true;
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

    private double _alpha;
    private double _fadeSpeed;
    private boolean _fading;
}
