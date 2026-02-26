using MyEngine = UnityEngine;
namespace IA26.Movement
{
    // dont worry about dis it for some moovment stuff
    public struct SteeringOutput
    {
        public MyEngine.Vector2 linear;
        public float angular;
        public SteeringOutput(MyEngine.Vector2 linear, float angular)
        {
            this.linear = linear;
            this.angular = angular;
        }
    }
    
}
