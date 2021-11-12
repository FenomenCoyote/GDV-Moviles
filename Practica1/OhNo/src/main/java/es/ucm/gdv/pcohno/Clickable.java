package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;

/**
 * Abstract class for clickable objects
 */
public abstract class Clickable {

    /**
     * Constructor
     * @param x: x position on screen
     * @param y: y position on screen
     * @param width: width on screen
     * @param height: height on screen
     */
    Clickable(int x, int y, int width, int height){
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    abstract public void render(Graphics graphics);

    abstract public void render(Graphics graphics, float alpha);

    /**
     * Checks if given position is inside object's rect
     * @param x: x value for position
     * @param y: y value for position
     * @return true if yes, false either
     */
    public boolean isOnMe(int x, int y){
        return x > this.x && x < this.x+this.width && y > this.y && y < this.y+this.height;
    }

    protected int x;
    protected int y;
    protected int width;
    protected int height;
}

