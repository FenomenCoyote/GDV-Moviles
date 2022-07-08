package es.ucm.gdv.engine;

public interface Engine {

    public void init();

    public void run();

    public void release();

    public void setApplication(Application app);

    public Graphics getGraphics();
    public Input getInput();

}