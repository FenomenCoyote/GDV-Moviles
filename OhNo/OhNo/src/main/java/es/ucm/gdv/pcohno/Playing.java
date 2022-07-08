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

        this._font1 = graphics.newFont("JosefinSans-Bold.ttf",64,false);
        this._font2 = graphics.newFont("JosefinSans-Bold.ttf",16,false);
        this._font3 = graphics.newFont("JosefinSans-Bold.ttf",26,false);
        this._font4 = graphics.newFont("JosefinSans-Bold.ttf",36,false);

        this._imgClose = graphics.newImage("close.png");
        this._imgUnDo = graphics.newImage("history.png");
        this._imgEye = graphics.newImage("eye.png");

        this._clickableClose = new ClickImage(this._imgClose, 150 - this._imgClose.getWidth()/2, 1050, 100, 100, 0.5f);
        this._clickableUnDo = new ClickImage(this._imgUnDo, 400 - this._imgUnDo.getWidth()/2, 1050, 100, 100, 0.5f);
        this._clickableEye = new ClickImage(this._imgEye, 650 - this._imgEye.getWidth()/2, 1050, 100, 100, 0.5f);

        this._boardSize = 4;
        this._text = this._boardSize + " x " + this._boardSize;
        this._nextText = this._boardSize + " x " + this._boardSize;

        this._showLocksTime = showLocksTime;
        this._textAlpha = 0f;
        this._textAlphaSpeed = 5f;
        this._textTransition = true;

        this._board = null;
        this._boardGenerator = null;
        this._pool = new CellPool();
    }

    /**
     * Called every frame after update
     */
    @Override
    public void render() {
        int alpha = (int)(_alphaTransition * 255f);

        //Bottom Percentage
        _graphics.setColor((alpha << 24) | 0x00333333);
        _graphics.setFont(_font2);
        _graphics.drawText(_board.getPercentage() + "%",200,510);

        //Top title
        _graphics.setColor(((int)(alpha * _textAlpha) << 24) | 0x00333333);
        _graphics.setFont(_font1);

        //If _text is null, means no special text is needed, thus we render size x size
        if(_text == null)
            _graphics.drawText(_boardSize + " x " + _boardSize, 200, 80);
        else {
            _graphics.save();
            if(_text == "Splendid")
                _graphics.setFont(_font4);
            else
                _graphics.setFont(_font3);
            //Draw text lines separated by '\n' character
            String texts[] = _text.split("\n");
            for (String t_ : texts) {
                _graphics.drawText(t_, 200, 60 );
                _graphics.translate(0, 25);
            }
            _graphics.restore();
        }
        //Render the board
        _graphics.setFont(_font1);
        _board.render(_graphics, _alphaTransition);

        //Render the bottom images
        _graphics.scale(0.5f, 0.5f);
        _clickableClose.render(_graphics, _alphaTransition);
        _clickableUnDo.render(_graphics, _alphaTransition);
        _clickableEye.render(_graphics, _alphaTransition);
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

        updateTextTransition(elapsedTime);

        updateInputs();

        //Updates board for the animations timer
        _board.update(elapsedTime);

        //If its gameOver
        if(_board.wrongCell() == null){
            if(_text != "Splendid") {
                nextTextTransition();
                _nextText = "Splendid";
                _board.setFinished();
                _board.highlightCircle(0, 0, false);
            }
        }
        //If there are no unassigned cells yet there is a mistake
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
     * Checks for every input if there is something to do
     */
    private void updateInputs(){
        Input.TouchEvent t = _input.getTouchEvent();
        while(t != null){
            _input.releaseEvent(t);
            //Only accept Touch events
            if(t.type != Input.TouchEvent.TouchEventType.Touch){
                t = _input.getTouchEvent();
                continue;
            }

            //Nothing special setted
            if(_text != null){
                //Game over
                if(_text == "Splendid")
                    setNextState(OhNoApplication.State.Menu);
                else {
                    //Fade in to default text
                    if(!_textTransition){
                        nextTextTransition();
                        _nextText = null;
                    }
                    _board.highlightCircle(0, 0, false);
                }
            }

            if(_clickableClose.isOnMe(t.x * 2, t.y * 2))
                onClose();
            else if(_clickableEye.isOnMe(t.x * 2, t.y * 2))
                onEye();
            else if(_clickableUnDo.isOnMe(t.x * 2, t.y * 2))
                onUndo();
            else
                _board.isOnMe(t.x, t.y);

            //Get next event
            t = _input.getTouchEvent();
        }
    }

    /**
     * When user hits close image
     */
    private void onClose(){
        _input.clearEvents();
        setNextState(OhNoApplication.State.Menu);
    }

    /**
     * When user hits hint image
     */
    private void onEye(){
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

    /**
     * When user hits undo image
     */
    private void onUndo(){
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

    /**
     * Texts fade in/out animations
     * @param elapsedTime
     */
    private void updateTextTransition(double elapsedTime){
        if(_textTransition) {
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
        _board = new Board(_boardSize, _graphics.newImage("lock.png"), _showLocksTime, _pool);
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

    private MyFont _font1, _font2, _font3, _font4;

    private Image _imgClose, _imgUnDo, _imgEye;

    private ClickImage _clickableClose, _clickableUnDo, _clickableEye;

    private double _showLocksTime;
    private float _textAlpha, _textAlphaSpeed;
    private boolean _textTransition;

    private String _text, _nextText;
}
