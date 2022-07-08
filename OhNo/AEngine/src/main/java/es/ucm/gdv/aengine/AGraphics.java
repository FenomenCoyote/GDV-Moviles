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


    /**
     * Render the game
     * @param a
     */
    public void render(Application a)
    {
        while (!_holder.getSurface().isValid());

        _canvas = _holder.lockCanvas();

        _width = _canvas.getWidth();
        _height = _canvas.getHeight();
        clear(0xffffffff);

        calculateTranslationScale();

        a.render();
        _holder.unlockCanvasAndPost(_canvas);
    }

    /**
     * Not currently needed
     */
    @Override
    public void release() {

    }

    /**
     * Creates an Image from name and loads it
     * @param name
     * @return
     */
    @Override
    public Image newImage(String name) {
        AImage img = new AImage();
        img.loadImage(name, _assets);
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
        AFont f = new AFont();
        f.loadFont(filename, size, isBold, _assets);
        return f;
    }


    /**
     * Clear the window (not the white bands)
     * @param color
     */
    @Override
    public void clear(int color) {
        _canvas.drawColor(color);
    }

    /**
     * Canvas operation
     * @param x
     * @param y
     */
    @Override
    public void translate(int x, int y) {
        _canvas.translate(x, y);
    }

    /**
     * Canvas operation
     * @param x
     * @param y
     */
    @Override
    public void scale(float x, float y) {
        _canvas.scale(x, y);
    }

    /**
     * Saves current canvas state
     */
    @Override
    public void save() {
        _canvas.save();
    }

    /**
     * Restores previous canvas state
     */
    @Override
    public void restore() {
        _canvas.restore();
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
        Bitmap img = ((AImage)image).get_image();
        int a = (int)(alpha*255);
        _paint.setAlpha(a);
        _canvas.drawBitmap(img, x, y, _paint);
        _paint.setAlpha(255);
    }

    /**
     * Sets color to draw with
     * @param argb
     */
    @Override
    public void setColor(int argb) {
        _paint.setColor(argb);
    }

    /**
     * Sets font to draw text with
     * @param font
     */
    @Override
    public void setFont(MyFont font) {
        this._font = (AFont)font;
        _paint.setTypeface(_font.get_font());
    }

    /**
     * Fills entire circle
     * @param cx
     * @param cy
     * @param r
     */
    @Override
    public void fillCircle(int cx, int cy, int r) {
        _canvas.drawCircle(cx, cy, r, _paint);
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
        _paint.setStrokeWidth(strokeWidth);
        _paint.setStyle(Paint.Style.STROKE);
        _canvas.drawCircle(cx, cy, r, _paint);
        _paint.setStyle(Paint.Style.FILL);
        _paint.setStrokeWidth(1);
    }

    /**
     * Draws text using previous loaded font
     * @param text
     * @param x
     * @param y
     */
    @Override
    public void drawText(String text, int x, int y) {
        _paint.setTextSize(_font.get_size());
        _paint.setFakeBoldText(_font.is_isBold());
        _canvas.drawText(text, x, y, _paint);
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

    private Canvas _canvas;
    private SurfaceHolder _holder;
    private SurfaceView _view;
    private AssetManager _assets;
    private Paint _paint;
}
