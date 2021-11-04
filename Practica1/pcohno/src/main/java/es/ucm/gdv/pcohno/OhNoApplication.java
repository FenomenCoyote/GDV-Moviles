package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Application;
import es.ucm.gdv.engine.Engine;
import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

public class OhNoApplication implements Application {

    enum State {
        Inicio, Menu, Playing, End
    }

    @Override
    public void init(Engine engine) {
        graphics = engine.getGraphics();
        input = engine.getInput();

        state = State.Menu;

        graphics.setLogicalSize(400, 600);
        font1 = graphics.newFont("Resources/fonts/Molle-Regular.ttf",64,false);
        font2 = graphics.newFont("Resources/fonts/JosefinSans-Bold.ttf",32,false);
        imgClose= graphics.newImage("Resources/sprites/close.png");
        imgQ= graphics.newImage("Resources/sprites/q42.png");
        imgEye= graphics.newImage("Resources/sprites/eye.png");
        imgHistory= graphics.newImage("Resources/sprites/history.png");
        imgLock= graphics.newImage("Resources/sprites/lock.png");
    }

    @Override
    public void update() {
        switch (state) {
            case Inicio:
                break;
            case Menu:
                break;
            case Playing:
                break;
            case End:
                break;
        }
    }

    private void startGame(int boardSize){
        board = new Board(boardSize);
        board.setForGame();
        //board.setForGame2();
    }

    private void renderInicio(){
        graphics.setColor(0xff000000);
        graphics.setFont(font1);
        graphics.drawText("Oh no", 100, 100);

        graphics.setFont(font2);
        graphics.save();
        graphics.scale(1.5f, 1.5f);
        graphics.drawText("Jugar", 100, 200);
        graphics.restore();

        graphics.setColor(0xff666666);
        graphics.scale(0.5f, 0.5f);
        graphics.drawText("Un juego copiado a Q42", 300, 850);
        graphics.drawText("Creado por Martin Kool", 300, 900);

        graphics.scale(0.1f, 0.1f);
        graphics.drawImage(imgQ, 200 * 20, 500 * 20);
    }

    private void renderMenu(){
        graphics.setColor(0xff000000);
        graphics.setFont(font1);
        graphics.drawText("Oh no", 100, 100);

        graphics.setColor(0xff222222);
        graphics.setFont(font2);
        graphics.drawText("Eliga el tamaño a jugar", 50, 200);

        graphics.save();

        int colorRed = 0xffff4848;
        int colorBlue = 0xff44aaff;

        graphics.translate(50, 225);
        renderCircle("4", colorBlue);

        graphics.translate(100, 0);
        renderCircle("5", colorRed);

        graphics.translate(100, 0);
        renderCircle("6", colorBlue);

        graphics.translate(-200, 100);
        renderCircle("7", colorRed);

        graphics.translate(100, 0);
        renderCircle("8", colorBlue);;

        graphics.translate(100, 0);
        renderCircle("9", colorRed);

        graphics.restore();

        graphics.scale(0.5f, 0.5f);
        graphics.drawImage(imgClose, 200 * 2, 525 * 2);
    }

    private void renderCircle(String number, int color){
        graphics.setColor(color);
        graphics.fillCircle(0, 0, 80);
        graphics.setColor(0xffffffff);
        graphics.drawText(number, 30, 55);
    }

    private void renderPlaying(){
        graphics.setFont(font2);
        board.render(graphics);
    }

    private void renderEnd(){

    }

    @Override
    public void render() {
        graphics.clear(0xfff6f6f6);
        switch (state) {
            case Inicio:
                renderInicio();
                break;
            case Menu:
                renderMenu();
                break;
            case Playing:
                renderPlaying();
                break;
            case End:
                renderEnd();
                break;
        }
    }

    @Override
    public void release() {

    }

    State state;

    Board board;

    Graphics graphics;
    Input input;
    Image imgClose, imgQ, imgEye, imgHistory, imgLock;
    MyFont font1, font2;
}
