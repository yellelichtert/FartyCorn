using Behaviours.Obstacles;
using Random = UnityEngine.Random;

namespace Obstacles
{
    public class Obstacle01 : ObstacleBase
    {
        protected override float GetRandomHeight()
        {
            if (Random.Range(1,30) % 2 == 1)
            {
                return heightVariation;       
            }
            else
            {
                return -heightVariation;
            }
        }
    }
}
