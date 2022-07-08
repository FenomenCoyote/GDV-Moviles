package es.ucm.gdv.engine;

public abstract class MyGraphics implements Graphics {

    /**
     * Adds bands at the top and at the bottom, or in both sides
     * to keep a logicalHeight/logicalWidth relation
     */
    protected void calculateTranslationScale(){
        //Adjust high to be exact to logicalHeight
        float heightRelation = (float) _height / _logicalHeight;
        if(_logicalWidth * heightRelation > _width){ //If width is too small, bands on top and bottom
            _scale = (float) _width / _logicalWidth;
            _offsetX = 0;
            _offsetY = (_height -(int)(_logicalHeight * _scale))/2;
        }
        else { //If width is wide enough, bands on the left and the right
            _scale = heightRelation;
            _offsetY = 0;
            _offsetX = (_width -(int)(_logicalWidth * _scale))/2;
        }
        translate(_offsetX, _offsetY);
        scale(_scale, _scale);
    }

    /**
     * Logical size setter
     * @param width
     * @param height
     */
    @Override
    public void setLogicalSize(int width, int height) {
        _logicalWidth = width;
        _logicalHeight = height;
    }

    public int getOffsetX() {
        return _offsetX;
    }

    public int getOffsetY() {
        return _offsetY;
    }

    public float getScale() {
        return _scale;
    }

    protected int _width, _height;
    protected int _logicalWidth, _logicalHeight;

    protected int _offsetX, _offsetY;
    protected float _scale;
}
