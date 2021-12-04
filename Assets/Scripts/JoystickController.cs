using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public static Vector2 dir;
    public Transform circle;
    public Transform outerCircle;
    private bool touchStart = false;
    private Vector2 source, dist;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            source = Input.mousePosition;
            //source = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            circle.transform.position = source;
            outerCircle.transform.position = source;
            circle.GetComponent<Image>().enabled = true;
            outerCircle.GetComponent<Image>().enabled = true;
        }
        if (Input.GetMouseButton(0))
        {
            touchStart = true;
            dist = Input.mousePosition;
            //dist = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        else
        {
            dir = Vector2.zero;
            touchStart = false;
        }
        if (touchStart)
        {
            Vector2 offset = dist - source;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            dir = direction;
            circle.transform.position = new Vector2(source.x + dir.x*20f, source.y + dir.y*20f);
        }
        else
        {
            circle.GetComponent<Image>().enabled = false;
            outerCircle.GetComponent<Image>().enabled = false;
        }
    }
}
