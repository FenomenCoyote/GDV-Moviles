package es.ucm.gdv.pcohno;

import com.sun.tools.javac.util.Pair;

public class Board {

    public Board(int size){
        _board = new Cell[size][size];
        _size = size;
        for (int i = 0; i < size; ++i){
            for (int j = 0; j < size; ++j){
                _board[i][j] = new Cell(i, j, size);
            }
        }
    }

    /**
     * @return pair of the first wrong cell. If all are correct, returns (-1, -1)
     */
    public Pair<Integer, Integer> wrongCell(){
        for (int i = 0; i < _size; ++i){
            for (int j = 0; j < _size; ++j){
                if(!_board[i][j].isRight(_board))
                    return new Pair<Integer, Integer>(i, j);
            }
        }
        return new Pair<Integer, Integer>(-1, -1);
    }

    public void getHint() {
        Cell hintCell;
        for (int i = 0; i < _size; ++i){
            for (int j = 0; j < _size; ++j){
                hintCell = _board[i][j].getHint();
                Pair<Integer, Integer> hintPos = hintCell.getPos();
                if(hintPos.fst != -1 && hintPos.snd != -1) {
                    _board[hintPos.fst][hintPos.snd] = hintCell;
                    return;
                }
            }
        }
    }

    public int getSize(){ return _size;}

    private Cell[][] _board;
    private int _size;
}