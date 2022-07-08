package es.ucm.gdv.pcohno;


import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

/**
 * Class for Start screen state
 */
public class Start extends State {

    /**
     * Constructor
     * @param graphics object for rendering
     * @param input object to get input events
     */
    public Start(Graphics graphics, Input input) {
        super(graphics, input);
        _font1 = graphics.newFont("Molle-Regular.ttf",96,false);
        _font2 = graphics.newFont("JosefinSans-Bold.ttf",56,true);
        _font3 = graphics.newFont("JosefinSans-Bold.ttf",20,true);
        _imgQ = graphics.newImage("q42.png");
    }

    /**
     * Called every frame after update
     */
    @Override
    public void render() {
        int alpha = (int)(_alphaTransition * 255f);

        _graphics.setColor((alpha << 24) | 0x00333333);
        _graphics.setFont(_font1);
        _graphics.drawText("Oh no", 200, 125);

        _graphics.setFont(_font2);
        _graphics.drawText("Jugar", 200, 300);

        _graphics.setColor((alpha << 24) | 0x00aaaaaa);
        _graphics.setFont(_font3);
        _graphics.drawText("Un juego copiado a Q42", 200, 410);
        _graphics.drawText("Creado por Martin Kool", 200, 440);

        _graphics.scale(0.05f, 0.05f);
        _graphics.drawImage(_imgQ, 200 * 20 - _imgQ.getWidth() / 2, 480 * 20, _alphaTransition);
        _graphics.scale(20, 20);
    }

    /**
     * Called every frame
     * @param elapsedTime time since last frame
     * @return returns next state if game should change to it, null either
     */
    @Override
    public OhNoApplication.State update(double elapsedTime) {
        OhNoApplication.State s = super.update(elapsedTime);
        if(s != null)
            return s;

        Input.TouchEvent e = _input.getTouchEvent();
        while(e != null){
            _input.releaseEvent(e);
            if(e.type != Input.TouchEvent.TouchEventType.Touch){
                e = _input.getTouchEvent();
                continue;
            }

            _input.clearEvents();
            setNextState(OhNoApplication.State.Menu);
            break;
        }

        return null;
    }

    private final MyFont _font1;
    private final MyFont _font2;
    private final MyFont _font3;

    private final Image _imgQ;
}
