using System;

namespace HeistClassical
{
    public class Muscle : IRobber
    {
        public string SpecialtyName { get; set; } = "Muscle";
        public string Name { get; set; }
        public int SkillLevel { get; set; }
        public int PercentageCut { get; set; }

        public void PerformSkill(Bank bank)
        {
            bank.SecurityGaurdScore -= SkillLevel;
            Console.WriteLine($"{Name} is disarming the security guards. Decreased security {SkillLevel} points");

            if (bank.SecurityGaurdScore <= 0)
            {
                Console.WriteLine($"{Name} has disarmed all the guards!");
            }

        }
    }
}