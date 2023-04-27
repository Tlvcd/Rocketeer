using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWater : MonoBehaviour
{
    public float velocity;
    [SerializeField] float Force;
    public float Height;
    [SerializeField] float DefaultHeight;
    [SerializeField] float Stiffnes;
    [SerializeField][Min(0)] float spring_K=0.015f;
    [SerializeField] float loss;
    [SerializeField] float damp=0.03f;
    [SerializeField] float motion_factor = 0.05f;



    private void Start()
    {
        DefaultHeight = transform.position.y;
        Height = transform.position.y;
        velocity = 0;
        /*var mesh = new Mesh();
        GetComponent(MeshFilter).mesh = mesh;
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;*/
    }

    public void WaterCalculate(float spring_const, float dampening)
    {
        Height = transform.position.y;
        float x = Height - DefaultHeight;
        loss = -dampening * velocity;
        Force = -spring_const * x + loss;


        velocity += Force;
        Vector2 Increment = new Vector2(transform.position.x, transform.position.y + velocity);
        transform.position = Increment;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D collider = collision.GetComponent<Rigidbody2D>();
        if(!collider) return;
        velocity = collider.velocity.y *motion_factor;
    }
}
