package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Graphics;
import es.ucm.gdv.engine.Image;
import es.ucm.gdv.engine.Input;
import es.ucm.gdv.engine.MyFont;

/**
 * Class for Playing state
 */
public class Playing extends State {

    /**
     * Constructor
     * @param graphics object for rendering
     * @param input object to get input events
     * @param showLocksTime time to show locks when clicking a locked cell
     */
    public Playing(Graphics graphics, Input input, double showLocksTime) {
        super(graphics, input);

        _font1 = graphics.newFont("JosefinSans-Bold.ttf",64,false);
        _font2 = graphics.newFont("JosefinSans-Bold.ttf",16,false);
        _font3 = graphics.newFont("JosefinSans-Bold.ttf",26,false);

        _imgClose = graphics.newImage("close.png");
        _imgUnDo = graphics.newImage("history.png");
        _imgEye = graphics.newImage("eye.png");

        clickableClose = new ClickImage(_imgClose, 150 - _imgClose.getWidth()/2, 1050, 100, 100, 0.5f);
        clickableUnDo = new ClickImage(_imgUnDo, 400 - _imgUnDo.getWidth()/2, 1050, 100, 100, 0.5f);
        clickableEye = new ClickImage(_imgEye, 650 - _imgEye.getWidth()/2, 1050, 100, 100, 0.5f);

        _boardSize = 4;
        _text = _boardSize + " x " + _boardSize;
        _nextText = _boardSize + " x " + _boardSize;

        this._showLocksTime = showLocksTime;
        this._textAlpha = 0f;
        this._textAlphaSpeed = 5f;
        this._textTransition = true;

        this._pool = new CellPool();
    }

    /**
     * Called every frame after update
     */
    @Override
    public void render() {
        int alpha = (int)(alphaTransition * 255f);
        graphics.setColor((alpha << 24) | 0x00333333);
        graphics.setFont(_font2);
        graphics.drawText(_board.getPercentage() + "%",200,510);
        graphics.setColor(((int)(alpha * _textAlpha) << 24) | 0x00333333);
        graphics.setFont(_font1);
        if(_text == null)
            graphics.drawText(_boardSize + " x " + _boardSize, 200, 80);
        else{
            graphics.save();
            if(_text == "Splendid")
                graphics.setFont(_font2);
            graphics.setFont(_font3);
            String texts[] = _text.split("\n");
            for (String t_ : texts) {
                graphics.drawText(t_, 200, 60 );
                graphics.translate(0, 25);
            }
            graphics.restore();
        }
        graphics.setFont(_font1);
        _board.render(graphics, alphaTransition);

        graphics.scale(0.5f, 0.5f);
        clickableClose.render(graphics, alphaTransition);
        clickableUnDo.render(graphics, alphaTransition);
        clickableEye.render(graphics, alphaTransition);
    }

    /**
     * Called every frame
     * @param elapsedTime time since last frame
     * @return returns next state if game should change to it, null either
     */
    @Override
    public OhNoApplication.State update(double elapsedTime) {
        OhNoApplication.State st = super.update(elapsedTime);
        if(st != null)
            return st;

        if(_textTransition){
            _textAlpha = Math.max(0f, Math.min(1f, _textAlpha + (float)elapsedTime * _textAlphaSpeed));
            if(_textAlpha <= 0f){
                _textAlphaSpeed = -_textAlphaSpeed;
                setNextText();
            }
            else if(_textAlpha >= 1f){
                _textTransition = false;
                _textAlphaSpeed = -_textAlphaSpeed;
            }
        }

        Input.TouchEvent t = input.getTouchEvent();
        while(t != null){
            input.releaseEvent(t);
            if(t.type != Input.TouchEvent.TouchEventType.Touch){
                t = input.getTouchEvent();
                continue;
            }

            if(_text != null){
                if(_text == "Splendid")
                    setNextState(OhNoApplication.State.Menu);
                else{
                    if(!_textTransition){
                        nextTextTransition();
                        _nextText = null;
                    }

                    _board.highlightCircle(0, 0, false);
                }
            }
            if(clickableClose.isOnMe(t.x * 2, t.y * 2)){
                input.clearEvents();
                setNextState(OhNoApplication.State.Menu);
            }
            else if(clickableEye.isOnMe(t.x * 2, t.y * 2))
            {
                CellHint hint = _board.getHint();
                if(hint == null){
                    nextTextTransition();
                    _nextText = "There is a mistake in the board";
                }
                else {
                    nextTextTransition();
                    _nextText = getTextFromHint(hint);
                    _board.highlightCircle(hint.pos.fst, hint.pos.snd, true);
                }
            }
            else if(clickableUnDo.isOnMe(t.x * 2, t.y * 2)) {
                CellHint hint = _board.undo();
                if (hint != null){
                    switch (hint.state) {
                        case Unassigned:
                            nextTextTransition();
                            _nextText = "This tile was reversed to its\nempty state";
                            break;
                        case Point:
                            nextTextTransition();
                            _nextText = "This tile was reversed to blue";
                            break;
                        case Wall:
                            nextTextTransition();
                            _nextText = "This tile was reversed to red";
                            break;
                    }
                    _board.highlightCircle(hint.pos.fst, hint.pos.snd, true);
                }
                else {
                    nextTextTransition();
                    _nextText = "Nothing to undo";
                }
            }
            else
            {
                _board.isOnMe(t.x, t.y);
            }

            t = input.getTouchEvent();
        }

        _board.update(elapsedTime);

        if(_board.wrongCell() == null){
            if(_text != "Splendid") {
                nextTextTransition();
                _nextText = "Splendid";
                _board.setFinished();
                _board.highlightCircle(0, 0, false);
            }
        }
        else if (_board.getPercentage() == 100){
            CellHint hint = _board.getHint();

            String auxText = getTextFromHint(hint);
            if(!auxText.equals(_nextText)){
                _nextText = auxText;
                nextTextTransition();
            }
            _board.highlightCircle(hint.pos.fst, hint.pos.snd, true);
        }

        return null;
    }

    /**
     * Initiates transition to next text using animation
     */
    private void nextTextTransition(){
        if(!_textTransition){
            _textTransition = true;
        }
    }

    /**
     * Sets text to next one
     */
    private void setNextText(){
        this._text = this._nextText;
    }

    /**
     * Called before first update
     * @param app
     */
    @Override
    public void init(OhNoApplication app) {
        _boardSize = app.getBoardSize();
        if(_board != null)
            _board.release(_pool);
        _board = new Board(_boardSize, graphics.newImage("lock.png"), _showLocksTime, _pool);
        _boardGenerator = new BoardGenerator(_boardSize, _board, _pool);
        _boardGenerator.setForGame();
        _text = null;
    }

    /**
     * Change state depending on hint
     * @param hint hint to know type
     * @return text to show according to hint type
     */
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

    private int _boardSize;

    private Board _board;
    private BoardGenerator _boardGenerator;
    private CellPool _pool;

    private MyFont _font1, _font2, _font3;

    private Image _imgClose, _imgUnDo, _imgEye;

    private ClickImage clickableClose,clickableUnDo, clickableEye;

    private double _showLocksTime;
    private float _textAlpha, _textAlphaSpeed;
    private boolean _textTransition;

    private String _text, _nextText;
}
