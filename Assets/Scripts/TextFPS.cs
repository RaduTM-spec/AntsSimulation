using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextFPS : MonoBehaviour
{

    public TMP_Text text;
    // Start is called before the first frame update
    [SerializeField] float fpsRefresh = .1f;
    float nextFPSRefresh = 0f;

    // Update is called once per frame
    void Update()
    {
        if (nextFPSRefresh <= 0f)
        {
            text.text = "FPS: " + (1.0f / Time.deltaTime).ToString("0.0");
            nextFPSRefresh = fpsRefresh;
        }
        else
        {
            nextFPSRefresh -= Time.deltaTime;
        }
    }

}
