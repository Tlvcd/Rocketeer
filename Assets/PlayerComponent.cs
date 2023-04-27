using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UniSense;
public class PlayerComponent : MonoBehaviour
{
    #region variables
    private Rigidbody2D RGBody;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject gunPoint;
    [SerializeField] Camera cam;
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject Ground_Check;
    [SerializeField] LayerMask GroundCheck_Layers;
    [SerializeField] CameraOperations CamOP;
    [SerializeField] Text UI_Speed;
    [SerializeField] Text UI_Ammo;
    int ClipSize;
    [SerializeField] int Clips;
    [SerializeField] int MaxAmmo;
    [SerializeField] bool InfAmmo;
    [SerializeField] PlayerInput playerinput;
    int CurrAmmo;
    Vector3 MousePos;
    float RotZ;
    Vector3 PreviousPos = Vector3.zero;
    bool IsReloading=false;

    #endregion



    private void Start()
    {
        playerinput = this.GetComponent<PlayerInput>();
        RGBody = this.GetComponent<Rigidbody2D>();
        ExplosionDestroy.OnExplosion += OnExplode;
        ClipSize = MaxAmmo / Clips;
        CurrAmmo = ClipSize;
        UpdateAmmoUI();
    }
    void Update()
    {
        if (Physics2D.CircleCast(Ground_Check.transform.position, 0.2f, -transform.up, 0, GroundCheck_Layers)) { RGBody.drag = 1; 
            //Debug.Log("drag increased.");
            }
        else RGBody.drag = 0;
        
        
    }
    public void GunPoint(InputAction.CallbackContext context) //function to point gun in the direction specified by input system.
    {
        if (playerinput.currentControlScheme == "dualsense") MousePos = context.ReadValue<Vector2>();
        else MousePos = cam.ScreenToWorldPoint(context.ReadValue<Vector2>()) - gun.transform.position;
        RotZ = Mathf.Atan2(MousePos.y, MousePos.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.AngleAxis(RotZ,gun.transform.forward);
    }
    private void FixedUpdate() 
    {
        UI_Speed.text = SpeedMeasure(); 
    }
    public void Shoot(InputAction.CallbackContext context) //instantiate a bullet when button is clicked
    {
        if (context.started)
        {
            if (!InfAmmo && CurrAmmo == 0) { Invoke("reload", 1); return; }
            CurrAmmo -= 1;
            UpdateAmmoUI();
            GameObject Bullet = Instantiate(Projectile, gunPoint.transform.position, gunPoint.transform.rotation);
            Bullet.GetComponent<Projectile>().ValuePass(RGBody.velocity);
            RGBody.AddForce(-gun.transform.right * 500);
        }
    }
    void UpdateAmmoUI()
    {
        UI_Ammo.text = CurrAmmo + "/" + Clips;
    }
    void reload()
    {
        if (Clips == 0||IsReloading) return;
        IsReloading = true;
        CurrAmmo = ClipSize;
        Clips -= 1;
        UpdateAmmoUI();
        Invoke("ReloadDebounce",0.5f);
    }
    void ReloadDebounce()
    {
        IsReloading = false;
    }
    void OnExplode(Vector3 pos) //function called when a bullet explodes
    {
        float ShakeStrength = Mathf.Clamp(1 - (Vector3.Distance(pos, transform.position) / 100), 0, 1);
        Debug.Log("distance: "+(Vector3.Distance(pos, transform.position))+"\nStrength: "+ ShakeStrength);
        Debug.Log("exploded!");
        CamOP.ShakeCam(0.1f,ShakeStrength);
        
    }
    string SpeedMeasure() //measure the player speed and return it as a string.
    {
        float speed = ((transform.position - PreviousPos).magnitude/Time.fixedDeltaTime)/2;
        PreviousPos = transform.position;
        return speed.ToString("F1") + " m/s" + "\n" + (int)(speed * 3.6f) + " km/h";
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Ground_Check.transform.position, 0.2f);
    }
}
