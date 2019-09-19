using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundSun : MonoBehaviour
{
    public Transform Sun;
    public Transform Mercury; //水星
    public Transform Venus; //金星
    public Transform Earth;
    public Transform Mars; //火星
    public Transform Jupiter; //木星
    public Transform Saturn;  //土星
    public Transform Uranus; //天王星
    public Transform Neptune; //海王星
    public Transform moon;

    // Start is called before the first frame update
    void Start()
    {
        Sun.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Mercury.RotateAround(Sun.position, new Vector3(0,1,1), 80 * Time.deltaTime);
        Mercury.Rotate(new Vector3(0,1,1) * 5 * Time.deltaTime);

        Venus.RotateAround(Sun.position, new Vector3(0,1,2), 70 * Time.deltaTime);
        Venus.Rotate(new Vector3(0,1,2) * 10 * Time.deltaTime);

        Earth.RotateAround(Sun.position, new Vector3(0,5,1), 60 * Time.deltaTime);
        Earth.Rotate(new Vector3(0,5,1) * 15 * Time.deltaTime);

        Mars.RotateAround(Sun.position, new Vector3(0,3,1), 50 * Time.deltaTime);
        Mars.Rotate(new Vector3(0,3,1) * 20 * Time.deltaTime);

        Jupiter.RotateAround(Sun.position, new Vector3(0,10,1), 40 * Time.deltaTime);
        Jupiter.Rotate(new Vector3(0,10,1) * 25 * Time.deltaTime);

        Saturn.RotateAround(Sun.position, new Vector3(0,4,1), 30 * Time.deltaTime);
        Saturn.Rotate(new Vector3(0,4,1) * 30 * Time.deltaTime);

        Uranus.RotateAround(Sun.position, new Vector3(0,2,1), 20 * Time.deltaTime);
        Uranus.Rotate(new Vector3(0,2,1) * 35 * Time.deltaTime);

        Neptune.RotateAround(Sun.position, new Vector3(0,8,1), 10 * Time.deltaTime);
        Neptune.Rotate(new Vector3(0,8,1) * 40 * Time.deltaTime);

        moon.transform.RotateAround(Earth.position, Vector3.up, 20 * Time.deltaTime);
    }
}
