using System;
using System.Collections.Generic;

class PragueParking
{
    static void Main(string[] args)
    {
        string[] parkingGarage = new string[100]; // Array som representerar 100 parkeringsplatser
        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("Välkommen till Iron Sheriffs parkering välj följande alternativen.");
            Console.WriteLine("1. Parkera fordon"); Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("2. Flytta fordon"); Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("3. Hämta fordon"); Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("4. Sök fordon"); Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("5. Visa parkeringsstatus"); Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("6. Optimera MC-parkering"); Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("7. Avsluta"); Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Välj ett alternativ (1-7): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ParkVehicle(parkingGarage);
                    break;
                case "2":
                    MoveVehicle(parkingGarage);
                    break;
                case "3":
                    RetrieveVehicle(parkingGarage);
                    break;
                case "4":
                    SearchVehicle(parkingGarage);
                    break;
                case "5":
                    ShowParkingStatus(parkingGarage);
                    break;
                case "6":
                    OptimizeMC(parkingGarage);
                    break;
                case "7":
                    isRunning = false;
                    Console.WriteLine("Programmet Stängs ner...");
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
        }
    }

    // Metod för att parkera ett fordon
    static void ParkVehicle(string[] parkingGarage)
    {
        Console.Write("Ange fordonstyp (CAR/MC): ");
        string vehicleType = Console.ReadLine().ToUpper();
        if (vehicleType != "CAR" && vehicleType != "MC")
        {
            Console.WriteLine("Ogiltig fordonstyp.");
            return;
        }

        Console.Write("Ange registreringsnummer: "); Console.ForegroundColor = ConsoleColor.DarkCyan;
        string regNr = Console.ReadLine().ToUpper();

        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] == null)
            {
                parkingGarage[i] = vehicleType + "#" + regNr;
                Console.WriteLine($"Fordon parkerat på plats {i + 1}.");
                return;
            }
            else if (vehicleType == "MC" && parkingGarage[i].StartsWith("MC") && !parkingGarage[i].Contains("|"))
            {
                parkingGarage[i] += "|" + vehicleType + "#" + regNr;
                Console.WriteLine($"MC parkerad tillsammans på plats {i + 1}.");
                return;
            }
        }

        Console.WriteLine("Inga lediga platser tillgängliga.");
    }

    // Metod för att flytta ett fordon
    static void MoveVehicle(string[] parkingGarage)
    {
        int currentSpot = ReadValidNumber("Ange nuvarande platsnummer (1-100): ") - 1; Console.ForegroundColor = ConsoleColor.Green;
        if (parkingGarage[currentSpot] == null)
        {
            Console.WriteLine("Ogiltig plats eller platsen är tom.");
            return;
        }

        int newSpot = ReadValidNumber("Ange ny platsnummer (1-100): ") - 1;
        if (parkingGarage[newSpot] != null)
        {
            Console.WriteLine("Ogiltig ny plats eller platsen är redan upptagen.");
            return;
        }

        parkingGarage[newSpot] = parkingGarage[currentSpot];
        parkingGarage[currentSpot] = null; // Rensa den gamla platsen
        Console.WriteLine($"Fordonet har flyttats från plats {currentSpot + 1} till plats {newSpot + 1}.");
    }

   // Metod för att hämta ett fordon
    static void RetrieveVehicle(string[] parkingGarage)
    {
        Console.Write("Ange registreringsnummer: "); Console.ForegroundColor = ConsoleColor.Blue;
        string regNr = Console.ReadLine().ToUpper();

        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] != null && parkingGarage[i].Contains(regNr))
            {
                parkingGarage[i] = null;
                Console.WriteLine($"Fordon med registreringsnummer {regNr} har hämtats från plats {i + 1}.");
                return;
            }
        }

        Console.WriteLine("Fordon hittas Ej.");
    }

    // Metod för att visa parkeringsstatus
    static void ShowParkingStatus(string[] parkingGarage)
    {
        Console.Clear();
        Console.WriteLine("Parkeringsstatus:");
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Plats {i + 1}: Ledig");
            }
            else if (parkingGarage[i].StartsWith("MC") && !parkingGarage[i].Contains("|"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Plats {i + 1}: Halvfull (1 MC)");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Plats {i + 1}: Full (Upptagen av {parkingGarage[i]})");
            }
            Console.ResetColor();
        }
    }

    // Metod för att optimera MC-parkering
    static void OptimizeMC(string[] parkingGarage)
    {
        List<string> workOrders = new List<string>();
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] != null && parkingGarage[i].StartsWith("MC") && !parkingGarage[i].Contains("|"))
            {
                for (int j = i + 1; j < parkingGarage.Length; j++)
                {
                    if (parkingGarage[j] != null && parkingGarage[j].StartsWith("MC") && !parkingGarage[j].Contains("|"))
                    {
                        parkingGarage[i] += "|" + parkingGarage[j];
                        workOrders.Add($"Flytta {parkingGarage[j]} från plats {j + 1} till plats {i + 1}");
                        parkingGarage[j] = null;
                        break;
                    }
                }
            }
        }

        Console.WriteLine("Optimering klar. Arbetsordrar:");
        foreach (string order in workOrders)
        {
            Console.WriteLine(order);
        }
    }

    // Metod för att söka efter ett fordon
    static void SearchVehicle(string[] parkingGarage)
    {
        Console.Write("Ange registreringsnummer: ");
        string regNr = Console.ReadLine().ToUpper();

        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] != null && parkingGarage[i].Contains(regNr))
            {
                Console.WriteLine($"Fordonet med registreringsnummer {regNr} står på plats {i + 1}.");
                return;
            }
        }

        Console.WriteLine("Fordon hittas Ej.");
    }

    // Metod för att läsa in ett giltigt tal med felhantering
    static int ReadValidNumber(string prompt)
    {
        int number;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out number) && number >= 1 && number <= 100)
            {
                return number;
            }
            Console.WriteLine("Ogiltigt nummer, pröva igen.");
        }
    }
}
