using UnityEngine;
using UnityEngine.UI;
using MilkShake;

public class WeaponShooting : Weapon
{
    //Weapon stats
    public int damage;
    public int magazineSize, bulletsPerTap;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots, recoil;
    public bool allowButtonHold;
    public Vector3 recoil_direction;
    public float rotation_speed;
    public float return_speed;

    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, can_shoot;

    //references
    GameObject player;
    Camera fpsCam;
    Transform camTransform;
    Transform camHolder;
    Transform playerTransform;
    Shaker cameraShaker;
    public ShakePreset shake_preset;
    public PickUpSystem pickupSys;


    public Transform attackPoint;
    RaycastHit rayHit;
    public LayerMask damageable;

    //UI
    UIManager uiManager;
    Text ammoCountText;
    hitmarker hitmrkr;
    hitmarker deathMarker;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic, cartridge;


    //called before start even if script is inactive
    private void Awake()
    {
        //UI
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        //get UI elements
        ammoCountText = uiManager.getAmmoCounter();
        hitmrkr = uiManager.getHitMarker();
        deathMarker = uiManager.getDeathHitMarker();

        //get transforms ffrom player gameobject
        player = FindObjectOfType<GameManager>().getRunnerReference();

        playerTransform = player.transform;
        camHolder = playerTransform.GetChild(2).GetChild(0);
        camTransform = camHolder.GetChild(0);
        fpsCam = camTransform.gameObject.GetComponent<Camera>();

        //pass transforms to pickupSystem script of the weapon
        pickupSys.setTransforms(playerTransform, camHolder.GetChild(1), camTransform);
    }

    // Start is called before the first frame update
    void Start()
    {
        readyToShoot = true;
        bulletsLeft = magazineSize;

        cameraShaker = fpsCam.GetComponent<Shaker>();

        ammoCountText.text = (bulletsLeft/bulletsPerTap).ToString();
    }


    // Update is called once per frame
    void Update()
    {
        shootWeapon();
    }
    

    private void shootWeapon()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
            //shooting = Input.GetMouseButtonDown(0);
        }

        //reload
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize & !reloading)
        {
            PlayerMovement.sprintLock();
            Reload();
        }
        //reload cancel when sprinting
        if (reloading && Input.GetButtonDown("Sprint"))
        {
            reloadCancel();
        }

        //shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            PlayerMovement.sprintLock();
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }



    private void Shoot()
    {
        Debug.Log("shoot");

        readyToShoot = false;

        //spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calculate direction with spread
        Vector3 direction = camTransform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(camTransform.position, direction, out rayHit, range))//, damageable))//, whatIsEnemy))
        {
            //Debug.Log(rayhit.collider.name);
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
            {
                //enemy needs to be tagged as "Enemy" and have a script with the "TakeDamage" function
                float dmg = rayHit.collider.GetComponent<EnemyStatus>().TakeDamage(damage* player.GetComponent<PlayerStatus>().getDmgMultiplier(), direction, rayHit.point);

                if (dmg > 0) {
                    //show hitmarker
                    hitmrkr.showHitMarker();
                }
                //enemy killed
                else if (dmg == -1) {
                    //show both hitmarkers
                    hitmrkr.showHitMarker();
                    deathMarker.showHitMarker();

                    //give buff to player
                    player.GetComponent<PlayerStatus>().buffDmgMultiplier(0.5f);
                }
            }
        }

        Debug.DrawLine(camTransform.position, rayHit.point, new Color(256, 0, 0), 10f);
        Debug.DrawRay(rayHit.point, rayHit.normal, new Color(0, 256, 0), 10f);

        //Graphics
        Instantiate(muzzleFlash, attackPoint.position, attackPoint.transform.rotation);
        GameObject bulletHole = Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(rayHit.normal));
        bulletHole.transform.localScale = new Vector3(4, 4, 4);

        bulletsLeft--;
        bulletsShot--;

        //UI
        ammoCountText.text = (bulletsLeft/bulletsPerTap).ToString();

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

        //recoil only after all bullets in the shot have been fired, or it's a burst weapon 
        if(bulletsShot == 0 || timeBetweenShots != 0)
        {
            //camera Shake
            cameraShaker.Shake(shake_preset);

            //recoil
            weaponRecoil(recoil, direction.normalized);
        }
    }



    private void Reload()
    {
        Debug.Log("reloading");

        reloading = true;
        //reload position
        transform.localRotation = Quaternion.Euler(new Vector3(-40, -10, 0));

        //UI
        ammoCountText.text = "RELOADING";

        Invoke("ReloadFinished", reloadTime);
    }



    private void ReloadFinished()
    {
        Debug.Log("reload finished");
        bulletsLeft = magazineSize;
        reloading = false;

        //UI
        ammoCountText.text = (bulletsLeft/bulletsPerTap).ToString();

        //returnn to normal position
        transform.localRotation = Quaternion.Euler(pickupSys.local_rotation);
    }



    public override void reloadCancel()
    {
        Debug.Log("reload cancelled");

        //reset everything and stop the invocation of reloadFinished
        reloading = false;
        transform.localRotation = Quaternion.Euler(pickupSys.local_rotation);
        CancelInvoke("ReloadFinished");

        //UI
        ammoCountText.text = (bulletsLeft / bulletsPerTap).ToString();
    }



    private void ResetShot()
    {
        readyToShoot = true;
    }



    public override void weaponRecoil(float recoil_strength, Vector3 recoil_direction)
    {
        //push player
        PlayerMovement.Recoil(recoil_strength, recoil_direction);

        /*
        //find smallest between recoil_diection x and y values
        float min;
        if (recoil_direction.x < recoil_direction.y)
        {
            min = recoil_direction.x;
        }
        else
        {
            min = recoil_direction.y;
        }
        //randomly change recoil pattern
        float r = Random.Range(min, -min);
        Vector3 rec_dir = recoil_direction + new Vector3(r, r, 0);

        Debug.Log(rec_dir);

        //rotate horizontally
        player_transform.Rotate(Vector3.up * rec_dir.y);
        //rotate vertically
        //camHolder.localRotation = Quaternion.Euler(-rec_dir.x, 0f, 0f);
        */

        camRecoil.Fire(recoil_direction, rotation_speed, return_speed, recoil_strength);
    }


    public override void UIAmmoCounterPickUp()
    {
        ammoCountText.text = (bulletsLeft / bulletsPerTap).ToString();
    }
    public override void UIAmmoCounterDrop()
    {
        ammoCountText.text = "--";
    }
}
