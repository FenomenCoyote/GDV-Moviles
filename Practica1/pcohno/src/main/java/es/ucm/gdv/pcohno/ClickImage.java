package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;

public class ClickImage extends Clickable {

    ClickImage(Image img, int x, int y, int w, int h) {
        super(x, y, w, h);
        image = img;
    }

    @Override
    public void render(Graphics graphics) {
        graphics.drawImage(image, this.x, this.y);
    }

    private Image image;
}