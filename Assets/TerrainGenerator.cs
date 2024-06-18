using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{

    [SerializeField] private Slider _SpeedSlider;

    private Material mat;

    private Mesh mesh;

    private Vector3[] vertices;

    private int[] triangles;

    [SerializeField, Range(20, 50)] private int xLength = 20, zLength = 20;

    // Start is called before the first frame update
    void Start()
    {

        mesh = new();

        mat = GetComponent<MeshRenderer>().material;

        GetComponent<MeshFilter>().mesh = mesh;

        GetComponent<MeshRenderer>().material = mat;

        DefineBaseShape();

        SetMesh();

    }

    void Update()
    {

        mat.SetFloat( "_Speed", _SpeedSlider.value * 2f );

    }
    void DefineBaseShape()
    {

        vertices = new Vector3[ ( xLength + 1 ) * ( zLength + 1 ) ];

        for ( int i = 0, z = 0; z <= zLength; z++ )
        {

            for ( int x = 0; x <= xLength; x++ )
            {

                vertices[ i ] = new Vector3( x, 0, z );
                i++;

            }

        }

        triangles = new int[ xLength * zLength * 6 ];

        int v = 0, t = 0;

        for ( int z = 0; z < zLength; z++ )
        {

            for ( int x = 0; x < xLength; x++ )
            {
     
                triangles[ t ] = v;
                triangles[ t + 1 ] = v + xLength + 1;
                triangles[ t + 2 ] = v + 1;
                triangles[ t + 3 ] = v + 1;
                triangles[ t + 4 ] = v + xLength + 1;
                triangles[ t + 5 ] = v + xLength + 2;

                v++;
                t += 6;

            }

            v++;

        }

    }

    void SetMesh()
    {

        mesh.Clear();

        mesh.vertices = vertices;

        mesh.triangles = triangles;

        mesh.RecalculateNormals();

    }

/*    private void OnDrawGizmos()
    {

        if ( vertices == null ) return;

        for ( int i = 0; i < vertices.Length; i++ )
        {

            Gizmos.DrawSphere( vertices[ i ], .1f );

        }

    }*/

}
