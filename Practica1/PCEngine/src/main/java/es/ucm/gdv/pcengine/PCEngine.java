package es.ucm.gdv.pcengine;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;

public class PCEngine implements Engine {
    @Override
    public void init() {
        Window ventana = new Window("OhNo!");
        if (!ventana.init(400, 600))
            // Ooops. Ha fallado la inicialización.
            return;
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

            _app.update(elapsedTime);

            // Informe de FPS
            if (currentTime - informePrevio > 1000000000l) {
                long fps = frames * 1000000000l / (currentTime - informePrevio);
                System.out.println("" + fps + " fps");
                frames = 0;
                informePrevio = currentTime;
            }
            ++frames;

			/*
			// Posibilidad: cedemos algo de tiempo. es una medida conflictiva...
			try {
				Thread.sleep(1);
			}
			catch(Exception e) {}
			*/
        } // while
    }

    @Override
    public void release() {
        //??
    }

    @Override
    public void setApplication(Application app) {
        _app = app;
    }

    PCGraphics _graphics = null;
    PCInput _input = null;
    Application _app = null;
}