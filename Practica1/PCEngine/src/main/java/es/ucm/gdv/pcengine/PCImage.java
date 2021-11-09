package es.ucm.gdv.pcengine;

import java.io.File;
import java.io.IOException;

import es.ucm.gdv.engine.Image;

public class PCImage implements Image {

    public PCImage() {
        sprite = null;
    }

    /**
     * Loads image from $rootDir/data/sprites/
     * @param route
     */
    public void loadImage(String route) {
        try {
            sprite = javax.imageio.ImageIO.read(new File("data/sprites/" + route));
        } catch (IOException e) {
            System.out.println("No se pudo cargar la imagen: " + route);
        }
    }

    @Override
    public int getWidth() {
        return sprite.getWidth(null);
    }

    @Override
    public int getHeight() {
        return sprite.getHeight(null);
    }

    public java.awt.Image getSprite(){
        return sprite;
    }

    java.awt.Image sprite;
}
