package es.ucm.gdv.pcohno;

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

        text = boardSize + " x " + boardSize;
        nextText = boardSize + " x " + boardSize;

        this.showLocksTime = showLocksTime;
        this.textAlpha = 0f;
        this.textAlphaSpeed = 5f;
        this.textTransition = true;
    }

    @Override
    public void render() {
        int alpha = (int)(alphaTransition * 255f);
        graphics.setColor((alpha << 24) | 0x00333333);
        graphics.setFont(font2);
        graphics.drawText(board.getPercentage() + "%",200,510);
        graphics.setColor(((int)(alpha * textAlpha) << 24) | 0x00333333);
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
        board.render(graphics, alphaTransition);

        graphics.scale(0.5f, 0.5f);
        clickableClose.render(graphics, alphaTransition);
        clickableUnDo.render(graphics, alphaTransition);
        clickableEye.render(graphics, alphaTransition);
    }

    @Override
    public OhNoApplication.State update(double elapsedTime) {
        OhNoApplication.State st = super.update(elapsedTime);
        if(st != null)
            return st;

        if(textTransition){
            textAlpha = Math.max(0f, Math.min(1f, textAlpha + (float)elapsedTime * textAlphaSpeed));
            if(textAlpha <= 0f){
                textAlphaSpeed = -textAlphaSpeed;
                setNextText();
            }
            else if(textAlpha >= 1f){
                textTransition = false;
                textAlphaSpeed = -textAlphaSpeed;
            }
        }

        Input.TouchEvent t = input.getTouchEvent();
        while(t != null){
            input.releaseEvent(t);
            if(t.type != Input.TouchEvent.TouchEventType.Touch){
                t = input.getTouchEvent();
                continue;
            }

            if(text != null){
                if(text == "Splendid")
                    setNextState(OhNoApplication.State.Menu);
                else{
                    if(!textTransition){
                        nextTextTransition();
                        nextText = null;
                    }

                    board.highlightCircle(0, 0, false);
                }
            }
            if(clickableClose.isOnMe(t.x * 2, t.y * 2)){
                input.clearEvents();
                setNextState(OhNoApplication.State.Menu);
            }
            else if(clickableEye.isOnMe(t.x * 2, t.y * 2))
            {
                CellHint hint = board.getHint();
                if(hint == null){
                    nextTextTransition();
                    nextText = "There is a mistake in the board";
                }
                else {
                    nextTextTransition();
                    nextText = getTextFromHint(hint);
                    board.highlightCircle(hint.pos.fst, hint.pos.snd, true);
                }
            }
            else if(clickableUnDo.isOnMe(t.x * 2, t.y * 2)) {
                CellHint hint = board.undo();
                if (hint != null){
                    switch (hint.state) {
                        case Unassigned:
                            nextTextTransition();
                            nextText = "This tile was reversed to its\nempty state";
                            break;
                        case Point:
                            nextTextTransition();
                            nextText = "This tile was reversed to blue";
                            break;
                        case Wall:
                            nextTextTransition();
                            nextText = "This tile was reversed to red";
                            break;
                    }
                    board.highlightCircle(hint.pos.fst, hint.pos.snd, true);
                }
                else {
                    nextTextTransition();
                    nextText = "Nothing to undo";
                }
            }
            else
            {
                board.isOnMe(t.x, t.y);
            }

            t = input.getTouchEvent();
        }

        board.update(elapsedTime);

        if(board.wrongCell() == null){
            if(text != "Splendid") {
                nextTextTransition();
                nextText = "Splendid";
                board.setFinished();
                board.highlightCircle(0, 0, false);
            }
        }
        else if (board.getPercentage() == 100){
            CellHint hint = board.getHint();

            String auxText = getTextFromHint(hint);
            if(!auxText.equals(nextText)){
                nextText = auxText;
                nextTextTransition();
            }
            board.highlightCircle(hint.pos.fst, hint.pos.snd, true);
        }

        return null;
    }

    private void nextTextTransition(){
        if(!textTransition){
            textTransition = true;
        }
    }

    private void setNextText(){
        this.text = this.nextText;
    }

    @Override
    public void init(OhNoApplication app) {
        boardSize = app.getBoardSize();
        board = new Board(boardSize, graphics.newImage("lock.png"), showLocksTime);
        boardGenerator = new BoardGenerator(boardSize, board);
        boardGenerator.setForGame();
        text = null;
    }

    private String getTextFromHint(CellHint hint){
        switch (hint.type) {
            case First:
                return "This number can see all its dots";
            case Second:
                return "Looking further in one direction\nwould exceed this number";
            case Third:
                return "One specific dot is included\nin all solutions imaginable";
            case Fourth:
                return "This number sees a bit too\nmuch";
            case Fifth:
                return "This number can't see enough";
            case Sixth:
                return "This one cant see anyone";
            case Seventh:
                return "A blue dot should always see\nat least one other";
        }
        return null;
    }

    private int boardSize;

    private Board board;
    private BoardGenerator boardGenerator;

    private MyFont font1,font2, font3;

    private Image imgClose,imgUnDo,imgEye;

    private ClickImage clickableClose,clickableUnDo, clickableEye;

    private double showLocksTime;
    private float textAlpha, textAlphaSpeed;
    private boolean textTransition;

    private String text, nextText;
}
