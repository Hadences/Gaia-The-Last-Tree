using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TripleShot : MonoBehaviour
{
    private GameObject ProjectilePrefab;
    private Transform centerPoint;
    private Transform aimTransform;

    public void onPlayerShootListener()
    {
        if (!gameObject.activeInHierarchy) return;
        ProjectilePrefab = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getProjectile();
        centerPoint = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getCenterPoint();
        aimTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getAimTransform();


        //when the player shoots
        GameObject projectile = Instantiate(ProjectilePrefab, aimTransform.position, aimTransform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        Projectile projectileScript = ProjectilePrefab.GetComponent<Projectile>();

        Vector3 dir = (aimTransform.position - centerPoint.transform.position).normalized;
        Vector3 Leftdir = UtilsClass.ApplyRotationToVector(dir, 25);
        Vector3 Rightdir = UtilsClass.ApplyRotationToVector(dir, -25);

        rb.AddForce(Leftdir * projectileScript.projectileData.projectileForce, ForceMode2D.Impulse);

        projectile = Instantiate(ProjectilePrefab, aimTransform.position, aimTransform.rotation);
        rb = projectile.GetComponent<Rigidbody2D>();
        projectileScript = ProjectilePrefab.GetComponent<Projectile>();


        rb.AddForce(Rightdir * projectileScript.projectileData.projectileForce, ForceMode2D.Impulse);

    }
}
