package es.ucm.gdv.pcengine;

import java.awt.Color;
import java.awt.image.BufferStrategy;

import javax.swing.JFrame;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.MyFont;

public class PCGraphics implements Graphics {

    public boolean init(int width, int height){
        window = new JFrame("OhNo!");

        window.setSize(width, height);
        window.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        window.setIgnoreRepaint(true);
        window.setVisible(true);

        // Intentamos crear el buffer strategy con 2 buffers.
        int intentos = 100;
        while(intentos-- > 0) {
            try {
                window.createBufferStrategy(2);
                break;
            }
            catch(Exception e) {
            }
        } // while pidiendo la creación de la buffeStrategy
        if (intentos == 0) {
            System.err.println("No pude crear la BufferStrategy");
            return false;
        }
        else {
            // En "modo debug" podríamos querer escribir esto.
            //System.out.println("BufferStrategy tras " + (100 - intentos) + " intentos.");
        }

        // Obtenemos el Buffer Strategy que se supone que acaba de crearse.
        strategy = window.getBufferStrategy();

        return true;
    }

    @Override
    public void release() {

    }

    public void render(Application app){
        // Pintamos el frame con el BufferStrategy
        width = window.getWidth();
        height = window.getHeight();
        do {
            do {
                awtGraphics = strategy.getDrawGraphics();
                try {
                    app.render();
                }
                finally {
                    awtGraphics.dispose();
                }
            } while(strategy.contentsRestored());
            strategy.show();
        } while(strategy.contentsLost());
    }

    @Override
    public Image newImage(String name) {
        return null;
    }

    @Override
    public MyFont newFont(String filename, int size, boolean isBold) {
        return null;
    }

    @Override
    public void clear(int argb) {
        setColor(argb);
        awtGraphics.fillRect(0, 0, width, height);
    }

    @Override
    public void translate(int x, int y) {

    }

    @Override
    public void scale(float x, float y) {

    }

    @Override
    public void save() {

    }

    @Override
    public void restore() {

    }

    @Override
    public void setLogicalSize(int width, int height) {
        logicalWidth = width;
        logicalHeight = height;
    }

    @Override
    public void drawImage(Image image, String route) {

    }

    @Override
    public void setColor(int argb) {
        Color c = new Color(argb, true);
        awtGraphics.setColor(c);
    }

    @Override
    public void fillCircle(int cx, int cy, int r) {
        awtGraphics.fillOval(cx, cy, r, r);
    }

    @Override
    public void drawText(String text, int x, int y) {

    }

    @Override
    public int getWidth() {
        return width;
    }

    @Override
    public int getHeight() {
        return height;
    }

    private int logicalWidth, logicalHeight;
    private int width, height;

    private java.awt.Graphics awtGraphics;
    private JFrame window;
    private BufferStrategy strategy;
}
