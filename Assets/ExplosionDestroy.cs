using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroy : MonoBehaviour
{
    [SerializeField] float Force;
    [SerializeField] CameraOperations CamOP;
    public delegate void OnExploded(Vector3 pos);
    public static event OnExploded OnExplosion;
    private void Start()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 10f);
        foreach (Collider2D obj in col)
        {
            Rigidbody2D rgbody= obj.GetComponent<Rigidbody2D>();
            
            
            if (rgbody != null)
            {

                IDamageable Damagable = obj.GetComponent<IDamageable>();
                float NormalizedDistance = 1f - Mathf.Clamp(Vector2.Distance(rgbody.transform.position, transform.position) / 10f, 0f, 1f);
                rgbody.AddForce((Vector3.Normalize(rgbody.transform.position - transform.position)) * (Force * NormalizedDistance), ForceMode2D.Impulse);
                rgbody.AddTorque(Force);
                //Debug.Log(rgbody.gameObject+" Force: "+ (Force * NormalizedDistance)/Force+"%");
                //Debug.DrawLine(transform.position, obj.transform.position);
                if (Damagable != null) Damagable.TakeDamage(100,NormalizedDistance);
            }
        }
        OnExplosion(transform.position);
        Destroy(this, 2);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
}
public class Explosions
{
    public virtual void OnExplode()
    {
        Debug.Log("exploded");
    }
}
