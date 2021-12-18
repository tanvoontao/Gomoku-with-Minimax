using System;
using SplashKitSDK;
using System.Collections.Generic;


namespace GomokuGame
{
    /// <summary>
    /// A derived class (White Piece) inherit from its base class (Piece) which which contain a field: _img,
    /// </summary>
    public class WhitePiece:Piece
    {
        // -------------------- Fields -------------------- //
        private Bitmap _img;


        // -------------------- Constructor -------------------- //
        /// <summary>
        /// A White Piece constructor which receive a parameter that represent the location of a piece
        /// </summary>
        /// <param name="location"></param>
        public WhitePiece(Point2D location):base(location){
            _img = SplashKit.LoadBitmap("WhitePiece", "Resources/white.png");
        }


        // -------------------- Properties -------------------- //
        // -------------------- Methods -------------------- //
        /// <summary>
        /// A method to draw a piece at its location and use its piece image
        /// </summary>
        public override void Draw(){
            SplashKit.DrawBitmap(_img, base.Location.X, base.Location.Y);
        }

        /// <summary>
        /// A method to return its piece type : White Piece
        /// </summary>
        /// <returns></returns>
        public override PieceType GetPieceType(){
            return PieceType.WHITE;
        }
    }
}