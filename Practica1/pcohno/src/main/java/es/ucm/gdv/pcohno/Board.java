package es.ucm.gdv.pcohno;

import com.sun.tools.javac.util.Pair;

import java.util.Random;

import es.ucm.gdv.engine.Graphics;

public class Board {

    static Pair<Integer, Integer>[] _dirs = new Pair[]{
            new Pair(-1, 0),
            new Pair(1, 0),
            new Pair(0, -1),
            new Pair(0, 1),
    };

    static enum Dirs {Up, Down, Left, Right}

    public Board(int size){
        _board = new Cell[size + 2][size + 2];

        _size = size;
        this._hint = new Hint(_board);

        for (int i = 0; i < size + 2; ++i){
            for (int j = 0; j < size + 2; ++j){
                _board[i][j] = new Cell();
                if(i == 0 || j == 0 || i == size + 1 || j == size + 1){
                    _board[i][j].setState(Cell.State.Wall);
                    _board[i][j].setLocked(true);
                }
            }
        }

    }

    public void setForGame() {

/*        _board[2][1].setState(Cell.State.Wall);
        _board[2][1].setLocked(true);

        _board[1][2].setState(Cell.State.Wall);
        _board[1][2].setLocked(true);

        _board[2][3].setState(Cell.State.Point);
        _board[2][3].setMustWatch(2);
        _board[2][3].setLocked(true);

        _board[3][2].setState(Cell.State.Point);
        _board[3][2].setMustWatch(1);
        _board[3][2].setLocked(true);

        _board[4][3].setState(Cell.State.Point);
        _board[4][3].setMustWatch(2);
        _board[4][3].setLocked(true);

        _board[4][4].setState(Cell.State.Point);
        _board[4][4].setMustWatch(4);
        _board[4][4].setLocked(true);*/

        Cell[][] puzzle = new Cell[_size + 2][_size + 2];

        for (int i = 0; i < _size + 2; ++i){
            for (int j = 0; j < _size + 2; ++j){
                puzzle[i][j] = new Cell(_board[i][j]);
            }
        }

        //Hasta que haya un puzzle que tenga solucion
        while(true) {
            clean(puzzle);
            randomize(puzzle);
            if(!wrongInitialBoard(puzzle)) {
                print(puzzle);
                prune(puzzle);
                copyToBoard(puzzle);
                print(_board);
                if (resolve(puzzle)) { //Si se puede resolver
                    return;
                }
            }
        }
    }

    private void prune(Cell[][] puzzle){
        Random rng = new Random();
        int changeWall = 2;
        int changePoint = 3;
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size +1; ++j){
                Cell c = puzzle[i][j];
                if(!(c.getLocked() && c.getState() == Cell.State.Point))
                    if(c.getState() == Cell.State.Wall && rng.nextInt(changeWall) == 0)
                        c.setState(Cell.State.Unassigned);
                    else if(c.getState() == Cell.State.Point && rng.nextInt(changePoint) == 0)
                        c.setState(Cell.State.Unassigned);
            }
        }
    }

    public void setForGame2(){
        Cell[][] puzzle = new Cell[_size + 2][_size + 2];

        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size +1; ++j){
                puzzle[i][j] = new Cell(_board[i][j]);
            }
        }

        Random rng = new Random();
        do{
            clean(puzzle);
            int points = 0;
            int walls = 0;
            int numWalls = (_size*_size)/8 + rng.nextInt((_size*_size)/3);
            int maxPoints = (_size*_size)/4 + rng.nextInt((_size*_size)/2);

            print(puzzle);

            for(int i=1; i< _size+1; i++){
                for(int j=1; j<_size+1; j++){
                    Cell c = puzzle[i][j];
                    if(rng.nextFloat() <= 0.8f || points >= maxPoints){
                        c.setState(Cell.State.Wall);
                        walls++;
                    }
                    else{
                        c.setState(Cell.State.Point);
                        c.setMustWatch(1 + rng.nextInt((_size - 1) * 2));
                        points++;
                    }
                    c.setLocked(true);
                }
            }
            print(puzzle);
            if(walls < numWalls || wrongInitialBoard(puzzle)){
                continue;
            }

            while(walls > numWalls){
                int posX = 1 + rng.nextInt(_size);
                int posY = 1 + rng.nextInt(_size);
                if(puzzle[posX][posY].getState() == Cell.State.Wall) {
                    puzzle[posX][posY].setLocked(false);
                    puzzle[posX][posY].setState(Cell.State.Unassigned);
                    walls--;
                }
                print(puzzle);
            }

            print(puzzle);
        } while(!resolve(puzzle));
    }


    public void render(Graphics graphics) {
        graphics.save();

        int spacing = 105;
        float aux = 380 / (float)(spacing * _size);
        graphics.scale(aux, aux);
        graphics.translate(((spacing + 20) / 2) , +spacing/2);

        for(int i = 0; i < _size; ++i){
            for (int j = 0; j < _size; j++) {
                _board[i + 1][j + 1].render(graphics);
                graphics.translate(spacing, 0);
            }
            graphics.translate(-spacing * _size, spacing);
        }

        graphics.restore();
    }

    public void isOnMe(int x, int y){

        int spacing = 100;
        float aux = 400 / (float)(spacing * _size);

        x /= aux;
        y /= aux;
        x -= ((spacing + 0) / 2) * aux;
        y -= spacing * aux + spacing / 2 * aux;

        for(int i = 0; i < _size; ++i){
            for (int j = 0; j < _size; j++) {
                if(_board[i + 1][j + 1].isOnMe(x, y)){
                    System.out.print("celda: " + (i) + ", " + (j) + '\t');
                    return;
                }
                x -= spacing;
            }
            x += spacing * _size * aux;
            y -= spacing * aux;
        }

    }


    public void print() {
        print(_board);
    }

    /**
     * @return pair of the first wrong cell. If all are correct, returns (-1, -1)
     */
    public Pair<Integer, Integer> wrongCell() {
        return wrongCell(_board);
    }

    public Pair<Integer, Integer> wrongCell(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                if(!isCellRight(i, j, puzzle))
                    return new Pair<Integer, Integer>(i, j);
            }
        }
        return null;
    }

    public boolean wrongInitialBoard(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                Cell c = puzzle[i][j];
                if(c.getState() == Cell.State.Point)
                    return (c.getLocked() && (lookDirections(i, j, puzzle) > c.getMustWatch())) ||
                            (!c.getLocked() && (lookDirections(i, j, puzzle) > 0));
            }
        }
        return false;
    }

    private boolean isCellRight(int row, int col, Cell[][] puzzle){
        Cell c = puzzle[row][col];
        if(c.getLocked() && c.getState() == Cell.State.Point)
            return c.getMustWatch() == lookDirections(row, col, puzzle);
        else if(c.getState() == Cell.State.Point)
            return lookDirections(row, col, puzzle) > 0;
        else
            return c.getState() == Cell.State.Wall;
    }

    //Devolver nueva celda puesta o una cela mal muesta
    public void getHint() {
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j){
                CellHint h = null;
                try {
                    h = _hint.getPositiveHint(i, j);
                } catch (Exception e){
                    e.printStackTrace();
                }
                if(h == null)
                    continue;
                _board[h.pos.fst][h.pos.snd].setState(h.state);
                return;
            }
        }
    }

    private int lookDirections(int row, int col, Cell[][] puzzle){
        int seeing = 0;

        //Look up
        seeing += lookDirection(Dirs.Up, row, col, puzzle);
        //Look down
        seeing+= lookDirection(Dirs.Down, row, col, puzzle);
        //Look left
        seeing += lookDirection(Dirs.Left, row, col, puzzle);
        //Look right
        seeing += lookDirection(Dirs.Right, row, col, puzzle);

        return seeing;
    }

    private int posibleSeeingDirection(Cell[][] puzzle, Dirs direction, int row, int col){
        Pair<Integer,Integer> dir = _dirs[direction.ordinal()];

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

    private int lookDirection(Dirs direction, int row, int col){
        return lookDirection(direction, row, col, _board);
    }

    private int lookDirection(Dirs direction, int row, int col, Cell[][] board){
        Pair<Integer,Integer> dir = _dirs[direction.ordinal()];

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

        for (int i = 1; i < _size + 1; i++) {
            for (int j = 1; j < _size + 1; j++) {

                Cell c = puzzle[i][j];

                if(c.getState() != Cell.State.Unassigned)
                    continue;

                if(rng.nextInt(rngWall) == 0){ //Pared
                    c.setState(Cell.State.Wall);
                    c.setLocked(true);
                }
                else{ //Punto importante

                    int seeingDir[] = new int[4];

                    seeingDir[Dirs.Up.ordinal()] = lookDirection(Dirs.Up ,i, j, puzzle);
                    seeingDir[Dirs.Down.ordinal()]  = lookDirection(Dirs.Down ,i, j, puzzle);
                    seeingDir[Dirs.Left.ordinal()]  = lookDirection(Dirs.Left ,i, j, puzzle);
                    seeingDir[Dirs.Right.ordinal()]  = lookDirection(Dirs.Right ,i, j, puzzle);
                    int seeing = seeingDir[0] + seeingDir[1] + seeingDir[2] + seeingDir[3];

                    int posibleDir[] = new int[4];

                    posibleDir[Dirs.Up.ordinal()] = posibleSeeingDirection(puzzle, Dirs.Up ,i, j);
                    posibleDir[Dirs.Down.ordinal()]= posibleSeeingDirection(puzzle, Dirs.Down ,i, j);
                    posibleDir[Dirs.Left.ordinal()] = posibleSeeingDirection(puzzle, Dirs.Left ,i, j);
                    posibleDir[Dirs.Right.ordinal()] = posibleSeeingDirection(puzzle, Dirs.Right ,i, j);
                    int posible = posibleDir[0] + posibleDir[1] + posibleDir[2] + posibleDir[3];

                    if(seeing + posible == 0){
                        c.setState(Cell.State.Wall);
                        c.setLocked(true);
                        continue;
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
                    int tries = 10;
                    while (extraWatch > 0 && tries-- > 0){
                        for (Dirs d : Dirs.values()) {
                            int n = rng.nextInt(Math.min(posibleDir[d.ordinal()] + 1, extraWatch + 1));
                            expand(puzzle, i, j, seeingDir[d.ordinal()] + n, d);
                            seeingDir[d.ordinal()] += n;
                            posibleDir[d.ordinal()] -= n;
                            extraWatch -= n;
                        }
                    }
                    if(tries < 0)
                        return;
                }
            }
        }

        /*
        int randomCells = _size + rng.nextInt(_size/2);
        for(int i = 0; i < randomCells; ++i){
            int posX, posY;
            posX = 1 + rng.nextInt(_size);
            posY = 1 + rng.nextInt(_size);
            Cell c = puzzle[posX][posY];
            if(c.getLocked()){
                --i;
                continue;
            }
            if(rng.nextInt(randomCells) == 0){
                c.setState(Cell.State.Wall);
            }
            else {
                c.setState(Cell.State.Point);
                c.setMustWatch(1 + rng.nextInt((_size - 1) * 2));
            }
            c.setLocked(true);
        }*/
    }

    private void expand(Cell[][] puzzle, int i, int j, int k, Dirs dir){
        Cell c;
        for (int l = 1; l <= k; l++) {
             c = puzzle[i + l * _dirs[dir.ordinal()].fst][j + l * _dirs[dir.ordinal()].snd];
             if(!c.getLocked())
                 c.setState(Cell.State.Point);
        }
        puzzle[i + (k + 1) * _dirs[dir.ordinal()].fst][j + (k + 1) * _dirs[dir.ordinal()].snd].setState(Cell.State.Wall);
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
        return wrongCell(puzzle) == null;
    }

    private void copyToBoard(Cell[][] puzzle){
        for (int i = 1; i < _size + 1; ++i){
            for (int j = 1; j < _size + 1; ++j) {
                _board[i][j].setLocked(false);
                _board[i][j].setState(puzzle[i][j].getState());
                _board[i][j].setMustWatch(puzzle[i][j].getMustWatch());
                _board[i][j].setLocked(puzzle[i][j].getLocked());
            }
        }
    }

    private void print(Cell[][] board) {
        System.out.println("=============================");
        for (int i = 1; i < _size + 1; ++i) {
            for (int j = 1; j < _size + 1; ++j) {
                switch (board[i][j].getState()){
                    case Null:
                        break;
                    case Unassigned:
                        System.out.print("?");
                        break;
                    case Point:
                        if(board[i][j].getLocked())
                            System.out.print(board[i][j].getMustWatch());
                        else
                            System.out.print("o");
                        break;
                    case Wall:
                        System.out.print("x");
                        break;
                }
                System.out.print('\t');
            }
            System.out.println("\n");
        }
    }

    private Cell[][] _board;
    private int _size;
    private Hint _hint;
}