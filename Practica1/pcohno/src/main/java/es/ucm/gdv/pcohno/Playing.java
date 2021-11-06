package es.ucm.gdv.pcohno;

import com.sun.tools.javac.util.Pair;

import java.util.ArrayList;
import java.util.Stack;

import javax.swing.Painter;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

public class Playing extends State {

    public Playing(Graphics graphics, Input input) {
        super(graphics, input);

        font1 = graphics.newFont("Resources/fonts/JosefinSans-Bold.ttf",64,false);
        font2 = graphics.newFont("Resources/fonts/JosefinSans-Bold.ttf",16,false);

        imgClose = graphics.newImage("Resources/sprites/close.png");
        imgUnDo = graphics.newImage("Resources/sprites/history.png");
        imgEye = graphics.newImage("Resources/sprites/eye.png");

        clickableClose = new ClickImage(imgClose, 150 - imgClose.getWidth()/2, 1050, 100, 100);
        clickableUnDo = new ClickImage(imgUnDo, 400 - imgUnDo.getWidth()/2, 1050, 100, 100);
        clickableEye = new ClickImage(imgEye, 650 - imgEye.getWidth()/2, 1050, 100, 100);
    }

    @Override
    public void render() {

        graphics.setColor(0xff222222);
        graphics.setFont(font2);
        graphics.drawText(board.getPercentage() + "%",200,510);
        graphics.setFont(font1);
        graphics.drawText(boardSize + " x " + boardSize, 200, 80);

        board.render(graphics);

        graphics.scale(0.5f, 0.5f);
        clickableClose.render(graphics);
        clickableUnDo.render(graphics);
        clickableEye.render(graphics);
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
            else if(clickableEye.isOnMe(t.x * 2, t.y * 2))
            {
                events.clear();
            }
            else if(clickableUnDo.isOnMe(t.x * 2, t.y * 2))
            {
                events.clear();
                board.undo();
            }
            else
            {
                board.isOnMe(t.x, t.y);
            }

        }

        if(board.wrongCell() == null){
            System.out.println("Ganaste!!!");
            return OhNoApplication.State.Menu;
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

    private MyFont font1,font2;

    private Image imgClose,imgUnDo,imgEye;

    private ClickImage clickableClose,clickableUnDo, clickableEye;
}
