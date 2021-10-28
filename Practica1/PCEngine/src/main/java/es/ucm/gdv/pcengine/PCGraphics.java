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

        setLogicalSize(width, height);
        window.setSize(width, height);
        scale = 1;
        offsetX = offsetY = 0;
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

        calculateTranslationScale();

        do {
            do {
                awtGraphics = strategy.getDrawGraphics();
                try {
                    //TODO clears donde haya offset
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
        PCImage img = new PCImage();
        img.loadImage(name);
        return img;
    }

    @Override
    public MyFont newFont(String filename, int size, boolean isBold) {
        PCFont font = new PCFont();
        font.loadFont(filename, size, isBold);
        return font;
    }

    @Override
    public void clear(int argb) {
        setColor(argb);
        awtGraphics.fillRect(0, 0, width, height);
    }

    @Override
    public void translate(int x, int y) {
        awtGraphics.translate(x, y);
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
    public void setColor(int argb) {
        Color c = new Color(argb, true);
        awtGraphics.setColor(c);
    }

    @Override
    public void setFont(MyFont font) {
        awtGraphics.setFont(((PCFont)font).getFont());
    }

    @Override
    public void fillCircle(int cx, int cy, int r) {
        //TODO: Convertir x e y a realPosition
        awtGraphics.fillOval(cx, cy, r, r);
    }

    @Override
    public void drawImage(Image image, int x, int y) {
        //TODO: Convertir x e y a realPosition
        awtGraphics.drawImage(((PCImage)image).getSprite(), x, y, null);
    }

    @Override
    public void drawText(String text, int x, int y) {
        //TODO: Convertir x e y a realPosition
        awtGraphics.drawString(text, x, y);
    }

    @Override
    public int getWidth() {
        return width;
    }

    @Override
    public int getHeight() {
        return height;
    }

    private void calculateTranslationScale(){
        //Ajustar el alto para que sea exacto al height
        float heightRelation = (float)height/logicalHeight;


        if(logicalWidth * heightRelation > width){ //Si el width es muy pequeño para eso, padding arriba y abajo
            offsetX = 0;
            //TODO offsetY = algo;
            scale = heightRelation;
        }
        else { //Si el width es grande padding izquierda y derecha
            offsetY = 0;
            //TODO offsetX = algoDiferente;
            scale = (float)width/logicalWidth;
        }


    }

    private int logicalWidth, logicalHeight;
    private int width, height;
    float scale;
    int offsetX, offsetY;

    private java.awt.Graphics awtGraphics;
    private JFrame window;
    private BufferStrategy strategy;
}
