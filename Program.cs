using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HeistClassical
{
    class Program
    {
        static void Main(string[] args)
        {
            var roladex = new List<IRobber>();

            while (true)
            {
                var robber = CollectRobberInfo();

                if (robber == null)break;

                roladex.Add(robber);
            }

            var bank = CreateRandomBank();

            PrintBankSummary(bank);

            var crew = new List<IRobber>();

            while (true)
            {
                var recruit = CollectRoladexMember(roladex, crew);

                if (recruit == null)break;

                crew.Add(recruit);
            }

            PerformHeist(bank, crew);

        }

        private static void PerformHeist(Bank bank, List<IRobber> robbers)
        {
            robbers.ForEach(r =>
            {
                r.PerformSkill(bank);
                Thread.Sleep(2000);
            });

            var resultMessage = bank.IsSecure ?
                "Failed to rob the bank!" :
                "Successfully robbed the bank!";

            Console.WriteLine(resultMessage);
        }

        private static IRobber CollectRoladexMember(List<IRobber> roladex, List<IRobber> crew)
        {
            var availableCut = 100 - crew.Aggregate(0, (total, member) => member.PercentageCut + total);
            var options = roladex.Where(x =>
            {
                return !crew.Contains(x) && x.PercentageCut <= availableCut;
            });

            var index = -1;
            while (index < 0 || index >= options.Count())
            {
                Console.WriteLine($"Choose a crew member (1-{options.Count()})");

                for (int i = 0; i < options.Count(); i++)
                {
                    var option = options.ElementAt(i);
                    Console.WriteLine($"{i+1}. {option.Name} [{option.SkillLevel}% {option.SpecialtyName}]: {option.PercentageCut}% Cut");
                }

                var indexString = Prompt();

                if (string.IsNullOrWhiteSpace(indexString))return null;

                int.TryParse(indexString, out index);

                index -= 1;
            }

            return options.ElementAt(index);
        }

        private static Bank CreateRandomBank()
        {
            var rand = new Random();

            return new Bank
            {
                AlarmScore = rand.Next(1, 100),
                    SecurityGaurdScore = rand.Next(1, 100),
                    VaultScore = rand.Next(1, 100)
            };
        }

        private static void PrintBankSummary(Bank bank)
        {
            var als = bank.AlarmScore;
            var sgs = bank.SecurityGaurdScore;
            var vs = bank.VaultScore;
            string mostSecure;
            string leastSecure;

            if (als >= sgs && als >= vs)mostSecure = "Alarm";
            else if (sgs >= als && sgs >= vs)mostSecure = "Security Guards";
            else mostSecure = "Vault";

            if (als <= sgs && als <= vs)leastSecure = "Alarm";
            else if (sgs <= als && sgs <= vs)leastSecure = "Security Guards";
            else leastSecure = "Vault";

            Console.WriteLine("Bank Recon Report:");
            Console.WriteLine($"Most Secure: {mostSecure}");
            Console.WriteLine($"Least Secure: {leastSecure}");
        }

        private static IRobber CollectRobberInfo()
        {
            Console.Clear();
            Console.WriteLine("Enter the name of a contact");
            var name = Prompt();

            if (String.IsNullOrWhiteSpace(name))return null;

            var specialty = 0;
            while (specialty < 1 || specialty > 3)
            {
                Console.Clear();
                Console.WriteLine($"What is {name}'s specialty?");
                Console.WriteLine("1. Hacker (disables alarms)");
                Console.WriteLine("2. Muscle (disarms guards)");
                Console.WriteLine("3. Lock Specialist (cracks vaults)");

                var specialtyString = Prompt();
                int.TryParse(specialtyString, out specialty);
            }

            var skillLevel = 0;
            while (skillLevel < 1 || skillLevel > 100)
            {
                Console.Clear();
                Console.WriteLine($"What is {name}'s skill level? (1-100)");

                var skillLevelString = Prompt();
                int.TryParse(skillLevelString, out skillLevel);
            }

            var cut = 0;
            while (cut < 1 || cut > 100)
            {
                Console.Clear();
                Console.WriteLine($"What is {name}'s cut? (1-100)");

                var cutString = Prompt();
                int.TryParse(cutString, out cut);
            }

            switch (specialty)
            {
                case 1:
                    return new Hacker
                    {
                        Name = name,
                            SkillLevel = skillLevel,
                            PercentageCut = cut
                    };
                case 2:
                    return new Muscle
                    {
                        Name = name,
                            SkillLevel = skillLevel,
                            PercentageCut = cut
                    };
                case 3:
                    return new LockPickSpecialist
                    {
                        Name = name,
                            SkillLevel = skillLevel,
                            PercentageCut = cut
                    };
                default:
                    return null;
            }
        }

        private static string Prompt()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }
    }
}