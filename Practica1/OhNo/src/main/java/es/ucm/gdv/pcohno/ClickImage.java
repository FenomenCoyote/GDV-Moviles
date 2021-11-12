package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;

public class ClickImage extends Clickable {

    /**
     * Constructor
     * @param img image to show
     * @param x x position on screen
     * @param y y position on screen
     * @param w width on screen
     * @param h height on screen
     * @param alpha alpha factor (0-1)
     */
    ClickImage(Image img, int x, int y, int w, int h, float alpha) {
        super(x, y, w, h);
        _image = img;
        this._alpha = alpha;
    }

    /**
     * Called every frame after update
     * @param graphics object for rendering
     */
    @Override
    public void render(Graphics graphics) {
        graphics.drawImage(_image, this.x, this.y, this._alpha);
    }

    /**
     * Called every frame after update
     * @param graphics object for rendering
     * @param alpha alpha factor (0-1)
     */
    @Override
    public void render(Graphics graphics, float alpha) {
        graphics.drawImage(_image, this.x, this.y, alpha);
    }

    private Image _image;
    private float _alpha;

}
