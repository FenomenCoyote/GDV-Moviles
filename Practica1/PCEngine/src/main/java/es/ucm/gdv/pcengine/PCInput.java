package es.ucm.gdv.pcengine;

import java.util.List;

import es.ucm.gdv.engine.Input;
import java.awt.event.ActionListener;

public class PCInput implements Input {
    @Override
    public List<TouchEvent> getTouchEvents() { return touchEvents; }

    List<TouchEvent> touchEvents;
}
