namespace HeistClassical
{
    public class Bank
    {
        public int CashOnHand { get; set; }
        public int AlarmScore { get; set; }
        public int VaultScore { get; set; }
        public int SecurityGaurdScore { get; set; }
        public bool IsSecure => AlarmScore > 0 || VaultScore > 0 || SecurityGaurdScore > 0;
    }
}