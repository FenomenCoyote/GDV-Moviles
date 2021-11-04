package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;

public class ClickCircle extends Clickable {

    ClickCircle(int color, int x, int y, int r, String number) {
        super(x - r, y - r, r * 2, r * 2);
        this.color = color;
        this.number = number;
        this.r = r;
    }

    @Override
    public void render(Graphics graphics) {
        graphics.setColor(this.color);
        graphics.fillCircle(x, y, r);
        graphics.setColor(0xffffffff);
        graphics.drawText(this.number, x, y + 16);
    }

    private int r;
    private int color;
    private String number;
}
