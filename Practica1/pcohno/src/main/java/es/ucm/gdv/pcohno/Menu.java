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
        clickableClose = new ClickImage(imgClose, 200 - imgClose.getWidth()/2, 500, 100, 100);

        clickablesCircles.add(new ClickCircle(0xff44aaff, 100, 325, 80, "4"));
        clickablesCircles.add(new ClickCircle(0xffff4848, 200, 325, 80, "5"));
        clickablesCircles.add(new ClickCircle(0xff44aaff, 300, 325, 80, "6"));

        clickablesCircles.add(new ClickCircle(0xffff4848, 100, 425, 80, "7"));
        clickablesCircles.add(new ClickCircle(0xff44aaff, 200, 425, 80, "8"));
        clickablesCircles.add(new ClickCircle(0xffff4848, 300, 425, 80, "9"));
    }

    @Override
    public void render() {
        graphics.setColor(0xff222222);

        graphics.setFont(font1);
        graphics.drawText("Oh no", 200, 100);

        graphics.setFont(font2);
        graphics.drawText("Eliga el tamaño a jugar", 200, 200);

        graphics.save();

        graphics.setFont(font3);
        graphics.translate(80, 60);

        for (Clickable c : clickablesCircles)
            c.render(graphics);

        graphics.restore();

        clickableClose.render(graphics);

    }

    @Override
    public OhNoApplication.State update() {
        ArrayList<Input.TouchEvent> events = input.getTouchEvents();
        while(!events.isEmpty()){
            Input.TouchEvent t = events.remove(0);

            int i = 0;
            for (Clickable c : clickablesCircles) {
                if (c.isOnMe(t.x, t.y)) {
                    boardSize = 4 + i;
                    return OhNoApplication.State.Playing;
                }
                ++i;
            }

            if(clickableClose.isOnMe(t.x, t.y))
                return OhNoApplication.State.Start;
        }
        return null;
    }

    @Override
    public void init() {

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
