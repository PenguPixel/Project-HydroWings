using Unity.VisualScripting;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] private Transform[] backgrounds;   // Array containing background layers
    [SerializeField] private float smoothing = 10f;     // Smoothness of parallax effect
    [SerializeField] private float multiplier = 0.15f;    // Parallax effect strength between background layers 

    private Transform cam;  // Main camera

    private float[] startPositionsX;
    private float startCamX;
    
    //private Vector3 previousCamPos;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        startCamX = cam.position.x;
        
        startPositionsX = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            startPositionsX[i] = backgrounds[i].position.x;
        }
    }

    void LateUpdate()
    {
        float camTravel = cam.position.x - startCamX;
        
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallaxFactor = 1f - (i * multiplier);

            float targetX = startPositionsX[i] + (camTravel * parallaxFactor);
            
            Vector3 targetPosition = new Vector3(targetX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, targetPosition, smoothing * Time.deltaTime);
        }
    }
}
