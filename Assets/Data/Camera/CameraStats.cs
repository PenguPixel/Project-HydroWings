using UnityEngine;

[CreateAssetMenu(fileName = "CameraStats", menuName = "Scriptable Objects/CameraStats")]
public class CameraStats : ScriptableObject
{
    public float speed;
    public float maxCamOffsetY;
    public float smoothTime;
}
