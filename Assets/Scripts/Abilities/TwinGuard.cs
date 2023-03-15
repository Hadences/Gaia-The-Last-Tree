using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinGuard : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private GameObject Shield1;
    [SerializeField] private GameObject Shield2;
    // Start is called before the first frame update
    private void OnEnable()
    {

        Shield1.transform.position = new Vector3(transform.position.x, Shield1.transform.position.y + radius, transform.position.z);
        Shield2.transform.position = new Vector3(transform.position.x, Shield2.transform.position.y - radius, transform.position.z);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + 5);
    }
}
