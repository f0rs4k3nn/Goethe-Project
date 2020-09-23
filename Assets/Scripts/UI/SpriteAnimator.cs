using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{
    public float timeBetweenFrames = 0.1f;
    public bool randomFrames;

    public Sprite[] sprites;

    private Image _image;
    private int _currentIndex;
    private int _length;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _length = sprites.Length;

        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        while(true)
        {
            if (randomFrames)
            {
                _currentIndex = Random.Range(0, _length);
            }
            else
            {
                _currentIndex++;

                if (_currentIndex == _length)
                {
                    _currentIndex = 0;
                }
            }

            _image.sprite = sprites[_currentIndex];

            yield return new WaitForSeconds(timeBetweenFrames);
        }
    } 
}
