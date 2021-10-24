package es.ucm.gdv.engine;

import java.awt.Color;

public interface Graphics {

    public Image newImage(String name);
    public MyFont newFont(String filename, int size, boolean isBold);
    public void clear(int color);
    public void translate(int x,int y);
    public void scale(float x,float y);
    public void save();
    public void restore();
    public void renderImage(Image image,String route);
    public void setColor(Color color);
    public void fillCircle(int cx, int cy, int r);
    public void drawText(String text, int x, int y);
    int getWidth();
    int getHeight();
}
