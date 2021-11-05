package es.ucm.gdv.pcohno;

import java.util.ArrayList;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

public class Start extends State {

    public Start(Graphics graphics, Input input) {
        super(graphics, input);
        font1 = graphics.newFont("Resources/fonts/Molle-Regular.ttf",96,false);
        font2 = graphics.newFont("Resources/fonts/JosefinSans-Bold.ttf",56,false);
        font3 = graphics.newFont("Resources/fonts/JosefinSans-Bold.ttf",20,false);
        imgQ = graphics.newImage("Resources/sprites/q42.png");
    }

    @Override
    public void render() {
        graphics.setColor(0xff333333);
        graphics.setFont(font1);
        graphics.drawText("Oh no", 200, 125);

        graphics.setFont(font2);
        graphics.drawText("Jugar", 200, 300);

        graphics.setColor(0xffaaaaaa);
        graphics.setFont(font3);
        graphics.drawText("Un juego copiado a Q42", 200, 410);
        graphics.drawText("Creado por Martin Kool", 200, 440);

        graphics.scale(0.05f, 0.05f);
        graphics.drawImage(imgQ, 200 * 20 - imgQ.getWidth() / 2, 480 * 20);
        graphics.scale(20, 20);
    }

    @Override
    public OhNoApplication.State update() {
        ArrayList<Input.TouchEvent> events = input.getTouchEvents();
        if(!events.isEmpty()){
            events.clear();
            return OhNoApplication.State.Menu;
        }
        return null;
    }

    @Override
    public void init(OhNoApplication app) {

    }

    private final MyFont font1;
    private final MyFont font2;
    private final MyFont font3;

    private final Image imgQ;
}
