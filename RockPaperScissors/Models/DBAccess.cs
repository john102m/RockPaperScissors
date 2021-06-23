/**
 * DBAccess class used to connect to the remote database
 * Individual game statistics are stored in the databaes
 * 
 * 
 * 
 * John McKinney  23/06/2021
 * 
 * 
 */



using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace RockPaperScissors.Models
{
    class DBAccess
    {
        private string _connectionString;
        public DBAccess(IConfiguration iconfiguration)  //constructor uses default connection.....
        {
            _connectionString = iconfiguration.GetConnectionString("DefaultConnection");
        }

        /**
         *
         * save the game stats 
         * <param name="winner">name of the games winner</param>
         * <param name="mostmoves">name of the move that was used most</param> 
         * <param name="numTurns">number of turns to win the game  (round)</param>
         */

        public bool AddGameStats(GameStats gameStats)
        {
            bool result = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string saveGame = "INSERT into games (winner,mostmoves,turns,date) VALUES (@Winner,@Moves,@Turns,@Date)";

                    //
                    //use parameters as standard practice !!
                    //referencing this link
                    //https://www.c-sharpcorner.com/blogs/access-sql-server-database-in-net-core-console-application
                    //
                    //

                    using (SqlCommand querySaveGame = new SqlCommand(saveGame))
                    {
                        querySaveGame.Connection = conn;
                        querySaveGame.Parameters.Add("@Winner", SqlDbType.VarChar, 20).Value = gameStats.Winner;
                        querySaveGame.Parameters.Add("@Moves", SqlDbType.VarChar, 10).Value = gameStats.MostMoves;
                        querySaveGame.Parameters.Add("@Turns", SqlDbType.Int).Value = gameStats.Turns;
                        querySaveGame.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now;

                        conn.Open();

                        querySaveGame.ExecuteNonQuery();
                    }

                    conn.Close();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;

                
            }

            return result;
        }


        /**
         * return the list of game stats from the db
         */
        public List<GameStats> GetGameStats()
        {
            var listGameStats = new List<GameStats>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM games ", conn)
                    {
                        CommandType = CommandType.Text
                    };
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        listGameStats.Add(new GameStats
                        {
                            GameId = Convert.ToInt32(rdr[0]),
                            Winner = rdr[1].ToString(),
                            MostMoves = rdr[2].ToString(),
                            Turns = Convert.ToInt32(rdr[3]),
                            Date = Convert.ToDateTime(rdr[4])
                        });


                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listGameStats;
        }


    }
}
