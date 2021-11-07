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
        Start, Menu, Loading, Playing
    }

    @Override
    public void init(Engine engine) {
        graphics = engine.getGraphics();
        input = engine.getInput();

        state = State.Start;

        states = new ArrayList<>();

        states.add(new Start(graphics, input));
        states.add(new Menu(graphics, input));
        states.add(new Loading(graphics, input));
        states.add(new Playing(graphics, input, 0.5));
        graphics.setLogicalSize(400, 600);

        lastFrameTime = System.nanoTime();
    }

    @Override
    public void update() {

        long currentTime = System.nanoTime();
        long nanoElapsedTime = currentTime - lastFrameTime;
        lastFrameTime = currentTime;
        double elapsedTime = (double) nanoElapsedTime / 1.0E9;

        State s = states.get(state.ordinal()).update(elapsedTime);
        if(s != null) {
            state = s;
            if(state == State.Playing) {
                boardSize = ((Menu)states.get(State.Menu.ordinal())).getBoardSize();
                states.get(state.ordinal()).init(this);
            }
        }
    }

    @Override
    public void render() {
        states.get(state.ordinal()).render();
    }

    @Override
    public void release() {

    }

    public int getBoardSize(){
        return boardSize;
    }

    OhNoApplication.State state;
    ArrayList<es.ucm.gdv.pcohno.State> states;

    Graphics graphics;
    Input input;

    private int boardSize;

    Image imgClose, imgQ, imgEye, imgHistory, imgLock;
    MyFont font1, font2;

    long lastFrameTime;
}
