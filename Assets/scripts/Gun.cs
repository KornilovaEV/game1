using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float offset;
    public float startTimeBtwShots; //время перезарядки
    
    public GunType gunType;
    public Joystick joystick;
    public GameObject bullet; // патрон
    public Transform shotPoint; //место, откуда пуля вылетает

    private Player player;
    private Vector3 difference;
    private float timeBtwShots; //время между выстрелами
    private float rotZ;//вращение пушки

    public enum GunType{Default, Enemy}

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(player.controlType == Player.ControlType.PC && gunType == GunType.Default){
            joystick.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(gunType == GunType.Default){
            if(player.controlType == Player.ControlType.PC){
                difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }
            else if(player.controlType == Player.ControlType.Android){
                rotZ = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;
            }
        }
        else if (gunType == GunType.Enemy){
            difference = player.transform.position - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        
    
    transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
    
    if(timeBtwShots <=0)
    {  
        if(Input.GetMouseButton(0) && player.controlType == Player.ControlType.PC || gunType == GunType.Enemy){   
            Shoot();
        }
        else if(player.controlType == Player.ControlType.Android && Mathf.Abs(joystick.Horizontal) > 0.3f || Mathf.Abs(joystick.Vertical) > 0.3f){
            if(joystick.Vertical != 0 || joystick.Horizontal != 0){
                Shoot();
            }
        }
    }
    else{
        timeBtwShots -= Time.deltaTime;
        }
    }

    public void Shoot(){
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        timeBtwShots = startTimeBtwShots;

    }


}
