package es.ucm.gdv.pcengine;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;

public class PCEngine implements Engine {
    @Override
    public void init() {

        _graphics = new PCGraphics();
        _graphics.init(400, 600);

        _input = new PCInput(_graphics);

        //_input.init();
        _app.init(this);
    }

    @Override
    public void run() {

        // Vamos allá.
        long lastFrameTime = System.nanoTime();

        long informePrevio = lastFrameTime; // Informes de FPS
        int frames = 0;

        // Bucle principal
        while(true) {
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;

            _app.update();
            _graphics.render(_app);
        } // while
    }

    @Override
    public void release() {
        //??
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

    PCGraphics _graphics = null;
    PCInput _input = null;
    Application _app = null;
    MouseListener _mouseListener = null;
}