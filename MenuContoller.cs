using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.IO;

namespace GomokuGame
{
    /// <summary>
    /// A MenuController class which control the gomokugame, menu and profile obj
    /// </summary>
    public class MenuController
    {
        // -------------------- Fields -------------------- //
        private Profile _player;
        private GomokuGame _gomokuGame;
        private Menu _gameMenu;
        private SoundEffect _winSound;


        // -------------------- Constructor -------------------- //
        /// <summary>
        /// A constructor to initialize the player, gomokugame, and menu object
        /// </summary>
        public MenuController(){
            _player = new Profile("Unknown",0,0);
            _gomokuGame = new GomokuGame();
            _gameMenu = new Menu();
            _winSound = SplashKit.SoundEffectNamed("win_sound");
        }


        // -------------------- Methods -------------------- //
        /// <summary>
        /// A method to draw gomoku game or menu
        /// </summary>
        public void Draw(){
            if(_gameMenu.GameStart){
                _gomokuGame.Draw();
                _gameMenu.Draw();
            }else{
                _gameMenu.Draw();
            }
        }

        /// <summary>
        /// A method to control timer to reset or start count down while game switch to other player
        /// </summary>
        public void ControlTimer(){
            if(_gameMenu.GameStart && !_gomokuGame.AutoPlayAvailable){
                _gomokuGame.TimerController();
            }
        }

        /// <summary>
        /// A method to read input test
        /// </summary>
        public void ControlInputField(){
            if(_gameMenu.HomePage){ // if at home page
                // finished reeading text
                if(!SplashKit.ReadingText()){

                    // if the player cancel by hitting 'escape'
                    if(SplashKit.TextEntryCancelled()){
                        _gameMenu.InputName = "Your name...";
                    }else{
                        // else player hit 'enter'
                        _gameMenu.InputName = SplashKit.TextInput();
                        SplashKit.StartReadingText(_gameMenu.TxtArea,"Your name...");
                    }
                }
            }
        }

        /// <summary>
        /// A method to toggle the menu 
        /// </summary>
        /// <param name="p"></param>
        public void ToggleMenu(Point2D p){
            if(_gameMenu.HomePage){ // if at home page
                if( _gameMenu.SelectButtonAt("Login", p) ){
                    if(_gameMenu.InputName != ""){
                        _player = _gomokuGame.Login(_gameMenu.InputName);
                        if(_player != null){
                            _gameMenu.DisableAllMenu();
                            _gameMenu.MainPage = true;
                        }else{
                            _gameMenu.InputName = "Account not exist. Please register. ";
                        }
                    }
                }else if( _gameMenu.SelectButtonAt("Register", p) ){
                    if(_gameMenu.InputName != ""){
                        _player = _gomokuGame.Register(_gameMenu.InputName);
                        if(_player != null){
                            _gameMenu.DisableAllMenu();
                            _gameMenu.MainPage = true;
                        }else{
                            _gameMenu.InputName = "Account exist. Please Login. ";
                        }
                    }
                }
            }

            if(_gameMenu.MainPage){ // if at main page
                if( _gameMenu.SelectButtonAt("PvP", p) ){
                    _gomokuGame.AutoPlayAvailable = false;
                    _gameMenu.DisableAllMenu();
                    _gameMenu.GameStart = true;
                }else if( _gameMenu.SelectButtonAt("PvC", p) ){
                    _gomokuGame.AutoPlayAvailable = true;
                    _gameMenu.DisableAllMenu();
                    _gameMenu.GameStart = true;
                }else if( _gameMenu.SelectButtonAt("Profile", p) ){
                    _gameMenu.DisableAllMenu();
                    _gameMenu.ProfilePage = true;
                }
            }

            if(_gameMenu.ProfilePage){ // if at player profile page
                _gameMenu.ProfileDetails = _player;
                if( _gameMenu.SelectButtonAt("Back", p) ){
                    _gameMenu.DisableAllMenu();
                    _gameMenu.MainPage = true;
                }
            }

            if(_gameMenu.WinnerPage){ // if a winner page
                if( _gameMenu.SelectButtonAt("OK", p) ){
                    _gameMenu.DisableAllMenu();
                    _gameMenu.GameStart = true;
                    _gomokuGame.Restart();
                }
            }

            if(_gameMenu.GameStart){ // if at the game
                if( _gameMenu.SelectButtonAt("BackFromGame", p) ){
                    _gameMenu.DisableAllMenu();
                    _gameMenu.MainPage = true;
                    _gomokuGame.Restart();
                }
            }
        }

        /// <summary>
        /// A method to let GomokuGame obj place a piece
        /// </summary>
        /// <param name="p"></param>
        public void GomokuGamePlaceAPiece(Point2D p){
            if(_gameMenu.GameStart){
                Piece piece = _gomokuGame.PlaceAPiece(p);
                
                if(piece != null){
                    
                    _gomokuGame.PlacePiece(piece);

                    if(_gomokuGame.AutoPlayAvailable && _gomokuGame.CurrPlayer == PieceType.WHITE){
                        //Piece pieceAuto  = _gomokuGame.ComputerPlaceAPiece();
                        Piece pieceAuto = _gomokuGame.ComputerPlaceABestPiece();
                        if (pieceAuto != null){
                            _gomokuGame.PlacePiece(pieceAuto);
                        }
                    }

                    // if winner exist or board full of pieces -> update player profile
                    if(_gomokuGame.Winner != PieceType.NONE || _gomokuGame.BoardFullOfPieces()){
                        _gameMenu.Winner = _gomokuGame.Winner;

                        SplashKit.PlaySoundEffect(_winSound);
                        _gomokuGame.UpdatePlayerDetails(_player);

                        _gameMenu.DisableAllMenu();
                        // display winner page
                        _gameMenu.WinnerPage = true;
                    }
                }
            }
        }

        /// <summary>
        /// A method to check whether the timer timed out and display winner
        /// </summary>
        public void CheckTimerTimedOut(){
            if(_gameMenu.GameStart){

                ToggleMouseVisibility();

                if(_gomokuGame.TimerTimedOut("black") || _gomokuGame.TimerTimedOut("white")){
                    
                    if(_gomokuGame.TimerTimedOut("black")){
                        _gameMenu.Winner = PieceType.WHITE;
                        _gomokuGame.Winner = PieceType.WHITE;
                    }else if(_gomokuGame.TimerTimedOut("white")){
                        _gameMenu.Winner = PieceType.BLACK;
                        _gomokuGame.Winner = PieceType.BLACK;
                    }

                    SplashKit.PlaySoundEffect(_winSound);
                    _gomokuGame.UpdatePlayerDetails(_player);
                    _gameMenu.DisableAllMenu();
                    // display winner page
                    _gameMenu.WinnerPage = true;
                }
            }
        }

        /// <summary>
        /// A method to hide or show the mouse while it move to clickable node
        /// </summary>
        public void ToggleMouseVisibility(){
            if(_gomokuGame.CanBePlaced(SplashKit.MousePosition())){
                SplashKit.HideMouse();
            }else{
                SplashKit.ShowMouse();
            }
        }
    }
}