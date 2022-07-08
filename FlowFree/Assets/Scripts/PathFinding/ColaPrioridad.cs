/*
 Cola de prioridad basada en el video https://www.youtube.com/watch?v=3Dw5d7PlcTM&ab_channel=SebastianLague
 y adaptada a la aplicacion de un laberinto de nodos
*/

namespace flow
{
    /// <summary>
    /// Cola de prioridad que ordena los Nodos de menor a mayor Heuristica
    /// </summary>
    public class ColaPrioridad
    {
        Nodo[] nodos;
        int numElementos;

        /// <summary>
        /// Constructora de clase
        /// </summary>
        /// <param name="tamanioCola">Tamanio maximo de la cola</param>
        public ColaPrioridad(int tamanioCola)
        {
            nodos = new Nodo[tamanioCola];
            numElementos = 0;
        }

        /// <summary>
        /// Introduce un Nodo a la cola de prioridad
        /// </summary>
        /// <param name="nuevoNodo"></param>
        public void Introducir(Nodo nuevoNodo)
        {
            nuevoNodo.indiceCola = numElementos;
            nodos[numElementos] = nuevoNodo;
            Flota(nuevoNodo);
            ++numElementos;
        }

        /// <summary>
        /// Devuelve el elemento mas prioritario de la cola y lo elimina
        /// </summary>
        /// <returns></returns>
        public Nodo EliminarPrimero()
        {
            Nodo primerNodo = nodos[0];
            --numElementos;
            nodos[0] = nodos[numElementos];
            nodos[0].indiceCola = 0;
            Hundir(nodos[0]);
            return primerNodo;
        }

        /// <summary>
        /// Flota un nodo para reubicarlo en la cola de prioridad
        /// </summary>
        /// <param name="nodo"></param>
        public void ActualizaNodo(Nodo nodo)
        {
            Flota(nodo);
        }

        /// <summary>
        /// Devuelve el numero de nodos que se encuentran en la cola actualmente
        /// </summary>
        /// <returns></returns>
        public int NumeroElementos()
        {
            return numElementos;
        }

        /// <summary>
        /// Devuelve true si contiene el nodo por parametro
        /// </summary>
        /// <param name="nodo">nodo a comparar</param>
        /// <returns></returns>
        public bool Contiene(Nodo nodo)
        {
            return Equals(nodos[nodo.indiceCola], nodo);
        }

        /// <summary>
        /// Hunde un elemento para reubicarlo en la cola de prioridad 
        /// </summary>
        /// <param name="nodo"></param>
        void Hundir(Nodo nodo)
        {
            while (true)
            {
                int indiceHijoIzq = nodo.indiceCola * 2 + 1;
                int indiceHijoDer = nodo.indiceCola * 2 + 2;
                int indiceCambio = 0;

                if (indiceHijoIzq < numElementos)
                {
                    indiceCambio = indiceHijoIzq;

                    if (indiceHijoDer < numElementos)
                    {
                        if (nodos[indiceHijoIzq].Heuristica() > nodos[indiceHijoDer].Heuristica())
                        {
                            indiceCambio = indiceHijoDer;
                        }
                    }

                    if (nodo.Heuristica() > nodos[indiceCambio].Heuristica())
                    {
                        Intercambia(nodo, nodos[indiceCambio]);
                    }
                    else return;
                }
                else return;
            }
        }

        /// <summary>
        /// Flota un nodo para reubicarlo en la cola
        /// </summary>
        /// <param name="nodo"></param>
        void Flota(Nodo nodo)
        {
            int indicePadre = (nodo.indiceCola - 1) / 2;

            while (true)
            {
                Nodo nodoPadre = nodos[indicePadre];
                if (nodo.Heuristica() < nodoPadre.Heuristica())
                {
                    Intercambia(nodo, nodoPadre);
                }
                else break;

                indicePadre = (nodo.indiceCola - 1) / 2;
            }

        }

        /// <summary>
        /// Intercambia dos nodos en la cola de prioridad
        /// </summary>
        /// <param name="nodoA"></param>
        /// <param name="nodoB"></param>
        void Intercambia(Nodo nodoA, Nodo nodoB)
        {
            nodos[nodoA.indiceCola] = nodoB;
            nodos[nodoB.indiceCola] = nodoA;
            int indiceNodoA = nodoA.indiceCola;
            nodoA.indiceCola = nodoB.indiceCola;
            nodoB.indiceCola = indiceNodoA;
        }
    }
}
