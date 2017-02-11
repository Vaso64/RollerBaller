using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private Player Player;
    private Vector3 offset;
    private float gyroCamMove = 0f;
    public float gyroX;
    private float speed;
    public float gyroSenstivity = 35;
    
	void Start () {
        offset = transform.position - Player.transform.position;
	}

	void Update () {
        transform.position = Player.transform.position + new Vector3(Player.Xmove * gyroCamMove, 0, 0) + offset;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -Player.Xmove * gyroSenstivity);
        transform.position += new Vector3(gyroX * 1.25f, 0, 0);
    }
}
