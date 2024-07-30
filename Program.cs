using AutomobiliuNuoma.Core.Contracts;
using AutomobiliuNuoma.Core.Models;
using AutomobiliuNuoma.Core.Repositories;
using AutomobiliuNuoma.Core.Service;
using System;
using System.Collections.Generic;



namespace ManoPrograma
{
    public class PagrindineKlase
    {
        static void Main()
        {
            IAutonuomaService autonuomaService = SetupDependencies();
            while (true)
            {
                Console.WriteLine("1. Rodyti visus automobilius");
                Console.WriteLine("2. Rodyti visus klientus");
                Console.WriteLine("3. Formuoti nuomos uzsakyma");
                Console.WriteLine("4. Prideti nauja klienta");
                Console.WriteLine("5. Prideti nauja automobili");
                Console.WriteLine("6. Gauti visus uzsakymus");
                string pasirinkimas = Console.ReadLine();
                switch (pasirinkimas)
                {
                    case "1":
                        List<Automobilis> auto = autonuomaService.GautiVisusAutomobilius();
                        foreach (Automobilis a in auto)
                        {
                            Console.WriteLine(a);
                        }
                        break;
                    case "2":
                        List<Klientas> klientai = autonuomaService.GautiVisusKlientus();
                        foreach (Klientas a in klientai)
                        {
                            Console.WriteLine(a);
                        }
                        break;
                    case "3":
                        Console.WriteLine("Nuomos uzsakymas: ");
                        foreach (Klientas k in autonuomaService.GautiVisusKlientus())
                        {
                            Console.WriteLine(k);
                        }

                        Console.WriteLine("Iveskite norimo kliento varda");
                        string vardas = Console.ReadLine();
                        Console.WriteLine("Iveskite norimo kliento pavarde");
                        string pavarde = Console.ReadLine();

                        foreach (Automobilis a in autonuomaService.GautiVisusAutomobilius())
                        {
                            Console.WriteLine(a);
                        }

                        Console.WriteLine("Pasirinkite automobili pagal Id sarase: ");
                        int autoId = int.Parse(Console.ReadLine());

                        Console.WriteLine("Iveskite kiek dienu autommobilis yra isnuomojamas: ");
                        int dienos = int.Parse(Console.ReadLine());

                        autonuomaService.SukurtiNuoma(vardas, pavarde, autoId, DateTime.Now, dienos);

                        break;

                    case "4":
                        Console.WriteLine("Irasykite kliento varda");
                        string klientoVardas = Console.ReadLine();
                        Console.WriteLine("Irasykite kliento pavarde");
                        string klientoPavarde = Console.ReadLine();
                        Console.WriteLine("Irasykite kliento gimimo data (yyyy-MM-dd)");
                        DateOnly klientoGimimoData = DateOnly.Parse(Console.ReadLine());

                        Klientas naujasKlientas = new Klientas(klientoVardas, klientoPavarde, klientoGimimoData);
                        autonuomaService.PridetiKlienta(naujasKlientas);

                        break;

                    case "5":
                        Console.WriteLine("Irasykite automobilio marke");
                        string marke = Console.ReadLine();
                        Console.WriteLine("Irasykite automobilio modeli");
                        string modelis = Console.ReadLine();
                        Console.WriteLine("Irasykite automobilio nuomos kaina");
                        double nuomosKaina = double.Parse(Console.ReadLine());

                        Console.WriteLine("Pasirinkite automobilio tipa (1: NaftosKuroAutomobilis, 2: Elektromobilis): ");
                        int tipas = int.Parse(Console.ReadLine());

                        Console.WriteLine("Irasykite automobilio Id");
                        int autoId5; 

                        if (int.TryParse(Console.ReadLine(), out autoId5))
                        {
                            Automobilis naujasAutomobilis = null;

                            if (tipas == 1)
                            {
                                Console.WriteLine("Irasykite degalu sanaudas");
                                double degaluSanaudos = double.Parse(Console.ReadLine());
                                naujasAutomobilis = new NaftosKuroAutomobilis(autoId5, marke, modelis, nuomosKaina, degaluSanaudos);
                            }
                            else if (tipas == 2)
                            {
                                Console.WriteLine("Irasykite baterijos talpa");
                                int baterijosTalpa = int.Parse(Console.ReadLine());
                                Console.WriteLine("Irasykite krovimo laika");
                                int krovimoLaikas = int.Parse(Console.ReadLine());
                                naujasAutomobilis = new Elektromobilis(autoId5, marke, modelis, nuomosKaina, baterijosTalpa, krovimoLaikas);
                            }
                            else
                            {
                                Console.WriteLine("Neteisingas automobilio tipas. Bandykite dar karta.");
                                break;
                            }

                            if (naujasAutomobilis != null)
                            {
                                autonuomaService.PridetiNaujaAutomobili(naujasAutomobilis);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Neteisingas automobilio Id. Bandykite dar karta.");
                        }
                        break;

                    case "6":
                        var uzsakymai = autonuomaService.GautiVisusUzsakymus();
                        if (uzsakymai.Count == 0)
                        {
                            Console.WriteLine("Nera uzsakymu.");
                        }
                        else
                        {
                            foreach (var uzsakymas in uzsakymai)
                            {
                                Console.WriteLine($"Uzsakovas: {uzsakymas.Uzsakovas.Vardas} {uzsakymas.Uzsakovas.Pavarde}, Nuomuojamas Auto: {uzsakymas.NuomuojamasAuto.Marke} {uzsakymas.NuomuojamasAuto.Modelis}, Nuomos Pradzia: {uzsakymas.NuomosPradzia.ToShortDateString()}, Dienu Kiekis: {uzsakymas.DienuKiekis}, Pabaigos Data: {uzsakymas.gautiPabaigosData().ToShortDateString()}, Nuomos Kaina: {uzsakymas.skaiciuotiNuomosKaina()}");
                            }
                        }
                        break;

                    default:
                        Console.WriteLine("Neteisingas pasirinkimas. Bandykite dar karta.");
                        break;
                }

            }
        }




        public static IAutonuomaService SetupDependencies()
        {
            IKlientaiRepository klientaiRepository = new KlientaiFileRepository("C:\\Users\\mantv\\source\\repos\\Automobilių nuomos sistema\\Klientai.csv");
            IAutomobiliaiRepository automobiliaiRepository = new AutomobiliaiFileRepository("C:\\Users\\mantv\\source\\repos\\Automobilių nuomos sistema\\Automobiliai.csv");
            IKlientaiService klientaiService = new KlientaiService(klientaiRepository);
            IAutomobiliaiService automobiliaiService = new AutomobiliaiService(automobiliaiRepository);
            return new AutonuomosService(klientaiService, automobiliaiService);
        }


    }
}




    




