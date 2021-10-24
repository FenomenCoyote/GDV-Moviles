package es.ucm.gdv.engine;

import java.util.List;


public interface Input
{

    class TouchEvent
    {
        enum TouchEventType{
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
}
