namespace flow
{
    public class Nodo
    {
        public int x;
        public int y;

        public bool arriba;
        public bool abajo;
        public bool izquierda;
        public bool derecha;

        public int distanciaDestino;
        public int distanciaComienzo;

        public Nodo anterior;
        public int indiceCola;

        public Tile tile;

        public bool initialOrEnd;

        public Nodo(Tile t, int x, int y)
        {
            arriba = !t.hasWall(Logic.Dir.Up);
            abajo = !t.hasWall(Logic.Dir.Down);
            izquierda = !t.hasWall(Logic.Dir.Left);
            derecha = !t.hasWall(Logic.Dir.Right);

            this.tile = t;
            this.initialOrEnd = t.isInitialOrEnd();

            this.x = x;
            this.y = y;
        }

        public int Heuristica()
        {
            return distanciaComienzo + distanciaDestino;
        }
    }
}
