package es.ucm.gdv.aengine;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;

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

    private AGraphics _graphics = null;
    private AInput _input = null;
    private Application _app = null;
}
