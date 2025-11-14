using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class FanCulling : MonoBehaviour
{
    [SerializeField] Camera mainCamera;



    List<MeshRenderer> meshes = new();
    Plane[] planes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        meshes = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());



        foreach (var mesh in meshes)
        {

            mesh.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        foreach (var mesh in meshes)
        {

          //  mesh.enabled = GeometryUtility.TestPlanesAABB(planes, mesh.bounds);
        }

    }
}
