using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterComponent : MonoBehaviour
{
    [SerializeField] int resolution;
    [SerializeField] GameObject dyn_wat;
    float water_distance;
    Vector2 initial_pos;
    DynamicWater[] waterArr;
    [SerializeField] float spread = 0.0002f;
    [SerializeField] int passes = 8;
    Mesh mesh;
    [SerializeField]int[] tri;

    private void Start()
    {

        waterArr = new DynamicWater[resolution+1];
        initial_pos = new Vector2(transform.position.x - (transform.lossyScale.x / 2), transform.position.y + (transform.lossyScale.y / 2));
        water_distance = 1f / resolution;

        Debug.Log(water_distance + " water distance");
        float curr_dis = 0f;
        for (int i = 0; i <= resolution; i++)
        {

            GameObject spawn = Instantiate(dyn_wat, transform);
            Vector2 vec = new Vector2(-0.5f + curr_dis, 0.5f);
            spawn.transform.localPosition = vec;
            waterArr[i] = spawn.GetComponent<DynamicWater>();
            curr_dis += water_distance;
            Debug.Log(curr_dis + " current distance");
        }
        mesh = new Mesh();
        mesh.name = "liquid_tex";
        GetComponent<MeshFilter>().sharedMesh = mesh;
        mesh.MarkDynamic();
        CalcTriangles();
        ReCalcMesh();

        //splash(2, 5);
    }
    void CalcTriangles()
    {
        tri = new int[(resolution+1)*3];
        tri[0] =1;
        tri[1] = 0;
        tri[2] = 2;
        int curr_point=3;
        for (int i = 3; i < (resolution+1)*3; i=i+3)
        {
            Debug.Log(curr_point+" current point"+i+" current loop");
            tri[i] = curr_point;
            tri[i + 1] = 1;
            tri[i + 2] = curr_point - 1;
            curr_point++;
        }
    

    }
    void ReCalcMesh()
    {
        List<Vector3> verts = new List<Vector3>()
        {
             new Vector3(-0.5f, -0.5f),
            new Vector3(0.5f, -0.5f)
        };
        for (int i = 0; i <= resolution; i++)
        {
            verts.Add(new Vector3(waterArr[i].transform.localPosition.x, waterArr[i].transform.localPosition.y));
        }

        //Debug.Log(verts.Count+" verts count");
        
        mesh.SetVertices(verts);
        mesh.triangles = tri;
        mesh.RecalculateNormals();
    }
    private void FixedUpdate()
    {
        ReCalcMesh();
        float[] left = new float[waterArr.Length];
        float[] right = new float[waterArr.Length];
        foreach (DynamicWater WaterBody in waterArr)
        {
            WaterBody.WaterCalculate(0.015f, 0.03f);
        }
        /*for(int a = 0; a < waterArr.Length; a++)
        {
            left[a] = 0;
            right[a] = 0;
        }*/
        int length = waterArr.Length - 1;
        for (int p = 0; p < passes; p++)
        {
            for (int j = 0; j < waterArr.Length; j++)
            {
                if (j < waterArr.Length)
                {
                    //Debug.Log(j + " current loop\ncurrent contain " + right.Length);
                    if (j + 1 == waterArr.Length) return;
                    right[j] = spread * (waterArr[j].Height - waterArr[j+1].Height);

                    waterArr[j+1].velocity += right[j];
                }
                if (j > 0)
                {
                    left[j] = spread * (waterArr[j].Height - waterArr[j - 1].Height);
                    waterArr[j - 1].velocity += left[j];
                    right[j] = 0;
                }
                

            }
        }
        //ReCalcMesh();
    }
    private void splash(int index,float speed)
    {
        if (index >= 0 && index < waterArr.Length)
        {
            waterArr[index].velocity += speed;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
