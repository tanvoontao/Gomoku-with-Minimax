using System;
using System.Collections.Generic;
using NUnit.Framework;
using SplashKitSDK;

namespace GomokuGame
{
    [TestFixture()]
    public class GomokuTest
    {
        [Test()]
        public void TestCheckHorizontallyWinner(){
            // test check winner if the pieces place horizontally
            GomokuGame gomokuGame = new GomokuGame();
            Point2D p = new Point2D();
            p.X = 0;
            p.Y = 0;
            Piece piece;
            

            for(int i = 0; i < 4; i++){
                p.X += 75;
                p.Y = 75;
                piece = gomokuGame.PlaceAPiece(p);
                gomokuGame.PlacePiece(piece);
                for(int z = 0; z < 1; z++){
                    p.Y = 150;
                    piece = gomokuGame.PlaceAPiece(p);
                    gomokuGame.PlacePiece(piece);
                }
            }
            p.X = 375;
            p.Y = 75;
            piece = gomokuGame.PlaceAPiece(p);
            gomokuGame.PlacePiece(piece);

            Assert.AreEqual(PieceType.BLACK, gomokuGame.Winner);

        }

        [Test()]
        public void TestCheckVerticallyWinner(){
            // test check winner if the pieces place vertically
            GomokuGame gomokuGame = new GomokuGame();
            Point2D p = new Point2D();
            p.X = 0;
            p.Y = 0;
            Piece piece;
            

            for(int i = 0; i < 4; i++){
                p.X = 75;
                p.Y += 75;
                piece = gomokuGame.PlaceAPiece(p);
                gomokuGame.PlacePiece(piece);
                for(int z = 0; z < 1; z++){
                    p.X = 150;
                    piece = gomokuGame.PlaceAPiece(p);
                    gomokuGame.PlacePiece(piece);
                }
            }
            p.X = 75;
            p.Y = 375;
            piece = gomokuGame.PlaceAPiece(p);
            gomokuGame.PlacePiece(piece);

            Assert.AreEqual(PieceType.BLACK, gomokuGame.Winner);
        }

        [Test()]
        public void TestCheckDiagonallyWinner(){
            // test check winner if the pieces place diagonally
            GomokuGame gomokuGame = new GomokuGame();
            Point2D p = new Point2D();
            p.X = 75;
            p.Y = 75;
            Piece piece;

            for(int i = 0; i < 4; i++){
                piece = gomokuGame.PlaceAPiece(p);
                gomokuGame.PlacePiece(piece);
                
                for(int z = 0; z < 1; z++){
                    p.X += 75;
                    piece = gomokuGame.PlaceAPiece(p);
                    gomokuGame.PlacePiece(piece);
                    p.Y += 75;
                }
            }
            p.X = 375;
            p.Y = 375;
            piece = gomokuGame.PlaceAPiece(p);
            gomokuGame.PlacePiece(piece);

            Assert.AreEqual(PieceType.BLACK, gomokuGame.Winner);
        }

        [Test()]
        public void TestCheckOnTheMiddleWinner(){
            // test check winner if the pieces place in the middle of 4 pieces, whether the function will turn around and check opposite direction
            GomokuGame gomokuGame = new GomokuGame();
            Point2D p = new Point2D();
            p.X = 75;
            p.Y = 75;
            Piece piece;

            for(int i = 0; i < 5; i++){

                if(p.X != 225 && p.Y != 225){
                    piece = gomokuGame.PlaceAPiece(p);
                    gomokuGame.PlacePiece(piece);
                }
                
                
                for(int z = 0; z < 1; z++){
                    p.X += 75;
                    
                    if(p.X != 300 && p.Y != 225){
                        piece = gomokuGame.PlaceAPiece(p);
                        gomokuGame.PlacePiece(piece);
                    }
                    
                    p.Y += 75;
                }
            }
            
            p.X = 225;
            p.Y = 225;
            piece = gomokuGame.PlaceAPiece(p);
            gomokuGame.PlacePiece(piece);

            Assert.AreEqual(PieceType.BLACK, gomokuGame.Winner);
        }
    }
}

