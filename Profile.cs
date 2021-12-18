using System;
using SplashKitSDK;
using System.Collections.Generic;


namespace GomokuGame
{
    /// <summary>
    /// A Profile class which containe several information: player id, total games, and number of wins
    /// </summary>
    public class Profile
    {
        // -------------------- Fields -------------------- //
        private string _playerId;
        private int _totalGames;
        private int _noOfWins;


        // -------------------- Constructor -------------------- //
        /// <summary>
        /// A constructor to initalize the player details
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="totalGames"></param>
        /// <param name="noOfWins"></param>
        public Profile(string playerId, int totalGames, int noOfWins){
            _playerId = playerId;
            _totalGames = totalGames;
            _noOfWins = noOfWins;
        }


        // -------------------- Properties -------------------- //
        /// <summary>
        /// A property which able to modify and view the player id
        /// </summary>
        /// <value></value>
        public string PlayerId{
            get { return _playerId; }
            set { _playerId = value; }
        }

        /// <summary>
        /// A property which able to modify and view the total game of the player
        /// </summary>
        /// <value></value>
        public int TotalGames{
            get { return _totalGames; }
            set { _totalGames = value; }
        }

        /// <summary>
        /// A property which able to modify and view the number of wins of the player
        /// </summary>
        /// <value></value>
        public int NoOfWins{
            get { return _noOfWins; }
            set { _noOfWins = value; }
        }


        // -------------------- Methods -------------------- //
        /// <summary>
        /// A method to increment the total game by 1 
        /// </summary>
        public void UpdateTotalGames(){
            _totalGames += 1;
        }

        /// <summary>
        /// A method to increment the number of wins by 1
        /// </summary>
        public void UpdateNoOfWins(){
            _noOfWins += 1;
        }
    }
}