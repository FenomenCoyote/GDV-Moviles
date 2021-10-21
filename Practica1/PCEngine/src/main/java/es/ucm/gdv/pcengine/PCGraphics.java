package es.ucm.gdv.pcengine;

import es.ucm.gdv.engine.Graphics;

public class PCGraphics implements Graphics {

    // Obtenemos el Buffer Strategy que se supone que acaba de crearse.
    java.awt.image.BufferStrategy strategy = ventana.getBufferStrategy();

    // Pintamos el frame con el BufferStrategy
    do {
        do {
            Graphics graphics = strategy.getDrawGraphics();
            try {
                ventana.render(graphics);
            }
            finally {
                graphics.dispose();
            }
        } while(strategy.contentsRestored());
        strategy.show();
    } while(strategy.contentsLost());
}
