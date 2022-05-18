
//using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int damage;
    public int health;
    public float startTimeBtwAttack;
    public float startStopTime;

    public float speed;
    private float timeBtwAttack;
    private float stopTime;

    [HideInInspector] public bool playerNotInRoom;
    private bool stopped;

    public Player player;
    private Animator anim;
    public GameObject deathEffect;
    public AddRoom room;

    private bool facingRight = false;



    private void Start(){
        anim = GetComponent<Animator>();
        anim.SetBool("Running", true);
        player = FindObjectOfType<Player>();
        room = GetComponentInParent<AddRoom>();

    }
    
    private void Update()
    {
        if(!playerNotInRoom)
        {
            if(stopTime <= 0){
            stopped = false;
        }
            else {
            stopped = true;
            stopTime -= Time.deltaTime;
        }}

        else{
            stopped = true;
        }   

        if(health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            room.enemies.Remove(gameObject);
        }

        if(player.transform.position.x > transform.position.x){
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else{
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if(!stopped)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }


    public void TakeDamage(int damage)
    {
        stopTime = startStopTime;
        health -= damage;
    }
//ВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВВ
    public void OnEnemyAttack()
    {
        Instantiate(deathEffect, player.transform.position, Quaternion.identity);
        player.ChangeHealth(-damage);
        timeBtwAttack = startTimeBtwAttack;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(timeBtwAttack <= 0)
            {
                //OnEnemyAttack();
                anim.SetTrigger("EnemyAttack");
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

}
