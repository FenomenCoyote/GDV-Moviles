package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;

public class ClickCircle extends Clickable {

    /**
     * Constructor
     * @param r: radius of clickable area
     */
    ClickCircle(int r){
        super(0,0, r, r);
        this._r = r;
    }

    /**
     * Constructor
     * @param color: circle color
     * @param x x position on screen
     * @param y y position on screen
     * @param r radius of circle
     * @param number text for clickable circle
     */
    ClickCircle(int color, int x, int y, int r, String number) {
        super(x, y, r, r );
        this._color = color;
        this._number = number;
        this._r = r;
    }

    /**
     * Called every frame after update, to show itself on screen
     * @param graphics object for rendering
     */
    @Override
    public void render(Graphics graphics) {
        graphics.setColor(this._color);
        graphics.fillCircle(x, y, _r);
        graphics.setColor(0xffffffff);
        graphics.drawText(this._number, x, y + 16);
    }

    /**
     * Called every frame after update, to show itself on screen
     * @param graphics object for rendering
     * @param alpha alpha factor (0-1)
     */
    @Override
    public void render(Graphics graphics, float alpha) {
        int a = (int)(alpha * 255f) << 24;
        graphics.setColor(this._color | a);
        graphics.fillCircle(x, y, _r);
        graphics.setColor(a | 0x00ffffff);
        graphics.drawText(this._number, x, y + 16);
    }

    /**
     * Checks if given position is inside object's rect
     * @param x: x value for position
     * @param y: y value for position
     * @return true if yes, false either
     */
    @Override
    public boolean isOnMe(int x, int y){
        float a = (x - this.x);
        float b = (y - this.y);
        return Math.sqrt(a * a + b * b) < this._r;
    }

    private int _r;
    private int _color;
    private String _number;
}
