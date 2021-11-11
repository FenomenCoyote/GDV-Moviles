package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;

public abstract class Clickable {

    Clickable(int x, int y, int width, int height){
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    abstract public void render(Graphics graphics);

    abstract public void render(Graphics graphics, float alpha);

    public boolean isOnMe(int x, int y){
        return x > this.x && x < this.x+this.width && y > this.y && y < this.y+this.height;
    }

    protected int x;
    protected int y;
    protected int width;
    protected int height;
}

