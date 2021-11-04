package es.ucm.gdv.engine;

import java.util.List;


public interface Input
{

    class TouchEvent
    {
        public TouchEvent(){
            type = TouchEventType.Touch;
            x = 0;
            y = 0;
            id = 0;
        }

        public enum TouchEventType{
            Touch,
            Release,
            Slide
        }

        public TouchEventType type;
        public int x;
        public int y;
        public int id;
    }

    List<TouchEvent> getTouchEvents();
    void addEvent(TouchEvent e);
}
