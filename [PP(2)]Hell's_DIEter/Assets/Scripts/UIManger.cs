using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour
{
    private Player playerScript;
    public Slider WeightSlider;
    public Slider HPSlider;
    public Slider FuelSlider;
    private Text weightText;
    private Text hpText;
    private Text fuelText;

    private float weight;
    private float hp;
    private float fuel;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        WeightSlider.maxValue = playerScript.GetWeightInfo();
        HPSlider.maxValue = playerScript.GetHPInfo();
        FuelSlider.maxValue = playerScript.GetMaxFuelInfo();

        weightText = WeightSlider.GetComponentInChildren<Text>();
        hpText = HPSlider.GetComponentInChildren<Text>();
        fuelText = FuelSlider.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGauge();
    }

    private void UpdateGauge()
    {
        weight = playerScript.GetWeightInfo();
        hp = playerScript.GetHPInfo();
        fuel = playerScript.GetFuelInfo();

        WeightSlider.value = weight;
        HPSlider.value = hp;
        FuelSlider.value = fuel;

        weightText.text = weight.ToString();
        hpText.text = hp.ToString();
        fuel = (int)fuel;
        fuelText.text = fuel.ToString();

        if (WeightSlider.value <= 0)
            WeightSlider.transform.Find("Fill Area").gameObject.SetActive(false);
        else
            WeightSlider.transform.Find("Fill Area").gameObject.SetActive(true);

        if (HPSlider.value <= 0)
            HPSlider.transform.Find("Fill Area").gameObject.SetActive(false);
        else
            HPSlider.transform.Find("Fill Area").gameObject.SetActive(true);

        if (FuelSlider.value <= 0)
            FuelSlider.transform.Find("Fill Area").gameObject.SetActive(false);
        else
            FuelSlider.transform.Find("Fill Area").gameObject.SetActive(true);
    }
}
