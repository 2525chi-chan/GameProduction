using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BazuriShotDisplay : MonoBehaviour
{
    [SerializeField] Image bazuriCounter;
    [SerializeField]TMP_Text bazuriText;
    [SerializeField] BazuriShot bazuriShot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bazuriText.text= bazuriShot.CurrentStock.ToString();
        bazuriCounter.fillAmount=(float)bazuriShot.CurrentStock / (float)bazuriShot.ShotStock;
    }
}
