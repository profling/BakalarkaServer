using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySqlConnector;

namespace BakalarkaServer
{
   static class DBConnector
    {
      
       static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder("Server=34.107.13.57;User ID=app;Password=lov3MaxiKing;Database=bakalarka");
        
            /*builder.SslCert = @"Bakalarka.client-cert.pem";
            builder.SslKey = @"Bakalarka.client-key.pem";
            builder.SslCa = @"Bakalarka.server-ca.pem";
            builder.SslMode = MySqlSslMode.VerifyFull;*/
        
   
            
        /**
         * Provedeni prikazu Select. Pokud probehne v poradku vrati data jinak vrati null
         */
        
       static public MySqlDataReader ProvedeniPrikazuSelect(MySqlCommand prikaz)
        {
            try
            {                
                MySqlConnection pripojeni = new MySqlConnection(builder.ConnectionString);
                pripojeni.Open();
                prikaz.Connection = pripojeni;
                try
                {
                    MySqlDataReader data = prikaz.ExecuteReader();
                    return data;
                }
                catch(MySqlException e)
                {
                    return null;
                }
            }
            catch (MySqlException e)
            {
                return null;
            }
        }
        /**
         * Provedeni prikazu update, delete, insert. Pokud probehne v poradku vrati null jinak vrati chybovou hlasku
         */
       static public String ProvedeniPrikazuOstatni( MySqlCommand prikaz)
        {
            try
            {
                MySqlConnection pripojeni = new MySqlConnection(builder.ConnectionString);
                pripojeni.Open();
                prikaz.Connection=pripojeni;
                try
                {
                    prikaz.ExecuteNonQuery();
                    pripojeni.Close();
                    return null;
                }
                catch (MySqlException e)
                {
                    return e.ToString();
                }

            }
            catch (MySqlException e)
            {
                return e.ToString();
            }

        }
        
    }
}
