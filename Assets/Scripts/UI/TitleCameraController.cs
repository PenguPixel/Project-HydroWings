using UnityEngine;

public class TitleCameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraStart;
    [SerializeField] private Transform cameraEnd;
    [SerializeField] private float moveDuration = 8f;

    private float timer;

    private void Start()
    {
        transform.position = cameraStart.position;
    }

    private void Update()
    {
        if (timer < moveDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / moveDuration);

            // weiche Bewegung
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(
                cameraStart.position,
                cameraEnd.position,
                t);
        }
    }
}