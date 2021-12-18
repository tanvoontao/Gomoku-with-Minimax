using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.IO;


namespace GomokuGame
{
    public class Program
    {
        public static void Main()
        {
            SplashKit.LoadFont("font", "Resources/ComicSansMS3.ttf");
            SplashKit.LoadMusic("bgm", "Resources/bgm.mp3");
            SplashKit.LoadSoundEffect("place_piece", "Resources/place_piece_sound.mp3");
            SplashKit.LoadSoundEffect("win_sound", "Resources/win_sound.wav");

            Music bgm = SplashKit.MusicNamed("bgm");
            
            Window GomokuGameWindow = new Window("Gomoku Game", 750, 750);
            Point2D p = new Point2D();

            MenuController menuController = new MenuController();
            

            do{
                SplashKit.ProcessEvents(); // indicate there is event coming in

                SplashKit.ClearScreen(Color.White); // with a clear white screen

                
                if(!SplashKit.MusicPlaying()){
                    SplashKit.PlayMusic(bgm,1,0.2F);
                }
                SplashKit.ShowMouse();

                // ------------------------------------------------------------ //

                menuController.ControlTimer();
                menuController.ControlInputField();

                if(SplashKit.MouseClicked(MouseButton.LeftButton)){
                    p.X = SplashKit.MouseX();
                    p.Y = SplashKit.MouseY();

                    menuController.ToggleMenu(p);
                    menuController.GomokuGamePlaceAPiece(p);
                }

                menuController.CheckTimerTimedOut();
                menuController.Draw();
                
                // ------------------------------------------------------------ //


                SplashKit.RefreshScreen(60); // refresh the screen every 60 ms

            }while(! SplashKit.QuitRequested()); // close btn at top right

            SplashKit.FreeAllMusic();
            SplashKit.FreeAllSoundEffects();
            SplashKit.FreeAllTimers();
        }
    }
}

