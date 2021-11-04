package es.ucm.gdv.pcengine;

import java.util.ArrayList;
import java.util.List;

import es.ucm.gdv.engine.Input;
import java.awt.event.ActionListener;
import java.util.ListIterator;

public class PCInput implements Input {

    public PCInput(PCGraphics graphics){
        _mouseListener = new MouseListener(this, graphics);
        touchEvents = new ArrayList<TouchEvent>();
    }

    @Override
    synchronized public ArrayList<TouchEvent> getTouchEvents() { return touchEvents; }

    @Override
    synchronized public void addEvent(TouchEvent e) {
        touchEvents.add(e);
    }

    /*synchronized public void releaseEvent(TouchEvent e){

    }*/

    ArrayList<TouchEvent> touchEvents;

    MouseListener _mouseListener;
}
