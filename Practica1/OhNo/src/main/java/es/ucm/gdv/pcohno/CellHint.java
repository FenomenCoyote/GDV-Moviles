package es.ucm.gdv.pcohno;


import es.ucm.gdv.engine.Pair;

public class CellHint{

    public CellHint(Cell.State hintState, int row, int col){
        this(hintState, row, col, Hint.Type.First);
    }

    public CellHint(Cell.State hintState, int row, int col, Hint.Type t){
        this.state = hintState;
        this.pos = new Pair(row, col);
        this.type = t;
    }

    public CellHint(){
        this.state = Cell.State.Unassigned;
        this.pos = new Pair(-1, -1);
    }

    public Hint.Type type;
    public Cell.State state;
    public Pair pos;
}