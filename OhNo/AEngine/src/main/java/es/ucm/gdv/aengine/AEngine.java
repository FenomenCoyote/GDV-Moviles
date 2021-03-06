package es.ucm.gdv.aengine;

import android.view.SurfaceView;

import androidx.appcompat.app.AppCompatActivity;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;

public class AEngine implements Engine, Runnable {
    /**
     * We create the SurfaceView, AGraphics and AInput
     * @param c class for android activities
     */
    public AEngine(AppCompatActivity c) {
        SurfaceView surfaceView = new SurfaceView(c);
        c.setContentView(surfaceView);
        _graphics = new AGraphics(surfaceView, c.getAssets());
        _input = new AInput(_graphics);
    }

    /**
     * We initialise the application
     */
    @Override
    public void init() {
        _app.init(this);
    }

    /**
     * Main loop
     */
    @Override
    public void run()
    {
        if (_renderThread != Thread.currentThread()) {
            // ¿¿Quién es el tuercebotas que está llamando al
            // run() directamente?? Programación defensiva
            // otra vez, con excepción, por merluzo.
            throw new RuntimeException("run() should not be called directly");
        }

        // Antes de saltar a la simulación, confirmamos que tenemos
        // un tamaño mayor que 0. Si la hebra se pone en marcha
        // muy rápido, la vista podría todavía no estar inicializada.
        while(_running && _graphics.getWidth() == 0)
            // Espera activa. Sería más elegante al menos dormir un poco.
            ;

        long lastFrameTime = System.nanoTime();

        // Bucle principal.
        while(_running) {

            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;

            _app.update(elapsedTime);

            _graphics.render(_app);
        }
    }

    /**
     * Not currently needed
     */
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

    /**
     * When Android unpauses our game
     */
    public void onResume()
    {
        if (!_running) {
            // Solo hacemos algo si no nos estábamos ejecutando ya
            // (programación defensiva, nunca se sabe quién va a
            // usarnos...)
            _running = true;
            // Lanzamos la ejecución de nuestro método run()
            // en una hebra nueva.
            _renderThread = new Thread(this);
            _renderThread.start();
        }
    }

    /**
     * When Android pauses our game
     */
    public void onPause()
    {
        if (_running) {
            _running = false;
            while (true) {
                try {
                    _renderThread.join();
                    _renderThread = null;
                    break;
                } catch (InterruptedException ie) {
                    // Esto no debería ocurrir nunca...
                }
            }
        }
    }

    private Thread _renderThread;
    private AGraphics _graphics = null;
    private AInput _input = null;
    private Application _app = null;
    private volatile boolean _running = false;


}
