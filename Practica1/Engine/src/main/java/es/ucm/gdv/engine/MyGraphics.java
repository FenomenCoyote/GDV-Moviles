package es.ucm.gdv.engine;

public abstract class MyGraphics implements Graphics {

    protected void calculateTranslationScale(){
        //Ajustar el alto para que sea exacto al height
        float heightRelation = (float)height/logicalHeight;

        if(logicalWidth * heightRelation > width){ //Si el width es muy pequeño para eso, padding arriba y abajo
            scale = (float)width/logicalWidth;
            offsetX = 0;
            offsetY = (height-(int)(logicalHeight*scale))/2;
        }
        else { //Si el width es grande padding izquierda y derecha
            scale = heightRelation;
            offsetY = 0;
            offsetX = (width-(int)(logicalWidth*scale))/2;
        }
        translate(offsetX, offsetY);
        scale(scale, scale);
    }

    @Override
    public void setLogicalSize(int width, int height) {
        logicalWidth = width;
        logicalHeight = height;
    }

    protected int width, height;
    protected int logicalWidth, logicalHeight;

    protected int offsetX, offsetY;
    protected float scale;
}
