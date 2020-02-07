using UnityEngine;

namespace MPGameLib.Extensions
{
    public static class ParticleExtensions
    {
        public static void StopAndClear(this ParticleSystem particleSystem)
        {
            particleSystem.Clear();
            particleSystem.Stop();
        }
    }
}