package es.ucm.gdv.pcohno;

import java.util.ArrayDeque;
import java.util.Queue;

public class CellPool {
    /**
     * Does 256 new Cells once to create a pool
     */
    public CellPool() {
        this._availableCells = new ArrayDeque<>();
        //theorical maximum of cells needed is 11*11*2
        //11 is 9 (maximum board size) + 2 (exterior perma walls)
        //1 for the board and 1 for the generation
        //total: 242
        createNewCells(256);
    }

    /**
     * Gets a cell from the pool
     * If, for some reason, the pool is exhausted, it creates 8 more cells
     * @return
     */
    public Cell retrieveCell(){
        Cell c = _availableCells.poll();

        //safety measure
        if(c == null){
            createNewCells(8);
            c = _availableCells.poll();
        }

        return c;
    }

    /**
     * Releases cell in order to be used again in the future
     * @param c
     */
    public void releaseCell(Cell c){
        _availableCells.add(c);
    }

    /**
     * Releases entire cell matrix in order to use the cells again in the future
     * @param cellArray
     */
    public void releaseCell(Cell[][] cellArray){
        for (Cell[] row: cellArray) {
            for (Cell c: row) {
                _availableCells.add(c);
            }
        }
    }

    /**
     * Makes new n cells and adds them to the pool
     * @param n
     */
    private void createNewCells(int n){
        for (int i = 0; i < n; i++) {
            _availableCells.add(new Cell());
        }
    }

    private Queue<Cell> _availableCells;
}
