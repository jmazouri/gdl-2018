using System;

namespace Player
{
    [Serializable]
    public struct MovementSetting
    {
        public float MaxSpeed;
        public float NoiseLevel;
        public float Acceleration;

        //TODO: if there is time make this dependent on the floor instead of running speed
        public float Friction;

        public MovementSetting(float maxSpeed, float noiseLevel, float acceleration, float friction)
        {
            MaxSpeed = maxSpeed;
            NoiseLevel = noiseLevel;
            Acceleration = acceleration;
            Friction = friction;
        }

    }
}
