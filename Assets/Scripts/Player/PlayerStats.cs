using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth;
    public AudioClip Diesound;
    public AudioClip hurtSound;
    [SerializeField]
    private GameObject
        deathChunkParticle,
        deathBloodParticle;

    public float currentHealth;
    private GameManager GM;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
       soundManager.Instance.playSound(hurtSound);
        if(currentHealth <= 0.0f)
        {
            soundManager.Instance.playSound(Diesound);
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        GM.Respawn();
        Destroy(gameObject);
    }
}
