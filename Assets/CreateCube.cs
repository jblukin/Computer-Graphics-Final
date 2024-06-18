using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCube : MonoBehaviour
{

    public Material material;

    void MakeCube()
    {

        Vector3[] vertices = {
        new Vector3 (0.5f, -0.5f, 0.5f),
        new Vector3 (-0.5f, -0.5f, 0.5f),
        new Vector3 (0.5f, 0.5f, 0.5f),
        new Vector3 (-0.5f, 0.5f, 0.5f),
        new Vector3 (0.5f, 0.5f, -0.5f),
        new Vector3 (-0.5f, 0.5f, -0.5f),
        new Vector3 (0.5f, -0.5f, -0.5f),
        new Vector3 (-0.5f, -0.5f, -0.5f),
        new Vector3 (0.5f, 0.5f, 0.5f),
        new Vector3 (-0.5f, 0.5f, 0.5f),
        new Vector3 (0.5f, 0.5f, -0.5f),
        new Vector3 (-0.5f, 0.5f, -0.5f),
        new Vector3 (0.5f, -0.5f, -0.5f),
        new Vector3 (0.5f, -0.5f, 0.5f),
        new Vector3 (-0.5f, -0.5f, 0.5f),
        new Vector3 (-0.5f, -0.5f, -0.5f),
        new Vector3 (-0.5f, -0.5f, 0.5f),
        new Vector3 (-0.5f, 0.5f, 0.5f),
        new Vector3 (-0.5f, 0.5f, -0.5f),
        new Vector3 (-0.5f, -0.5f, -0.5f),
        new Vector3 (0.5f, -0.5f, -0.5f),
        new Vector3 (0.5f, 0.5f, -0.5f),
        new Vector3 (0.5f, 0.5f, 0.5f),
        new Vector3 (0.5f, -0.5f, 0.5f)
        };

        int[] triangles = {
        //triangle 0
        0, 2, 3,
        //triangle 1
		0, 3, 1,
        //2
        8, 4, 5,
        //3
		8, 5, 9,
        //4
        10, 6, 7,
		//5
        10, 7, 11,
        //6
        12, 13, 14, 
        //7
		12, 14, 15,
        //8
        16, 17, 18,
        //9
		16, 18, 19,
        //10
        20, 21, 22,
        //11
		20, 22, 23
        };

        Color[] faceColors =
        {
            Color.black,
            Color.black,
            Color.black,
            Color.black,
            Color.yellow,
            Color.yellow,
            Color.yellow,
            Color.yellow,
            Color.yellow,
            Color.yellow,
            Color.white,
            Color.white,
            Color.gray,
            Color.gray,
            Color.gray,
            Color.gray,
            Color.magenta,
            Color.magenta,
            Color.magenta,
            Color.magenta,
            Color.cyan,
            Color.cyan,
            Color.cyan,
            Color.cyan

        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        //mesh.SetColors(faceColors);

        mesh.RecalculateBounds();

        mesh.Optimize();
    }

    private void Start()
    {

        MakeCube();

        GetComponent<MeshRenderer>().material = material;

        StartCoroutine(nameof(ColorSwap));

    }

    private void Update()
    {

        GetComponent<MeshRenderer>().material = material;

    }

    IEnumerator ColorSwap()
    {

        while (true)
        {

            material.SetColor("_MainColor", Random.ColorHSV());

            yield return new WaitForSecondsRealtime(2.5f);

        }

    }

}
