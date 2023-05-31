using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;
    private Vector3 offset;
    public bool followPlayer;

    private void Awake()
    {
        followPlayer = true;
        player = FindObjectOfType<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(followPlayer)
        transform.position = offset + player.transform.position;
    }

    public void Zoom(Enemy enemy)
    {
        followPlayer = false;

        Vector3 pos = transform.position;

        pos.x = (enemy.transform.position.x + player.transform.position.x) / 2;
        pos.y = (enemy.transform.position.y + player.transform.position.y) / 2;
        pos.z = -1;
        transform.position = pos;
        Camera.main.orthographicSize = 2;
    }

    public void Unzoom()
    {
        followPlayer = true;
        Camera.main.orthographicSize = 5;
    }
}
