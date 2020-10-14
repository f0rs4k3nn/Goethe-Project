using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseSystem : MonoBehaviour
{
    [Serializable]
    public class CharacterInfo
    {
        public string name;
        public int cost;
        public bool isPurchased
        {
            get
            {
                return PlayerPrefs.GetInt(name + "isPurchased", 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(name+"isPurchased", (value)?1:0);
            }
        }

        public Sprite sprite;
    }
    
    public CharacterInfo[] characterInfos;

    public TextMeshProUGUI TotalScrapText;
    
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI ActivateText;
    public Image previewImage;

    private int currentCharacterIndex = 0;

    private void Awake()
    {
        TotalScrapText.text = GameManager.Instance.TotalScrap.ToString();

        for(int i = 0; i < characterInfos.Length; ++i)
            PlayerPrefs.DeleteKey(characterInfos[i].name + "isPurchased");
        GameManager.Instance.TotalScrap = 100;
    }

    private void Start()
    {
        currentCharacterIndex = GameManager.Instance.CurrentCharacterModelIndex;
        UpdateInfo(currentCharacterIndex);
    }

    public void OnActivateCharacterButton()
    {
        ActivateCharacter(currentCharacterIndex);
    }

    public void OnIndexButton(int index)
    {
        currentCharacterIndex = index;
        UpdateInfo(index);
    }
    
    public void ActivateCharacter(int index)
    {
        if (characterInfos[index].isPurchased) // Activate
        {
            GameManager.Instance.CurrentCharacterModelIndex = index;
            
            UpdateInfo(index);
        }
        else if(characterInfos[index].cost <= GameManager.Instance.TotalScrap) // Purchase
        {
            GameManager.Instance.TotalScrap -= characterInfos[index].cost; // Transaction
            characterInfos[index].isPurchased = true;

            GameManager.Instance.CurrentCharacterModelIndex = index; // Activation By Default
            
            UpdateInfo(index);
        }
        else
        {
            DOTween.To(() => ActivateText.text, x => ActivateText.text = x, "Not enough Scrap _", .2f
            );
        }
        
    }

    public void UpdateInfo(int index)
    {
        // Infos
        DOTween.To(() => NameText.text, x => NameText.text = x, characterInfos[index].name, .15f);
        previewImage.sprite = characterInfos[index].sprite;

        // Activation Button
        if (index == GameManager.Instance.CurrentCharacterModelIndex)
        {
            CostText.text = ""; // Remove Cost
            DOTween.To(() => ActivateText.text, x => ActivateText.text = x, "Activated", .05f);
        }
        else
        {
            if (characterInfos[index].isPurchased)
            {
                CostText.text = ""; // Remove Cost
                ActivateText.text = "[Activate]";
            }
            else
            {
                CostText.text = "Cost: " + characterInfos[index].cost; // Print Cost
                ActivateText.text = "[Purchase]";
            }
        }

        TotalScrapText.text = GameManager.Instance.TotalScrap.ToString();
    }
}
