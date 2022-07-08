package es.ucm.gdv.pcengine;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;

public class PCEngine implements Engine {

    /**
     * creates graphics and input and initializes app
     */
    @Override
    public void init() {

        _graphics = new PCGraphics();
        _graphics.init(400, 600);
        _input = new PCInput(_graphics);

        _app.init(this);
    }

    /**
     * Main loop
     */
    @Override
    public void run() {

        long lastFrameTime = System.nanoTime();

        // Bucle principal
        while(true) {
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;

            _app.update(elapsedTime);
            _graphics.render(_app);
        } // while
    }

    /**
     * Should be called after the 'run' method has ended
     * Tells the app to release in case it needs to do it
     */
    @Override
    public void release() {
        _app.release();
        _graphics.release();
        //_input.release();
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


    private PCGraphics _graphics = null;
    private PCInput _input = null;
    private Application _app = null;
}