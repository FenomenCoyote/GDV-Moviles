package es.ucm.gdv.aengine;


import android.content.res.AssetManager;
import android.graphics.Typeface;

import es.ucm.gdv.engine.MyFont;

public class AFont implements MyFont {


    public void loadFont(String filename, int size, boolean isBold, AssetManager assets) {
        this.size = size;
        this.isBold = isBold;
        font = Typeface.createFromAsset(assets, "fonts/" + filename);
    }

    public Typeface getFont() {
        return font;
    }

    public int getSize() {
        return size;
    }

    public boolean isBold() {
        return isBold;
    }

    private int size;
    private boolean isBold;
    private Typeface font;
}
