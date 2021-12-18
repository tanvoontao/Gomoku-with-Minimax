using System;
using SplashKitSDK;

namespace GomokuGame
{
    /// <summary>
    /// A Button class which contain several fields: _width, _height, _x, _y, _txt, and _btnImg
    /// </summary>
    public class Button
    {
        // -------------------- Fields -------------------- //
        private int _width, _height;
        private float _x, _y;
        private Color _color;
        private string _txt;
        private Bitmap _btnImg;


        // -------------------- Constructor -------------------- //
        /// <summary>
        /// A constructor which initialize the width, height, x, y, text on the button, name of img, path of the img
        /// </summary>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="txt"></param>
        public Button(Color color, float x, float y, int width, int height, string txt, string name, string filename){
            _btnImg = SplashKit.LoadBitmap(name, filename);
            _color = color;
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _txt = txt;
        }

        
        // -------------------- Properties -------------------- //
        /// <summary>
        /// A property which able to modify and view the text on the button
        /// </summary>
        /// <value></value>
        public string Txt{
            get { return _txt; }
            set { _txt = value; }
        }


        // -------------------- Methods -------------------- //
        /// <summary>
        /// A method to draw a button
        /// </summary>
        public void Draw(){
            SplashKit.DrawBitmap(_btnImg, _x-2, _y-2);
        }

        /// <summary>
        /// A method to check whether the mouse position within the button
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool IsAt(Point2D pt)
        {
            Rectangle rect = new Rectangle();

            rect.X = _x;
            rect.Y = _y;
            rect.Width = _width;
            rect.Height = _height;

            if(SplashKit.PointInRectangle(pt, rect)){
                SplashKit.PlaySoundEffect(SplashKit.SoundEffectNamed("place_piece"));
                return true;
            }else{
                return false;
            }
        }

    }
}