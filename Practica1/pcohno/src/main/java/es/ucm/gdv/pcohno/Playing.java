package es.ucm.gdv.pcohno;

import java.util.ArrayList;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

public class Playing extends State {

    public Playing(Graphics graphics, Input input) {
        super(graphics, input);

        font1 = graphics.newFont("Resources/fonts/JosefinSans-Bold.ttf",64,false);

        imgClose = graphics.newImage("Resources/sprites/close.png");

        clickableClose = new ClickImage(imgClose, 150 - imgClose.getWidth()/2, 1050, 100, 100);
    }

    @Override
    public void render() {
        graphics.setColor(0xff222222);
        graphics.setFont(font1);
        graphics.drawText(boardSize + " x " + boardSize, 200, 80);

        graphics.translate(0, 100);
        board.render(graphics);
        graphics.translate(0, -100);

        graphics.scale(0.5f, 0.5f);
        clickableClose.render(graphics);
    }

    @Override
    public OhNoApplication.State update() {
        ArrayList<Input.TouchEvent> events = input.getTouchEvents();
        while(!events.isEmpty()){
            Input.TouchEvent t = events.remove(0);
            if(clickableClose.isOnMe(t.x * 2, t.y * 2)){
                events.clear();
                return OhNoApplication.State.Menu;
            }
            board.isOnMe(t.x, t.y);
        }
        return null;
    }

    @Override
    public void init(OhNoApplication app) {
        boardSize = app.getBoardSize();
        board = new Board(boardSize);
        board.setForGame();
    }

    private int boardSize;

    private Board board;

    private MyFont font1;

    private Image imgClose;

    private ClickImage clickableClose;
}
