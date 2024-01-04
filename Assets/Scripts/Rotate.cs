using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 20f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 25) * Time.deltaTime * speed);
    }
}
