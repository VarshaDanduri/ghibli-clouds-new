using System.Collections.Generic;
using UnityEngine;

namespace GhibliClouds.Runtime
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GhibliCloudMetaMesh : MonoBehaviour
    {
        [Header("Mesh")]
        [SerializeField] private int longitude = 32;
        [SerializeField] private int latitude = 16;
        [SerializeField] private float baseRadius = 1.5f;

        [Header("Worley Noise")]
        [SerializeField] private int seed = 1;
        [SerializeField] private float noiseFrequency = 2.4f;
        [SerializeField] private float noiseStrength = 0.4f;

        [Header("Meta Mesh")]
        [SerializeField] private int metaballCount = 5;
        [SerializeField] private float metaballRadius = 0.8f;
        [SerializeField] private float metaballStrength = 0.6f;

        private readonly List<Vector3> _metaballCenters = new();
        private Mesh _mesh;

        private void OnEnable()
        {
            Regenerate();
        }

        private void OnValidate()
        {
            longitude = Mathf.Clamp(longitude, 8, 128);
            latitude = Mathf.Clamp(latitude, 4, 64);
            baseRadius = Mathf.Max(0.05f, baseRadius);
            noiseFrequency = Mathf.Max(0.001f, noiseFrequency);
            metaballCount = Mathf.Clamp(metaballCount, 1, 20);
            metaballRadius = Mathf.Max(0.05f, metaballRadius);
            Regenerate();
        }

        [ContextMenu("Regenerate Cloud Meta Mesh")]
        public void Regenerate()
        {
            EnsureMesh();
            GenerateMetaballCenters();
            BuildMesh();
        }

        private void EnsureMesh()
        {
            if (_mesh != null)
            {
                return;
            }

            var meshFilter = GetComponent<MeshFilter>();
            var existing = meshFilter.sharedMesh;
            if (existing != null && existing.name == "GhibliCloudMetaMesh")
            {
                _mesh = existing;
                return;
            }

            _mesh = new Mesh
            {
                name = "GhibliCloudMetaMesh"
            };
            meshFilter.sharedMesh = _mesh;
        }

        private void GenerateMetaballCenters()
        {
            _metaballCenters.Clear();
            var old = Random.state;
            Random.InitState(seed);

            for (var i = 0; i < metaballCount; i++)
            {
                var point = Random.insideUnitSphere * baseRadius * 0.7f;
                _metaballCenters.Add(point);
            }

            Random.state = old;
        }

        private void BuildMesh()
        {
            var verts = new List<Vector3>();
            var uvs = new List<Vector2>();
            var tris = new List<int>();

            for (var lat = 0; lat <= latitude; lat++)
            {
                var v = lat / (float)latitude;
                var phi = Mathf.PI * v;
                var sinPhi = Mathf.Sin(phi);
                var cosPhi = Mathf.Cos(phi);

                for (var lon = 0; lon <= longitude; lon++)
                {
                    var u = lon / (float)longitude;
                    var theta = Mathf.PI * 2f * u;
                    var dir = new Vector3(
                        sinPhi * Mathf.Cos(theta),
                        cosPhi,
                        sinPhi * Mathf.Sin(theta));

                    var samplePos = dir * baseRadius * noiseFrequency;
                    var worley = 1f - WorleyNoise3D.Sample(samplePos, seed);
                    var meta = EvaluateMetaballField(dir * baseRadius);
                    var radiusOffset = (worley * noiseStrength) + (meta * metaballStrength);
                    verts.Add(dir * (baseRadius + radiusOffset));
                    uvs.Add(new Vector2(u, v));

                    if (lat == latitude || lon == longitude)
                    {
                        continue;
                    }

                    var i = lat * (longitude + 1) + lon;
                    var next = i + longitude + 1;

                    tris.Add(i);
                    tris.Add(next);
                    tris.Add(i + 1);

                    tris.Add(i + 1);
                    tris.Add(next);
                    tris.Add(next + 1);
                }
            }

            _mesh.Clear();
            _mesh.SetVertices(verts);
            _mesh.SetUVs(0, uvs);
            _mesh.SetTriangles(tris, 0);
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
        }

        private float EvaluateMetaballField(Vector3 p)
        {
            var density = 0f;
            var radiusSq = metaballRadius * metaballRadius;
            foreach (var center in _metaballCenters)
            {
                var distSq = (p - center).sqrMagnitude;
                density += radiusSq / (distSq + radiusSq);
            }

            return Mathf.Clamp01(density / metaballCount);
        }
    }
}
