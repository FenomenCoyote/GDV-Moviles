package es.ucm.gdv.pcengine;

import java.awt.Point;
import java.awt.event.MouseEvent;

import es.ucm.gdv.engine.Input;

public class MouseListener implements javax.swing.event.MouseInputListener{

    MouseListener(PCInput input, PCGraphics graphics){
        this._pcInput = input;
        this._graphics = graphics;
        this._wasReleased = true;
        graphics.getWindow().addMouseListener(this);
        graphics.getWindow().addMouseMotionListener(this);
    }

    @Override
    public void mousePressed(MouseEvent mouseEvent) {
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
