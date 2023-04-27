using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    [SerializeField]
    Rigidbody2D RGbody;
    [SerializeField]
    PointEffector2D Effector;
    [SerializeField]
    Collider2D sphereExplode;
    Vector3 InitialPos;
    [SerializeField]
    GameObject ExplosionPrefab;
    Vector3 ParentVel;
    void Start()
    {
        InitialPos = transform.position;
        RGbody.velocity = (transform.right * 30)+ new Vector3(0,ParentVel.y);
        Invoke(nameof(ExplodeTrigger), 12);
    }
    public void TakeDamage(float damage,float distance)
    {
        if (damage * distance > 30)
        {
            ExplodeTrigger();
        }
    }
    public void ValuePass(Vector3 velocity)
    {
        ParentVel = velocity;
    }
    private void FixedUpdate()
    {
        RGbody.MoveRotation(Quaternion.LookRotation(RGbody.velocity, transform.up));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Effector")) return;
        ExplodeTrigger();
    }
    void ExplodeTrigger()
    {
        Instantiate(ExplosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
