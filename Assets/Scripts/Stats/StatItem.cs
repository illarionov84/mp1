using UnityEngine;
using UnityEngine.UI;

public class StatItem : MonoBehaviour {
    
    [SerializeField] Text value;
    [SerializeField] Button upgradeButton;

    public void ChangeStat(int stat) {
        value.text = stat.ToString();
    }

    public void SetUpgradable(bool upgradable) {
        upgradeButton.gameObject.SetActive(upgradable);
    }
}
