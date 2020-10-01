using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    public GameObject launchPoint;
    public GameObject prefabProjectile;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    public float velocityMult = 8f;
    private Rigidbody projectileRigidbody;
    
    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }
    void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }
    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }
    void OnMouseDown()
    {
        //przycisk naciśnięty nad procą
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        //projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
    void Update()
    {
        //jeśli proca nie działa w trybie aimingMode, nie uruchamiaj tego kodu
        if (!aimingMode) return;
        //pobierz współrzęde myszy na ekranie 2D
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //oblicz wektor różnicy między launchPos i mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //ogranicznie wartości mouseDelta do promienia zderzacza kulistego dla obiektu Slinshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        //przemieść pocisk do nowego położenia
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {
            //przycisk myszy został zwolniony
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
        }
    }
}
