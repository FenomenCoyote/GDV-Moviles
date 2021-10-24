package es.ucm.gdv.pcengine;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.io.FileInputStream;
import java.io.InputStream;

public class PCFont {

    private final Font _font;

    public PCFont(String font){
        Font baseFont = null;
        try (InputStream is = new FileInputStream("Bangers-Regular.ttf")) {
            baseFont = Font.createFont(Font.TRUETYPE_FONT, is);
        }
        catch (Exception e) {
            // Ouch. No está.
            System.err.println("Error cargando la fuente: " + e);
        }

        // baseFont contiene el tipo de letra base en tamaño 1. La
        // usamos como punto de partida para crear la nuestra, más
        // grande y en negrita.
        _font = baseFont.deriveFont(Font.BOLD, 40);
    }

    public void renderText(java.awt.Graphics graphics, String text, int posX, int posY, Color color){
        if (_font != null) {
            graphics.setColor(color);
            graphics.setFont(_font);
            graphics.drawString(text, posX, posY);
        }
    }
}
