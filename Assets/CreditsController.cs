using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start()
    {

        Sequence creditsSequence = DOTween.Sequence();
        creditsSequence.Append(doText(text, "Goethe Institute Bucharest\nPresents..._"));
        creditsSequence.Append(doFadeOut(text, 1f, 4f));
        creditsSequence.Append(doText(text, "Goethe Institute Bucharest\nPresents..._"));
        creditsSequence.Append(doFadeOut(text, 1f, 4f));
        creditsSequence.Append(doText(text, "Collaboration of BUG Lab and \n Retina Film Production _"));
        creditsSequence.Append(doFadeOut(text, 1f, 4f));
        creditsSequence.Append(doText(text, "Romania\nTurkey\nSerbia\nMontenegro\nBosnia&Herzegovina\nMoldova _"));
        creditsSequence.Append(doFadeOut(text, 1f, 4f));
        creditsSequence.PrependInterval(1f);
        
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
        Tween myTween = DOTween.ToAlpha(() => text.color, x => text.color = x, 0, 1f);
        myTween.OnComplete(() =>
        {
            target.text = "";
            target.alpha = 0f;
        });
        myTween.SetDelay(delay);

        return myTween;
    }
}
