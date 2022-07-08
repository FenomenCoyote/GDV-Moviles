package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;

public class Cell {

    public enum State {Null, Unassigned, Point, Wall}

    /**
     * Default constructor
     */
    public Cell(){
        this._locked = false;
        this._mustWatch = -1;
        this._state = State.Unassigned;

        this._alpha = 1;
        this._fadeSpeed = -10;
        this._fading = false;
    }

    /**
     * Constructor
     * @param locked: false is cell state can be changed in the future
     * @param mustWatch: points this cell should watch (only for locked points)
     * @param state: cell's initial state
     */
    public Cell(boolean locked, int mustWatch, State state){
        this._locked = locked;
        this._mustWatch = mustWatch;
        this._state = state;

        this._alpha = 1;
        this._fadeSpeed = -10;
        this._fading = false;
    }

    /**
     * Copy constructor
     * @param cell: cell to copy
     */
    public Cell(Cell cell){
        this._locked = cell._locked;
        this._mustWatch = cell._mustWatch;
        this._state = cell._state;

        this._alpha = 1;
        this._fadeSpeed = -10;
        this._fading = false;
    }

    /**
     * Initializes cell copying another one
     * @param cell: cell to copy
     */
    public void init(Cell cell) {
        this._locked = cell._locked;
        this._mustWatch = cell._mustWatch;
        this._state = cell._state;

        this._alpha = 1;
        this._fadeSpeed = -10;
        this._fading = false;
    }

    /**
     * Called every frame after update. The cell shows itself on screen
     * @param graphics: Object used for rendering
     * @param scale: scale for circle
     * @param alpha: alpha for circle
     */
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

    /**
     * Called every frame. Used to update logical representation
     * @param elapsedTime
     */
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

    /**
     * State setter
     * @param state new cell state
     * @return true if cell state has changed
     */
    public boolean setState(State state) {
        if(!_locked){
            _state = state;
            return true;
        }
        return false;
    }

    /**
     * Initiates cell transition to next state using an animation
     */
    public void nextStateTransition(){
        if(!_fading){
            _fading = true;
        }
    }

    /**
     * Returns cell's next state
     */
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

    /**
     * State getter
     * @return cell's state
     */
    public State getState() { return _state;  }

    /**
     * MustWatch getter
     * @return number of points cell must be watching in the solution,
     *  -1 if cell is a wall or a not locked point
     */
    public int getMustWatch(){ return _mustWatch; }

    /**
     * Locked property getter
     * @return boolean meaning if cell is locked, true means yes and false means no
     */
    public boolean getLocked(){ return _locked; }

    /**
     * Must watch setter (doesn't check if cell is a wall), used on board generation
     * @param mustWatch: number of points cell must watch in solution
     */
    public void setMustWatch(int mustWatch){ _mustWatch = mustWatch; }

    /**
     * Locked setter
     * @param locked: true if cell's state shouldn't be changed in the future, false either
     */
    public void setLocked(boolean locked){ _locked = locked; }

    private boolean _locked;
    private int _mustWatch;
    private State _state;

    private double _alpha;
    //Speed for fade-in-out animation
    private double _fadeSpeed;
    //True if fade-in-out animation is happening
    private boolean _fading;
}
