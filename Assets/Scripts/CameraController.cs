using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float speed = 2f;

    void Start()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
    
    void LateUpdate()
    {
        transform.position += Vector3.right * (speed * Time.deltaTime);
    }
}
