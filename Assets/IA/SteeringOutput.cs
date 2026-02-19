using MyEngine = UnityEngine;
namespace IA26.Movement
{
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

