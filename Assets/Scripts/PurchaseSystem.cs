﻿using System;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseSystem : MonoBehaviour
{
    [Serializable]
    public class CharacterInfo
    {
        public string name;
        public TranslationController.KEY key;
        public int cost;
        public bool isPurchased
        {
            get
            {
                return PlayerPrefs.GetInt(name + "isPurchased", 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(name + "isPurchased", (value) ? 1 : 0);
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
    private TranslationController _translation;

    private void Start()
    {
        _translation = GameManager.Instance.translationController;

        currentCharacterIndex = GameManager.Instance.CurrentCharacterModelIndex;
        UpdateInfo(currentCharacterIndex);
        Debug.Log("setActiveFalse");
        this.gameObject.SetActive(false);
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
            DOTween.To(() => ActivateText.text, x => ActivateText.text = x, _translation.GetTranslation(TranslationController.KEY.NOT_ENOUGH_SCRAP), .2f
            );
        }
        
    }

    public void UpdateInfo(int index)
    {
        // Infos
        DOTween.To(() => NameText.text, x => NameText.text = x, _translation.GetTranslation(characterInfos[index].key), .15f);
        previewImage.sprite = characterInfos[index].sprite;

        // Activation Button
        if (index == GameManager.Instance.CurrentCharacterModelIndex)
        {
            CostText.text = ""; // Remove Cost
            DOTween.To(() => ActivateText.text, x => ActivateText.text = x, _translation.GetTranslation(TranslationController.KEY.ACTIVATED), .05f);
        }
        else
        {
            if (characterInfos[index].isPurchased)
            {
                CostText.text = ""; // Remove Cost
                ActivateText.text = _translation.GetTranslation(TranslationController.KEY.ACTIVATE);
            }
            else
            {
                CostText.text = _translation.GetTranslation(TranslationController.KEY.COST) + characterInfos[index].cost; // Print Cost
                ActivateText.text = _translation.GetTranslation(TranslationController.KEY.PURCHASE);
            }
        }

        TotalScrapText.text = GameManager.Instance.TotalScrap.ToString();
    }
}
