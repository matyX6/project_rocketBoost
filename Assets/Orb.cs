using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{

    int rotationMode;

    // Use this for initialization
    void Start()
    {
        rotationMode = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        RotateOrb(rotationMode);
    }

    private void RotateOrb(int mode)
    {
        switch (mode)
        {
            case 1:
                transform.Rotate(Vector3.up * 1.1f);
                transform.Rotate(Vector3.right * 1.1f);
                break;

            case 2:
                transform.Rotate(-Vector3.up * 1.1f);
                transform.Rotate(Vector3.right * 1.1f);
                break;

            case 3:
                transform.Rotate(Vector3.up * 1.1f);
                transform.Rotate(-Vector3.right * 1.1f);
                break;

            default:
                transform.Rotate(-Vector3.up * 1.1f);
                transform.Rotate(-Vector3.right * 1.1f);
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
