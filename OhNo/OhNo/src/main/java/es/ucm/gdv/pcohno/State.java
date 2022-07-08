package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;

public abstract class State {

    public State(Graphics graphics, Input input) {
        this._graphics = graphics;
        this._input = input;
        this._alphaTransition = 0;
        this._alphaTransitionSpeed = 10;
        this._inTransition = true;
    }

    abstract public void render();

    /**
     * Manages fade in/out animations when this states enters / exits
     * @param elapsedTime
     * @return
     */
    public OhNoApplication.State update(double elapsedTime) {
        _alphaTransition = Math.max(0f, Math.min(1f, _alphaTransition + (float)elapsedTime * _alphaTransitionSpeed));
        _inTransition = !(_alphaTransition <= 0f || _alphaTransition >= 1f);

        if(!_inTransition && _nextState != null)
            return _nextState;

        return null;
    }

    /**
     * Called when this states enters in action to initiliaze it if necesary
     * @param app OhNoApplication stores information needed from the previous state
     */
    public void init(OhNoApplication app) {

    }

    /**
     * Starts fade out transition
     * @param s
     */
    protected void setNextState(OhNoApplication.State s){
        _nextState = s;
        _alphaTransitionSpeed = -_alphaTransitionSpeed;
    }

    /**
     * Starts fade in transition
     * @param inTransition
     * @param alphaTransition
     */
    public void setInTransition(boolean inTransition, float alphaTransition) {
        this._inTransition = inTransition;
        this._alphaTransition = alphaTransition;
        _nextState = null;
        if(_alphaTransitionSpeed < 0)
            _alphaTransitionSpeed = -_alphaTransitionSpeed;
    }

    protected Graphics _graphics;
    protected Input _input;

    protected float _alphaTransition;
    protected float _alphaTransitionSpeed;

    protected boolean _inTransition;
    protected OhNoApplication.State _nextState;
}
