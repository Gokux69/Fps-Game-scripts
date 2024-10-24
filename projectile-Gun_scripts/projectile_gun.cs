using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

using UnityEngine.UI;



public class projectile_gun : MonoBehaviour
{
   public GameObject bullet;
   
   
   //bullet force
   public float shootForce , upwardForce;
   
   //recoil
   public Rigidbody rb;
   public float recoilForce;
   
   //Gun stats
   public float timeBetweenShooting,spread ,reloadTime , timeBetweenShots;
   public int magzineSize, bulletsPerTap;
   public bool allowButtonHold;

   int bulletLeft, bulletShot;
   
   //situations
   private bool shooting , readyToshoot, reloading;
    //Refrences
    public Camera fpsCamera;
    public Transform attackPoint;
    
    //bug fixing
    public bool allowInvoke = true;
    //graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    
    private void Awake()
    {
       
        //magzine is full
        bulletLeft = magzineSize;
        readyToshoot = true;
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        //Set ammo display, if it exists :D
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletLeft / bulletsPerTap + " / " + magzineSize / bulletsPerTap);
        
       // if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
      //  else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        
        //Reloading 
        if (Input.GetKeyDown(KeyCode.R) && bulletLeft < magzineSize && !reloading) Reload();
        //Reload automatically when trying to shoot without ammo
        if (readyToshoot && shooting && !reloading && bulletLeft <= 0) Reload();
        
        //shooting
        if (readyToshoot && shooting && !reloading && bulletLeft > 0)
        {
            //set bulletshots to zero
            bulletShot = 0;
            Shoot();
        }
        
    }

    public void Shoot()
    {
       readyToshoot = false;
       


       //Find the exact hit position using a raycast
       Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
       RaycastHit hit;


       //check if ray hits something
       Vector3 targetPoint;
       if (Physics.Raycast(ray, out hit))
           targetPoint = hit.point;
       else
           targetPoint = ray.GetPoint(75); //Just a point far away from the player
       //direction from attack point 
       Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
       
       //calculate spread
       float x = UnityEngine.Random.Range(-spread, spread);
      float y = UnityEngine.Random.Range(-spread, spread);
       //Calculate new direction with spread
       Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

       //instantiate bullet/projectile
       GameObject currentBullet = Instantiate(bullet,attackPoint.position,Quaternion.identity);
       currentBullet.transform.forward = directionWithSpread.normalized;
       currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce , ForceMode.Impulse);
     //  currentBullet.GetComponent<Rigidbody>().AddForce(fpsCamera.transform.up * upwardForce , ForceMode.Impulse);
       
       //Instantiate muzzle flash, if you have one
      // if (muzzleFlash != null)
         //  Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
       
       
       //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
       if (allowInvoke)
       {
          // rb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
           Invoke("ResetShot", timeBetweenShooting);
           allowInvoke = false;
       }
//if more than one bulletsPerTap make sure to repeat shoot function
       if (bulletShot < bulletsPerTap && bulletLeft > 0)
           Invoke("Shoot", timeBetweenShots);
       bulletLeft--;
       bulletShot++;
       //Add recoil to player (should only be called once)
      
    }

    private void ResetShot()
    {
            //Allow shooting and invoking again
            readyToshoot = true;
            allowInvoke = true;
        
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
    }
    private void ReloadFinished()
    {
        //Fill magazine
        bulletLeft = magzineSize;
        reloading = false;
    }
    
}
