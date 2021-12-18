using System;
using SplashKitSDK;
using System.Collections.Generic;

namespace GomokuGame
{
    /// <summary>
    /// A Timer class which contain several fields: seconds, timer obj, start time, x, y
    /// </summary>
    public class GameTimer
    {
        // -------------------- Fields -------------------- //
        private int _seconds;
        private Timer _timer;
        private uint _startTime;
        private int _x;
        private int _y;


        // -------------------- Constructor -------------------- //
        /// <summary>
        /// A constructor which initialize the seconds, timer obj, start time, x and y of the timer
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="timerName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public GameTimer(int seconds, string timerName, int x, int y){
            _seconds = seconds;
            _timer = SplashKit.CreateTimer(timerName);
            _startTime = _timer.Ticks; // 0 milliseconds
            _x = x;
            _y = y;
        }


        // -------------------- Properties -------------------- //
        /// <summary>
        /// A property that able to modify and view the seconds of timere
        /// </summary>
        /// <value></value>
        public int Seconds{
            get { return _seconds; }
            set { _seconds = value; }
        }


        // -------------------- Methods -------------------- //
        /// <summary>
        /// A method to draw the timer text
        /// </summary>
        public void Draw(){
            
            Color color = (_seconds < 6) ? Color.Red : Color.Black;
            SplashKit.DrawText( (Convert.ToDateTime($"00 : 00 : {_seconds}")).ToString("mm:ss"), color, SplashKit.FontNamed("font"), 40, _x,_y);
        }

        /// <summary>
        /// A method to decrement the seconds by 1
        /// </summary>
        public void StartCountDown(){
            Start();
            if( (_timer.Ticks - _startTime) / 1000 == 1){
                _seconds -= 1;
                _startTime = _timer.Ticks;
            }
        }

        /// <summary>
        /// A method to start the timer
        /// </summary>
        public void Start(){
            if(!_timer.IsStarted){
                _timer.Start();
            }
        }

        /// <summary>
        /// A method to reset the seconds of the timer
        /// </summary>
        public void Reset(){
            if(_timer.IsStarted){
                _timer.Stop();
                _seconds = 30;
                _startTime = _timer.Ticks;
            }
        }
    }
}