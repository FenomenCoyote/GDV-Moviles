package es.ucm.gdv.pcohno;

import java.util.ArrayList;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

public class Menu extends State {

    public Menu(Graphics graphics, Input input) {
        super(graphics, input);

        font1 = graphics.newFont("Resources/fonts/Molle-Regular.ttf",64,false);
        font2 = graphics.newFont("Resources/fonts/JosefinSans-Bold.ttf",32,false);
        font3 = graphics.newFont("Resources/fonts/JosefinSans-Bold.ttf",48,false);

        imgClose = graphics.newImage("Resources/sprites/close.png");

        clickablesCircles = new ArrayList<>();
        clickableClose = new ClickImage(imgClose, 400 - imgClose.getWidth()/2, 1000, 100, 100);

        int red = 0xffff384a;
        int blue = 0xff1cc0e0;

        clickablesCircles.add(new ClickCircle(blue, 0, 0, 40, "4"));
        clickablesCircles.add(new ClickCircle(red, 90, 0, 40, "5"));
        clickablesCircles.add(new ClickCircle(blue, 180, 0, 40, "6"));

        clickablesCircles.add(new ClickCircle(red, 0, 90, 40, "7"));
        clickablesCircles.add(new ClickCircle(blue, 90, 90, 40, "8"));
        clickablesCircles.add(new ClickCircle(red, 180, 90, 40, "9"));
    }

    @Override
    public void render() {
        graphics.setColor(0xff333333);

        graphics.setFont(font1);
        graphics.drawText("Oh no", 200, 100);

        graphics.setFont(font2);
        graphics.drawText("Eliga el tamaño a jugar", 200, 200);

        graphics.save();

        graphics.setFont(font3);
        graphics.translate(110, 300);

        for (Clickable c : clickablesCircles)
            c.render(graphics);

        graphics.restore();

        graphics.scale(0.5f, 0.5f);

        clickableClose.render(graphics);

    }

    @Override
    public OhNoApplication.State update() {
        ArrayList<Input.TouchEvent> events = input.getTouchEvents();
        while(!events.isEmpty()){
            Input.TouchEvent t = events.remove(0);
            if(t.type != Input.TouchEvent.TouchEventType.Touch)
                continue;
            int i = 0;
            for (Clickable c : clickablesCircles) {
                if (c.isOnMe(t.x - 100, t.y - 300)) {
                    boardSize = 4 + i;
                    events.clear();
                    return OhNoApplication.State.Playing;
                }
                ++i;
            }

            if(clickableClose.isOnMe(t.x * 2, t.y * 2)){
                events.clear();
                return OhNoApplication.State.Start;
            }

        }
        return null;
    }

    @Override
    public void init(OhNoApplication app) {

    }

    public int getBoardSize() {
        return boardSize;
    }

    private int boardSize;

    private final ArrayList<Clickable> clickablesCircles;
    private final ClickImage clickableClose;

    private MyFont font1;
    private MyFont font2;
    private MyFont font3;

    private Image imgClose;

}
