package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Pair;

/**
 * Class which contains hint info: position of cell and state
 * it should be on (null if the hint means something is wrong)
 */
public class CellHint{

    /**
     * Constructor
     * @param hintState: initial hint state
     * @param row: cell row on board
     * @param col: cell column on board
     */
    public CellHint(Cell.State hintState, int row, int col){
        this(hintState, row, col, Hint.Type.First);
    }

    /**
     * Constructor
     * @param hintState: initial hint state
     * @param row: cell row on board
     * @param col: cell column on board
     * @param t: type(number of hint)
     */
    public CellHint(Cell.State hintState, int row, int col, Hint.Type t){
        this.state = hintState;
        this.pos = new Pair(row, col);
        this.type = t;
    }

    /**
     * Default constructor
     */
    public CellHint(){
        this.state = Cell.State.Unassigned;
        this.pos = new Pair(-1, -1);
        this.type = Hint.Type.First;
    }

    /**
     * number of hint
     */
    public Hint.Type type;

    /**
     * state cell should be on
     * (null if hint means something is wrong)
     */
    public Cell.State state;

    /**
     * position of the cell the hint is based on
     */
    public Pair pos;
}