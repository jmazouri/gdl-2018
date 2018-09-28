using System;

namespace Player
{
    [Serializable]
    public struct MovementSetting
    {
        public float MaxSpeed;
        public float NoiseLevel;

        public MovementSetting(float maxSpeed, float noiseLevel)
        {
            MaxSpeed = maxSpeed;
            NoiseLevel = noiseLevel;
        }

    }
}
