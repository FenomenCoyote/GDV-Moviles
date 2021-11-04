package es.ucm.gdv.pcengine;

import java.awt.Point;
import java.awt.event.MouseEvent;

import es.ucm.gdv.engine.Input;

public class MouseListener implements javax.swing.event.MouseInputListener{

    MouseListener(PCInput input, PCGraphics graphics){
        pcInput = input;
        this.graphics = graphics;
        graphics.getWindow().addMouseListener(this);
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

    private Point translateCoordinates(int x, int y){
        x-= graphics.getOffsetX();
        y-= graphics.getOffsetY();
        x/=graphics.getScale();
        y/=graphics.getScale();
        return new Point(x, y);
    }

    PCInput pcInput;
    PCGraphics graphics;

}
