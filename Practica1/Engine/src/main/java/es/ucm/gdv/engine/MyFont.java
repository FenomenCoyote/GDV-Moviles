package es.ucm.gdv.engine;

import java.awt.Font;
import java.io.FileInputStream;
import java.io.InputStream;

public interface MyFont {
    public void loadFont(String route, int size, boolean isBold);
}
