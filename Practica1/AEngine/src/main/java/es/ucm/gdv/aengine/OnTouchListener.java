package es.ucm.gdv.aengine;

import android.graphics.Point;
import android.view.MotionEvent;
import android.view.View;

import es.ucm.gdv.engine.Input;

public class OnTouchListener implements View.OnTouchListener {


    OnTouchListener(AInput aInput, AGraphics aGraphics)
    {
        this.aInput=aInput;
        this.graphics=aGraphics;
        aGraphics.getSurfaceView().setOnTouchListener(this);
    }

    @Override
    public boolean onTouch(View v, MotionEvent event) {

        Input.TouchEvent e = new Input.TouchEvent();

        Point p = translateCoordinates((int)event.getX(), (int)event.getY());

        e.x = p.x;
        e.y = p.y;
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

    private Point translateCoordinates(int x, int y){
        x-= graphics.getOffsetX();
        y-= graphics.getOffsetY();
        x/=graphics.getScale();
        y/=graphics.getScale();
        return new Point(x, y);
    }

    AInput aInput;
    AGraphics graphics;
}
