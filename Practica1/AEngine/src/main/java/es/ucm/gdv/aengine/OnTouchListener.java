package es.ucm.gdv.aengine;

import android.graphics.Point;
import android.view.MotionEvent;
import android.view.View;

import es.ucm.gdv.engine.Input;

/**
 * Listener class to add recognized events to Input queue
 */
public class OnTouchListener implements View.OnTouchListener {

    /**
     * Constructor
     * @param aInput Input class to add events
     * @param aGraphics rendering object, used to add listener
     */
    OnTouchListener(AInput aInput, AGraphics aGraphics)
    {
        this._aInput =aInput;
        this._graphics =aGraphics;
        aGraphics.getSurfaceView().setOnTouchListener(this);
    }

    /**
     * Inherited method, called when an event is generated
     * @param v View object
     * @param event event detected
     * @return true if event was generated, false if pool was finished
     */
    @Override
    public boolean onTouch(View v, MotionEvent event) {

        if(event.getAction() == MotionEvent.ACTION_MOVE){
            for(int i=0; i<event.getPointerCount(); i++){
                Point p = translateCoordinates((int)event.getX(i), (int)event.getY(i));
                Input.TouchEvent e = _aInput.getReadyTouchEvent();
                if(e==null) return false;
                e.x = p.x;
                e.y = p.y;
                e.id = event.getPointerId(i);
                e.type = Input.TouchEvent.TouchEventType.Slide;
                _aInput.addEvent(e);
            }
        }
        else{
            int i = event.getActionIndex();
            Point p = translateCoordinates((int)event.getX(i), (int)event.getY(i));
            Input.TouchEvent e = _aInput.getReadyTouchEvent();
            if(e==null)
                return false;
            e.x = p.x;
            e.y = p.y;
            e.id = event.getPointerId(i);
            switch (event.getActionMasked()) {
                case MotionEvent.ACTION_DOWN:
                    e.type = Input.TouchEvent.TouchEventType.Touch;
                    break;
                case MotionEvent.ACTION_UP:
                    e.type = Input.TouchEvent.TouchEventType.Release;
                    break;
                case MotionEvent.ACTION_POINTER_UP:
                    e.type = Input.TouchEvent.TouchEventType.Release;
                    break;
                case MotionEvent.ACTION_POINTER_DOWN:
                    e.type = Input.TouchEvent.TouchEventType.Touch;
                    break;
            }
            _aInput.addEvent(e);
        }
        return true;
    }

    /**
     * Translate event coordinates on screen to logical coordinates
     * @param x event x position on screen coordinates
     * @param y event y position on screen coordinates
     * @return a point meaning x an y position on logical coordinates
     */
    private Point translateCoordinates(int x, int y){
        x-= _graphics.getOffsetX();
        y-= _graphics.getOffsetY();
        x/= _graphics.getScale();
        y/= _graphics.getScale();
        return new Point(x, y);
    }

    private AInput _aInput;
    private AGraphics _graphics;
}
