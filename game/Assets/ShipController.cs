using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public float maxHealth;

    [HideInInspector]
    public bool isDead;

    private float currentHealth;

    private Animator animator;
    private SquadController Squad;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        Squad = GetComponentInParent<SquadController>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isDead)
        {
            if (collider.tag == "Projectile")
            {
                BeamProjectile beam = collider.GetComponent<BeamProjectile>();
                if (beam.team != tag)
                {
                    TakeDamage(beam.damage);
                    Destroy(collider.gameObject, beam.thirstPower);
                }
            }
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
        Squad.attackCooldown += 2*Squad.stepCooldown;
        Squad.PlayExplosion();
        isDead = true;
        transform.SetParent(null);
        GetComponent<Collider2D>().enabled = false;
        

        // explosion animation
        animator.SetTrigger("Die");
        //...

        // & Destroy
        Destroy(gameObject, 4f);
    }
}
