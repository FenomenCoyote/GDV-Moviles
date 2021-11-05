package es.ucm.gdv.pcengine;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics2D;
import java.awt.geom.AffineTransform;
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

        saveColor = Color.white;
        saveFont = null;

        matrix = new double[6];

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
                //try {
                    awtGraphics = (Graphics2D)strategy.getDrawGraphics();
                //}
                //catch (Exception e){
                    //awtGraphics = (Graphics2D)strategy.getDrawGraphics();
                //}
                try {
                    //Clear de toda la pantalla
                    Color previousColor = awtGraphics.getColor();
                    setColor(0xffffffff);
                    awtGraphics.fillRect(0, 0, width, height);
                    awtGraphics.setColor(previousColor);

                    calculateTranslationScale();

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
        awtGraphics.fillRect(0, 0, logicalWidth, logicalHeight);
    }

    @Override
    public void translate(int x, int y) {
        awtGraphics.translate(x, y);
    }

    @Override
    public void scale(float x, float y) {
        awtGraphics.scale(x, y);
    }

    @Override
    public void save() {
        saveColor = awtGraphics.getColor();
        saveFont = awtGraphics.getFont();

        //awtGraphics.getTransform().getMatrix(matrix);
        tr = awtGraphics.getTransform();
    }

    @Override
    public void restore() {
        awtGraphics.setColor(saveColor);
        awtGraphics.setFont(saveFont);

        awtGraphics.setTransform(tr);
        //awtGraphics.getTransform().setTransform(matrix[0], matrix[1],matrix[2],matrix[3],matrix[4],matrix[5]);
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
        Font f = ((PCFont)font).getFont();
        awtGraphics.setFont(f);
    }

    @Override
    public void fillCircle(int cx, int cy, int r) {
        awtGraphics.fillOval(cx - r, cy - r, r * 2, r * 2);
    }

    @Override
    public void drawImage(Image image, int x, int y) {
        awtGraphics.drawImage(((PCImage)image).getSprite(), x, y, null);
    }

    @Override
    public void drawText(String text, int x, int y) {
        int w = awtGraphics.getFontMetrics(awtGraphics.getFont()).stringWidth(text);
        awtGraphics.drawString(text, x - w/2, y);
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
        double heightRelation = (double)height/logicalHeight;

        if(logicalWidth * heightRelation > width){ //Si el width es muy pequeño para eso, padding arriba y abajo
            scale = (double)width/logicalWidth;
            offsetX = 0;
            offsetY = (height-(int)(logicalHeight*scale))/2;
        }
        else { //Si el width es grande padding izquierda y derecha
            scale = heightRelation;
            offsetY = 0;
            offsetX = (width-(int)(logicalWidth*scale))/2;
        }
        awtGraphics.translate(offsetX, offsetY);
        awtGraphics.scale(scale, scale);
    }

    public int getOffsetX() {
        return offsetX;
    }

    public int getOffsetY() {
        return offsetY;
    }

    public double getScale() {
        return scale;
    }

    public JFrame getWindow() {
        return window;
    }

    private int logicalWidth, logicalHeight;
    private int width, height;

    private int offsetX, offsetY;
    double scale;

    Color saveColor;
    Font saveFont;

    double[] matrix;
    AffineTransform tr;

    private java.awt.Graphics2D awtGraphics;

    private JFrame window;
    private BufferStrategy strategy;
}

