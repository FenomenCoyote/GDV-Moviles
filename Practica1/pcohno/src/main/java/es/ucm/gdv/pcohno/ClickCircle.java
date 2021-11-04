package es.ucm.gdv.pcohno;

import java.awt.Color;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;

public class ClickCircle extends Clickable {

    ClickCircle(int color, int x, int y, int r, String number) {
        super(x, y, r, r);
        this.color = color;
        this.number = number;
    }

    @Override
    public void render(Graphics graphics) {
        graphics.save();
        graphics.setColor(this.color);
        graphics.fillCircle(x, y, width);
        graphics.setColor(0xffffffff);
        graphics.drawText(this.number, x+width/2, y+height/2);
        graphics.restore();
    }

    private int color;
    private String number;
}
