package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;

public class ClickCircle extends Clickable {

    ClickCircle(int r){
        super(0,0, r, r);
        this.r = r;
    }

    ClickCircle(int color, int x, int y, int r, String number) {
        super(x, y, r, r );
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

    @Override
    public void render(Graphics graphics, float alpha) {
        int a = (int)(alpha * 255f) << 24;
        graphics.setColor(this.color | a);
        graphics.fillCircle(x, y, r);
        graphics.setColor(a | 0x00ffffff);
        graphics.drawText(this.number, x, y + 16);
    }

    @Override
    public boolean isOnMe(int x, int y){
        float a = (x - this.x);
        float b = (y - this.y);
        return Math.sqrt(a * a + b * b) < this.r;
    }

    private int r;
    private int color;
    private String number;
}
