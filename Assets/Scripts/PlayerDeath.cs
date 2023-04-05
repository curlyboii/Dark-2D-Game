using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{

    Rigidbody2D rb;
    public GameObject DeathEffect;
    public GameObject Player;
    public float waitTimeRespawn;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    } 
        
        
       private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Death")
            {
                Die();

            }
        }
        void Die()
        {
            Instantiate(DeathEffect, transform.position, transform.rotation);

        Player.SetActive(false);
        Invoke("ReloadScene", waitTimeRespawn);
        

        }

        void ReloadScene()
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

}



