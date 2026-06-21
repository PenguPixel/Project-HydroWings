using UnityEngine;

public class WaterStateController : MonoBehaviour
{
    private float _currentWaterAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentWaterAmount = GetComponent<Character>().Stats.MaxWaterAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
