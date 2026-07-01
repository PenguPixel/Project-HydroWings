using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class UnderwaterController : MonoBehaviour
{
    [Header("Depth Parameters")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private int depth = 0;
    
    [Header("Post Processing Volume")]
    [SerializeField] private Volume postProcessingVolume;
    
    [Header("Post Processing Profiles")]
    [SerializeField] private VolumeProfile surfacePostProcessing;
    [SerializeField] private VolumeProfile underwaterPostProcessing;

    public static UnityEvent<bool> OnSubmerged = new UnityEvent<bool>();
    private bool _wasSubmergedLastFrame;
    
    // Update is called once per frame
    void Update()
    {
        bool isCurrentlySubmerged = mainCamera.position.y < depth;
        
        if (isCurrentlySubmerged != _wasSubmergedLastFrame)
        {
            EnableEffects(isCurrentlySubmerged);
            OnSubmerged?.Invoke(isCurrentlySubmerged);
            _wasSubmergedLastFrame = isCurrentlySubmerged;
        }
    }

    private void EnableEffects(bool active)
    {
        if (active)
        {
            postProcessingVolume.profile = underwaterPostProcessing;
        }
        else
        {
            postProcessingVolume.profile = surfacePostProcessing;
        }
    }
}
