package es.ucm.gdv.pcohno;

import java.util.ArrayList;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;

public class OhNoApplication implements Application {

    enum State {
        Start, Menu, Loading, Playing
    }

    /**
     * Called before the main loop
     * @param engine
     */
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
    }

    /**
     * Called every frame
     * @param elapsedTime: time since last frame
     */
    @Override
    public void update(double elapsedTime) {
        State s = states.get(state.ordinal()).update(elapsedTime);
        if(s != null) {
            state = s;
            if(state == State.Playing) {
                boardSize = ((Menu)states.get(State.Menu.ordinal())).getBoardSize();
                states.get(state.ordinal()).init(this);
            }
        }
    }

    /**
     * Called every frame after update
     */
    @Override
    public void render() {
        states.get(state.ordinal()).render();
    }

    /**
     * Called after the main loop
     */
    @Override
    public void release() {

    }

    /**
     * Used by Playing State to retrieve board size information gathered in Menu State
     * @return
     */
    public int getBoardSize(){
        return boardSize;
    }

    OhNoApplication.State state;
    ArrayList<es.ucm.gdv.pcohno.State> states;

    Graphics graphics;
    Input input;

    private int boardSize;
}
