package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

public class OhNoApplication implements Application {
    @Override
    public void init(Engine engine) {
        graphics = engine.getGraphics();
        input = engine.getInput();

        graphics.setLogicalSize(400, 600);
        img= graphics.newImage("Resources/sprites/close.png");
        font = graphics.newFont("Resources/fonts/Molle-Regular.ttf",50,false);
    }

    @Override
    public void update() {

    }

    @Override
    public void render() {
        graphics.clear(0xffff0000);
        graphics.setColor(0xff000000);

        graphics.save();
        graphics.translate(100, 100);
        graphics.drawImage(img,0,0);
        graphics.restore();

        graphics.fillCircle(0,0,50);

        graphics.setFont(font);
        graphics.drawText("OhNo¡¡¡",100,300);
    }

    @Override
    public void release() {

    }

    Graphics graphics;
    Input input;
    Image img;
    MyFont font;
}
