package es.ucm.gdv.pcohno;


import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;


public class Start extends State {

    public Start(Graphics graphics, Input input) {
        super(graphics, input);
        _font1 = graphics.newFont("Molle-Regular.ttf",96,false);
        _font2 = graphics.newFont("JosefinSans-Bold.ttf",56,true);
        _font3 = graphics.newFont("JosefinSans-Bold.ttf",20,true);
        _imgQ = graphics.newImage("q42.png");
    }

    @Override
    public void render() {
        int alpha = (int)(alphaTransition * 255f);

        graphics.setColor((alpha << 24) | 0x00333333);
        graphics.setFont(_font1);
        graphics.drawText("Oh no", 200, 125);

        graphics.setFont(_font2);
        graphics.drawText("Jugar", 200, 300);

        graphics.setColor((alpha << 24) | 0x00aaaaaa);
        graphics.setFont(_font3);
        graphics.drawText("Un juego copiado a Q42", 200, 410);
        graphics.drawText("Creado por Martin Kool", 200, 440);

        graphics.scale(0.05f, 0.05f);
        graphics.drawImage(_imgQ, 200 * 20 - _imgQ.getWidth() / 2, 480 * 20, alphaTransition);
        graphics.scale(20, 20);
    }

    @Override
    public OhNoApplication.State update(double elapsedTime) {
        OhNoApplication.State s = super.update(elapsedTime);
        if(s != null)
            return s;

        Input.TouchEvent e = input.getTouchEvent();
        while(e != null){
            input.releaseEvent(e);
            if(e.type != Input.TouchEvent.TouchEventType.Touch){
                e = input.getTouchEvent();
                continue;
            }

            input.clearEvents();
            setNextState(OhNoApplication.State.Menu);
            break;
        }

        return null;
    }

    @Override
    public void init(OhNoApplication app) {

    }

    private final MyFont _font1;
    private final MyFont _font2;
    private final MyFont _font3;

    private final Image _imgQ;
}
