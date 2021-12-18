using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.IO;


namespace GomokuGame
{
    /// <summary>
    /// A GomokuGame class which responsible to handle the logic of this game, and contain several fields: _board, _currPlayer, _btn, _autoPlayAvalable, and _gameRecordsFilePath
    /// </summary>
    public class GomokuGame
    {
        // -------------------- Fields -------------------- //
        private Board _board;
        private PieceType _currPlayer;
        private PieceType _winner;
        private bool _autoPlayAvalable;
        private string _gameRecordsFilePath;
        private GameTimer _blackPieceTimer;
        private GameTimer _whitePieceTimer;
        private SoundEffect _placePieceSound;
        private Dictionary<string, int> _score;


        // -------------------- Constructor -------------------- //
        /// <summary>
        /// A GomokuGame constrcutor to initialize the information of a game
        /// </summary>
        public GomokuGame(){
            _board = new Board();
            _currPlayer = PieceType.BLACK;
            _winner = PieceType.NONE;
            _autoPlayAvalable = false;
            _gameRecordsFilePath = "Resources/gameRecords.txt";

            _blackPieceTimer = new GameTimer(30, "BlackPieceTimer", 600,690);
            _whitePieceTimer = new GameTimer(30,"WhitePieceTimer",600,0);
            _placePieceSound = SplashKit.SoundEffectNamed("place_piece");

            _score = new Dictionary<string, int>(){
                {"FiveInRow",   10000000},
                {"LiveFour",    10000},
                {"LiveThree",   100},
                {"LiveTwo",     10}
            };
        }


        // -------------------- Properties -------------------- //
        /// <summary>
        /// A read-only property which return the winner of the game
        /// </summary>
        /// <value></value>
        public PieceType Winner{
            get { return _winner; }
            set { _winner = value; }
        }

        /// <summary>
        /// A read-only property which return the current player of the game
        /// </summary>
        /// <value></value>
        public PieceType CurrPlayer{
            get { return _currPlayer; }
        }

        /// <summary>
        /// A property which able to modify and view whether the auto play feature available: by default is not (false)
        /// </summary>
        /// <value></value>
        public bool AutoPlayAvailable{
            get { return _autoPlayAvalable; }
            set { _autoPlayAvalable = value; }
        }



        // -------------------- Methods -------------------- //


        /// <summary>
        /// A method to check whether the position can be place
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool CanBePlaced(Point2D loc){
            return _board.CanBePlaced(loc);
        }

        /// <summary>
        /// A method to place a piece, before check the winner, and finally toggle the current player
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Piece PlaceAPiece(Point2D loc){
            Piece piece = _board.PlaceAPiece(loc, _currPlayer, false);
            if(piece != null){
                
                checkWinner();
                // toggle piece type
                if(_currPlayer == PieceType.BLACK){
                    _currPlayer = PieceType.WHITE;
                }else if(_currPlayer == PieceType.WHITE){
                    _currPlayer = PieceType.BLACK;
                }
                return piece;
            }
            return null;
        }

        /// <summary>
        /// A method to draw the board or the winner screen
        /// </summary>
        public void Draw(){
            _board.Draw();
            if(!_autoPlayAvalable){
                if(_currPlayer == PieceType.BLACK){
                SplashKit.DrawText("It's BLACK now! ", Color.Black, SplashKit.FontNamed("font"), 30, 300,690);
                }else{
                    SplashKit.DrawText("It's WHITE now! ", Color.White, SplashKit.FontNamed("font"), 30, 300,0);
                }
                _blackPieceTimer.Draw();
                _whitePieceTimer.Draw();
            }
        }

        /// <summary>
        /// A method to add piece to a list of pieces, and play and place piece sound effect
        /// </summary>
        /// <param name="p"></param>
        public void PlacePiece(Piece p){
            SplashKit.PlaySoundEffect(_placePieceSound);
            _board.PlacePiece(p);
        }

        /// <summary>
        /// A method to restart the game, by initializing the game and board information
        /// </summary>
        public void Restart(){
            _board.ClearPieces();
            _board.Reset();
            _winner = PieceType.NONE;
            _currPlayer = PieceType.BLACK;
            _blackPieceTimer.Reset();
            _whitePieceTimer.Reset();
        }

       
        /// <summary>
        /// A method to let the computer to place a piece
        /// </summary>
        /// <returns></returns>
        public Piece ComputerPlaceAPiece(){
            Point2D p = _board.AutoPlay();
            if(p.X == -1){
                return null;
            }
            Piece pp = PlaceAPiece(p);
            return pp;
        }

        /// <summary>
        /// A method for player to login after checking whether the player exist in the txt file
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Profile Login(string playerId){
            // if player exist -> return profile obj -> login
            
            Profile PlayerExist = IsPlayerExist(playerId);
            if(PlayerExist != null){
                return PlayerExist;
            }else{
                return null;
            }
        }

        /// <summary>
        /// A method for player to register after checking whether the player exist in the txt file
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Profile Register(string playerId){
            // if player exist -> return null -> can't register
            Profile PlayerExist = IsPlayerExist(playerId);
            if(PlayerExist != null){
                return null;
            }else{
                Profile p = new Profile(playerId, 0, 0);
                string playerDetails = $"{p.PlayerId},{p.TotalGames},{p.NoOfWins}\n";
                File.AppendAllText(_gameRecordsFilePath, playerDetails);
                return p;
            }
        }

        /// <summary>
        /// A method for game to update the player details such as total games every time after round
        /// </summary>
        /// <param name="player"></param>
        public void UpdatePlayerDetails(Profile player){
            player.UpdateTotalGames();
            if(_winner == PieceType.BLACK){
                player.UpdateNoOfWins();
            }

            string[] records = File.ReadAllLines(_gameRecordsFilePath);
            string[] recordsToStore = new string[records.Length];

            int i =  0;

            foreach(string record in records){
                
                string[] recordList = record.Split(',');
                if(recordList[0] == player.PlayerId){
                    recordList[1] = Convert.ToString(player.TotalGames);
                    recordList[2] = Convert.ToString(player.NoOfWins);
                }
                string r = string.Join(',',recordList);
                recordsToStore[i++] = r;
            }
            
            File.WriteAllLines(_gameRecordsFilePath, recordsToStore);
        }

        /// <summary>
        /// A method to check whether the player exist in the txt file
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Profile IsPlayerExist(string playerId){
            string[] records = File.ReadAllLines(_gameRecordsFilePath);
            foreach(string record in records){
                string[] recordList = record.Split(',');
                if(recordList[0] == playerId){
                    Profile p = new Profile(recordList[0],int.Parse(recordList[1]),int.Parse(recordList[2]));
                    return p;
                }
            }
            return null;
        }

        /// <summary>
        /// A method to return true if the board full of pieces
        /// </summary>
        /// <returns></returns>
        public bool BoardFullOfPieces(){
            return (!_board.IsTherePlaces());
        }


        /// <summary>
        /// A method to reset and start timer count timer while game switch to other player
        /// </summary>
        public void TimerController(){
            if(_currPlayer == PieceType.BLACK){
                _blackPieceTimer.StartCountDown();
                _whitePieceTimer.Reset();
            }else if(_currPlayer == PieceType.WHITE){
                _whitePieceTimer.StartCountDown();
                _blackPieceTimer.Reset();
            }
        }

        /// <summary>
        /// A method to return true if the timer seconds below 1
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public bool TimerTimedOut(string timer){
            if( timer=="black" && _blackPieceTimer.Seconds < 1 ){
                return true;
            }else if( timer=="white" && _whitePieceTimer.Seconds < 1 ){
                return true;
            }
            return false;
        }

        // -------------------------------------------------- // 

        /// <summary>
        /// A method to place the best piece
        /// </summary>
        /// <returns></returns>
        public Piece ComputerPlaceABestPiece(){
            Point2D bestLoc = FindTheBestLocation();
            if(bestLoc.X == -1){
                return null;
            }
            Piece computerPiece = PlaceAPiece(bestLoc);
            return computerPiece;
        }

        /// <summary>
        /// A method to convert the node to window position
        /// </summary>
        /// <returns></returns>
        public Point2D FindTheBestLocation(){
            Point2D bestLoc = new Point2D();
            Point2D nodeId = FindTheBestNode();

            bestLoc.X = (int)nodeId.X * 75 + 75;
            bestLoc.Y = (int)nodeId.Y * 75 + 75;

            return bestLoc;
        }

        /// <summary>
        /// A method to find the best node predicted by minimax algorithm
        /// </summary>
        /// <returns></returns>
        public Point2D FindTheBestNode(){
            Point2D currLoc = new Point2D();
            int score;
            int bestScore = int.MinValue;
            Point2D bestNodeId = new Point2D();

            for (int nodeIdX = 0; nodeIdX < _board.NodeCount; nodeIdX++){
                for (int nodeIdY = 0; nodeIdY < _board.NodeCount; nodeIdY++){
                    if(_board.GetPieceType(nodeIdX,nodeIdY) == PieceType.NONE){

                        // current node's location
                        currLoc.X = nodeIdX*75+75;
                        currLoc.Y = nodeIdY*75+75;
                        
                        // board place a piece, at the same time, also record the last placed node (SIM)
                        // white piece (ai) turn now
                        _board.PlaceAPiece(currLoc, PieceType.WHITE, true);

                        // find the best score and node using minimax algorithm
                        score = Minimax(3, int.MinValue, int.MaxValue, false);

                        _board.TwoDPieces[nodeIdX, nodeIdY] = null;

                        // replace the best score and node
                        if(score > bestScore){
                            bestScore = score;
                            bestNodeId.X = nodeIdX;
                            bestNodeId.Y = nodeIdY;
                        }

                    }
                }
            }
            return bestNodeId;

        }

        /// <summary>
        /// An algorithm to search the possibility in the game tree
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="isAITurn"></param>
        /// <returns></returns>
        public int Minimax(int depth, int alpha, int beta, bool isAITurn){
            Point2D currLoc = new Point2D();
            
            if(depth == 0 || EvaluationCriteriaOf("FiveInRow", 5, true) != 0){
                return Evaluation();
            }

            if(isAITurn){
                int maxEval = int.MinValue;
                for (int nodeIdX = 0; nodeIdX < _board.NodeCount; nodeIdX++){
                    for (int nodeIdY = 0; nodeIdY < _board.NodeCount; nodeIdY++){
                        if(_board.GetPieceType(nodeIdX, nodeIdY) == PieceType.NONE){

                            currLoc.X = nodeIdX*75+75;
                            currLoc.Y = nodeIdY*75+75;

                            _board.PlaceAPiece(currLoc,PieceType.WHITE,true);
                            int eval = Minimax(depth-1, alpha, beta, false);
                            _board.TwoDPieces[nodeIdX, nodeIdY] = null;
                            
                            maxEval = Math.Max(maxEval, eval);
                            

                            alpha = Math.Max(alpha, eval);
                            if(beta <= alpha){
                                break;
                            }
                            
                        }
                    }
                }
                return maxEval;

            }else{
                int minEval = int.MaxValue;
                for (int nodeIdX = 0; nodeIdX < _board.NodeCount; nodeIdX++){
                    for (int nodeIdY = 0; nodeIdY < _board.NodeCount; nodeIdY++){
                        if(_board.GetPieceType(nodeIdX, nodeIdY) == PieceType.NONE){

                            currLoc.X = nodeIdX*75+75;
                            currLoc.Y = nodeIdY*75+75;

                            _board.PlaceAPiece(currLoc,PieceType.BLACK, true);
                            int eval = Minimax(depth-1, alpha, beta, true);
                            _board.TwoDPieces[nodeIdX, nodeIdY] = null;

                            minEval = Math.Min(minEval, eval);
                            


                            beta = Math.Min(beta, eval);
                            if(beta <= alpha){
                                break;
                            }
                        }
                    }
                }
                return minEval;
            }
        }

        /// <summary>
        /// An evaluation method to determine the score of the node
        /// </summary>
        /// <returns></returns>
        public int Evaluation(){
            // -1 means nothing happen
            int score = 0;
            
            if(EvaluationCriteriaOf("FiveInRow", 5, true) == 0 && 
            EvaluationCriteriaOf("LiveFour", 4, true) == 0 && 
            EvaluationCriteriaOf("LiveThree", 3, true) == 0 && 
            EvaluationCriteriaOf("LiveTwo", 2, true) == 0){
                score = 0;
            }else{
                score = EvaluationCriteriaOf("FiveInRow", 5, true) + EvaluationCriteriaOf("LiveFour", 4, true) + EvaluationCriteriaOf("LiveThree", 3, true) + EvaluationCriteriaOf("LiveTwo", 2, true);
            }

            if(BoardFullOfPieces()){
                // tie
                score = 0;
            }
            
            // return no result
            return score;
        }

        /// <summary>
        /// A method to check who is the winner
        /// </summary>
        public void checkWinner(){
            if(EvaluationCriteriaOf("FiveInRow", 5, false) != 0){
                _winner = CurrPlayer;
            }
        }


        public int EvaluationCriteriaOf(string name, int noOfPieces, bool isSimulation){
            int score = 0;
            int centerX;
            int centerY;
            PieceType type;
            if(isSimulation){
                centerX = (int)_board.LastPlacedNodeSIM.X;
                centerY = (int)_board.LastPlacedNodeSIM.Y;
                type = _board.TwoDPieces[centerX, centerY].GetPieceType();
            }else{
                centerX = (int)_board.LastPlacedNode.X;
                centerY = (int)_board.LastPlacedNode.Y;
                type = _currPlayer;
            }

            for(int xDir = -1; xDir <= 1; xDir++){
                for(int yDir = -1; yDir <= 1; yDir++){
                    if(xDir == 0 && yDir == 0){ // if it's the center, then the rest of the code skip it
                        continue;
                    }

                    // record the amount of same piece
                    int count = 1;
                    while (count < noOfPieces){
                        int targetX = centerX + count * xDir;
                        int targetY = centerY + count * yDir;

                        // check if the piece type is same
                        if( ExceedBoundary(targetX, targetY) || 
                            IsNoPieceHere(targetX, targetY) || 
                            IsPieceTypeNotTheSame(type, targetX, targetY)){
                            
                            // turn around from centerXY
                            int countReverse = 0;
                            while(countReverse+count < noOfPieces){
                                countReverse++;
                                targetX = centerX + xDir * countReverse * -1;
                                targetY = centerY + yDir * countReverse * -1;

                                if( ExceedBoundary(targetX, targetY) || 
                                    IsNoPieceHere(targetX, targetY) || 
                                    IsPieceTypeNotTheSame(type, targetX, targetY)){
                                        break;
                                }
                                if(countReverse+count == noOfPieces){
                                    if(type == PieceType.BLACK){
                                        return -_score[name];
                                    }
                                    return _score[name];
                                }

                            }

                            break;
                        }

                        count ++;
                        
                    }

                    if(count == noOfPieces){
                        if(type == PieceType.BLACK){
                            return -_score[name];
                        }
                        return _score[name];
                    }

                }
            }
            return score;

            
            
        }

        
        /// <summary>
        /// A method to check whether the node exceed boundary
        /// </summary>
        /// <param name="targetX"></param>
        /// <param name="targetY"></param>
        /// <returns></returns>
        public bool ExceedBoundary(int targetX, int targetY){
            return 
            targetX < 0 || targetX >= _board.NodeCount || 
            targetY < 0 || targetY >= _board.NodeCount;
        }
        
        /// <summary>
        /// A method to check whether the node is PieceType.None
        /// </summary>
        /// <param name="targetX"></param>
        /// <param name="targetY"></param>
        /// <returns></returns>
        public bool IsNoPieceHere(int targetX, int targetY){
            return _board.GetPieceType(targetX, targetY) == PieceType.NONE;
        }
        
        /// <summary>
        /// A method to check whethet the piece on the node is not the same as parameter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="targetX"></param>
        /// <param name="targetY"></param>
        /// <returns></returns>
        public bool IsPieceTypeNotTheSame(PieceType type, int targetX, int targetY){
            return _board.GetPieceType(targetX, targetY) != type;
        }
        
    }
}


