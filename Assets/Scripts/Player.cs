using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private SpawnManager spawnmanager;
    private float Yoffset;
    public bool GameEnded = false;
    public float coins = 0;
    public Text PausedUI;
    public Text GameOverUI;
    public Text CoinUI;
    public Text scoreUI;
    public Text AnyKeyUI;
    public Text gyro;
    public GameObject CoinReset;
    public GameObject SensitivityUI;
    private int sensitivitySet = 1;
    public bool gyroAvaliable;
    private bool pauseSwitcher;
    private bool gamePaused = false;
    private float nextPeriod;
    public float Xmove;
    private float sensitivity = 2.5f;
    public float speed;
    public float score;
    public float turning;
    private float UI_Coins;
    private float t = 0;
    private float scoreTrigger = 100;
    public Material platform_mat;
    public Material cube_mat;
    public Color[] platform_color;
    public Color[] cube_color;
    private Color old_platform_color;
    private Color old_cube_color;
    private int color_pos = 0;
    private bool old_changed = false;

    void Start()
    {
        coins = PlayerPrefs.GetFloat("money");
        CoinReset.SetActive(false);
        SensitivityUI.SetActive(false);
        Input.gyro.enabled = true;
        //get Rigidbody component
        rb = GetComponent<Rigidbody>();
        //Increase Velocity in realtime
        InvokeRepeating("Acceleration", 0.5f, 0.1f);
        spawnmanager = GameObject.Find("Spawner").GetComponent<SpawnManager>();
        if (SystemInfo.supportsGyroscope == true)
        {
            gyroAvaliable = true;
        }
        platform_mat = Resources.Load("Platform", typeof(Material)) as Material;
        cube_mat = Resources.Load("Cube", typeof(Material)) as Material;
        platform_mat.color = platform_color[color_pos];
        cube_mat.color = cube_color[color_pos];
    }

    void Update()
    {
        ColorLerp();
        Movement();
        //Score UI
        score = transform.position.z;
        scoreUI.text = "SCORE\n" + score.ToString("F0");
        //Coins UI
        CoinUI.text = "COINS\n" + coins.ToString("F0");

        //Getting Y offset
        Yoffset = spawnmanager.offset_y;

        //Pause Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        //Fall down
        if (transform.position.y < -10 + Yoffset)
        {
            GameOver();
        }

        //Restart Game
        if (GameEnded == true && Input.anyKeyDown)
        {
            Restart();
        }
    }

    //Cube collide
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Cube")
        {
            GameOver();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Coin")
        {
            coins++;
            /*transform.localScale += new Vector3(0.25f, 0.25f, 0.25f);*/
            Destroy(col.gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.right * turning * Xmove);

        //relative force to Z axis
        rb.AddForce(Vector3.forward * speed, ForceMode.Acceleration);
    }
    
    private void Movement()
    {
        //X axis movement
        if (gyroAvaliable == true)
        {
            Xmove = Input.gyro.gravity.x * sensitivity;
            if (Xmove > 1)
            {
                Xmove = 1;
            }
            if (Xmove < -1)
            {
                Xmove = -1;
            }
        }
        if (gyroAvaliable == false)
        {
            Xmove = Input.GetAxis("Horizontal");
        }

    }

    private void GameOver()
    {
        PlayerPrefs.SetFloat("money", coins);
        GameEnded = true;
        GameOverUI.text = "GAME OVER";
        AnyKeyUI.text = "Press any key to continue";
        GetComponent<MeshRenderer>().enabled = false;
        rb.isKinematic = true;
    }

    private void Restart()
    {
        transform.position = new Vector3(0, 1.5f, 0);
        GetComponent<MeshRenderer>().enabled = true;
        rb.isKinematic = false;
        GameEnded = false;
        GameOverUI.text = "";
        AnyKeyUI.text = "";
        speed = 32;
        turning = 35;
        score = 0;
        color_pos = 0;
        platform_mat.color = platform_color[color_pos];
        cube_mat.color = cube_color[color_pos];
        old_changed = false;
        t = 0;
        scoreTrigger = 100;
        Physics.gravity = new Vector3(0, -25.0F, 0);
        spawnmanager.offsetLeft = -30;
        spawnmanager.offset_y = 0.0f;
        spawnmanager.z = 0f;
        spawnmanager.InitialSpawn();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        if (gamePaused == false)
        {
            CoinReset.SetActive(true);
            SensitivityUI.SetActive(true);
            Time.timeScale = 0;
            PausedUI.text = "GAME PAUSED";
            gamePaused = true;
            return;
        }

        if (gamePaused == true)
        {
            CoinReset.SetActive(false);
            SensitivityUI.SetActive(false);
            PausedUI.text = "";
            gamePaused = false;
            Time.timeScale = 1;
            return;
        }
    }

    void Acceleration()
    {
        speed += 0.25f;
        turning += 0.2f;
        Physics.gravity += new Vector3(0, -0.25F, 0);
    }

    public void Sensitivity()
    {
        sensitivitySet++;
        if (sensitivitySet == 3)
        {
            sensitivitySet = 0;
            sensitivity = 1.5f;
            SensitivityUI.GetComponentInChildren<Text>().text = "Sensitivity: Low";
        }
        if (sensitivitySet == 1)
        {
            sensitivity = 3.5f;
            SensitivityUI.GetComponentInChildren<Text>().text = "Sensitivity: Medium";
        }
        if (sensitivitySet == 2)
        {
            sensitivity = 7;
            SensitivityUI.GetComponentInChildren<Text>().text = "Sensitivity: High";
        }
    }
    public void ResetMoney()
    {
        coins = 0;
        PlayerPrefs.SetFloat("Money", 0);
    }

    public void ColorLerp()
    {
        if (scoreTrigger < score)
        {
            if (old_changed == false)
            {
                old_platform_color = platform_color[color_pos];
                old_cube_color = cube_color[color_pos];
                color_pos++;
                if (color_pos == platform_color.Length)
                {
                    color_pos = 0;
                }
                old_changed = true;
            }
            if (t <= 1)
            {
                t += Time.deltaTime * 0.5f;
                platform_mat.color = Color.Lerp(old_platform_color, platform_color[color_pos], t);
                cube_mat.color = Color.Lerp(old_cube_color, cube_color[color_pos], t);
            }
            if (t > 1)
            {
                scoreTrigger += 100;
                t = 0;
                old_changed = false;
            }
        }
    }
}
