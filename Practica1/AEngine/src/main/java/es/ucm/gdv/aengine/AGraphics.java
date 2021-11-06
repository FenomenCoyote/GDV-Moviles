package es.ucm.gdv.aengine;

import android.content.Context;
import android.graphics.Canvas;
import android.view.SurfaceHolder;
import android.view.SurfaceView;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.MyFont;

public class AGraphics extends SurfaceView implements Graphics  {



    public AGraphics(Context context) {
        super(context);
        _holder=getHolder();
    }

    @Override
    public boolean init(int width, int height) {

        return false;
    }

    public void render(Application a)
    {
        while (!_holder.getSurface().isValid());

        Canvas canvas = _holder.lockCanvas();
        a.render();
        _holder.unlockCanvasAndPost(canvas);

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
    public void drawImage(Image image, int x, int y) {

    }

    @Override
    public void setColor(int rgba) {

    }

    @Override
    public void setFont(MyFont font) {

    }

    @Override
    public void fillCircle(int cx, int cy, int r) {

    }

    @Override
    public void drawCircle(int cx, int cy, int r) {

    }

    @Override
    public void drawText(String text, int x, int y) {

    }

    private SurfaceHolder _holder;
}
