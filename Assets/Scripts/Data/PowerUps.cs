using System;
using System.Collections.Generic;
using PowerUps;

namespace Data
{
    public static class PowerUps
    {
        public static PowerUpBase GetRandom()
        {
            return _powerUps[UnityEngine.Random.Range(0, _powerUps.Count-1)];
        }

        
        private static List<PowerUpBase> _powerUps = new()
        {
            new JetPackPowerUp()
            
        };
        
    }
}