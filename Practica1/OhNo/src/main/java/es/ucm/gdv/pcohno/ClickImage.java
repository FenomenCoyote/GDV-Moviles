package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;

public class ClickImage extends Clickable {

    ClickImage(Image img, int x, int y, int w, int h, float alpha) {
        super(x, y, w, h);
        image = img;
        this.alpha = alpha;
    }

    @Override
    public void render(Graphics graphics) {
        graphics.drawImage(image, this.x, this.y, this.alpha);
    }

    @Override
    public void render(Graphics graphics, float alpha) {
        graphics.drawImage(image, this.x, this.y, alpha);
    }

    private Image image;
    float alpha;
}
