using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    public int numClouds = 40; //liczba chmur do wygenerowania
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f; //Modyfikuje prędkość chmur
    private GameObject[] cloudInstances;
    void Awake()
    {
        cloudInstances = new GameObject[numClouds];
        GameObject anchor = GameObject.Find("CloudAnchor"); //odszukuje rodzica
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            cloud = Instantiate<GameObject>(cloudPrefab);
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            float scaleU = Random.value; //zdefiniuj rozmiar chmury
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //mniejsze chmury powinny być bliżej powierzchni ziemi i dalej (od kamery)
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU;
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            cloud.transform.SetParent(anchor.transform);
            cloudInstances[i] = cloud;
        }
    }
    void Update()
    {
        //Przetwórz każdą utworzoną chmurę
        foreach (GameObject cloud in cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //Większe chmury poruszają się szybciej, jeżeli poruszała się zbyt szybko na lewo to przemieść ją maksymalnie na prawo
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            if (cPos.x <= cloudPosMin.x)
            {
                cPos.x = cloudPosMax.x;
            }
            //zastosuj nowe położenie
            cloud.transform.position = cPos;
        }
    }
}
