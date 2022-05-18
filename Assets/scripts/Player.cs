using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Controls")]
    public ControlType controlType;
    public Joystick joystick;
    public enum ControlType{PC, Android}

    [Header("Speed")]
    public float speed;
    public GameObject SpeedPl;
    public SpeedPotion speedTimer;

    //public GameObject speedEffect;

    [Header("Health")]
    public int health;
    public Text healthDisplay;
    public GameObject potionEffect;

    [Header("Shield")]
    public GameObject shield;
    public Shield shieldTimer;
    public GameObject shieldEffect;

    [Header("Weapons")]
    public List<GameObject> unlockedWeapons;
    public GameObject[] allWeapons;
    public Image weaponsIcon;
    public Bullet bullet;
    
    
    [Header("Key")]
    public GameObject keyIcon;
    public GameObject wallEffect;


    private Rigidbody2D rb; 
    private Vector2 moveInput; //направление тела игрока
    private Vector2  moveVelocity; //итоговая скорость игрока
    private Animator anim;//

    private bool facingRight = false;
    private bool keyButtonPushed;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if(controlType == ControlType.PC)
        {
            joystick.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(controlType == ControlType.PC)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") );
        }
        else if(controlType == ControlType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        
        moveVelocity = moveInput.normalized * speed;

        if(moveInput.x == 0)
        {
            anim.SetBool("isRunning", false);
        }

        else
        {
            anim.SetBool("isRunning", true);
        }

        if(!facingRight && moveInput.x>0)
        {
            Flip();
        }
        else if(facingRight && moveInput.x<0)
        {
            Flip();
        }

        if(health <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            SwitchWeapon();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void ChangeHealth(int healthValue){
        if(!shield.activeInHierarchy || shield.activeInHierarchy && healthValue > 0){
        health += healthValue;
        healthDisplay.text  = "HP: " + health;
        }
        else if(shield.activeInHierarchy && healthValue < 0){
            shieldTimer.ReduceTimer(healthValue);
        }

    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Potion")){
            
            ChangeHealth(10);
            Instantiate(potionEffect, transform.position, Quaternion.identity);//POTION Eff
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Speed")){
            if(!SpeedPl.activeInHierarchy){
                
                SpeedPl.SetActive(true);
                speedTimer.gameObject.SetActive(true);
                speedTimer.isCooldown = true;
                Instantiate(shieldEffect, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                
            }
            else {
                speedTimer.ResetTimer();
                Instantiate(shieldEffect, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }  
        }

        else if(other.CompareTag("Shield")){
            if(!shield.activeInHierarchy){
                shield.SetActive(true);
                shieldTimer.gameObject.SetActive(true);
                shieldTimer.isCooldown = true;
                Instantiate(shieldEffect, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
            else {
                shieldTimer.ResetTimer();
                Instantiate(shieldEffect, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }  
        }
        else if(other.CompareTag("Weapon")){
            for(int i=0;i< allWeapons.Length;i++){
                if(other.name == allWeapons[i].name){
                    unlockedWeapons.Add(allWeapons[i]);
                }
            }
            SwitchWeapon();
            Destroy(other.gameObject);
        }

        else if(other.CompareTag("Key")){
            keyIcon.SetActive(true);
            Destroy(other.gameObject);
        }
    }
    public void SwitchWeapon(){
        for(int i=0;i< unlockedWeapons.Count;i++){
            if(unlockedWeapons[i].activeInHierarchy){
                unlockedWeapons[i].SetActive(false);
                if(i != 0){
                    unlockedWeapons[i - 1].SetActive(true);
                    weaponsIcon.sprite = unlockedWeapons[i - 1].GetComponent<SpriteRenderer>().sprite;
                }
                else {
                    unlockedWeapons[unlockedWeapons.Count - 1].SetActive(true);
                    weaponsIcon.sprite = unlockedWeapons[unlockedWeapons.Count - 1].GetComponent<SpriteRenderer>().sprite;
                }
                weaponsIcon.SetNativeSize();
                break;
            }
        }
    }

    public void OnKeyButtonDown(){
        keyButtonPushed = !keyButtonPushed;
    }
    private void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("Door") && keyButtonPushed && keyIcon.activeInHierarchy){
            Instantiate(wallEffect, other.transform.position, Quaternion.identity);
            keyIcon.SetActive(false);
            other.gameObject.SetActive(false);
            keyButtonPushed = false;
        }
    }
}
