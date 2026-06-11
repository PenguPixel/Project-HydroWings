using Unity.VisualScripting;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] private Transform[] backgrounds;   // Array containing background layers
    [SerializeField] private float smoothing = 10f;     // Smoothness of parallax effect
    [SerializeField] private float multiplier = 15f;    // Parallax effect strength between background layers 

    private Transform cam;  // Main camera
    private Vector3 previousCamPos;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;
    }

    void Update()
    {
        for (var i = 0; i < backgrounds.Length; i++)
        {
            var parallax = (previousCamPos.x - cam.position.x) * (i * multiplier);
            var targetX = backgrounds[i].position.x + parallax;
            
            var targetPosition = new Vector3(targetX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(targetPosition, backgrounds[i].position, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
