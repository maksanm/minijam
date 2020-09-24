using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StationContoller : MonoBehaviour
{
    public float maxHealth;
    public GameObject HitParticle;

    private Animator animator;

    [HideInInspector]
    public float currentHealth;

    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
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
        if (collider.tag == "Enemy")
        {
            SquadController enemy = collider.gameObject.GetComponent<SquadController>();
            if (enemy)
            {
                enemy.CallEngage(gameObject);
            }
                
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
       
        if (HitParticle)
        {
            GameObject ParticleSystemInst = Instantiate(HitParticle, transform.position, Quaternion.identity);
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

        // & Destroy
        Destroy(gameObject, 4f);
        Invoke("Death", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Death()
    {
        SceneManager.LoadScene("GameOver");
    }
}
