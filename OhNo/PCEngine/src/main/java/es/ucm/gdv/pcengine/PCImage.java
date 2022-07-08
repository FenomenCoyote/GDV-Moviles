package es.ucm.gdv.pcengine;

import java.io.File;
import java.io.IOException;

import es.ucm.gdv.engine.Image;

public class PCImage implements Image {

    public PCImage() {
        _sprite = null;
    }

    /**
     * Loads image from $rootDir/data/sprites/
     * @param route
     */
    public void loadImage(String route) {
        try {
            _sprite = javax.imageio.ImageIO.read(new File("data/sprites/" + route));
        } catch (IOException e) {
            System.out.println("No se pudo cargar la imagen: " + route);
        }
    }

    @Override
    public int getWidth() {
        return _sprite.getWidth(null);
    }

    @Override
    public int getHeight() {
        return _sprite.getHeight(null);
    }

    public java.awt.Image getSprite(){
        return _sprite;
    }

    private java.awt.Image _sprite;
}
