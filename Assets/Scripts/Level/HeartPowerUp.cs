using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HeartPowerUp : MonoBehaviour
{
    [SerializeField] private int restoreLifeAmount = 5;
    [SerializeField] private float rotationSpeed = 5f;
    public static UnityEvent<int> OnHeartCollected = new UnityEvent<int>();

    private void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            Debug.Log(other.name+ " Hat Herz berührt!");
            OnHeartCollected.Invoke(restoreLifeAmount);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
