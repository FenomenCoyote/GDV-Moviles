package es.ucm.gdv.engine;


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

    TouchEvent getTouchEvent();
    void clearEvents();
    void releaseEvent(TouchEvent e);
    void addEvent(TouchEvent e);
}
