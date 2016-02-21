using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject gun;
    public CharacterController player;
    public static bool shooting;
    public static bool ADS;
    public static bool switchADS;
    private float shootDelay;
    public static bool OverHeat;
    private bool CanCover;
    private bool Covering;
    private bool showText;
    public Slider HeatSlider;
    public Text CoverPopUp;
    public float crouchingSpeed;
    public float CspeedUp;
    public float crouchHeight;
    public static float GunHeat;
    public float TimerCover;
    public float hSliderValue = 0;

    // Use this for initialization
    void Start()
    {
        shootDelay = 0;
        shooting = false;
        switchADS = false;
        ADS = false;
        crouchingSpeed = 0.1f;
        CspeedUp = crouchingSpeed;
        GunHeat = 0.0f;
        showText = false;
        TimerCover = 0.0f;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().name == "Cover")
        {
            Debug.Log("I'm Working");
            showText = true;
            TimerCover = 0.0f;
            CanCover = true;
        }
        if (col.GetComponent<Collider>().name == "Uncover")
        {
            CanCover = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.F))
        {
            HUD.enemyDie = true;
            HUD.score += 80;
        }*/

        // Shoot delay
        if (shooting && shootDelay < 0.8f)
        {
            shootDelay += 1 * Time.deltaTime;
        }
        else if (shooting && shootDelay >= 0.8f)
        {
            shootDelay = 0;
            shooting = false;
        }

        //Shooting mechanics
        if (Input.GetMouseButtonDown(0) && !shooting && !switchADS && !OverHeat)
        {
            if (!ADS)
            {
                Debug.Log("Pressed left click.");
                gun.GetComponent<Animator>().Play("Gun_Shoot");
            }
            else if (ADS)
            {
                Debug.Log("Pressed left click.");
                gun.GetComponent<Animator>().Play("GunADS_Shoot");
            }
        }

        //Crouching
        if (Input.GetKey("x"))
        {
            crouchHeight -= 0.1f * (Time.deltaTime * 10);
            CspeedUp -= crouchingSpeed * (Time.deltaTime * 10);
            if (crouchHeight < -0.6f)
            {
                crouchHeight = -0.6f;
            }
            if (CspeedUp < -0.3f)
            {
                CspeedUp = -0.3f;
            }
        }

        //Cover
        else if (CanCover && Input.GetKey("z"))
        {
            CspeedUp = -0.6f;
        }
        else
        {
            crouchHeight += 0.1f * (Time.deltaTime * 10);
            CspeedUp += crouchingSpeed * (Time.deltaTime * 10);
            if (crouchHeight > -0.1f)
            {
                crouchHeight = 0.0f;
            }
            if (CspeedUp > -0.2f)
            {
                CspeedUp = 0.0f;
            }
        }
        transform.localPosition = new Vector3(0, CspeedUp, 0);
        Debug.Log(player.height);
        player.height = crouchHeight + 1.8f;
        GunHeat -= Time.deltaTime / 2;
        TimerCover += Time.deltaTime;

        if (TimerCover > 3.0)
        {
            showText = false;
        }

        if (GunHeat < 0.0f)
        {
            GunHeat = 0.0f;
            OverHeat = false;
        }
        if (GunHeat > 1.5f) { OverHeat = true; }
        HeatSlider.value = GunHeat;

        //Aim mechanics
        if (Input.GetMouseButtonDown(1))
        {
            if (!ADS && !shooting && !switchADS)
            {
                Debug.Log("Pressed right click.");
                gun.GetComponent<Animator>().Play("GunToADS");
                switchADS = true;
                ADS = true;
            }
            else if (ADS && !shooting && !switchADS)
            {
                Debug.Log("Pressed right click.");
                gun.GetComponent<Animator>().Play("GunFromADS");
                switchADS = true;
                ADS = false;
            }
        }
    }

    void OnGUI()
    {
        if (showText)
        {
            GUIStyle Coverstyle = new GUIStyle();
            Coverstyle.alignment = TextAnchor.MiddleCenter;
            Coverstyle.fontSize = 50;
            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height - 40, 400, 30), "Hold Z for cover.", Coverstyle);
        }
    }
}
