using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Cube;
    public GameObject Coin;
    public float cubeChance;
    public float coinChance;
    public GameObject SpinCube;
    public float spinCubeChance;
    public GameObject[] Platforms;
    public GameObject LINE;
    public float[] ObjectChance;
    private bool picked;
    private int pos = 0;
    public float[] objectMax = new float[19];
    public float offset_x;
    public float z = 0f;
    public float randomMax = 0f;
    private float random;
    public float offsetLeft;
    public float BaseOffsetLeft;
    public float offset_y = 0.0f;

    // Update is called on start
    private void Start()
    {
        BaseOffsetLeft = offsetLeft * -1;
        RangeSet();
        InitialSpawn();
        InvokeRepeating("UpdateX", 0, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z + 33 > z)
        {
            Instantiate(LINE, new Vector3(offsetLeft + BaseOffsetLeft, offset_y, z), Quaternion.identity);
            z += 3;
            offset_y -= 0.01f;
        }
    }

    private void RangeSet()
    {
        for(pos = 0; pos < ObjectChance.Length; pos++)
        {
            randomMax += ObjectChance[pos];
            objectMax[pos] = randomMax;
        }
    }

    public void InitialSpawn()
    {
        offset_x = offsetLeft;
        while (z <= 18f)
        {
            Instantiate(LINE, new Vector3(offsetLeft + BaseOffsetLeft, offset_y, z), Quaternion.identity);
            offset_y -= 0.01f;
            z += 3f;
        }
    }

    private void UpdateX()
    {
        if (transform.position.x < (offsetLeft + BaseOffsetLeft) - 3)
        {
            offsetLeft -= 3;
        }
        if (transform.position.x > (offsetLeft + BaseOffsetLeft) + 3)
        {
            offsetLeft += 3;
        }
    }

}
