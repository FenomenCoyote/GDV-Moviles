package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;

public abstract class State {

    public State(Graphics graphics, Input input) {
        this.graphics = graphics;
        this.input = input;
    }

    abstract public void render();
    abstract public OhNoApplication.State update();
    abstract public void init();

    protected Graphics graphics;
    protected Input input;
}
