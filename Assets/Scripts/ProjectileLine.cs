using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //singleton
    public float minDist = 0.1f;
    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;
    void Awake()
    {
        S = this;
        line = GetComponent<LineRenderer>();
        //miej wyłączony LineRenderer aż do momentu, gdt będzie potrzebny
        line.enabled = false;
        points = new List<Vector3>();
    }
    //To jestw właściwość (czyli metoda przedsatwiająca się jako pole)
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //Gdy przypiszemy coś do _poi, musimy wszystko zresetować
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }
    public void AddPoint()
    {
        //dadaj punkt do lini
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return; //wróć, jeżeli nowy punkt nie jest położony odpowiednio daleko od poprzedniego punktu
        }
        if (points.Count ==0) //jeśli jest to miejsce oddawania strzału
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //ustaw dwa pierwsze punkty
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        }
        else
        {
            //Normalny sposób dodawania punktu
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count-1, lastPoint);
            line.enabled = true;
        }
    }
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                return (Vector3.zero);
            }
            return(points[points.Count-1]);
        }
    }
    void FixedUpdate()
    {
        if (poi == null)
        {
            //jeśli poi nie został zdefiniowany, poszukaj go
            if (FollowCam.POI !=null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; //wróć, jeśli nie znaleźliśmy poi
                }
            }
            else
            {
                return; //wróć, jeśli nie znaleźliśmy poi
            }
        }
        //Jeśli poi istnieje, jego położenie jest dodawane w każdym wywołaniu FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
        {
            poi = null;
        }
    }
}
