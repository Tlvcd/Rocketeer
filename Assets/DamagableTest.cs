using UnityEngine;
using UnityEngine.UI;

public class DamagableTest : MonoBehaviour, IDamageable
{
    [SerializeField] float MaxHealth = 200;
    float Health;
    [SerializeField] Image Bar;
    [SerializeField] GameObject Canvas;
    public void TakeDamage(float damage,float distance)
    {

        Health -= damage*distance;
        Bar.fillAmount = Health / MaxHealth;
        if (Health != MaxHealth) Canvas.SetActive(true);
        if (Health <= 0) Destroy(gameObject);

    }
    private void Start()
    {
        Health = MaxHealth;
    }
    private void Update()
    {
        if (!Canvas.activeInHierarchy) return;
        Canvas.transform.rotation = Quaternion.identity;
        Canvas.transform.position = new Vector3(transform.position.x, transform.position.y+2, -0.5f);
    }

}
