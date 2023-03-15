using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBlast : MonoBehaviour
{
    [SerializeField] private LayerMask TargetLayer;
    private bool canUse = true;
    [SerializeField] private float cooldown = 5;
    public GameObject ParticleEffect;

    private void OnEnable()
    {
        ParticleEffect.SetActive(false);
    }
    public void PlayerTakeDamageEvent()
    {
        if (!gameObject.activeInHierarchy) return;
        //add particle effect
        if(!canUse) return;

        canUse= false;
        

        ParticleEffect.SetActive(true);
        SoundManager.Instance.HIT.Play();

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 3, TargetLayer);
        if (targets.Length > 0)
        {
            foreach (Collider2D tar in targets)
            {
                Vector3 blastDirection = tar.transform.position - gameObject.transform.position;
                blastDirection.Normalize();
                if (blastDirection != Vector3.zero)
                {
                    tar.gameObject.transform.position = gameObject.transform.position + (blastDirection*5);
                }
            }
        }
        StartCoroutine(resetUse());
    }

    IEnumerator resetUse()
    {
        yield return new WaitForSeconds(cooldown);
        canUse = true;
    }
}
