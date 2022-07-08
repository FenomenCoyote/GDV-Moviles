package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Pair;

import static es.ucm.gdv.pcohno.Board._dirs;

public class BoardUtils {

    /**
     * Looks for cell at row, col in direction to check how many cells does it see in that direction
     * @param direction
     * @param row
     * @param col
     * @param board
     * @return
     */
    public int lookDirection(Board.Dirs direction, int row, int col, Cell[][] board){
        Pair dir = _dirs[direction.ordinal()];

        int seeing = 0;
        row = row + dir.fst;
        col = col + dir.snd;

        while(board[row][col].getState() == Cell.State.Point){
            ++seeing;
            row += dir.fst;
            col += dir.snd;
        }
        return seeing;
    }

    /**
     * Looks in all direction of cell at row, col and returns how many cells that it sees
     * @param row
     * @param col
     * @param board
     * @return
     */
    public int lookDirections(int row, int col, Cell[][] board){
        int seeing = 0;

        //Look up
        seeing += lookDirection(Board.Dirs.Up, row, col, board);
        //Look down
        seeing+= lookDirection(Board.Dirs.Down, row, col, board);
        //Look left
        seeing += lookDirection(Board.Dirs.Left, row, col, board);
        //Look right
        seeing += lookDirection(Board.Dirs.Right, row, col, board);

        return seeing;
    }

    /**
     * Gets how many unassigned cells could potentially see in direction the cell at row, col
     * @param puzzle
     * @param direction
     * @param row
     * @param col
     * @return
     */
    public int posibleSeeingDirection(Cell[][] puzzle, Board.Dirs direction, int row, int col){
        Pair dir = _dirs[direction.ordinal()];

        int seeing = 0;
        row = row + dir.fst;
        col = col + dir.snd;

        while(puzzle[row][col].getState() != Cell.State.Wall){
            if(puzzle[row][col].getState() == Cell.State.Unassigned)
                ++seeing;
            row += dir.fst;
            col += dir.snd;
        }
        return seeing;
    }

    /**
     * Gets an array of 4 indicating how many does it see in that direction
     * @param row
     * @param col
     * @param puzzle
     * @return
     */
    public int[] getView(int row, int col, Cell[][] puzzle) {
        int[] view = new int[4];
        view[Board.Dirs.Up.ordinal()] = lookDirection(Board.Dirs.Up, row, col, puzzle);
        view[Board.Dirs.Down.ordinal()] = lookDirection(Board.Dirs.Down, row, col, puzzle);
        view[Board.Dirs.Left.ordinal()] = lookDirection(Board.Dirs.Left, row, col, puzzle);
        view[Board.Dirs.Right.ordinal()] = lookDirection(Board.Dirs.Right, row, col, puzzle);
        return view;
    }
}
