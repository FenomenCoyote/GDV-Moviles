package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;

public class OhNoApplication implements Application {
    @Override
    public void init(Engine engine) {
        graphics = engine.getGraphics();
        input = engine.getInput();

        graphics.setLogicalSize(400, 600);
    }

    @Override
    public void update() {

    }

    @Override
    public void render() {
        graphics.clear(0xffff0000);
        graphics.setColor(0x880000ff);
        graphics.fillCircle(200, 300,200);
    }

    @Override
    public void release() {

    }

    Graphics graphics;
    Input input;
}
