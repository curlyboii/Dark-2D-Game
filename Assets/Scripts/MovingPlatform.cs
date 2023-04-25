using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.U2D;

public class MovingPlatform : MonoBehaviour
{

    public Transform platform;
    public Transform startPoint;
    public Transform endPoint;
    Vector2 targetPosition;

    int diraction = 1; 
    public float speed;

    private void Start()
    {
        targetPosition= startPoint.position;
    }

    private void Update()
    {
        //Vector2 target = currentMovementTarget();

        //platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);

        //float distance = (target - (Vector2)platform.position).magnitude;
        //if (distance < 0.1f)
        //{
        //    diraction *= -1;
        //}

        if (Vector2.Distance(transform.position, startPoint.position) < .1f) targetPosition = endPoint.position;
        if (Vector2.Distance(transform.position, endPoint.position) < .1f) targetPosition = startPoint.position;

        transform.position =Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    Vector2 currentMovementTarget()
    {
        if(diraction == 1)
        {
            return startPoint.position;

        }
        else
        {
            return endPoint.position; 
        }

    }

    private void OnDrawGizmos()
    {
        //visualization
        if(platform!= null && startPoint!= null && endPoint!= null)
        {
            Gizmos.DrawLine(platform.transform.position, startPoint.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
          // Debug.Log("Player on platform");

         }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
           // Debug.Log("Player exit platform");

        }
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.name == "Cave - Platforms_13")
    //    {
    //        collision.transform.SetParent(this.transform);
    //        Debug.Log("Player on platform");

    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.name == "Cave - Platforms_13")
    //    {
    //        collision.transform.SetParent(null);
    //        Debug.Log("Player exit platform");

    //    }
    //}

}
