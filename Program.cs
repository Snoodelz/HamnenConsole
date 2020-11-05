using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace HamnenConsole
{
    class Program
    {
        static Boat[,] harbor = new Boat[64, 2];
        static List<Boat> boats = new List<Boat>();
        static readonly Random rnd = new Random();
        static int removedBoats = 0;

        static void Main(string[] args)
        {
            LoadBoatsFromFile();
            while (true)
            {
                BoatsLeaving();
                AddRandomBoats(5);


                ShowStatistics();
                Console.ReadLine();
            }
        }
        static void BoatsLeaving()
        {
            foreach (var boat in boats.ToList())
            {
                Console.WriteLine(boat.DaysStayed);
                if (boat.DaysToStay <= boat.DaysStayed)
                {
                    for (int i = 0; i < harbor.GetLength(0); i++)
                    {
                        for (int j = 0; j < harbor.GetLength(1); j++)
                        {
                            if (harbor[i, j] == boat)
                                harbor[i, j] = null;
                        }
                    }
                    //När båten ska ta bort
                    boats.Remove(boat);
                }
                boat.DaysStayed++;
            }
        }
        static void AddRandomBoats(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var randomBoat = rnd.Next(0, 4);
                switch (randomBoat)
                {
                    case 0:
                        AddBoatToHarbor(new RowBoat());
                        break;
                    case 1:
                        AddBoatToHarbor(new PowerBoat());
                        break;
                    case 2:
                        AddBoatToHarbor(new SailingBoat());
                        break;
                    case 3:
                        AddBoatToHarbor(new CargoShip());
                        break;
                }

            }
        }
        static void AddBoatToHarbor(Boat boat)
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
                        boat.StartingPlace = i + 1;
                        boat.EndingPlace = i + 1;
                        //boat.PlaceOnHarbor = i.ToString();
                        harbor[i, j] = boat;
                        boats.Add(boat);
                        //returnerar då båten är inlagd.
                        return;
                    }
                    //Koden kommer hit om den har gått igenom koden under 1 gång innan då den har ökat freespace med 1 för det fanns en halv hamnplats ledig.
                    //Denna kollar vidare om det finns plats för båten som kommer till hamnen.
                    else if (freeSpace > 0 && harbor[i, j] == null)
                    {
                        freeSpace++;
                        if (freeSpace == boatSpace)
                        {
                            float endingPlace = startingPlace + boat.SpaceTaken;
                            for (int k = startingPlace; k < endingPlace; k++)
                            {
                                for (int x = 0; x < harbor.GetLength(1); x++)
                                {
                                    harbor[k, x] = boat;
                                }
                            }
                            boat.StartingPlace = startingPlace + 1;
                            boat.EndingPlace = endingPlace;
                            //boat.PlaceOnHarbor = startingPlace + "-" + (endingPlace -1);
                            boats.Add(boat);
                            //Returnerar då Båten har lagts in i listan
                            return;
                        }
                    }
                    //Koden kommer hit om platsen är ledig och om platsen är första halvan då en båt större än en roddbåt behöver mer utrymme en 1 hamnplatstotalt.
                    else if (j == 0 && harbor[i, j] == null)
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
            removedBoats++;
            Debug.Print(boat.BoatType + " kom inte in i hamnen.");
        }
        static void ShowStatistics()
        {
            //Visar statistik om hamnen och de olika båtarna i hamnen.
            int rowBoatAmount = boats.Where(b => b is RowBoat).Count();
            int sailingBoatAmount = boats.Where(b => b is SailingBoat).Count();
            int powerBoatAmount = boats.Where(b => b is PowerBoat).Count();
            int cargoShipAmount = boats.Where(b => b is CargoShip).Count();
            int totalWeight = boats.Sum(b => b.Weight);
            double averageSpeed = Math.Round(boats.Average(b => b.MaxSpeed));
            int emptyPlaces = 0;
            List<Boat> SortedBoats = boats.OrderBy(o => o.StartingPlace).ToList();

            for (int i = 0; i < harbor.GetLength(0); i++)
            {
                if (harbor[i, 0] == null)
                    emptyPlaces++;
            }

            Console.Clear();
            Console.WriteLine("{0, 7}{1,15}{2,10}{3,10}{4,10}{5,20}", "Plats", "Båttyp", "Id", "Vikt", "MaxHast", "Övrigt");
            foreach (var boat in SortedBoats)
            {
                if (boat.StartingPlace == boat.EndingPlace)
                    Console.Write($"{boat.StartingPlace,7}");
                else
                {
                    string place = boat.StartingPlace + " - " + boat.EndingPlace;
                    Console.Write($"{place,7}");
                }
                // Console.WriteLine("\t" + boat.BoatType + "\t" + boat.Id + "\t" + boat.Weight + "\t" + boat.KnotToKmh() + " km/h " + "\t" + boat.UniqueProperty());
                Console.WriteLine("{0, 15}{1,10}{2,10}{3,10}{4,20}", boat.BoatType, boat.Id, boat.Weight, boat.KnotToKmh() + " km/h", boat.UniqueProperty());

            }

            Console.WriteLine();
            Console.WriteLine($"Roddbåtar: {rowBoatAmount} Segelbåtar: {sailingBoatAmount} Motorbåtar: {powerBoatAmount} LastFartyg: {cargoShipAmount} Avvisade: {removedBoats}");
            Console.WriteLine($"Total vikt i hamnen: {totalWeight} Medeltal av hastighet: {averageSpeed} knop");
            Console.WriteLine("Lediga platser: " + emptyPlaces);

            Save("Boats.txt", SortedBoats);

        }
        static void Save<T>(string fileName, List<T> list)
        {
            // Gain code access to the file that we are going
            // to write to
            try
            {
                // Create a FileStream that will write data to file.
                using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, list);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static List<T> Load<T>(string fileName)
        {
            var list = new List<T>();
            if (File.Exists(fileName))
            {

                try
                {
                    // Create a FileStream will gain read access to the
                    // data file.
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        var formatter = new BinaryFormatter();
                        list = (List<T>)
                            formatter.Deserialize(stream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return list;
        }
        static void LoadBoatsFromFile()
        {
            //Ladda båtarna från filen och lägg till de i hamnen.
            List<Boat> boatsFromFile = Load<Boat>("Boats.txt");
            foreach (var boat in boatsFromFile)
            {
                AddBoatToHarbor(boat);
            }
            if (boatsFromFile.Count > 0)
            {
                ShowStatistics();
                Console.ReadLine();
            }
        }
    }
}
