package es.ucm.gdv.pcengine;

import java.awt.Font;
import java.io.FileInputStream;
import java.io.InputStream;

import es.ucm.gdv.engine.MyFont;

public class PCFont implements MyFont {

    /**
     * Loads font from $rootDir/data/fonts/
     * @param route
     * @param size
     * @param isBold
     */
    public void loadFont(String route, int size, boolean isBold) {
        Font baseFont = null;
        try (InputStream is = new FileInputStream("data/fonts/" + route)) {
            baseFont = Font.createFont(Font.TRUETYPE_FONT, is);
        }
        catch (Exception e) {
            // Ouch. No está.
            System.err.println("Error cargando la fuente: " + e);
        }

        int bold = isBold ? 1 : 0;
        _font = baseFont.deriveFont(bold, size);
    }

    public Font getFont(){
        return _font;
    }

    private Font _font;
}
