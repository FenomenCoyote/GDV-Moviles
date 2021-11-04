package es.ucm.gdv.pcengine;

import java.awt.Point;
import java.awt.event.MouseEvent;

import es.ucm.gdv.engine.Input;

public class MouseListener implements javax.swing.event.MouseInputListener{

    MouseListener(PCInput input){
        pcInput = input;
        offsetX = 0;
        offsetY = 0;
        scale = 0;
    }

    @Override
    public void mouseClicked(MouseEvent mouseEvent) {
        Point p = translateCoordinates(mouseEvent.getX(), mouseEvent.getY());

        Input.TouchEvent e = new Input.TouchEvent();
        e.x = p.x;
        e.y = p.y;
        e.type = Input.TouchEvent.TouchEventType.Touch;
        e.id = mouseEvent.getButton();

        pcInput.addEvent(e);
    }

    @Override
    public void mousePressed(MouseEvent mouseEvent) {

    }

    @Override
    public void mouseReleased(MouseEvent mouseEvent) {

    }

    @Override
    public void mouseEntered(MouseEvent mouseEvent) {

    }

    @Override
    public void mouseExited(MouseEvent mouseEvent) {

    }

    @Override
    public void mouseDragged(MouseEvent mouseEvent) {

    }

    @Override
    public void mouseMoved(MouseEvent mouseEvent) {

    }

    public void setOffsetScale(int x, int y, float actualScale){
        offsetX = x;
        offsetY = y;
        scale = actualScale;
    }

    private Point translateCoordinates(int x, int y){
        x-= offsetX;
        y-= offsetY;
        x*=scale;
        y*=scale;
        return new Point(x, y);
    }

    PCInput pcInput;

    int offsetX, offsetY;
    float scale;
}
