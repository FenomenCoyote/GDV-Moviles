package es.ucm.gdv.pcengine;

import java.awt.AlphaComposite;
import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics2D;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferStrategy;

import javax.swing.JFrame;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.MyFont;
import es.ucm.gdv.engine.MyGraphics;

public class PCGraphics extends MyGraphics {

    /**
     * Creates the window to render and the buffer strategy (2 buffers)
     * @param width
     * @param height
     * @return
     */
    public boolean init(int width, int height){
        _window = new JFrame("OhNo!");

        setLogicalSize(width, height);
        _window.setSize(width, height);

        _saveColor = Color.white;
        _saveFont = null;

        _window.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        _window.setIgnoreRepaint(true);
        _window.setVisible(true);

        // Intentamos crear el buffer strategy con 2 buffers.
        int intentos = 100;
        while(intentos-- > 0) {
            try {
                _window.createBufferStrategy(2);
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
        _strategy = _window.getBufferStrategy();

        return true;
    }

    /**
     * Not needed for now
     */
    @Override
    public void release() {

    }

    /**
     * Render in awtGraphics
     * @param app
     */
    public void render(Application app){
        // Pintamos el frame con el BufferStrategy
        _width = _window.getWidth();
        _height = _window.getHeight();

        do {
            do {
                _awtGraphics = (Graphics2D) _strategy.getDrawGraphics();
                try {
                    //Clear de toda la pantalla
                    Color previousColor = _awtGraphics.getColor();

                    setColor(0xffffffff);
                    _awtGraphics.fillRect(0, 0, _width, _height);

                    _awtGraphics.setColor(previousColor);

                    calculateTranslationScale();

                    app.render();
                }
                finally {
                    _awtGraphics.dispose();
                }
            } while(_strategy.contentsRestored());
            _strategy.show();
        } while(_strategy.contentsLost());
    }

    /**
     * Creates an Image from name and loads it
     * @param name
     * @return
     */
    @Override
    public Image newImage(String name) {
        PCImage img = new PCImage();
        img.loadImage(name);
        return img;
    }

    /**
     * Creates a MyFont from filename and loads it
     * @param filename
     * @param size
     * @param isBold
     * @return
     */
    @Override
    public MyFont newFont(String filename, int size, boolean isBold) {
        PCFont font = new PCFont();
        font.loadFont(filename, size, isBold);
        return font;
    }

    /**
     * Clear the window (not the white bands)
     * @param argb
     */
    @Override
    public void clear(int argb) {
        setColor(argb);
        _awtGraphics.fillRect(0, 0, _logicalWidth, _logicalHeight);
    }

    /**
     * Canvas operation
     * @param x
     * @param y
     */
    @Override
    public void translate(int x, int y) {
        _awtGraphics.translate(x, y);
    }

    /**
     * Canvas operation
     * @param x
     * @param y
     */
    @Override
    public void scale(float x, float y) {
        _awtGraphics.scale(x, y);
    }

    /**
     * Saves current canvas state
     */
    @Override
    public void save() {
        _saveColor = _awtGraphics.getColor();
        _saveFont = _awtGraphics.getFont();

        //awtGraphics.getTransform().getMatrix(matrix);
        _tr = _awtGraphics.getTransform();
    }

    /**
     * Restores previous canvas state
     */
    @Override
    public void restore() {
        _awtGraphics.setColor(_saveColor);
        _awtGraphics.setFont(_saveFont);

        _awtGraphics.setTransform(_tr);
        //awtGraphics.getTransform().setTransform(matrix[0], matrix[1],matrix[2],matrix[3],matrix[4],matrix[5]);
    }

    /**
     * Sets color to draw with
     * @param argb
     */
    @Override
    public void setColor(int argb) {
        Color c = new Color(argb, true);
        _awtGraphics.setColor(c);
    }

    /**
     * Sets font to draw text with
     * @param font
     */
    @Override
    public void setFont(MyFont font) {
        Font f = ((PCFont)font).getFont();
        _awtGraphics.setFont(f);
    }

    /**
     * Fills entire circle
     * @param cx
     * @param cy
     * @param r
     */
    @Override
    public void fillCircle(int cx, int cy, int r) {
        _awtGraphics.fillOval(cx - r, cy - r, r * 2, r * 2);
    }

    /**
     * Draws circumference
     * @param cx
     * @param cy
     * @param r
     * @param strokeWidth
     */
    @Override
    public void drawCircle(int cx, int cy, int r, int strokeWidth) {
        _awtGraphics.setStroke(new BasicStroke(strokeWidth));
        _awtGraphics.drawOval(cx - r, cy - r, r * 2, r * 2);
        _awtGraphics.setStroke(new BasicStroke(1));
    }

    /**
     * Draws image with alpha
     * @param image
     * @param x
     * @param y
     * @param alpha
     */
    @Override
    public void drawImage(Image image, int x, int y, float alpha) {
        AlphaComposite ac = AlphaComposite.getInstance(AlphaComposite.SRC_OVER, alpha);
        Graphics2D g2d = (Graphics2D)this._awtGraphics;
        g2d.setComposite(ac);
        _awtGraphics.drawImage(((PCImage)image).getSprite(), x, y, null);
        ac = AlphaComposite.getInstance(AlphaComposite.SRC_OVER, 1.0f);
        g2d.setComposite(ac);
    }

    /**
     * Draws text using previous loaded font
     * @param text
     * @param x
     * @param y
     */
    @Override
    public void drawText(String text, int x, int y) {
        int w = _awtGraphics.getFontMetrics(_awtGraphics.getFont()).stringWidth(text);
        _awtGraphics.drawString(text, x - w/2, y);
    }

    public int getWidth() {
        return _width;
    }

    public int getHeight() {
        return _height;
    }

    public JFrame getWindow() {
        return _window;
    }

    private Color _saveColor;
    private Font _saveFont;

    private AffineTransform _tr;

    private java.awt.Graphics2D _awtGraphics;

    private JFrame _window;
    private BufferStrategy _strategy;
}

