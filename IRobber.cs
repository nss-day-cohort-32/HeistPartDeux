namespace HeistClassical
{
    public interface IRobber
    {
        string SpecialtyName { get; set; }
        string Name { get; set; }
        int SkillLevel { get; set; }
        int PercentageCut { get; set; }
        void PerformSkill(Bank bank);
    }
}