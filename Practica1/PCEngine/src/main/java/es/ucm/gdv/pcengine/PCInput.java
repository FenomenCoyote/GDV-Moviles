package es.ucm.gdv.pcengine;

import java.util.ArrayDeque;
import java.util.ArrayList;
import java.util.Queue;

import es.ucm.gdv.engine.Input;

public class PCInput implements Input {

    /**
     * We initialize the mouseListener,the touchEvents list and
     * the readyTouchEvents with 16 touchEvents
     */
    public PCInput(PCGraphics graphics){
        _mouseListener = new MouseListener(this, graphics);
        _touchEvents = new ArrayList<TouchEvent>();
        _readyTouchEvents = new ArrayDeque<>();

        for (int i = 0; i < 16; i++) {
            _readyTouchEvents.add(new TouchEvent());
        }
    }

    /**
     * Gets the first element (index 0) from the touchEvents list
     */
    @Override
    synchronized public TouchEvent getTouchEvent() {
        if(_touchEvents.isEmpty())
            return null;
        return _touchEvents.remove(0);
    }

    @Override
    synchronized public void clearEvents() {
        for(TouchEvent t : _touchEvents)
            _readyTouchEvents.add(t);
        _touchEvents.clear();
    }

    /**
     * Adds an event to the touchEvents list
     */
    @Override
    synchronized public void addEvent(TouchEvent e) {
        _touchEvents.add(e);
    }

    /**
     * Gets the first touchEvent from the readyTouchEvents queue
     */
    synchronized public TouchEvent getReadyTouchEvent() {
        return _readyTouchEvents.poll();
    }

    /**
     * Adds an event to the readyTouchEvents queue
     */
    @Override
    synchronized public void releaseEvent(TouchEvent e){
        _readyTouchEvents.add(e);
    }

    private ArrayList<TouchEvent> _touchEvents;
    private Queue<TouchEvent> _readyTouchEvents;

    //Needed to preserve the object in memory
    private MouseListener _mouseListener;
}
