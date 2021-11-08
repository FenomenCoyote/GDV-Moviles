package es.ucm.gdv.pcohno;

import java.util.Random;

import es.ucm.gdv.engine.Pair;

public class BoardGenerator {

    public BoardGenerator(int size, Board board){
        _size = size;
        _board = board;
    }

    public void setForGame() {
        Cell[][] puzzle = new Cell[_size + 2][_size + 2];

        for (int i = 0; i < _size + 2; ++i){
            for (int j = 0; j < _size + 2; ++j){
                puzzle[i][j] = new Cell(_board.getCell(i,j));
            }
        }

        //Hasta que haya un puzzle que tenga solucion
        while(true) {
            clean(puzzle);
            randomize(puzzle);
            if(!wrongInitialBoard(puzzle)) {
                prune(puzzle);
                _board.copyToBoard(puzzle);
                if (resolve(puzzle)) { //Si se puede resolver
                    _board.setUnassignedCells(_board.countUnassignedCells());
                    return;
                }
            }
        }
    }

    private void prune(Cell[][] puzzle){
        Random rng = new Random();
        int changeWall = 4;
        if(_size >= 6)
            changeWall = 3;
        else if(_size >= 8)
            changeWall = 2;
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

    public boolean wrongInitialBoard(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                Cell c = puzzle[i][j];
                if(c.getState() == Cell.State.Point)
                    return (c.getLocked() && (_board.lookDirections(i, j, puzzle) > c.getMustWatch())) ||
                            (!c.getLocked() && (_board.lookDirections(i, j, puzzle) > 0));
            }
        }
        return false;
    }

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

    private void randomize(Cell[][] puzzle){
        Random rng = new Random();
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

    private void putImportantPoint(Cell c, int i, int j, Cell[][] puzzle, Random rng){
        int seeingDir[] = new int[4];

        seeingDir[Board.Dirs.Up.ordinal()] = _board.lookDirection(Board.Dirs.Up ,i, j, puzzle);
        seeingDir[Board.Dirs.Down.ordinal()]  = _board.lookDirection(Board.Dirs.Down ,i, j, puzzle);
        seeingDir[Board.Dirs.Left.ordinal()]  = _board.lookDirection(Board.Dirs.Left ,i, j, puzzle);
        seeingDir[Board.Dirs.Right.ordinal()]  = _board.lookDirection(Board.Dirs.Right ,i, j, puzzle);
        int seeing = seeingDir[0] + seeingDir[1] + seeingDir[2] + seeingDir[3];

        int posibleDir[] = new int[4];

        posibleDir[Board.Dirs.Up.ordinal()] = _board.posibleSeeingDirection(puzzle, Board.Dirs.Up ,i, j);
        posibleDir[Board.Dirs.Down.ordinal()]= _board.posibleSeeingDirection(puzzle, Board.Dirs.Down ,i, j);
        posibleDir[Board.Dirs.Left.ordinal()] = _board.posibleSeeingDirection(puzzle, Board.Dirs.Left ,i, j);
        posibleDir[Board.Dirs.Right.ordinal()] = _board.posibleSeeingDirection(puzzle, Board.Dirs.Right ,i, j);
        int posible = posibleDir[0] + posibleDir[1] + posibleDir[2] + posibleDir[3];

        if(seeing + posible == 0){
            c.setState(Cell.State.Wall);
            c.setLocked(true);
            return;
        }

        c.setState(Cell.State.Point);

        int extraWatch = Math.max(1, rng.nextInt(_size + 1));

        if(seeing + extraWatch > _size)
            extraWatch = _size - seeing;

        if(extraWatch > posible)
            extraWatch = posible;

        c.setMustWatch(seeing + extraWatch);
        c.setLocked(true);

        //expandir a puntitos alrededors
        int tries = 100;
        while (extraWatch > 0 && tries-- > 0){
            for (Board.Dirs d : Board.Dirs.values()) {
                int n = rng.nextInt(Math.min(posibleDir[d.ordinal()] + 1, extraWatch + 1));
                if(n == 0 && extraWatch == 1 && posibleDir[d.ordinal()] > 0)
                    n = 1;
                expand(puzzle, i, j, seeingDir[d.ordinal()] + n, d);
                seeingDir[d.ordinal()] += n;
                posibleDir[d.ordinal()] -= n;
                extraWatch -= n;
            }
        }
        if(tries < 0){
            System.out.print(".");
            return;
        }
    }

    private void expand(Cell[][] puzzle, int i, int j, int k, Board.Dirs dir){
        Cell c;
        Pair[] dirs = _board.getDirs();
        for (int l = 1; l <= k; l++) {
            c = puzzle[i + l * dirs[dir.ordinal()].fst][j + l * dirs[dir.ordinal()].snd];
            if(!c.getLocked())
                c.setState(Cell.State.Point);
        }
        puzzle[i + (k + 1) * dirs[dir.ordinal()].fst][j + (k + 1) * dirs[dir.ordinal()].snd].setState(Cell.State.Wall);
        puzzle[i + (k + 1) * dirs[dir.ordinal()].fst][j + (k + 1) * dirs[dir.ordinal()].snd].setLocked(true);
    }

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
        //print(puzzle);
        return _board.wrongCell(puzzle) == null;
    }


    int _size;
    Board _board;
}
