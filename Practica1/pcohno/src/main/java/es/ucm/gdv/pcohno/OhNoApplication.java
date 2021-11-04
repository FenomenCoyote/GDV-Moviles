package es.ucm.gdv.pcohno;

import java.util.ArrayList;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

public class OhNoApplication implements Application {

    enum State {
        Start, Menu, Playing, End
    }

    @Override
    public void init(Engine engine) {
        graphics = engine.getGraphics();
        input = engine.getInput();

        state = State.Start;

        states = new ArrayList<>();

        states.add(new Start(graphics, input));
        states.add(new Menu(graphics, input));
        graphics.setLogicalSize(400, 600);

       /*font1 = graphics.newFont("Resources/fonts/Molle-Regular.ttf",64,false);
        font2 = graphics.newFont("Resources/fonts/JosefinSans-Bold.ttf",32,false);
        imgClose = graphics.newImage("Resources/sprites/close.png");
        imgQ = graphics.newImage("Resources/sprites/q42.png");
        imgEye = graphics.newImage("Resources/sprites/eye.png");
        imgHistory = graphics.newImage("Resources/sprites/history.png");
        imgLock = graphics.newImage("Resources/sprites/lock.png");*/
    }

    @Override
    public void update() {
        State s = states.get(state.ordinal()).update();
        if(s != null){
            state = s;
            if(state == State.Playing)
                states.get(state.ordinal()).init();
        }
    }

    private void startGame(int boardSize){
        board = new Board(boardSize);
        board.setForGame();
        //board.setForGame2();
    }

    @Override
    public void render() {
        graphics.clear(0xfff6f6f6);
        states.get(state.ordinal()).render();
    }

    @Override
    public void release() {

    }

    OhNoApplication.State state;
    ArrayList<es.ucm.gdv.pcohno.State> states;

    Graphics graphics;
    Input input;

    Board board;

    ArrayList<Clickable> clickables;


    Image imgClose, imgQ, imgEye, imgHistory, imgLock;
    MyFont font1, font2;
}
