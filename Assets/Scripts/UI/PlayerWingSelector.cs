using UnityEngine;

public class PlayerWingSelector : MonoBehaviour
{
    [Header("Playable Wings")]
    [SerializeField] private GameObject dolphWing;
    [SerializeField] private GameObject penguWing;

    private void Awake()
    {
        ActivateSelectedWing();
    }

    private void ActivateSelectedWing()
    {
        if (dolphWing == null || penguWing == null)
        {
            Debug.LogWarning(
                "PlayerWingSelector: Dolph Wing oder Pengu Wing wurde nicht zugewiesen."
            );

            return;
        }

        PlayableWing selectedWing = CharacterSelection.SelectedWing;

        switch (selectedWing)
        {
            case PlayableWing.DolphWing:
                dolphWing.SetActive(true);
                penguWing.SetActive(false);
                break;

            case PlayableWing.PenguWing:
                dolphWing.SetActive(false);
                penguWing.SetActive(true);
                break;

            default:
                Debug.LogWarning(
                    "PlayerWingSelector: Kein Wing ausgewählt. Dolph Wing wird als Standard verwendet."
                );

                dolphWing.SetActive(true);
                penguWing.SetActive(false);
                break;
        }

        Debug.Log($"Aktiver Wing im Level: {selectedWing}");
    }
}