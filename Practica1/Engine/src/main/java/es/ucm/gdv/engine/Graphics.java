package es.ucm.gdv.engine;

import java.awt.Color;

public interface Graphics {

    public boolean init(int width, int height);
    public void release();

    public Image newImage(String name);
    public MyFont newFont(String filename, int size, boolean isBold);

    //Para el canvas
    public void clear(int color);
    public void translate(int x,int y);
    public void scale(float x,float y);
    public void save();
    public void restore();

    public void setLogicalSize(int width, int height);

    public void drawImage(Image image,String route);
    public void setColor(int r, int g, int b, int a);
    public void fillCircle(int cx, int cy, int r);
    public void drawText(String text, int x, int y);
    int getWidth();
    int getHeight();
}
