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
    [SerializeField] private LevelFlowData levelFlowData;
    
    [Header("Upgrade Base Values")]
    [SerializeField] private float healthUpgradeAmount = 20f;
    [SerializeField] private float resourceUpgradeAmount = 10f;
    [SerializeField] private float damageUpgradeAmount = 5f;
    [SerializeField] private float upgradeCostMultiplier = 1.5f;
    
    
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI currentScoreText;

    [Header("ValueUI")] 
    [SerializeField] private TextMeshProUGUI healthAmountText;
    [SerializeField] private TextMeshProUGUI resourceAmountText;
    [SerializeField] private TextMeshProUGUI damageAmountText;
    
    [Header("CostUI")]
    [SerializeField] private TextMeshProUGUI healthCostText;
    [SerializeField] private TextMeshProUGUI resourceCostText;
    [SerializeField] private TextMeshProUGUI damageCostText;
    
    [Header("StatsUI")]
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI maxResourceText;
    [SerializeField] private TextMeshProUGUI maxAttackDamageText;
    
    [Header("UISounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip denySound;
    [SerializeField, Range(0f, 1f)] private float uIVolume = 1f;
    
    public static UnityEvent<int> OnLoadNextLevel = new UnityEvent<int>();

    /*private int _lastHealthUpgradeCost;
    private int _lastResourceUpgradeCost;
    private int _lastDamageUpgradeCost;
    */

    private float _currentMaxHealth;
    private float _currentMaxWaterResource;
    private float _currentAttackDamage;

    private void Awake()
    {
        // ONLY FOR DEBUG
        //_lastOriginSceneIndex = testIndexForDebug;
        //bountyProgressionData.currentUpgradeScore = testScoreForDebug;
        //---------------------------------------------------------------------------
        
        currentScoreText.text = bountyProgressionData.currentUpgradeScore.ToString();

        healthAmountText.text = "+ " + healthUpgradeAmount;
        resourceAmountText.text = "+ " + resourceUpgradeAmount;
        damageAmountText.text = "+ " + damageUpgradeAmount;
        
        healthCostText.text = bountyProgressionData.currentHealthUpgradeCost.ToString();
        resourceCostText.text = bountyProgressionData.currentResourceUpgradeCost.ToString();
        damageCostText.text = bountyProgressionData.currentDamageUpgradeCost.ToString();
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

    public void LoadNextLevel()
    {
        int nextSceneIndex = levelFlowData.currentLevelIndex + 1;
        Debug.Log("nächster Szenen Index in UpgradeSceneController: " + nextSceneIndex);
        levelFlowData.currentLevelIndex = nextSceneIndex;
        OnLoadNextLevel.Invoke(nextSceneIndex);
    }

    public void UpgradeMaxHealth()
    {
        int currentCost = bountyProgressionData.currentHealthUpgradeCost;
        
        if (bountyProgressionData.currentUpgradeScore >= currentCost)
        {
            playerProgressionData.maxHealth += healthUpgradeAmount;
            
            bountyProgressionData.currentUpgradeScore -= currentCost;
            currentScoreText.text = bountyProgressionData.currentUpgradeScore.ToString();
            
            PlaySuccessSound();
        }
        else
        {
            PlayDenySound();
            return;
        }
        
        bountyProgressionData.currentHealthUpgradeCost = Mathf.CeilToInt(currentCost * upgradeCostMultiplier);
        healthCostText.text = bountyProgressionData.currentHealthUpgradeCost.ToString();
    }

    public void UpgradeMaxWaterResource()
    {
        int currentCost = bountyProgressionData.currentResourceUpgradeCost;
        
        if (bountyProgressionData.currentUpgradeScore >= currentCost)
        {
            playerProgressionData.maxResource += resourceUpgradeAmount;
            
            bountyProgressionData.currentUpgradeScore -= currentCost;
            currentScoreText.text = bountyProgressionData.currentUpgradeScore.ToString();
            
            PlaySuccessSound();
        }
        else
        {
            PlayDenySound();
            return;
        }
        
        bountyProgressionData.currentResourceUpgradeCost = Mathf.CeilToInt(currentCost * upgradeCostMultiplier);
        resourceCostText.text = bountyProgressionData.currentResourceUpgradeCost.ToString();
    }

    public void UpgradeAttackDamage()
    {
        int currentCost = bountyProgressionData.currentDamageUpgradeCost;
        
        if (bountyProgressionData.currentUpgradeScore >= currentCost)
        {
            playerProgressionData.attackDamage += damageUpgradeAmount;
            
            bountyProgressionData.currentUpgradeScore -= currentCost;
            currentScoreText.text = bountyProgressionData.currentUpgradeScore.ToString();
            
            PlaySuccessSound();
        }
        else
        {
            PlayDenySound();
            return;
        }
        
        bountyProgressionData.currentDamageUpgradeCost = Mathf.CeilToInt(currentCost * upgradeCostMultiplier);
        damageCostText.text = bountyProgressionData.currentDamageUpgradeCost.ToString();
    }

    public void PlayDenySound()
    {
        if (audioSource == null || denySound == null) return;
        audioSource.PlayOneShot(denySound, uIVolume);
    }

    public void PlaySuccessSound()
    {
        if (audioSource == null || successSound == null) return;
        audioSource.PlayOneShot(successSound, uIVolume);
    }
}
