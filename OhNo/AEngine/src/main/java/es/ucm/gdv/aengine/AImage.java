package es.ucm.gdv.aengine;

import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

import java.io.IOException;
import java.io.InputStream;

import es.ucm.gdv.engine.Image;

public class AImage implements Image {

    /**
     * Loads image from Assets/sprites/ folder
     * @param route
     * @param assetManager
     */
    public void loadImage(String route, AssetManager assetManager) {
        // load image
        try {
            InputStream is = assetManager.open("sprites/" + route);
            _image = BitmapFactory.decodeStream(is);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @Override
    public int getWidth() {
        return _image.getWidth();
    }

    @Override
    public int getHeight() {
        return _image.getHeight();
    }

    public Bitmap get_image(){
        return _image;
    }

    private Bitmap _image;
}
