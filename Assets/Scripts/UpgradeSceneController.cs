using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeSceneController : MonoBehaviour
{
    [SerializeField] private int testIndexForDebug;
    [SerializeField] private int testScoreForDebug;
    
    [Header("Upgrade Meshes")]
    [SerializeField] private List<GameObject> upgradeMeshes = new List<GameObject>();
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Data")] 
    [SerializeField] private BountyProgressionData bountyProgressionData;
    [SerializeField] private PlayerProgressionData playerProgressionData;
    
    [Header("Upgrade Base Values")]
    [SerializeField] private int baseHealthUpgradeCost = 100;
    [SerializeField] private float healthUpgradeAmount = 20f;
    [SerializeField] private int baseResourceUpgradeCost = 100;
    [SerializeField] private float resourceUpgradeAmount = 10f;
    [SerializeField] private int baseDamageUpgradeCost = 100;
    [SerializeField] private float damageUpgradeAmount = 5f;
    [SerializeField] private float upgradeCostMultiplier = 1.5f;
    
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI currentScoreText;
    
    [Header("CostUI")]
    [SerializeField] private TextMeshProUGUI healthCostText;
    [SerializeField] private TextMeshProUGUI resourceCostText;
    [SerializeField] private TextMeshProUGUI damageCostText;
    
    [Header("StatsUI")]
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI maxResourceText;
    [SerializeField] private TextMeshProUGUI maxAttackDamageText;
    
    public static UnityEvent<int> OnLoadNextLevel = new UnityEvent<int>();
    /*
    public static UnityEvent<int,float> OnHealthUpgraded = new UnityEvent<int, float>();
    public static UnityEvent<int, float> OnWaterResourceUpgraded = new UnityEvent<int, float>();
    public static UnityEvent<int, float> OnAttackDamageUpgraded = new UnityEvent<int, float>();
    */

    private int _lastOriginSceneIndex;
    private int _lastHealthUpgradeCost;
    private int _lastResourceUpgradeCost;
    private int _lastDamageUpgradeCost;
    private int _currentUpgradeScore;

    private float _currentMaxHealth;
    private float _currentMaxWaterResource;
    private float _currentAttackDamage;

    private void OnEnable()
    {
        GameManager.OnUpgradescreenLoad.AddListener(SetOriginSceneIndex);
        /*Character.OnMaxHealthChanged.AddListener(UpdateHealthStat);
        Character.OnMaxResourceChanged.AddListener(UpdateResourceStat);
        Character.OnMaxAttackDamageChanged.AddListener(UpdateDamageStat);
        */
    }

    private void OnDisable()
    {
        GameManager.OnUpgradescreenLoad.RemoveListener(SetOriginSceneIndex);
        /*Character.OnMaxHealthChanged.RemoveListener(UpdateHealthStat);
        Character.OnMaxResourceChanged.RemoveListener(UpdateResourceStat);
        Character.OnMaxAttackDamageChanged.RemoveListener(UpdateDamageStat);
        */
    }

    private void Awake()
    {
        // ONLY FOR DEBUG
        _lastOriginSceneIndex = testIndexForDebug;
        bountyProgressionData.currentUpgradeScore = testScoreForDebug;
        //---------------------------------------------------------------------------

        _currentUpgradeScore = bountyProgressionData.currentUpgradeScore;
        currentScoreText.text = _currentUpgradeScore.ToString();
        
        _lastHealthUpgradeCost = baseHealthUpgradeCost;
        _lastResourceUpgradeCost = baseResourceUpgradeCost;
        _lastDamageUpgradeCost = baseDamageUpgradeCost;
        
        healthCostText.text = _lastHealthUpgradeCost.ToString();
        resourceCostText.text = _lastResourceUpgradeCost.ToString();
        damageCostText.text = _lastDamageUpgradeCost.ToString();

    }

    private void Update()
    {
        foreach (GameObject mesh in upgradeMeshes)
        {
            mesh.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        
        maxHealthText.text = playerProgressionData.maxHealth.ToString();
        maxResourceText.text = playerProgressionData.maxResource.ToString();
        maxAttackDamageText.text = playerProgressionData.attackDamage.ToString();
    }

    private void SetOriginSceneIndex(int originSceneIndex)
    {
        _lastOriginSceneIndex = originSceneIndex;
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = _lastOriginSceneIndex++;
        OnLoadNextLevel.Invoke(nextSceneIndex);
    }

    public void UpgradeMaxHealth()
    {
        int currentCost = _lastHealthUpgradeCost;
        
        playerProgressionData.maxHealth += healthUpgradeAmount;
        
        _lastHealthUpgradeCost = Mathf.CeilToInt(currentCost * upgradeCostMultiplier);
        healthCostText.text = _lastHealthUpgradeCost.ToString();
    }

    public void UpgradeMaxWaterResource()
    {
        int currentCost = _lastResourceUpgradeCost;
        
        playerProgressionData.maxResource += resourceUpgradeAmount;
        
        _lastResourceUpgradeCost = Mathf.CeilToInt(currentCost * upgradeCostMultiplier);
        resourceCostText.text = _lastResourceUpgradeCost.ToString();
    }

    public void UpgradeAttackDamage()
    {
        int currentCost = _lastDamageUpgradeCost;
        
        playerProgressionData.attackDamage += damageUpgradeAmount;
        
        _lastDamageUpgradeCost = Mathf.CeilToInt(currentCost * upgradeCostMultiplier);
        damageCostText.text = _lastDamageUpgradeCost.ToString();
    }

    /*public void UpdateHealthStat(float newMaxHealth)
    {
        maxHealthText.text = newMaxHealth.ToString();
    }

    public void UpdateResourceStat(float newMaxResource)
    {
        maxResourceText.text = newMaxResource.ToString();
    }

    public void UpdateDamageStat(float newAttackDamage)
    {
        maxResourceText.text = newAttackDamage.ToString();
    }
    */
}
