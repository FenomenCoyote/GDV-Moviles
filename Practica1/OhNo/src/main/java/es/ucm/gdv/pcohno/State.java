package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;

public abstract class State {

    public State(Graphics graphics, Input input) {
        this.graphics = graphics;
        this.input = input;
        this.alphaTransition = 0;
        this.alphaTransitionSpeed = 10;
        this.inTransition = true;
    }

    abstract public void render();

    public OhNoApplication.State update(double elapsedTime) {
        alphaTransition = Math.max(0f, Math.min(1f, alphaTransition + (float)elapsedTime * alphaTransitionSpeed));
        inTransition = !(alphaTransition <= 0f || alphaTransition >= 1f);

        if(!inTransition && nextState != null)
            return nextState;

        return null;
    }

    abstract public void init(OhNoApplication app);

    protected void setNextState(OhNoApplication.State s){
        nextState = s;
        alphaTransitionSpeed = -alphaTransitionSpeed;
    }

    public void setInTransition(boolean inTransition, float alphaTransition) {
        this.inTransition = inTransition;
        this.alphaTransition = alphaTransition;
        nextState = null;
        if(alphaTransitionSpeed < 0)
            alphaTransitionSpeed = -alphaTransitionSpeed;
    }

    protected Graphics graphics;
    protected Input input;

    protected float alphaTransition;
    protected float alphaTransitionSpeed;

    protected boolean inTransition;
    protected OhNoApplication.State nextState;
}
