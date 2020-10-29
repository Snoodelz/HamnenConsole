using System;
using System.Collections.Generic;
using System.Text;

namespace HamnenConsole
{

    class Boat
    {
        private static readonly Random Random = new Random();

        public string BoatType { get; set; }
        public string Id { get; set; }
        public float SpaceTaken { get; set; }
        public int Weight { get; set; }
        public int MaxSpeed { get; set; }
        public int DaysToStay { get; set; }
        public int DaysStayed { get; set; } = 0;


        public int RandomNumber(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue + 1);
        }

        public string GenerateID(string type)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ";
            var stringChars = new char[3];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[Random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            var id = $"{type}-{finalString}";
            return id;
        }
    }
    class RowBoat : Boat
    {
        public RowBoat()
        {
            BoatType = "RoddBåt";
            SpaceTaken = 0.5f;
            Weight = RandomNumber(100, 300);
            MaxSpeed = RandomNumber(0, 3);
            DaysToStay = 1;
            Id = GenerateID("R");
            MaxPassengers = RandomNumber(1, 6);
        }

        public int MaxPassengers { get; set; }
    }
    class PowerBoat : Boat
    {
        public PowerBoat()
        {
            BoatType = "Motorbåt";
            SpaceTaken = 1f;
            Weight = RandomNumber(200, 3000);
            MaxSpeed = RandomNumber(0, 60);
            DaysToStay = 3;
            Id = GenerateID("M");
            HorsePowers = RandomNumber(10, 1000);
        }

        public int HorsePowers { get; set; }
    }
    class SailingBoat : Boat
    {
        public SailingBoat()
        {
            BoatType = "Segelbåt";
            SpaceTaken = 2f;
            Weight = RandomNumber(800, 6000);
            MaxSpeed = RandomNumber(0, 12);
            DaysToStay = 4;
            Id = GenerateID("S");
            BoatLength = RandomNumber(10, 60);
        }

        public int BoatLength { get; set; }
    }
    class CargoShip : Boat
    {
        public CargoShip()
        {
            BoatType = "Lastfartyg";
            SpaceTaken = 4f;
            Weight = RandomNumber(3000, 20000);
            MaxSpeed = RandomNumber(0, 20);
            DaysToStay = 6;
            Id = GenerateID("L");
            ContainerAmount = RandomNumber(0, 500);
        }
        public int ContainerAmount { get; set; }
    }

}
