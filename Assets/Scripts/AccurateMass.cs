using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AccurateMass : MonoBehaviour
{
    [SerializeField] float density;
    [SerializeField] bool calculateForChildren = true;

    void Start()
    {
        if (!GameManager.Instance.UseAccurateDensities) return;

        if (calculateForChildren)
        {
            foreach (Rigidbody childRb in GetComponentsInChildren<Rigidbody>())
            {
                childRb.mass = VolumeOfMesh(childRb.GetComponent<MeshFilter>().sharedMesh) * density;
            }
        } else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb) { rb.mass = VolumeOfMesh(GetComponent<MeshFilter>().sharedMesh) * density; }
        }
    }


    // Credit to "Statement" On Unity Answers and "Frank Krueger" On Stack Overflow for this code :)
    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    public float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }
}
