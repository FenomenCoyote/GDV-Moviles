package es.ucm.gdv.aengine;

import android.view.MotionEvent;
import android.view.View;

import es.ucm.gdv.engine.Input;

public class OnTouchListener implements View.OnTouchListener {


    OnTouchListener(AInput aInput,AGraphics aGraphics)
    {
        this.aInput=aInput;
        this.aGraphics=aGraphics;
        aGraphics.getSurfaceView().setOnTouchListener(this);
    }

    @Override
    public boolean onTouch(View v, MotionEvent event) {

        Input.TouchEvent e = new Input.TouchEvent();
        e.x = (int) event.getX();
        e.y = (int) event.getY();
        e.id = event.getAction();

        switch (e.id) {
            case MotionEvent.ACTION_DOWN:
                e.type = Input.TouchEvent.TouchEventType.Touch;
                break;
            case MotionEvent.ACTION_MOVE:
                e.type = Input.TouchEvent.TouchEventType.Slide;
                break;
            case MotionEvent.ACTION_UP:
                e.type = Input.TouchEvent.TouchEventType.Release;
                break;
        }

        aInput.addEvent(e);

        return true;
    }

    AInput aInput;
    AGraphics aGraphics;
}
