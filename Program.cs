using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace HamnenConsole
{
    class Program
    {
        static Boat[,] harbor = new Boat[64, 2];
        static List<Boat> Boats = new List<Boat>();
        static readonly Random rnd = new Random();

        static void Main(string[] args)
        {
            while (true)
            {
                foreach (var boat in Boats.ToList())
                {
                    Console.WriteLine(boat.DaysStayed);
                    if(boat.DaysToStay <= boat.DaysStayed)
                    {
                        for (int i = 0; i < harbor.GetLength(0); i++)
                        {
                            for (int j = 0; j < harbor.GetLength(1); j++)
                            {
                                if (harbor[i, j] == boat)
                                    harbor[i, j] = null;
                            }
                        }
                        Console.WriteLine("Båten åker iväg");
                        //När båten ska ta bort
                        Boats.Remove(boat);
                    }
                    boat.DaysStayed++;
                }
                //foreach (var boatPlace in harborList)
                //{
                //    if (boatPlace.Count > 0 && boatPlace[0] == boatPlace[1])
                //    {
                //        if (boatPlace[0].DaysStayed == boatPlace[0].DaysToStay)
                //        {
                //            boatPlace.Remove(boatPlace[1]);
                //            boatPlace.Remove(boatPlace[0]);
                //        }

                //        else
                //            boatPlace[0].DaysStayed++;
                //    }
                //    else
                //    {
                //        foreach (var placeHalf in boatPlace)
                //        {
                //            if (placeHalf.DaysStayed == placeHalf.DaysToStay)
                //                boatPlace.Remove(placeHalf);
                //            else
                //                placeHalf.DaysStayed++;
                //        }
                //    }

                //}


                for (int i = 0; i < 5; i++)
                {
                    var randomBoat = rnd.Next(0, 4);
                    switch (randomBoat)
                    {
                        case 0:
                            AddBoatToHarborArray(new RowBoat());
                            break;
                        case 1:
                            AddBoatToHarborArray(new PowerBoat());
                            break;
                        case 2:
                            AddBoatToHarborArray(new SailingBoat());
                            break;
                        case 3:
                            AddBoatToHarborArray(new CargoShip());
                            break;
                    }

                }

                Console.Clear();
                for (int i = 0; i < harbor.GetLength(0); i++)
                {
                    for (int j = 0; j < harbor.GetLength(1); j++)
                    {
                        if (harbor[i,j] != null)
                        Console.WriteLine($"{i}, {j}: {harbor[i,j].Id} {harbor[i,j].BoatType}");
                    }
                }
                

                Console.ReadLine();





            }
           


            void AddBoatToHarborArray(Boat boat)
            {
                //TODO Kanske inte funkar om det är en båt som är stor och den hittar en ledig ruta som inte är tillräkligt stor men det finns en öppen lucka längre in i hamnen.
                int startingPlace = 0;
                int freeSpace = 0;
                float boatSpace = boat.SpaceTaken * 2;
                for (int i = 0; i < harbor.GetLength(0); i++)
                {
                    for (int j = 0; j < harbor.GetLength(1); j++)
                    {
                        //Lägger in roddbåten i närmsta lucka.
                        if (harbor[i, j] == null && boat is RowBoat)
                        {
                            harbor[i, j] = boat;
                            Boats.Add(boat);
                            //returnerar då båten är inlagd.
                            return;
                        }
                        //Koden kommer hit om den har gått igenom koden under 1 gång innan då den har ökat freespace med 1 för det fanns en halv hamnplats ledig.
                        //Denna kollar vidare om det finns plats för båten som kommer till hamnen.
                        else if(freeSpace > 0 && harbor[i, j] == null)
                        {
                            freeSpace++;
                            if(freeSpace == boatSpace)
                            {
                                float endingPlace = startingPlace + boat.SpaceTaken;
                                for (int k = startingPlace; k < endingPlace; k++)
                                {
                                    for (int x = 0; x < harbor.GetLength(1); x++)
                                    {
                                        harbor[k, x] = boat;
                                    }
                                }
                                Boats.Add(boat);
                                //Returnerar då Båten har lagts in i listan
                                return;
                            }
                        }
                        //Koden kommer hit om platsen är ledig och om platsen är första halvan då en båt större än en roddbåt behöver mer utrymme en 1 hamnplatstotalt.
                        else if(j == 0 && harbor[i, j] == null)
                        {
                            startingPlace = i;
                            freeSpace++;
                        }
                        else
                        {
                            freeSpace = 0;
                        }
                    }


                }
                //om koden kommer hit finns det ingen plats för båten..
                Debug.Print(boat.BoatType + " kom inte in i hamnen.");
            }

        }
    }
}
