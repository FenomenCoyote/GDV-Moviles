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

        if(event.getAction() == MotionEvent.ACTION_MOVE){
            for(int i=0; i<event.getPointerCount(); i++){
                Point p = translateCoordinates((int)event.getX(i), (int)event.getY(i));
                Input.TouchEvent e = aInput.getReadyTouchEvent();
                if(e==null) return false;
                e.x = p.x;
                e.y = p.y;
                e.id = event.getPointerId(i);
                e.type = Input.TouchEvent.TouchEventType.Slide;
                aInput.addEvent(e);
            }
        }
        else{
            int i = event.getActionIndex();
            Point p = translateCoordinates((int)event.getX(i), (int)event.getY(i));
            Input.TouchEvent e = aInput.getReadyTouchEvent();
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
            aInput.addEvent(e);
        }
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
