package es.ucm.gdv.pcohno;

import java.util.Random;

public class BoardGenerator {

    public BoardGenerator(int size, Board board, CellPool pool){
        this._size = size;
        this._board = board;
        this._utils = new BoardUtils();
        this._pool = pool;
    }

    /**
     * Creates a puzzle and gives it to Board
     */
    public void setForGame() {
        Cell[][] puzzle = new Cell[_size + 2][_size + 2];

        for (int i = 0; i < _size + 2; ++i){
            for (int j = 0; j < _size + 2; ++j){
                puzzle[i][j] = _pool.retrieveCell();
                puzzle[i][j].init(_board.getCell(i,j));
            }
        }


        Random rng = new Random();
        //Until a puzzle have been generated
        while(true) {
            clean(puzzle);
            randomize(puzzle, rng);
            if(!wrongInitialBoard(puzzle) && seesJustRight(puzzle)) {
                //Puzzle is built correctly
                prune(puzzle);
                _board.copyToBoard(puzzle);
                if (resolve(puzzle)) {
                    //Done
                    _board.setUnassignedCells(_board.countUnassignedCells());
                    _pool.releaseCell(puzzle);
                    return;
                }
            }
        }
    }

    /**
     * If the board is built incorrectly
     * @param puzzle
     * @return true if yes, false either
     */
    public boolean wrongInitialBoard(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                Cell c = puzzle[i][j];
                if(c.getState() == Cell.State.Point)
                    //If its placed incorrectly, returns true
                    if ((c.getLocked() && (_utils.lookDirections(i, j, puzzle) != c.getMustWatch())) ||
                            (!c.getLocked() && (_utils.lookDirections(i, j, puzzle) == 0)))
                        return true;
            }
        }
        //No mistakes were found
        return false;
    }

    /**
     * Checks if no blue point sees more than the board's size
     * @param puzzle
     * @return true if yes, false either
     */
    private boolean seesJustRight(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                if(puzzle[i][j].getState() == Cell.State.Point && !puzzle[i][j].getLocked())
                {
                    if(_utils.lookDirections(i, j, puzzle) > _size)
                        return false;
                }
            }
        }
        return true;
    }

    /**
     * Prunes the puzzle leaving a few walls and the locked points.
     * If later, the puzzle can be resolved, this will be the board the user has to complete
     * @param puzzle
     */
    private void prune(Cell[][] puzzle){
        Random rng = new Random();
        int changeWall = 4;
        if(_size > 6)
            changeWall = 3;

        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size +1; ++j){
                Cell c = puzzle[i][j];
                if(!(c.getLocked() && c.getState() == Cell.State.Point))
                    if(c.getState() == Cell.State.Wall && rng.nextInt(changeWall) > 0){
                        c.setLocked(false);
                        c.setState(Cell.State.Unassigned);
                    }
                    else if(c.getState() == Cell.State.Point)
                        c.setState(Cell.State.Unassigned);
            }
        }
    }

    /**
     * Prepares the puzzle for the next try
     * @param puzzle
     */
    private void clean(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                Cell c = puzzle[i][j];
                c.setLocked(false);
                c.setMustWatch(-1);
                c.setState(Cell.State.Unassigned);
            }
        }
    }

    /**
     * At random, generates walls and points in an attempt to create a valid puzzle
     * @param puzzle
     * @param rng
     */
    private void randomize(Cell[][] puzzle, Random rng){
        int rngWall = 2;
        int rngPoint = _size;
        for (int i = 1; i < _size + 1; i++) {
            for (int j = 1; j < _size + 1; j++) {
                Cell c = puzzle[i][j];
                if (c.getState() == Cell.State.Unassigned){
                    if (rng.nextInt(rngWall) > 0) { //Pared
                        c.setState(Cell.State.Wall);
                        c.setLocked(true);
                    }
                    else { //Punto importante
                        putImportantPoint(c, i, j, puzzle, rng);
                    }
                } else if (c.getState() == Cell.State.Point && rng.nextInt(rngPoint) == 0){
                    putImportantPoint(c, i, j, puzzle, rng);
                }
            }
        }
    }

    /**
     * Puts a locked point at i, j. Also puts blue points and walls at the end of them
     * @param c
     * @param i
     * @param j
     * @param puzzle
     * @param rng
     */
    private void putImportantPoint(Cell c, int i, int j, Cell[][] puzzle, Random rng){
        int seeingDir[] = _utils.getView(i, j, puzzle);
        int seeing = seeingDir[0] + seeingDir[1] + seeingDir[2] + seeingDir[3];

        int posibleDir[] = new int[4];

        posibleDir[Board.Dirs.Up.ordinal()] = _utils.posibleSeeingDirection(puzzle, Board.Dirs.Up ,i, j);
        posibleDir[Board.Dirs.Down.ordinal()]= _utils.posibleSeeingDirection(puzzle, Board.Dirs.Down ,i, j);
        posibleDir[Board.Dirs.Left.ordinal()] = _utils.posibleSeeingDirection(puzzle, Board.Dirs.Left ,i, j);
        posibleDir[Board.Dirs.Right.ordinal()] = _utils.posibleSeeingDirection(puzzle, Board.Dirs.Right ,i, j);
        int posible = posibleDir[0] + posibleDir[1] + posibleDir[2] + posibleDir[3];

        c.setState(Cell.State.Point);

        int extraWatch = posible == 0 ? 0 : rng.nextInt(posible);

        if(seeing + extraWatch == 0){
            c.setState(Cell.State.Wall);
            c.setLocked(true);
            return;
        }

        if(seeing + extraWatch > _size)
            extraWatch = _size - seeing;

        c.setMustWatch(seeing + extraWatch);
        c.setLocked(true);

        //expandir a puntitos alrededors
        while (extraWatch > 0){
            for (Board.Dirs d : Board.Dirs.values()) {
                if(posibleDir[d.ordinal()] == 0)
                    continue;

                int n = 1 + rng.nextInt(Math.min(extraWatch, posibleDir[d.ordinal()]));
                expand(puzzle, i, j, seeingDir[d.ordinal()] + n, d);
                seeingDir[d.ordinal()] += n;
                posibleDir[d.ordinal()] -= n;
                extraWatch -= n;

                if(extraWatch == 0)
                    break;
            }
        }
        for (Board.Dirs d : Board.Dirs.values()) {
            int k = seeingDir[d.ordinal()];
            puzzle[i + (k + 1) * Board._dirs[d.ordinal()].fst][j + (k + 1) * Board._dirs[d.ordinal()].snd].setState(Cell.State.Wall);
            puzzle[i + (k + 1) * Board._dirs[d.ordinal()].fst][j + (k + 1) * Board._dirs[d.ordinal()].snd].setLocked(true);
        }
    }

    /**
     * Expands blue point in dir direction
     * @param puzzle
     * @param i
     * @param j
     * @param k
     * @param dir
     */
    private void expand(Cell[][] puzzle, int i, int j, int k, Board.Dirs dir){
        Cell c;
        for (int l = 1; l <= k; l++) {
            c = puzzle[i + l * Board._dirs[dir.ordinal()].fst][j + l * Board._dirs[dir.ordinal()].snd];
            if(!c.getLocked())
                c.setState(Cell.State.Point);
        }
    }

    /**
     * Tries to solve the puzzle using hints
     * @param puzzle
     * @return true if puzzle can be solved, false either
     */
    private boolean resolve(Cell[][] puzzle){
        boolean changed = true;
        Hint puzzleHint = new Hint(puzzle);
        while(changed){
            changed = false;
            for (int i = 1; i < _size + 1; ++i){
                for (int j = 1; j < _size + 1; ++j){
                    CellHint h = null;
                    try {
                        h = puzzleHint.getPositiveHint(i, j);
                    } catch (Exception e){
                        e.printStackTrace();
                    }
                    if(h == null)
                        continue;
                    puzzle[h.pos.fst][h.pos.snd].setState(h.state);
                    changed = true;
                }
            }
        }
        return _board.wrongCell(puzzle) == null;
    }

    private final BoardUtils _utils;
    private int _size;
    private Board _board;
    private final CellPool _pool;

}
