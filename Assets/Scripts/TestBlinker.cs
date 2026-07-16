using UnityEngine;

public class TestBlinker : MonoBehaviour
{
    [SerializeField] private bool _activateBlink = false;
    [SerializeField] private MeshRenderer _blinkRenderer;
    
    private MaterialPropertyBlock _propBlock;
    private static readonly int IsBlinking = Shader.PropertyToID("_IsBlinking");

    private void Start()
    {
        _propBlock = new MaterialPropertyBlock();
    }
    // Update is called once per frame
    void Update()
    {
        if (_blinkRenderer != null)
        {
            _blinkRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(IsBlinking, _activateBlink ? 1f : 0f);
            _blinkRenderer.SetPropertyBlock(_propBlock);
        }
    }
}
