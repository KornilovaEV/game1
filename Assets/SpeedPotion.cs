using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedPotion : MonoBehaviour
{
    public float cooldown;
    
    [HideInInspector] public bool isCooldown;

    private Image speedImage;
    private Player player;

    void Start()
    {
        speedImage = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isCooldown = true;
        player.speed = player.speed + 5;
        
    }

    void Update()
    {
        if(isCooldown){
            
            speedImage.fillAmount -= 1 / cooldown * Time.deltaTime;
            if(speedImage.fillAmount <= 0){
                speedImage.fillAmount = 1;
                player.speed = player.speed - 5;
                isCooldown = false;
                player.SpeedPl.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
    
    public void ResetTimer(){
        speedImage.fillAmount = 1;
    }

}

