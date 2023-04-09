using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{

    Rigidbody2D rb;
    public GameObject deathEffectPrefab;
    private GameObject deathEffect;
    public GameObject Player;
    public float waitTimeRespawn;
    private Vector3 respawnPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
    } 
        
        
       private void OnTriggerEnter2D(Collider2D collision)
        {
        //if (collision.gameObject.tag == "Death")
        //{
        //    Die();
        //}
        //else if (collision.gameObject.tag == "Checpoint")
        //{
        // respawnPoint = transform.position;
        //}
        if (collision.gameObject.tag == "Checkpoint")
            {
             respawnPoint = transform.position;
            }
        else if (collision.gameObject.tag == "Death")
        {
            Die();
        }
        }

    void Die()
        {
        GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        Destroy(deathEffect, 2f);
        Player.SetActive(false);
        Invoke("Respawn", waitTimeRespawn);
        }

        void Respawn()
        {
        Player.SetActive(true);
        transform.position = respawnPoint;

        }

}



