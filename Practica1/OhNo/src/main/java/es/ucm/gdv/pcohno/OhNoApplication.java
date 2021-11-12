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
        _graphics = engine.getGraphics();
        _input = engine.getInput();

        _state = State.Start;

        _states = new ArrayList<>();

        _states.add(new Start(_graphics, _input));
        _states.add(new Menu(_graphics, _input));
        _states.add(new Loading(_graphics, _input));
        _states.add(new Playing(_graphics, _input, 0.5));

        _graphics.setLogicalSize(400, 600);
    }

    /**
     * Called every frame
     * @param elapsedTime: time since last frame
     */
    @Override
    public void update(double elapsedTime) {
        State s = _states.get(_state.ordinal()).update(elapsedTime);

        if(s != null) {
            _state = s;
            _states.get(_state.ordinal()).setInTransition(true, 0);
            if(_state == State.Playing) {
                _boardSize = ((Menu) _states.get(State.Menu.ordinal())).getBoardSize();
                _states.get(_state.ordinal()).init(this);
            }
        }
    }

    /**
     * Called every frame after update
     */
    @Override
    public void render() {
        _states.get(_state.ordinal()).render();
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
    public int get_boardSize(){
        return _boardSize;
    }

    private OhNoApplication.State _state;
    private ArrayList<es.ucm.gdv.pcohno.State> _states;

    private Graphics _graphics;
    private Input _input;

    private int _boardSize;
}
