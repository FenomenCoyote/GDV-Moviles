package es.ucm.gdv.engine;

public interface Graphics {

    public void release();

    public Image newImage(String name);
    public MyFont newFont(String filename, int size, boolean isBold);

    //Para el canvas
    public void clear(int argb);
    public void translate(int x,int y);
    public void scale(float x,float y);
    public void save();
    public void restore();

    public void setLogicalSize(int width, int height);

    public void drawImage(Image image, int x, int y, float alpha);
    public void setColor(int argb);
    public void setFont(MyFont font);

    public void fillCircle(int cx, int cy, int r);
    public void drawCircle(int cx, int cy, int r, int strokeWidth);
    public void drawText(String text, int x, int y);

    int getWidth();
    int getHeight();
}
