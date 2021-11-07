package es.ucm.gdv.aengine;

import android.view.SurfaceView;

import java.util.ArrayList;
import es.ucm.gdv.engine.Input;

public class AInput implements Input {

    AInput(SurfaceView view,AGraphics aGraphics)
    {
        onTouchListener = new OnTouchListener(this,aGraphics);
        touchEvents = new ArrayList<TouchEvent>();
    }

    @Override
    public synchronized ArrayList<TouchEvent> getTouchEvents() {
        return touchEvents;
    }

    @Override
    public synchronized void addEvent(TouchEvent e) { touchEvents.add(e); }


    ArrayList<TouchEvent> touchEvents;
    OnTouchListener onTouchListener;
}
