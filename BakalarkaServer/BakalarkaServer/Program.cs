using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Drawing;
using MySqlConnector;
namespace BakalarkaServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Helloo World!");
                ZmenaPolohy();
                await Task.Delay(1000*60*5);//smycka trva 5 minut
            }
            
        }
        /*
         * Metoda na generovani a zmenu polohy produktu
         */
         static void ZmenaPolohy()
        {
            MySqlCommand prikazHra = new MySqlCommand("select * from bakalarka.hra;");
            MySqlDataReader dataHra = DBConnector.ProvedeniPrikazuSelect(prikazHra);
            double roh1x = 0, roh1y = 0, roh2x = 0, roh2y = 0, roh3x = 0, roh3y = 0, roh4x = 0, roh4y = 0;
            while (dataHra.Read())
            {
                roh1x = (double)dataHra["roh1X"];
                roh1y = (double)dataHra["roh1Y"];
                roh2x = (double)dataHra["roh2X"];
                roh2y = (double)dataHra["roh2Y"];
                roh3x = (double)dataHra["roh3X"];
                roh3y = (double)dataHra["roh3Y"];
                roh4x = (double)dataHra["roh4X"];
                roh4y = (double)dataHra["roh4Y"];
                Bod[] hraciPole = { new Bod(roh1x, roh1y), new Bod(roh2x, roh2y), new Bod(roh3x, roh3y), new Bod(roh4x, roh4y) };
                MySqlCommand prikazProdukty = new MySqlCommand("Select idprodukt from bakalarka.produkt where uroven=1;");
                MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikazProdukty);

                double[] y = { roh1y, roh2y, roh3y, roh4y };
                double yMin = y.Min();
                double yMax = y.Max();
                double[] x = { roh1x, roh2x, roh3x, roh4x };
                double xMin = x.Min();
                double xMax = x.Max();
                while (data.Read())
                {
                    Boolean prvniSouradnice = true;
                    Boolean druhaSouradnice = true;
                    Bod prvni=new Bod();
                    Bod druha = new Bod();
                    while (prvniSouradnice) // dokud neni souradnice v polygonu
                    {
                        prvni.X = new Random().NextDouble() * (xMax - xMin) + xMin;
                        prvni.Y = new Random().NextDouble() * (yMax - yMin) + yMin;
                        if (IsPointInPolygon(hraciPole, prvni)) prvniSouradnice = false;
                        
                    }
                    while (druhaSouradnice) // dokud neni souradnice v polygonu
                    {
                        druha.X = new Random().NextDouble() * (xMax - xMin) + xMin;
                        druha.Y = new Random().NextDouble() * (yMax - yMin) + yMin;
                        if (IsPointInPolygon(hraciPole, druha)) druhaSouradnice = false;

                    }
                    MySqlCommand prikazPoloha = new MySqlCommand("update bakalarka.polohaProduktu set x1=@x1, y1=@y1, x2=@x2,y2=@y2 where idhra=@idhra and idprodukt=@idprodukt;");
                    prikazPoloha.Parameters.AddWithValue("@x1", prvni.X);
                    prikazPoloha.Parameters.AddWithValue("@y1", prvni.Y);
                    prikazPoloha.Parameters.AddWithValue("@x2", druha.X);
                    prikazPoloha.Parameters.AddWithValue("@y2", druha.Y);
                    prikazPoloha.Parameters.AddWithValue("@idhra", (int)dataHra["idhra"]);
                    prikazPoloha.Parameters.AddWithValue("@idprodukt",(int)data["idprodukt"]);
                    String prubeh = DBConnector.ProvedeniPrikazuOstatni(prikazPoloha);
                    Console.WriteLine("Hotovo" + prubeh);
                    
                }
            }
        }
        /*
         * 
         */
        static bool IsPointInPolygon(Bod[] polygon, Bod testPoint)
                {
                    bool result = false;
                    int j = polygon.Count() - 1;
                    for (int i = 0; i < polygon.Count(); i++)
                    {
                        if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                        {
                            if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                            {
                                result = !result;
                            }
                        }
                        j = i;
                    }
                    return result;
                }

        



          

        
    }
}
