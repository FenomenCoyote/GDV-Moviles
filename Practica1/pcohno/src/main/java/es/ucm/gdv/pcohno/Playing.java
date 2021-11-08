package es.ucm.gdv.pcohno;

import java.util.ArrayList;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

public class Playing extends State {

    public Playing(Graphics graphics, Input input, double showLocksTime) {
        super(graphics, input);

        font1 = graphics.newFont("JosefinSans-Bold.ttf",64,false);
        font2 = graphics.newFont("JosefinSans-Bold.ttf",16,false);
        font3 = graphics.newFont("JosefinSans-Bold.ttf",26,false);

        imgClose = graphics.newImage("close.png");
        imgUnDo = graphics.newImage("history.png");
        imgEye = graphics.newImage("eye.png");

        clickableClose = new ClickImage(imgClose, 150 - imgClose.getWidth()/2, 1050, 100, 100, 0.5f);
        clickableUnDo = new ClickImage(imgUnDo, 400 - imgUnDo.getWidth()/2, 1050, 100, 100, 0.5f);
        clickableEye = new ClickImage(imgEye, 650 - imgEye.getWidth()/2, 1050, 100, 100, 0.5f);

        text = null;

        this.showLocksTime = this.initialShowLocksTime = showLocksTime;
    }

    @Override
    public void render() {

        graphics.setColor(0xff333333);
        graphics.setFont(font2);
        graphics.drawText(board.getPercentage() + "%",200,510);
        graphics.setFont(font1);
        if(text == null)
            graphics.drawText(boardSize + " x " + boardSize, 200, 80);
        else{
            graphics.save();
            if(text == "Splendid")
                graphics.setFont(font2);
            graphics.setFont(font3);
            String texts[] = text.split("\n");
            for (String t_ : texts) {
                graphics.drawText(t_, 200, 60 );
                graphics.translate(0, 25);
            }
            graphics.restore();
        }
        graphics.setFont(font1);
        board.render(graphics);

        graphics.scale(0.5f, 0.5f);
        clickableClose.render(graphics);
        clickableUnDo.render(graphics);
        clickableEye.render(graphics);
    }

    @Override
    public OhNoApplication.State update(double elapsedTime) {
        ArrayList<Input.TouchEvent> events = input.getTouchEvents();
        while(!events.isEmpty()){
            Input.TouchEvent t = events.remove(0);
            if(t.type != Input.TouchEvent.TouchEventType.Touch)
                continue;
            if(text != null){
                if(text == "Splendid")
                    return OhNoApplication.State.Menu;
                else{
                    text = null;
                    board.highlightCircle(0, 0, false);
                }
            }
            if(clickableClose.isOnMe(t.x * 2, t.y * 2)){
                events.clear();
                return OhNoApplication.State.Menu;
            }
            else if(clickableEye.isOnMe(t.x * 2, t.y * 2))
            {
                CellHint hint = board.getHint();

                if(hint == null)
                    text = "There is a mistake in the board";
                else {
                    switch (hint.type) {
                        case First:
                            text = "This number can see all its dots";
                            break;
                        case Second:
                            text = "Looking further in one direction\nwould exceed this number";
                            break;
                        case Third:
                            text = "One specific dot is included\nin all solutions imaginable";
                            break;
                        case Fourth:
                            text = "This number sees a bit too\nmuch";
                            break;
                        case Fifth:
                            text = "This number can't see enough";
                            break;
                        case Sixth:
                            text = "This one cant see anyone";
                            break;
                        case Seventh:
                            text = "A blue dot should always see\nat least one other";
                            break;
                    }
                    board.highlightCircle(hint.pos.fst, hint.pos.snd, true);
                }
            }
            else if(clickableUnDo.isOnMe(t.x * 2, t.y * 2)) {
                CellHint hint = board.undo();
                if (hint != null){
                    switch (hint.state) {
                        case Unassigned:
                            text = "This tile was reversed to its\nempty state";
                            break;
                        case Point:
                            text = "This tile was reversed to blue";
                            break;
                        case Wall:
                            text = "This tile was reversed to red";
                            break;
                    }
                    board.highlightCircle(hint.pos.fst, hint.pos.snd, true);
                }
                else {
                    text = "Nothing to undo";
                }
            }
            else
            {
                if(!board.isOnMe(t.x, t.y)){
                    if(board.getShowLocks()){
                        showLocksTime = initialShowLocksTime;
                    }
                    else board.setShowLocks(true);
                }
            }
        }

        if(board.getShowLocks()){
            showLocksTime -= elapsedTime;
            double s = showLocksTime/initialShowLocksTime;
            s = 1 + (s * 0.2);
            board.setBiggerCellScale(s);
            if(showLocksTime <= 0){
                board.setShowLocks(false);
                board.noBiggerCell();
                showLocksTime = initialShowLocksTime;
            }
        }

        if(board.wrongCell() == null){
            text = "Splendid";
            board.setFinished();
        }

        return null;
    }

    @Override
    public void init(OhNoApplication app) {
        boardSize = app.getBoardSize();
        board = new Board(boardSize, graphics.newImage("lock.png"));
        board.setForGame();
        text = null;
    }

    private int boardSize;

    private Board board;

    private MyFont font1,font2, font3;

    private Image imgClose,imgUnDo,imgEye;

    private ClickImage clickableClose,clickableUnDo, clickableEye;

    private String text;

    private double showLocksTime;
    private double initialShowLocksTime;
}
