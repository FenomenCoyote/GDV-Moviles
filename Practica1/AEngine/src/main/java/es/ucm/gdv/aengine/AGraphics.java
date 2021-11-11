package es.ucm.gdv.aengine;

import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.view.SurfaceHolder;
import android.view.SurfaceView;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.MyFont;
import es.ucm.gdv.engine.MyGraphics;

public class AGraphics extends MyGraphics {

    /**
     * Gets holder and creates Paint object
     * @param view
     * @param assets
     */
    public AGraphics(SurfaceView view, AssetManager assets) {
        this._holder = view.getHolder();
        this._view = view;
        this._assets = assets;
        this._font = null;
        this._paint = new Paint();
        _paint.setTextAlign(Paint.Align.CENTER);
    }


    public void render(Application a)
    {
        while (!_holder.getSurface().isValid())
            ;

        canvas = _holder.lockCanvas();

        _width = canvas.getWidth();
        _height = canvas.getHeight();
        clear(0xffffffff);

        calculateTranslationScale();

        a.render();
        _holder.unlockCanvasAndPost(canvas);
    }

    /**
     * Not currently needed
     */
    @Override
    public void release() {

    }

    @Override
    public Image newImage(String name) {
        AImage img = new AImage();
        img.loadImage(name, _assets);
        return img;
    }

    @Override
    public MyFont newFont(String filename, int size, boolean isBold) {
        AFont f = new AFont();
        f.loadFont(filename, size, isBold, _assets);
        return f;
    }

    @Override
    public void clear(int color) {
        canvas.drawColor(color);
    }

    @Override
    public void translate(int x, int y) {
        canvas.translate(x, y);
    }

    @Override
    public void scale(float x, float y) {
        canvas.scale(x, y);
    }

    @Override
    public void save() {
        canvas.save();
    }

    @Override
    public void restore() {
        canvas.restore();
    }

    @Override
    public void drawImage(Image image, int x, int y, float alpha) {
        Bitmap img = ((AImage)image).getImage();
        int a = (int)(alpha*255);
        _paint.setAlpha(a);
        canvas.drawBitmap(img, x, y, _paint);
        _paint.setAlpha(255);
    }

    @Override
    public void setColor(int argb) {
        _paint.setColor(argb);
    }

    @Override
    public void setFont(MyFont font) {
        this._font = (AFont)font;
        _paint.setTypeface(_font.getFont());
    }

    @Override
    public void fillCircle(int cx, int cy, int r) {
        canvas.drawCircle(cx, cy, r, _paint);
    }

    @Override
    public void drawCircle(int cx, int cy, int r, int strokeWidth) {
        _paint.setStrokeWidth(strokeWidth);
        _paint.setStyle(Paint.Style.STROKE);
        canvas.drawCircle(cx, cy, r, _paint);
        _paint.setStyle(Paint.Style.FILL);
        _paint.setStrokeWidth(1);
    }

    @Override
    public void drawText(String text, int x, int y) {
        _paint.setTextSize(_font.getSize());
        _paint.setFakeBoldText(_font.isBold());
        canvas.drawText(text, x, y, _paint);
    }

    public int getWidth() {
        return _view.getWidth();
    }

    public int getHeight() {
        return _view.getHeight();
    }

    public SurfaceView getSurfaceView()
    {
        return _view;
    }

    private AFont _font;

    private Canvas canvas;
    private SurfaceHolder _holder;
    private SurfaceView _view;
    private AssetManager _assets;
    private Paint _paint;
}
