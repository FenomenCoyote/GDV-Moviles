package es.ucm.gdv.pcengine;

import java.util.List;

import es.ucm.gdv.engine.Input;
import java.awt.event.ActionListener;

public class PCInput implements Input {
    @Override
    synchronized public List<TouchEvent> getTouchEvents() { return touchEvents; }

    @Override
    synchronized public void addEvent(TouchEvent e) {
        touchEvents.add(e);
    }

    List<TouchEvent> touchEvents;
}
