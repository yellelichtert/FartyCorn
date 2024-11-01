using System.Collections.Generic;
using System.Linq;
using Behaviours.Obstacles;
using UnityEngine;

namespace Managers
{
    public static class ObstacleManager
    {
        private static readonly List<ObstacleBase> _obstacles;
        private static List<ObstacleBase> _availableObstacles;

        
        static ObstacleManager()
        {
            _obstacles = Resources.LoadAll<ObstacleBase>("Obstacles").ToList();
            
            SetAvailableObstacles(0);

            ScoreManager.LevelChanged += SetAvailableObstacles;
        }


        public static ObstacleBase GetRandom()
          => _availableObstacles[Random.Range(0, _availableObstacles.Count-1)];
        
        static void SetAvailableObstacles(int newLevel)
            => _availableObstacles = _obstacles.FindAll(o => o.difficulty <= newLevel);
    }
}