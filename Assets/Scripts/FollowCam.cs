using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; //ta zmienna to punkt docelowy
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    public float camZ; //wymagana wartość Z położenia kamery

    void Start()
    {

    }
    void Awake()
    {
        camZ = this.transform.position.z;
    }
    void FixedUpdate()
    {
        Vector3 destination;
        //jeśli nie ma POI, zwróć (0,0,0)
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;
            //jeśli POI jest pociskiem, sprawdź czy jest nieruchomy
            if (POI.tag == "Projectile")
            {
                //jeśli jest w stanie uśpienia (nie porusza się), to wróć do domyślnego widoku
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }


        //ogranicz położenie X i Y do wartości minimalnych
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing); //Interpoluj od bieżacego położenia kamery do pkt. docelowego
        destination.z = camZ;
        transform.position = destination; //położenie kamery równe wartości destination
        //ustaw orthographicSieze kamery żeby cały czas było widoczne podłoże
        Camera.main.orthographicSize = destination.y + 10;
    }
    void Update()
    {
        
    }
}
