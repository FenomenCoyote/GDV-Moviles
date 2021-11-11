package es.ucm.gdv.pcengine;

import java.awt.Point;
import java.awt.event.MouseEvent;

import es.ucm.gdv.engine.Input;

public class MouseListener implements javax.swing.event.MouseInputListener{

    MouseListener(PCInput input, PCGraphics graphics){
        pcInput = input;
        this.graphics = graphics;
        wasReleased = true;
        graphics.getWindow().addMouseListener(this);
        graphics.getWindow().addMouseMotionListener(this);
    }

    @Override
    public void mousePressed(MouseEvent mouseEvent) {
        if(wasReleased){
            Point p = translateCoordinates(mouseEvent.getX(), mouseEvent.getY());

            Input.TouchEvent e = pcInput.getReadyTouchEvent();
            e.x = p.x;
            e.y = p.y;
            e.type = Input.TouchEvent.TouchEventType.Touch;
            e.id = mouseEvent.getButton();

            pcInput.addEvent(e);

            wasReleased = false;
        }
    }

    @Override
    public void mouseReleased(MouseEvent mouseEvent) {
        Point p = translateCoordinates(mouseEvent.getX(), mouseEvent.getY());

        Input.TouchEvent e = pcInput.getReadyTouchEvent();
        e.x = p.x;
        e.y = p.y;
        e.type = Input.TouchEvent.TouchEventType.Release;
        e.id = mouseEvent.getButton();

        pcInput.addEvent(e);
        wasReleased = true;
    }

    @Override
    public void mouseDragged(MouseEvent mouseEvent) {
        Point p = translateCoordinates(mouseEvent.getX(), mouseEvent.getY());

        Input.TouchEvent e = pcInput.getReadyTouchEvent();
        e.x = p.x;
        e.y = p.y;
        e.type = Input.TouchEvent.TouchEventType.Slide;
        e.id = mouseEvent.getButton();

        pcInput.addEvent(e);
    }

    @Override
    public void mouseClicked(MouseEvent mouseEvent) { }

    @Override
    public void mouseEntered(MouseEvent mouseEvent) { }

    @Override
    public void mouseExited(MouseEvent mouseEvent) { }

    @Override
    public void mouseMoved(MouseEvent mouseEvent) {

    }

    private Point translateCoordinates(int x, int y){
        x-= graphics.getOffsetX();
        y-= graphics.getOffsetY();
        x/=graphics.getScale();
        y/=graphics.getScale();
        return new Point(x, y);
    }

    private boolean wasReleased;
    PCInput pcInput;
    PCGraphics graphics;

}
