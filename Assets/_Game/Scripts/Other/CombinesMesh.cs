//using UnityEngine;

//public class CombinesMesh : MonoBehaviour
//{
//    public void CombineMeshes()
//    {
//        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
//        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

//        for (int i = 0; i < meshFilters.Length; i++)
//        {
//            combine[i].mesh = meshFilters[i].sharedMesh;
//            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
//            meshFilters[i].gameObject.SetActive(false); // Disable original objects
//        }

//        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
//        meshFilter.mesh = new Mesh();

//        // Set the index format to UInt32 to support more than 65535 vertices
//        meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

//        meshFilter.mesh.CombineMeshes(combine);

//        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
//        meshRenderer.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
//        gameObject.SetActive(true); // Enable combined object
//    }
//}
