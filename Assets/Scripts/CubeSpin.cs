using System.Collections;
using UnityEngine;

public class CubeSpin : MonoBehaviour {

    public GameObject edge;
    private SpawnManager spawnManager;
    private float steps = 5;
    private float direction;
	void Start ()
    {
        spawnManager = GameObject.Find("Spawner").GetComponent<SpawnManager>();
        if (transform.position.x > spawnManager.offsetLeft + 45)
        {
            direction = 1;
        }
        else if (transform.position.x <= spawnManager.offsetLeft + 45)
        {
            direction = -1;
            transform.localScale = new Vector3(2.995f, 2.995f, 2.995f);
        }
        StartCoroutine("spin");
    }
    
    IEnumerator spin()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
        edge.transform.position = transform.position + new Vector3 (-direction/2,-0.5f);
        while (true)
        {
            for (int i = 0; i < 90 / steps; i++)
            {
                transform.RotateAround(edge.transform.position, new Vector3(0,0,direction), steps);
                yield return new WaitForSeconds(0.005f);
            }
            edge.transform.position -= new Vector3(direction, 0, 0);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
