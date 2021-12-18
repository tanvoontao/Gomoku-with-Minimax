using System;
using SplashKitSDK;
using System.Collections.Generic;

namespace GomokuGame
{
    /// <summary>
    /// A Menu class contain all the menu and its fields in this game
    /// </summary>
    public class Menu
    {
        // -------------------- Fields -------------------- //
        private List<Button> _buttons;
        private string _inputName;
        private Rectangle _txtArea;
        private bool _homePage;
        private bool _mainPage;
        private bool _profilePage;
        private bool _winnerPage;
        private bool _gameStart;
        private Profile _profileDetails;
        private Bitmap _board;
        private Bitmap _menuBg;
        private PieceType _winner;



        // -------------------- Constructor -------------------- //
        /// <summary>
        /// A method to initialize the menu information
        /// </summary>
        public Menu(){
           _buttons = new List<Button>();
           _inputName = "unknown";

           // define area where txt should appear
           _txtArea = new Rectangle();
           _txtArea = SplashKit.RectangleFrom(150,300,450,75);

           _mainPage = _profilePage = _winnerPage = _gameStart = false;
           _homePage = true;
           _profileDetails = new Profile("unknown",0,0);

            Button[] menuBtn = 
            {
                new Button(Color.White, 150,450,150,75, "Login", "login", "Resources/login_btn.png"),
                new Button(Color.White, 450,450,150,75, "Register", "register", "Resources/register_btn.png"),
                new Button(Color.White, 450,225,225,75, "PvP", "pvp", "Resources/pvp_btn.png"),
                new Button(Color.White, 450,375,225,75, "PvC", "pvc", "Resources/pvc_btn.png"),
                new Button(Color.White, 450,525,225,75, "Profile", "profile", "Resources/profile_btn.png"),
                new Button(Color.White, 300,525,150,75, "Back", "back", "Resources/back_btn.png"),
                new Button(Color.Black, 300,525,150,75, "OK", "ok", "Resources/ok_btn.png"),
                new Button(Color.Black, -30,-15,150,75, "BackFromGame", "back", "Resources/back_btn.png")
            };
            foreach(Button b in menuBtn){
                AddBtn(b);
            }

            _board = SplashKit.LoadBitmap("board", "Resources/board.png");
            _menuBg = SplashKit.LoadBitmap("bg", "Resources/background.jpg");
        }

       

        // -------------------- Properties -------------------- //
        /// <summary>
        /// A property which able to modify and view the Input Name
        /// </summary>
        /// <value></value>
        public string InputName{
            get { return _inputName; }
            set { _inputName = value; }
        }

        /// <summary>
        /// A property which able to modify and view the Text Area
        /// </summary>
        /// <value></value>
        public Rectangle TxtArea{
            get { return _txtArea; }
            set { _txtArea = value; }
        }

        /// <summary>
        /// A property which able to modify and view the available of the HomePage menu
        /// </summary>
        /// <value></value>
        public bool HomePage{
            get { return _homePage; }
            set { _homePage = value; }
        }

        /// <summary>
        /// A property which able to modify and view the available of the MainPage menu
        /// </summary>
        /// <value></value>
        public bool MainPage{
            get { return _mainPage; }
            set { _mainPage = value; }
        }

        /// <summary>
        /// A property which able to modify and view the available of the ProfilePage menu
        /// </summary>
        /// <value></value>
        public bool ProfilePage{
            get { return _profilePage; }
            set { _profilePage = value; }
        }

        /// <summary>
        /// A property which able to modify and view the profile details
        /// </summary>
        /// <value></value>
        public Profile ProfileDetails{
            get { return _profileDetails; }
            set { _profileDetails = value; }
        }

        /// <summary>
        /// A property which able to modify and view the available of winner page
        /// </summary>
        /// <value></value>
        public bool WinnerPage{
            get { return _winnerPage; }
            set { _winnerPage = value; }
        }

        /// <summary>
        /// A property which able to modify and view the winner of the game
        /// </summary>
        /// <value></value>
        public PieceType Winner{
            get { return _winner; }
            set { _winner = value; }
        }

        /// <summary>
        /// A property which able to modify and view the available of game start
        /// </summary>
        /// <value></value>
        public bool GameStart{
            get { return _gameStart; }
            set { _gameStart = value; }
        }


        // -------------------- Methods -------------------- //
        /// <summary>
        /// A method to check whether the mouse position within the selected button
        /// </summary>
        /// <param name="b"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool SelectButtonAt(string btnTxt, Point2D pt){
            Button b = ClickButton(btnTxt);
            if( b!= null){
                return b.IsAt(pt);
            }
            return false;
        }

        /// <summary>
        /// A method to draw all the menu based on the bool value of each menu
        /// </summary>
        public void Draw(){
            if(_homePage || _mainPage || _profilePage || _winnerPage){
                // board bg
                SplashKit.DrawBitmap(_board, 0, 0);

                // menu bg
                SplashKit.DrawBitmap(_menuBg, 75, 150);
            }
            if(_homePage){
                // welcome to gomoku game txt (title)
                SplashKit.DrawText("Welcome to Gomoku Game! ", Color.White, SplashKit.FontNamed("font"), 48, 80,160);

                // text area prompt
                SplashKit.DrawText("Please fill in your name and press 'Enter': ", Color.White, SplashKit.FontNamed("font"), 24, 150,250);

                // input field (txt area)
                SplashKit.DrawRectangle(Color.White, _txtArea);

                // login and register btn
                foreach(Button b in _buttons ){
                    if(b.Txt == "Login" || b.Txt == "Register"){
                        b.Draw();
                    }
                }
                SplashKit.DrawText(_inputName, Color.White, SplashKit.FontNamed("font"), 40, 10,10);
                

                if(SplashKit.ReadingText()){
                    SplashKit.DrawCollectedText(Color.White, SplashKit.FontNamed("font"), 30, SplashKit.OptionDefaults());
                }
            }
            else if(_mainPage){
                string [] _gameRulesInstrcution = 
                {
                "Players alternate turns placing a",
                "stone of their color on an empty",
                "intersection. Black plays first.",
                "The winner is the first player",
                "to form an unbroken chain of",
                "five stones horizontally,",
                "vertically, or diagonally."
                };

                //SplashKit.FillRectangle(Color.White,225,300,300,75);
                SplashKit.DrawText($"Welcome! {_inputName}", Color.White, SplashKit.FontNamed("font"), 34, 75,225);
                SplashKit.DrawText("Players alternate turns placing a", Color.White, SplashKit.FontNamed("font"), 24, 75,300);
                int y = 300;
                foreach(string str in _gameRulesInstrcution){
                    SplashKit.DrawText(str, Color.White, SplashKit.FontNamed("font"), 24, 75,y);
                    y+=25;
                }


                // PvP, PvC, Profile button
                foreach(Button b in _buttons ){
                    if(b.Txt == "PvP" || b.Txt == "PvC" || b.Txt == "Profile"){
                        b.Draw();
                    }
                }

                SplashKit.DrawText("This Program built by Tan Voon Tao (101234693). ", Color.Black, SplashKit.FontNamed("font"), 30, 0,690);
            }
            else if(_profilePage){

                // title - game records
                SplashKit.DrawText("Game Records", Color.White, SplashKit.FontNamed("font"), 48, 225,150);
                // profile details
                SplashKit.DrawText($"Player Id: {_profileDetails.PlayerId}", Color.White, SplashKit.FontNamed("font"), 30, 225,300);
                SplashKit.DrawText($"Total Game: {_profileDetails.TotalGames}", Color.White, SplashKit.FontNamed("font"), 30, 225,330);
                SplashKit.DrawText($"No of Wins: {_profileDetails.NoOfWins}", Color.White, SplashKit.FontNamed("font"), 30, 225,360);


                // back button
                foreach(Button b in _buttons ){
                    if(b.Txt == "Back"){
                        b.Draw();
                    }
                }
            }
            else if(_winnerPage){
                // title - game records
                string txt = $"{_winner} is the Winner ! ";
                if(_winner == PieceType.NONE){
                    txt = "~~~ It is a tie! ~~~";
                }
                
                SplashKit.DrawText(txt, Color.White, SplashKit.FontNamed("font"), 48, 150,150);
                // back button
                foreach(Button b in _buttons ){
                    if(b.Txt == "OK"){
                        b.Draw();
                    }
                }
            }
            else if(_gameStart){
                foreach(Button b in _buttons ){
                    if(b.Txt == "BackFromGame"){
                        b.Draw();
                    }
                }
            }
        }

        /// <summary>
        /// A method to add the button to a list of buttons
        /// </summary>
        /// <param name="b"></param>
        public void AddBtn(Button b){
            _buttons.Add(b);
        }

        /// <summary>
        /// A method to return the selected button object
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public Button ClickButton(string txt){
            foreach(Button b  in _buttons){
                if(b.Txt == txt){
                    return b;
                }
            }
            return null;
        }

        /// <summary>
        /// A method to disable all the menu
        /// </summary>
        public void DisableAllMenu(){
            _homePage = false;
            _mainPage = false;
            _profilePage = false;
            _winnerPage = false;
            _gameStart = false;
        }
    }
}