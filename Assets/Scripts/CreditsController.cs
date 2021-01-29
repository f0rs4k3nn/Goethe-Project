using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    public TextMeshProUGUI text1, text2;

    private void Start()
    {

        Sequence creditsSequence = DOTween.Sequence();
        creditsSequence.PrependInterval(2f);
        creditsSequence.Append(doText(text1, "<b>Goethe-Institut Presents... _</b>"));
        creditsSequence.Append(doFadeOut(text1, 2f, 4f));
        creditsSequence.Append(doText(text1, "Romania\nTurkey\nSerbia\nMontenegro\nBosnia&Herzegovina\nMoldova _"));
        creditsSequence.Append(doFadeOut(text1, 4f, 4f));
        
        creditsSequence.Append(doText(text2, ">Piciu Alexia\nIlie Miruna-Corina\nLaura Pop\nMosneagu Teodora _"));
        creditsSequence.Append(doFadeOut(text2, 4f, 4f));
        creditsSequence.Append(doText(text2, ">Merla Antoniu-Stefan\nPrudius Marina\nMerjem Nuhic\nKatarina Kapetina _"));
        creditsSequence.Append(doFadeOut(text2, 4f, 2f));
        creditsSequence.Append(doText(text2, ">Umur Yildirim\nMehmet Yigit Balcik\nAleksandar Gemaljevic\nAnja Radulovic _"));
        creditsSequence.Append(doFadeOut(text2, 4f, 2f));
    }

    private Tween doText(TextMeshProUGUI target, string message, float duration = 2f)
    {
        Tween myTween = DOTween.To(() => target.text, x => target.text = x, message, duration);
        myTween.OnStart(() =>
        {
            target.text = "";
            target.alpha = 1f;
        });
        
        return myTween;
    }

    private Tween doFadeOut(TextMeshProUGUI target, float duration = 1f, float delay = 0f)
    {
        Tween myTween = DOTween.ToAlpha(() => target.color, x => target.color = x, 0, 1f);
        myTween.OnComplete(() =>
        {
            target.text = "";
            target.alpha = 0f;
        });
        myTween.SetDelay(delay);

        return myTween;
    }
}
