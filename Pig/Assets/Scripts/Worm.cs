using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public GameObject effect;
    public Transform grassTransform;
    Animator anim;
    public float waitTime;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(Waiting());
    }

    public void StoneParticlesRandomSpawn()
    {
        Vector3 particlesPosition = new Vector3(grassTransform.position.x + Random.Range(-2f, 2f), grassTransform.position.y + 3f, grassTransform.position.z);
        Instantiate(effect, particlesPosition, Quaternion.identity);
    }

    public void StoneParticlesSpawn()
    {
        Vector3 particlesPosition = new Vector3(grassTransform.position.x + Random.Range(-1f, 1f), grassTransform.position.y + 3f + Random.Range(-1f, 0), grassTransform.position.z);
        Instantiate(effect, particlesPosition, Quaternion.identity);
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("isActive", true);
    }

    public void ColliderActivation()
    {
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
    }
    public void ColliderDeactivation()
    {
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
