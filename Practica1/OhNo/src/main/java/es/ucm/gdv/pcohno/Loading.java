package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

public class Loading extends State {

    public Loading(Graphics graphics, Input input) {
        super(graphics, input);

        font1 = graphics.newFont("JosefinSans-Bold.ttf",64,true);
    }

    @Override
    public void render() {
        graphics.setColor(0xff333333);
        graphics.setFont(font1);
        graphics.drawText("Loading...", 200, 300);
    }

    @Override
    public OhNoApplication.State update(double elapsedTime) {
        return OhNoApplication.State.Playing;
    }

    @Override
    public void init(OhNoApplication app) {

    }

    private final MyFont font1;
}
