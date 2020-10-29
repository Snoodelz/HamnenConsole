using System;
using System.Collections.Generic;
using System.Text;

namespace HamnenConsole
{
    class TempGarbage
    {
        Boat[,] harbor = new Boat[64, 2];
        int place = 0;
            for (int i = 0; i< 5; i++)
            {
                var randomBoat = rnd.Next(0, 3);
        Boat boat;
                switch (randomBoat)
                {
                    case 0:
                        boat = new PowerBoat();
        harbor[place, 0] = boat;
                        harbor[place, 1] = boat;
                        place += 1;
                        break;
                    case 1:
                        boat = new SailingBoat();
        harbor[place, 0] = boat;
                        harbor[place, 1] = boat;
                        place += 1;
                        harbor[place, 0] = boat;
                        harbor[place, 1] = boat;
                        place += 1;
                        break;
                    case 2:
                        boat = new CargoShip();
        harbor[place, 0] = boat;
                        harbor[place, 1] = boat;
                        place += 1;
                        harbor[place, 0] = boat;
                        harbor[place, 1] = boat;
                        place += 1;
                        harbor[place, 0] = boat;
                        harbor[place, 1] = boat;
                        place += 1;
                        harbor[place, 0] = boat;
                        harbor[place, 1] = boat;
                        place += 1;
                        break;
                }
    void AddBoatToHarbor(Boat boat)
    {
        for (int i = 0; i < boat.SpaceTaken; i++)
        {
            var boatOnPlace = new List<Boat>();
            boatOnPlace.Add(boat);
            if (boat is RowBoat == false)
            {
                boatOnPlace.Add(boat);
            }
            harborList.Add(boatOnPlace);

        }
    }


    //var uniqueHarbor = flatHarbor.Distinct();

    //foreach (var item in uniqueHarbor)
    //{
    //    Console.WriteLine(item.Id + " " + item.BoatType + " " + item.DaysStayed);
    //}
    //var uniqueHarbor = harborList.GroupBy(x => x).Select(y => y.FirstOrDefault());
    //foreach (var boat in uniqueHarbor)
    //{
    //    if (boat[0] == boat[1])
    //        Console.WriteLine(boat[0].Id + " " + boat[0].BoatType);
    //    else
    //    {
    //        foreach (var placeHalf in boat)
    //        {
    //            Console.WriteLine(placeHalf.Id + " " + placeHalf.BoatType);
    //        }
    //    }
    //}
}
    }
}
