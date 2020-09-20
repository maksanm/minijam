using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationContoller : MonoBehaviour
{
    public float maxHealth;
    public GameObject HitParticle;

    private Animator animator;

    private float currentHealth;

    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isDead)
        {
            if (collider.tag == "Projectile")
            {
                BeamProjectile beam = collider.GetComponent<BeamProjectile>();
                if (collider.GetComponent<BeamProjectile>().team != tag)
                {
                    Destroy(collider.gameObject, 0.1f);
                    TakeDamage(beam.damage);
                }
            }
        }
        if (collider.tag == "Enemy" && collider)
        {
            SquadController enemy = collider.gameObject.GetComponent<SquadController>();
            Debug.Log(enemy);
            Debug.Log(gameObject);
            enemy.CallEngage(gameObject);
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;

        GameObject ParticleSystem = GetComponentInParent<SquadController>().hitParticleSystem;

        if (ParticleSystem)
        {
            GameObject ParticleSystemInst = Instantiate(ParticleSystem, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        isDead = true;
        GetComponent<Collider2D>().enabled = false;

        // explosion animation
        animator.SetTrigger("Die");
        //...

        // & Destroy
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
