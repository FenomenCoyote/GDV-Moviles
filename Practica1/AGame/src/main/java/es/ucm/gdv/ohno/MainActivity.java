package es.ucm.gdv.ohno;

import android.os.Bundle;

import androidx.appcompat.app.AppCompatActivity;

import es.ucm.gdv.aengine.AEngine;
import es.ucm.gdv.pcohno.OhNoApplication;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        _aEngine = new AEngine(this);
        _aEngine.setApplication(new OhNoApplication());
        _aEngine.init();

    }
    @Override
    protected void onResume() {

        // Avisamos a la vista (que es la encargada del active render)
        // de lo que está pasando.
        super.onResume();
        _aEngine.onResume();

    }
    @Override
    protected void onPause() {

        // Avisamos a la vista (que es la encargada del active render)
        // de lo que está pasando.
        super.onPause();
        _aEngine.onPause();

    }

    private AEngine _aEngine;
}