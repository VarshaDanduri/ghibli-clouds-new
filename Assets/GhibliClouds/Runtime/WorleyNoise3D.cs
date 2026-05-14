using UnityEngine;

namespace GhibliClouds.Runtime
{
    public static class WorleyNoise3D
    {
        public static float Sample(Vector3 p, int seed)
        {
            var baseCell = new Vector3Int(
                Mathf.FloorToInt(p.x),
                Mathf.FloorToInt(p.y),
                Mathf.FloorToInt(p.z));

            var minDistSq = float.MaxValue;

            for (var z = -1; z <= 1; z++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    for (var x = -1; x <= 1; x++)
                    {
                        var cell = baseCell + new Vector3Int(x, y, z);
                        var feature = cell + HashToUnit3(cell, seed);
                        var distSq = (feature - p).sqrMagnitude;
                        if (distSq < minDistSq)
                        {
                            minDistSq = distSq;
                        }
                    }
                }
            }

            return Mathf.Clamp01(Mathf.Sqrt(minDistSq));
        }

        private static Vector3 HashToUnit3(Vector3Int cell, int seed)
        {
            var h = (uint)(cell.x * 374761393 ^ cell.y * 668265263 ^ cell.z * 700001 ^ seed * 1442695041);
            h ^= h >> 13;
            h *= 1274126177u;
            h ^= h >> 16;

            var x = (h & 1023u) / 1023f;
            var y = ((h >> 10) & 1023u) / 1023f;
            var z = ((h >> 20) & 1023u) / 1023f;
            return new Vector3(x, y, z);
        }
    }
}
