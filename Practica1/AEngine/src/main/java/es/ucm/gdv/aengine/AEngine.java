package es.ucm.gdv.aengine;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;

public class AEngine implements Engine {
    @Override
    public void init() {

    }

    @Override
    public void run() {

    }

    @Override
    public void release() {

    }

    @Override
    public void setApplication(Application app) {
        _app = app;
    }

    @Override
    public Graphics getGraphics() {
        return _graphics;
    }

    @Override
    public Input getInput() {
        return _input;
    }

    private AGraphics _graphics = null;
    private AInput _input = null;
    private Application _app = null;
}
