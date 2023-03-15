using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinSouls : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private GameObject Soul1;
    [SerializeField] private GameObject Soul2;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Soul1.transform.position = new Vector3(transform.position.x, Soul1.transform.position.y + radius, transform.position.z);
        Soul2.transform.position = new Vector3(transform.position.x, Soul2.transform.position.y - radius, transform.position.z);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(transform.rotation.x,transform.rotation.y,transform.rotation.z+5);
    }
}
