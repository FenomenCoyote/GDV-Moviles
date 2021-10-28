package es.ucm.gdv.aengine;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.MyFont;

public class AGraphics implements Graphics {
    @Override
    public boolean init(int width, int height) {
        return false;
    }

    @Override
    public void release() {

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
    public void clear(int color) {

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

    }

    @Override
    public void drawImage(Image image, String route) {

    }

    @Override
    public void setColor(int rgba) {

    }

    @Override
    public void fillCircle(int cx, int cy, int r) {

    }

    @Override
    public void drawText(String text, int x, int y) {

    }

    @Override
    public int getWidth() {
        return 0;
    }

    @Override
    public int getHeight() {
        return 0;
    }

}
