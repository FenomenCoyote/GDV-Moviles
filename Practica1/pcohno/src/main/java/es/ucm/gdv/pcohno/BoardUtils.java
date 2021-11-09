package es.ucm.gdv.pcohno;

import es.ucm.gdv.engine.Pair;

import static es.ucm.gdv.pcohno.Board._dirs;

public class BoardUtils {

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

    public int[] getView(int row, int col, Cell[][] puzzle) {
        int[] view = new int[4];
        view[0] = lookDirection(Board.Dirs.Up, row, col, puzzle);
        view[1] = lookDirection(Board.Dirs.Down, row, col, puzzle);
        view[2] = lookDirection(Board.Dirs.Left, row, col, puzzle);
        view[3] = lookDirection(Board.Dirs.Right, row, col, puzzle);
        return view;
    }
}
