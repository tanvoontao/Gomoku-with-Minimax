using System;
using SplashKitSDK;
using System.Collections.Generic;

namespace GomokuGame
{
    /// <summary>
    /// A Board class which contain several fields: _pieces, _boardImg, _offset, _nodeRadius, _nodeDistance, _noMatchNode, _2dPieces, _nodeCount, _lastPlacedNode
    /// </summary>
    public class Board
    {
        // -------------------- Fields -------------------- //
        private List<Piece> _pieces;
        private Bitmap _boardImg;
        private int _offset;
        private int _nodeRadius;
        private int _nodeDistance;
        private int _nodeCount;
        private Point2D _noMatchNode;
        private Piece[,] _2dPieces;
        private Point2D _lastPlacedNode;
        private Point2D _lastPlacedNodeSIM;
        
        


        // -------------------- Constructor -------------------- //
        /// <summary>
        /// A constructor which initialize the board information below:
        /// _boardImg -> Resources/board.png
        /// _offset, _nodeDistance -> 75
        /// _nodeRadius -> 10
        /// _2dPieces -> [9,9]
        /// _nodeCount = 9
        /// </summary>
        public Board(){
            _pieces = new List<Piece>();
            _boardImg = SplashKit.LoadBitmap("board", "Resources/board.png");

            _offset = 75;
            _nodeRadius = 10;
            _nodeDistance = 75;
            _nodeCount = 9;
            _noMatchNode = new Point2D();
            _noMatchNode.X = _noMatchNode.Y = -1;
            
            _2dPieces = new Piece[_nodeCount,_nodeCount];

            _lastPlacedNode = _noMatchNode;

            _lastPlacedNodeSIM = _noMatchNode;
        }


        // -------------------- Properties -------------------- //
        /// <summary>
        /// A read-only properties which return the id of the last place node
        /// </summary>
        /// <value></value>
        public Point2D LastPlacedNode{
            get { return _lastPlacedNode; }
        }

        /// <summary>
        /// A read-only properties which return the total of node on one line
        /// </summary>
        /// <value></value>
        public int NodeCount{
            get { return _nodeCount; }
        }

        public Point2D LastPlacedNodeSIM{
            get { return _lastPlacedNodeSIM; }
        }
        public Piece[,] TwoDPieces{
            get { return _2dPieces; }
        }


        // -------------------- Methods -------------------- //
        /// <summary>
        /// A method to draw the board img, and each pieces
        /// </summary>
        public void Draw(){
            SplashKit.DrawBitmap(_boardImg, 0, 0);
            foreach(Piece p in _pieces){
                p.Draw();
            }
        }

        /// <summary>
        /// A method to add the piece object into a list of pieces
        /// </summary>
        /// <param name="p"></param>
        public void PlacePiece(Piece p){
            _pieces.Add(p);
        }

        /// <summary>
        /// A method to place the piece on the certain node after finding the closest that able to  be placed
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Piece PlaceAPiece(Point2D loc, PieceType type, bool isAITurn){
            
            // find the closest node
            Point2D nodeId = FindtheClosestNode(loc);
            int nodeIdX = Convert.ToInt32(nodeId.X);
            int nodeIdY = Convert.ToInt32(nodeId.Y);

            // if can't find (no match node) -> return null (no piece can place)
            if(nodeId.X == _noMatchNode.X)
                return null;
            
            // if found, check if the piece exist
            // if piece exist -> return null (no piece can place)
            if(_2dPieces[nodeIdX, nodeIdY] != null)
                return null;
            

            // based on type, create piece (black/white) object
            Point2D pos = ConvertToWindowPosition(nodeId);
            
            if(type == PieceType.BLACK){
                _2dPieces[nodeIdX, nodeIdY] = new BlackPiece(pos);
            }else if(type == PieceType.WHITE){
                _2dPieces[nodeIdX, nodeIdY] = new WhitePiece(pos);
            }

            // record the last placed node
            _lastPlacedNodeSIM = nodeId;
            if(!isAITurn){
                _lastPlacedNode = nodeId;
            }

            return _2dPieces[nodeIdX, Convert.ToInt32(nodeId.Y)];
            
        }

        /// <summary>
        /// A method to convert the node Id into a proper x and y coordinate
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public Point2D ConvertToWindowPosition(Point2D nodeId){
            // aim: board coordinate -> window coordinate
            Point2D loc = new Point2D();
            loc.X = nodeId.X * _nodeDistance + _offset;
            loc.Y = nodeId.Y * _nodeDistance + _offset;
            return loc;
        }

        /// <summary>
        /// A method to check whether node can be place a piece or not
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool CanBePlaced(Point2D loc){
            
            // find the closest node
            Point2D nodeId= FindtheClosestNode(loc);

            // if can't find -> return false
            if(nodeId.X == _noMatchNode.X)
                return false;
            
            // if found -> check if the piece exist
            // since our board is 2d 9x9 -> if exceed tha range 0-8 --> error
            if(_2dPieces[Convert.ToInt32(nodeId.X), Convert.ToInt32(nodeId.Y)] != null)
                return false;

            return true;
        }
        
        /// <summary>
        /// A method to find the closet node that can be place by using the location give (check 2D)
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Point2D FindtheClosestNode(Point2D loc){
            
            double nodeIdX = FindtheClosestNode(loc.X);
            if(nodeIdX == -1 || nodeIdX >= _nodeCount)
                return _noMatchNode;
            
            double nodeIdY = FindtheClosestNode(loc.Y);
            if(nodeIdY == -1 || nodeIdY >= _nodeCount)
                return _noMatchNode;

            Point2D matchNode = new Point2D();
            matchNode.X = nodeIdX;
            matchNode.Y = nodeIdY;

            return matchNode;
        }

        /// <summary>
        /// A method to find the closest node that can be place using the x or y given (check 1D)
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public double FindtheClosestNode(double pos){ // 1D View

            /*
                Imagine this is the board: (in 1D view)

                offset     distance
                    |  |   |   |
                    |  |---#---|---
                    |  |   |   |
                
                # -> piece
                x - offset / distance
                quotient -> the left node number
                remainder -> the distance from left node point 
            */

            
            if(pos < _offset - _nodeRadius)
                return -1;
            

            pos = pos - _offset;

            double quotient = pos / _nodeDistance;
            quotient = (int)quotient;

            double remainder = pos % _nodeDistance;
            remainder = (int)remainder;


            if(remainder <= _nodeRadius){
                return quotient;
            }else if(remainder >= (_nodeDistance - _nodeRadius) ){
                return quotient+1;
            }else{
                return -1;
            }
        }

        /// <summary>
        /// A method to get the piece type on the selected node
        /// </summary>
        /// <param name="nodeIdX"></param>
        /// <param name="nodeIdY"></param>
        /// <returns></returns>
        public PieceType GetPieceType(int nodeIdX, int nodeIdY){
            if( _2dPieces[nodeIdX, nodeIdY] == null ){
                return PieceType.NONE;
            }else{
                return _2dPieces[nodeIdX, nodeIdY].GetPieceType();
            }
        }

        /// <summary>
        /// Delete all the pieces on the board
        /// </summary>
        public void ClearPieces(){
            _pieces.Clear();
        }

        /// <summary>
        /// Reset all the board information as initialization
        /// </summary>
        public void Reset(){
            _lastPlacedNode = _noMatchNode;
            _lastPlacedNodeSIM = _noMatchNode;
            _2dPieces = new Piece[_nodeCount,_nodeCount];
        }

        /// <summary>
        /// A method to return random position on the board, that ltr be able use to place the piece automatically
        /// </summary>
        /// <returns></returns>
        public Point2D AutoPlay(){

            Random rX = new Random();
            Random rY = new Random();

            int posX = rX.Next(_nodeDistance-5, _nodeDistance*9+5);
            int posY = rY.Next(_nodeDistance-5, _nodeDistance*9+5);

            Point2D pt = new Point2D();
            pt.X = posX;
            pt.Y = posY;

            bool chkPos = IsTherePlaces();

            // check if the position can be place, if not, then find other random number
            if(chkPos){
                while (!CanBePlaced(pt)){
                    posX = rX.Next(_nodeDistance-5, _nodeDistance*9+5);
                    posY = rY.Next(_nodeDistance-5, _nodeDistance*9+5);
                    pt.X = posX;
                    pt.Y = posY;
                }
                return pt;
            }else{
                return _noMatchNode;
            }

        }


        /// <summary>
        /// A method to return true if there is still have place can place piece
        /// </summary>
        /// <returns></returns>
        public bool IsTherePlaces(){
            // check if the board is full of pieces -> if not, then skip
            foreach(Piece p in _2dPieces){
                if(p == null){
                    return true;
                }
            }
            return false;
        }
    }
}

