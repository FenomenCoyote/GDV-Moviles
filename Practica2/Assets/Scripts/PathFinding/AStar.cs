using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    /// <summary>
    /// Pathfinding 
    /// </summary>
    public class AStar 
    {
        /// <summary>
        /// Estructura que representa el laberinto
        /// </summary>
        private Nodo[,] laberinto;

        private Color color;

        private int lilPenalize=1, bigPenalize=20;

        /// <summary>
        /// Recibe tanto el laberinto generado como la dimension de sus casilla
        /// </summary>
        /// <param name="laberinto_"></param>
        public void RecibeLaberinto(Tile[,] laberinto_, int lilPenalize, int bigPenalize)
        {
            laberinto = new Nodo[laberinto_.GetLength(0), laberinto_.GetLength(1)];

            for (int i = 0; i < laberinto_.GetLength(0); ++i)
            {
                for (int j = 0; j < laberinto_.GetLength(1); ++j)
                {
                    laberinto[i, j] = new Nodo(laberinto_[i, j], i, j);
                }
            }

            this.lilPenalize = lilPenalize;
            this.bigPenalize = bigPenalize;
        }

        /// <summary>
        /// Algoritmo que genera un camino desde una posicion origen hasta una destino a traves del algoritmo A*
        /// </summary>
        /// <param name="ori"> Origen a partir del cual se va a calcular el camino</param>
        /// <param name="dest"> Destino del camino que se va a generar</param>

        /// <returns></returns>
        public List<Vector2> Astrella(Vector2 ori, Vector2 dest, Color color)
        {
            this.color = color;

            ColaPrioridad listaAbierta = new ColaPrioridad(laberinto.GetLength(0) * laberinto.GetLength(1));
            HashSet<Nodo> nodosCerrados = new HashSet<Nodo>();

            Vector2 origen = ori;
            Vector2 destino = dest;

            listaAbierta.Introducir(laberinto[(int)origen.x, (int)origen.y]);

            while (listaAbierta.NumeroElementos() > 0)
            {
                Nodo nodoActual = listaAbierta.EliminarPrimero();
                nodosCerrados.Add(laberinto[nodoActual.x, nodoActual.y]);

                if (nodoActual.x == destino.x && nodoActual.y == destino.y)
                {
                    break;
                }

                RelajaAdyacentes(nodoActual, laberinto[(int)destino.x, (int)destino.y], ref laberinto, ref listaAbierta, ref nodosCerrados);
            }

            
            return ConstruyeRecorrido(origen, destino);
        }

        /// <summary>
        /// A partir de las posiciones origen y destino se genera una lista de vector3 que supongan las posiciones del camino que debo seguir
        /// </summary>
        /// <param name="origen"> Origen del camino</param>
        /// <param name="destino">Destino al que debo llegar</param>
        /// <returns></returns>
        private List<Vector2> ConstruyeRecorrido(Vector2 origen, Vector2 destino)
        {
            List<Vector2> recorrido = new List<Vector2>();

            Nodo caminoActual = laberinto[(int)destino.x, (int)destino.y];
            while (caminoActual != laberinto[(int)origen.x, (int)origen.y])
            {
                recorrido.Add(new Vector2(caminoActual.x, caminoActual.y));
                caminoActual = caminoActual.anterior;
            }
            recorrido.Add(new Vector2(caminoActual.x, caminoActual.y));
            recorrido.Reverse();
            
            return recorrido;
        }

        /// <summary>
        /// Analizo los nodos adyacentes e intento añadirlos a la lista abierta para analizarlos en futuras iteraciones
        /// </summary>
        /// <param name="origen"> Nodo origen</param>
        /// <param name="destino"> Destino al que quiero llegar</param>
        /// <param name="laberinto"> Estriuctura que almacena el laberinto</param>
        /// <param name="listaAbierta"> Lista de nodos por analizar</param>
        /// <param name="nodosCerrados"> Lista de nodos analizados y/o descartados</param>
        private void RelajaAdyacentes(Nodo origen, Nodo destino, ref Nodo[,] laberinto, ref ColaPrioridad listaAbierta, ref HashSet<Nodo> nodosCerrados)
        {
            List<Nodo> candidatos = new List<Nodo>();

            if (origen.x + 1 < laberinto.GetLength(0) && !nodosCerrados.Contains(laberinto[origen.x + 1, origen.y]) && 
                laberinto[origen.x, origen.y].abajo && canGoTo(laberinto[origen.x + 1, origen.y])) candidatos.Add(laberinto[origen.x + 1, origen.y]);    //derecha
            
            if (origen.x - 1 >= 0 && !nodosCerrados.Contains(laberinto[origen.x - 1, origen.y]) && 
                laberinto[origen.x, origen.y].arriba && canGoTo(laberinto[origen.x - 1, origen.y])) candidatos.Add(laberinto[origen.x - 1, origen.y]);   //izquierda
            
            if (origen.y + 1 < laberinto.GetLength(1) && !nodosCerrados.Contains(laberinto[origen.x, origen.y + 1]) && 
                laberinto[origen.x, origen.y].derecha && canGoTo(laberinto[origen.x, origen.y + 1])) candidatos.Add(laberinto[origen.x, origen.y + 1]);    //abajo
            
            if (origen.y - 1 >= 0 && !nodosCerrados.Contains(laberinto[origen.x, origen.y - 1]) && 
                laberinto[origen.x, origen.y].izquierda && canGoTo(laberinto[origen.x, origen.y - 1])) candidatos.Add(laberinto[origen.x, origen.y - 1]);   //arriba

            int nuevaDistancia = origen.distanciaComienzo + 1;
            
            foreach (Nodo adyacente in candidatos)
            {
                if (adyacente.distanciaComienzo > nuevaDistancia || !listaAbierta.Contiene(adyacente))
                {
                    adyacente.distanciaComienzo = nuevaDistancia;

                    adyacente.distanciaDestino = Distancia(destino, adyacente);

                    if (adyacente.tile.getColor() != color && adyacente.tile.isActive())
                        adyacente.distanciaDestino += lilPenalize;
                    if (adyacente.initialOrEnd)
                        adyacente.distanciaDestino += bigPenalize;

                    adyacente.anterior = origen;
                    
                    if (!listaAbierta.Contiene(adyacente))
                    {
                        listaAbierta.Introducir(adyacente);
                    }
                }
            }
        }


        private bool canGoTo(Nodo d)
        {
            return !(d.initialOrEnd && d.tile.getColor() != color);
        }
  
        /// <summary>
        /// Distancia entre dos nodos
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int Distancia(Nodo a, Nodo b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

    }
}