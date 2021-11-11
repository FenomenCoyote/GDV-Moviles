package es.ucm.gdv.engine;

public interface Application {

    public void init(Engine engine);

    public void update(double elapsedTime);

    public void render();

    public void release();
}
