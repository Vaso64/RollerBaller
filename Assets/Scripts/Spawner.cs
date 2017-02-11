using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Player player;
    private SpawnManager spawner;
    private int pos = 0;
    private float offsetLeft;
    private float randomMax;
    private float random;
    public float offset_y;
    private float BaseOffsetLeft;
    private bool picked;
    private float[] objectMax = new float[19];
    public GameObject Holder;

    // Update is called on start
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        spawner = GameObject.Find("Spawner").GetComponent<SpawnManager>();
        offset_y = spawner.offset_y;
        randomMax = spawner.randomMax;
        objectMax = spawner.objectMax;
        InvokeRepeating("Dispawn", 0, 0.5f);
        LineSpawn();
    }

    private void LineSpawn()
    {
        offsetLeft = spawner.offsetLeft;
        BaseOffsetLeft = offsetLeft + spawner.BaseOffsetLeft * 2;
        //Spawn
        while (offsetLeft <= BaseOffsetLeft)
        {
            PickObject();
            Holder = Instantiate (spawner.Platforms[pos], new Vector3(offsetLeft, offset_y, transform.position.z), Quaternion.identity);
            Holder.transform.parent = gameObject.transform;
            if (spawner.cubeChance > Random.Range(0, 100))
            {
                Holder = Instantiate(spawner.Cube, new Vector3(offsetLeft + Random.Range(-1, 2), offset_y + 0.25F, transform.position.z + Random.Range(-1, 2)), Quaternion.identity);
                Holder.transform.parent = transform;
            }
            if (spawner.spinCubeChance > Random.Range(0, 100))
            {
                Holder = Instantiate(spawner.SpinCube, new Vector3(offsetLeft, offset_y + 1, transform.position.z + Random.Range(-1, 2)), Quaternion.identity);
                Holder.transform.parent = transform;
            }
            if (spawner.coinChance > Random.Range(0, 100))
            {
                Holder = Instantiate(spawner.Coin, new Vector3(offsetLeft + Random.Range(-1, 2), offset_y + 1, transform.position.z + Random.Range(-1, 2)), Quaternion.Euler(new Vector3(0, 90, 90)));
                Holder.transform.parent = transform;
            }
            offsetLeft += 3;
        }
    }

    private void Update()
    {
        if (player.GameEnded == true)
        {
            Destroy(gameObject);
        }
    }

    private void PickObject()
    {
        random = Random.Range(0f, randomMax);
        pos = 0;
        while (picked == false)
        {
            if (random < objectMax[pos])
            {
                picked = true;
            }
            else
            {
                pos++;
            }
        }
        picked = false;
    }

    void Dispawn()
    {
        if (transform.position.z + 9 < player.transform.position.z)
        {
            Destroy(gameObject);
        }
    }
}
