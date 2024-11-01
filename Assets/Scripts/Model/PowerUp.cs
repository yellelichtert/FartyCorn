using System;

namespace Model
{
    public class PowerUp : CollectableData
    {
        public int Power { get; set; }
        
        public int UpgradeLevels { get; set; }
        public int CurrentLevel { get; set; }
        
        public float UpgradeCost { get; set; }
        public float UpgradeMultiplier { get; set;}
    }
}