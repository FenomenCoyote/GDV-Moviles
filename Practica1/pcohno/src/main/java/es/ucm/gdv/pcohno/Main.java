package es.ucm.gdv.pcohno;

public class Main {
    public static void main(String[] args){
        Board board = new Board(3);
        board.setForGame();
        board.print();
        int a = 4;
        while(a-- > 0){
            board.getHint();
            board.print();
        }
    }
}
