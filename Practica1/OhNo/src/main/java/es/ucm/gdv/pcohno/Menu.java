package es.ucm.gdv.pcohno;

import java.util.ArrayList;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

/**
 * Class for menu state
 */
public class Menu extends State {

    /**
     * Constructor
     * @param graphics object for rendering
     * @param input object to get input events
     */
    public Menu(Graphics graphics, Input input) {
        super(graphics, input);

        _font1 = graphics.newFont("Molle-Regular.ttf",64,false);
        _font2 = graphics.newFont("JosefinSans-Bold.ttf",32,false);
        _font3 = graphics.newFont("JosefinSans-Bold.ttf",48,false);

        _imgClose = graphics.newImage("close.png");

        _clickablesCircles = new ArrayList<>();
        _clickableClose = new ClickImage(_imgClose, 400 - _imgClose.getWidth()/2, 1000, 100, 100, 1);

        int red = 0x00ff384a;
        int blue = 0x001cc0e0;

        _clickablesCircles.add(new ClickCircle(blue, 0, 0, 40, "4"));
        _clickablesCircles.add(new ClickCircle(red, 90, 0, 40, "5"));
        _clickablesCircles.add(new ClickCircle(blue, 180, 0, 40, "6"));

        _clickablesCircles.add(new ClickCircle(red, 0, 90, 40, "7"));
        _clickablesCircles.add(new ClickCircle(blue, 90, 90, 40, "8"));
        _clickablesCircles.add(new ClickCircle(red, 180, 90, 40, "9"));

        _boardSize = 4;
    }

    /**
     * Called every frame after update
     */
    @Override
    public void render() {
        int alpha = (int)(alphaTransition * 255f);
        graphics.setColor((alpha << 24) | 0x00333333);

        graphics.setFont(_font1);
        graphics.drawText("Oh no", 200, 100);

        graphics.setFont(_font2);
        graphics.drawText("Eliga el tamaño a jugar", 200, 200);

        graphics.save();

        graphics.setFont(_font3);
        graphics.translate(110, 300);

        for (Clickable c : _clickablesCircles)
            c.render(graphics, alphaTransition);

        graphics.restore();

        graphics.scale(0.5f, 0.5f);

        _clickableClose.render(graphics, alphaTransition);

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

        Input.TouchEvent t = input.getTouchEvent();
        while(t != null){
            input.releaseEvent(t);
            if(t.type != Input.TouchEvent.TouchEventType.Touch){
                t = input.getTouchEvent();
                continue;
            }

            int i = 0;
            for (Clickable c : _clickablesCircles) {
                if (c.isOnMe(t.x - 100, t.y - 300)) {
                    _boardSize = 4 + i;
                    input.clearEvents();
                    setNextState(OhNoApplication.State.Loading);
                }
                ++i;
            }

            if(_clickableClose.isOnMe(t.x * 2, t.y * 2)){
                input.clearEvents();
                setNextState(OhNoApplication.State.Start);
            }
            t = input.getTouchEvent();
        }
        return null;
    }

    /**
     * Called before first update
     * @param app object representing the game
     */
    @Override
    public void init(OhNoApplication app) {

    }

    /**
     * BoardSize getter
     * @return board size
     */
    public int getBoardSize() {
        return _boardSize;
    }

    private int _boardSize;

    private final ArrayList<Clickable> _clickablesCircles;
    private final ClickImage _clickableClose;

    private MyFont _font1;
    private MyFont _font2;
    private MyFont _font3;

    private Image _imgClose;

}
