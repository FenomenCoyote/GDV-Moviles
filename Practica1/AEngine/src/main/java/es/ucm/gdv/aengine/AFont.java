package es.ucm.gdv.aengine;


import android.content.res.AssetManager;
import android.graphics.Typeface;

import es.ucm.gdv.engine.MyFont;

public class AFont implements MyFont {

    /**
     * Loads font from assets/fonts/ folder
     * @param filename
     * @param size
     * @param isBold
     * @param assets
     */
    public void loadFont(String filename, int size, boolean isBold, AssetManager assets) {
        this._size = size;
        this._isBold = isBold;
        _font = Typeface.createFromAsset(assets, "fonts/" + filename);
    }

    public Typeface get_font() {
        return _font;
    }

    public int get_size() {
        return _size;
    }

    public boolean is_isBold() {
        return _isBold;
    }

    private int _size;
    private boolean _isBold;
    private Typeface _font;
}
