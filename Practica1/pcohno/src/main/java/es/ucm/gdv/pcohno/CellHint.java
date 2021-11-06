package es.ucm.gdv.pcohno;

import com.sun.tools.javac.util.Pair;

public class CellHint{

    public CellHint(Cell.State hintState, int row, int col){
        this(hintState, row, col, Hint.Type.First);
    }

    public CellHint(Cell.State hintState, int row, int col, Hint.Type t){
        this.state = hintState;
        this.pos = new Pair<Integer, Integer>(row, col);
        this.type = t;
    }

    public CellHint(){
        this.state = Cell.State.Unassigned;
        this.pos = new Pair<Integer, Integer>(-1, -1);
    }

    public Hint.Type type;
    public Cell.State state;
    public Pair<Integer, Integer> pos;
}