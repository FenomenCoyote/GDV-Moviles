package es.ucm.gdv.aengine;

import android.view.SurfaceView;

import java.util.ArrayDeque;
import java.util.ArrayList;
import java.util.Queue;

import es.ucm.gdv.engine.Input;

public class AInput implements Input {

    AInput(SurfaceView view, AGraphics aGraphics)
    {
        _onTouchListener = new OnTouchListener(this, aGraphics);
        _touchEvents = new ArrayList<TouchEvent>();

        _readyTouchEvents = new ArrayDeque<>();

        for (int i = 0; i < 16; i++) {
            _readyTouchEvents.add(new TouchEvent());
        }
    }

    @Override
    synchronized public TouchEvent getTouchEvent() {
        if(_touchEvents.isEmpty())
            return null;
        return _touchEvents.remove(0);
    }

    @Override
    public void clearEvents() {
        _touchEvents.clear();
    }

    synchronized public TouchEvent getReadyTouchEvent() {
        return _readyTouchEvents.poll();
    }

    @Override
    public void releaseEvent(TouchEvent e) {
        _readyTouchEvents.add(e);
    }

    @Override
    public synchronized void addEvent(TouchEvent e) { _touchEvents.add(e); }

    private ArrayList<TouchEvent> _touchEvents;
    private Queue<TouchEvent> _readyTouchEvents;
    private OnTouchListener _onTouchListener;
}
