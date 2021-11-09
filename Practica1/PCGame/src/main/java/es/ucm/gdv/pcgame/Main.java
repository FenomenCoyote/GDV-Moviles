package es.ucm.gdv.pcgame;

import es.ucm.gdv.pcengine.PCEngine;
import es.ucm.gdv.pcohno.OhNoApplication;

public class Main {
    public static void main(String[] args){

        PCEngine engine = new PCEngine();
        engine.setApplication(new OhNoApplication());
        engine.init();
        engine.run();
        engine.release();
    }
}