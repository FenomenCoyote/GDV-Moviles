package es.ucm.gdv.pcengine;

import java.awt.Point;
import java.awt.event.MouseEvent;

import es.ucm.gdv.engine.Input;

public class MouseListener implements javax.swing.event.MouseInputListener{

    /**
     * Initializes input receival
     * @param input
     * @param graphics
     */
    MouseListener(PCInput input, PCGraphics graphics){
        this._pcInput = input;
        this._graphics = graphics;
        this._wasReleased = true;
        graphics.getWindow().addMouseListener(this);
        graphics.getWindow().addMouseMotionListener(this);
    }

    /**
     * Called by swing when a mouse pressed event occurs
     * @param mouseEvent
     */
    @Override
    public void mousePressed(MouseEvent mouseEvent) {
        //Only when mouse isn't already pressed
        if(_wasReleased){
            Point p = translateCoordinates(mouseEvent.getX(), mouseEvent.getY());

            Input.TouchEvent e = _pcInput.getReadyTouchEvent();
            if(e == null)
                return;
            e.x = p.x;
            e.y = p.y;
            e.type = Input.TouchEvent.TouchEventType.Touch;
            e.id = mouseEvent.getButton();

            _pcInput.addEvent(e);

            _wasReleased = false;
        }
    }

    /**
     * Called by swing when the mouse stopped being pressed
     * @param mouseEvent
     */
    @Override
    public void mouseReleased(MouseEvent mouseEvent) {
        Point p = translateCoordinates(mouseEvent.getX(), mouseEvent.getY());

        Input.TouchEvent e = _pcInput.getReadyTouchEvent();
        if(e == null)
            return;
        e.x = p.x;
        e.y = p.y;
        e.type = Input.TouchEvent.TouchEventType.Release;
        e.id = mouseEvent.getButton();

        _pcInput.addEvent(e);
        _wasReleased = true;
    }

    /**
     * Called by swing when the mouse is being dragged
     * @param mouseEvent
     */
    @Override
    public void mouseDragged(MouseEvent mouseEvent) {
        Point p = translateCoordinates(mouseEvent.getX(), mouseEvent.getY());

        Input.TouchEvent e = _pcInput.getReadyTouchEvent();
        if(e == null)
            return;
        e.x = p.x;
        e.y = p.y;
        e.type = Input.TouchEvent.TouchEventType.Slide;
        e.id = mouseEvent.getButton();

        _pcInput.addEvent(e);
    }

    @Override
    public void mouseClicked(MouseEvent mouseEvent) { }

    @Override
    public void mouseEntered(MouseEvent mouseEvent) { }

    @Override
    public void mouseExited(MouseEvent mouseEvent) { }

    @Override
    public void mouseMoved(MouseEvent mouseEvent) { }

    /**
     * Translates physical coordinates to logical coordinates
     * @param x
     * @param y
     * @return
     */
    private Point translateCoordinates(int x, int y){
        x-= _graphics.getOffsetX();
        y-= _graphics.getOffsetY();
        x/= _graphics.getScale();
        y/= _graphics.getScale();
        return new Point(x, y);
    }

    private boolean _wasReleased;
    private PCInput _pcInput;
    private PCGraphics _graphics;

}
