package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

/**
 * Class for loading state
 */
public class Loading extends State {

    /**
     * Constructor
     * @param graphics object for rendering
     * @param input object to get input events
     */
    public Loading(Graphics graphics, Input input) {
        super(graphics, input);

        _font1 = graphics.newFont("JosefinSans-Bold.ttf",64,true);
    }

    /**
     * Called every frame after update
     */
    @Override
    public void render() {
        _graphics.setColor(0xff333333);
        _graphics.setFont(_font1);
        _graphics.drawText("Loading...", 200, 300);
    }

    /**
     * Called every frame
     * @param elapsedTime time since last frame
     * @return returns next state if game should change to it, null either
     */
    @Override
    public OhNoApplication.State update(double elapsedTime) {
        return OhNoApplication.State.Playing;
    }

       private final MyFont _font1;
}
