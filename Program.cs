﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HeistClassical
{
    class Program
    {
        static void Main(string[] args)
        {
            var rolodex = CreateStartingRolodex();

            while (true)
            {
                var robber = CollectRobberInfo(rolodex.Count);

                if (robber == null) break;

                rolodex.Add(robber);
            }

            var bank = CreateRandomBank();

            PrintBankSummary(bank);

            var crew = new List<IRobber>();

            while (true)
            {
                var recruit = CollectRolodexMember(rolodex, crew);

                if (recruit == null) break;

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

            if (!bank.IsSecure) PrintPayoutReport(bank, robbers);
        }

        private static void PrintPayoutReport(Bank bank, List<IRobber> robbers)
        {
            Console.WriteLine($"${bank.CashOnHand} stolen from the vault\n\n");
            robbers.ForEach(r =>
            {
                var robberTake = bank.CashOnHand * (r.PercentageCut / (double)100);
                Console.WriteLine($"{r.Name}: ${robberTake:f2}");
            });
            var usersPercentage = (100 - robbers.Select(r => r.PercentageCut).Sum());
            var usersTake = bank.CashOnHand * (usersPercentage / (double)100);
            Console.WriteLine($"You: ${usersTake:f2}");
        }

        private static IRobber CollectRolodexMember(List<IRobber> rolodex, List<IRobber> crew)
        {
            var availableCut = 100 - crew.Aggregate(0, (total, member) => member.PercentageCut + total);
            var options = rolodex.Where(x =>
            {
                return !crew.Contains(x) && x.PercentageCut <= availableCut;
            });

            Console.WriteLine($"Available Percentage: {availableCut}");

            var index = -1;
            while (index < 0 || index >= options.Count())
            {
                Console.WriteLine($"Choose a crew member (1-{options.Count()} or <Enter> to begin the heist)");

                for (int i = 0; i < options.Count(); i++)
                {
                    var option = options.ElementAt(i);
                    Console.WriteLine($"{i + 1}. {option.Name} [{option.SkillLevel}% {option.SpecialtyName}]: {option.PercentageCut}% Cut");
                }

                var indexString = Prompt();

                if (string.IsNullOrWhiteSpace(indexString)) return null;

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
                VaultScore = rand.Next(1, 100),
                CashOnHand = rand.Next(20_000, 1_000_000)
            };
        }

        private static void PrintBankSummary(Bank bank)
        {
            var als = bank.AlarmScore;
            var sgs = bank.SecurityGaurdScore;
            var vs = bank.VaultScore;
            string mostSecure;
            string leastSecure;

            if (als >= sgs && als >= vs) mostSecure = "Alarm";
            else if (sgs >= als && sgs >= vs) mostSecure = "Security Guards";
            else mostSecure = "Vault";

            if (als <= sgs && als <= vs) leastSecure = "Alarm";
            else if (sgs <= als && sgs <= vs) leastSecure = "Security Guards";
            else leastSecure = "Vault";

            Console.WriteLine("Bank Recon Report:");
            Console.WriteLine($"Most Secure: {mostSecure}");
            Console.WriteLine($"Least Secure: {leastSecure}");
        }

        private static IRobber CollectRobberInfo(int rolodexCount)
        {
            Console.Clear();
            Console.WriteLine($"Currently {rolodexCount} contacts in your rolodex");
            Console.WriteLine("Enter the name of a contact. (Press <Enter> to continue)");
            var name = Prompt();

            if (String.IsNullOrWhiteSpace(name)) return null;

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

        private static List<IRobber> CreateStartingRolodex()
        {
            return new List<IRobber>
            {
                new Hacker
                {
                    Name = "Mr. White",
                        SkillLevel = 40,
                        PercentageCut = 20
                },
                new Hacker
                {
                    Name = "Mr. Gray",
                        SkillLevel = 60,
                        PercentageCut = 25
                },
                new LockPickSpecialist
                {
                    Name = "Mr. Pink",
                        SkillLevel = 95,
                        PercentageCut = 50
                },
                new LockPickSpecialist
                {
                    Name = "Mr. Red",
                        SkillLevel = 60,
                        PercentageCut = 20
                },
                new LockPickSpecialist
                {
                    Name = "Mr. Orange",
                        SkillLevel = 20,
                        PercentageCut = 10
                },
                new Muscle
                {
                    Name = "Mr. Blue",
                        SkillLevel = 95,
                        PercentageCut = 50
                },
                new Muscle
                {
                    Name = "Mr. Blue",
                        SkillLevel = 50,
                        PercentageCut = 20
                }
            };
        }

        private static string Prompt()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }
    }
}