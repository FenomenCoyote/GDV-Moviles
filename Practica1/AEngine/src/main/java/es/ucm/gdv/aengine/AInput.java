package es.ucm.gdv.aengine;

import android.view.SurfaceView;

import java.util.ArrayDeque;
import java.util.ArrayList;
import java.util.Queue;

import es.ucm.gdv.engine.Input;

/**
 * Class which manages events queue
 */
public class AInput implements Input {

    /**
     * Constructor
     * @param aGraphics object for rendering, used to add input listener
     */
    AInput(AGraphics aGraphics)
    {
        _onTouchListener = new OnTouchListener(this, aGraphics);
        _touchEvents = new ArrayList<TouchEvent>();

        _readyTouchEvents = new ArrayDeque<>();

        for (int i = 0; i < 16; i++) {
            _readyTouchEvents.add(new TouchEvent());
        }
    }

    /**
     * Gets first touch event in queue
     * @return first Touch event in queue if queue is not empty
     */
    @Override
    synchronized public TouchEvent getTouchEvent() {
        if(_touchEvents.isEmpty())
            return null;
        return _touchEvents.remove(0);
    }

    /**
     * Clears event list
     */
    @Override
    synchronized public void clearEvents() {
        for(TouchEvent t : _touchEvents)
            _readyTouchEvents.add(t);
        _touchEvents.clear();
    }

    /**
     * Get a event which is not being used from the pool
     * Use this instead of creating a new TouchEvent
     * @return An unused TouchEvent to be used
     */
    synchronized public TouchEvent getReadyTouchEvent() {
        return _readyTouchEvents.poll();
    }

    /**
     * Adds event to pool so it can be used later
     * @param e event which is not useful anymore
     */
    @Override
    synchronized public void releaseEvent(TouchEvent e) {
        _readyTouchEvents.add(e);
    }

    /**
     * Add event to queue
     * @param e event to add
     */
    @Override
    synchronized public void addEvent(TouchEvent e) { _touchEvents.add(e); }

    private ArrayList<TouchEvent> _touchEvents;
    private Queue<TouchEvent> _readyTouchEvents;
    private OnTouchListener _onTouchListener;
}
