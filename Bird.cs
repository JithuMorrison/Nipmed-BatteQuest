using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private float speed;
    private bool willAttack;
    public float attackRange = 10f;
    public string targetTag = "Face";
    private Transform target;
    private Animator animator;

    public void Initialize(float speed, bool willAttack)
    {
        this.speed = speed;
        this.willAttack = willAttack;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (willAttack && target == null)
        {
            GameObject targetObject = GameObject.FindWithTag(targetTag);
            if (targetObject != null && Mathf.Abs(targetObject.transform.position.x - transform.position.x) <= attackRange)
            {
                target = targetObject.transform;
            }
        }

        if (target != null && willAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Face"))
        {
            Debug.Log("Bird collided with Face.");
            willAttack = false;
        }
    }
}
