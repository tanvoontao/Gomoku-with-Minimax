using System;
using SplashKitSDK;
using System.Collections.Generic;


namespace GomokuGame
{
    /// <summary>
    /// A base class of each pieces
    /// </summary>
    public abstract class Piece
    {
        // -------------------- Fields -------------------- //
        private Point2D _location;


        // -------------------- Constructor -------------------- //
        /// <summary>
        /// A constructor which receive a parameter that represent the location of the piece
        /// </summary>
        /// <param name="location"></param>
        public Piece(Point2D location){
            _location = location;
            _location.X = location.X - 50/2;
            _location.Y = location.Y - 50/2;
        }


        // -------------------- Properties -------------------- //
        /// <summary>
        /// A read-only property which return the location of the piece
        /// </summary>
        /// <value></value>
        public Point2D Location{
            get { return _location; }
        }


        // -------------------- Methods -------------------- //
        /// <summary>
        /// An abstract method to draw the piece
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// A abstract method to return the current piece type
        /// </summary>
        /// <returns></returns>
        public abstract PieceType GetPieceType();
    }
}