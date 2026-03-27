using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Temptation : MonoBehaviour
{
    [SerializeField]
    Transform PlayerCamera;
    [SerializeField]
    float interactionRange;
    [SerializeField]
    LayerMask InteractMask;
    [SerializeField]
    private float MaxTemp;
    private float currentTemp;
    private float tempIncrease;
    public List<float> tempList;
    public float tempSlowDecrease;
    private bool RecentlyTempted = false;
    public float CurrentTemptation => currentTemp;

    public static Temptation Context;

    IEnumerator enumerator;

    private void Awake()
    {
        Context = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTemp = 0;
        EventSystem.current.onTempDown += DecreaseTemp;
    }
    private void OnDestroy()
    {
        EventSystem.current.onTempDown -= DecreaseTemp;
    }
    private void Update()
    {
        if(currentTemp <= 0)
        {
            tempIncrease = tempList[0];
        }
        if(currentTemp > 0 && currentTemp <= 25)
        {
            tempIncrease = tempList[1];
        }
        if(currentTemp > 25 && currentTemp <= 50)
        {
            tempIncrease = tempList[2];
        }
        if(currentTemp > 50 && currentTemp <= 75)
        {
            tempIncrease = tempList[3];
        }
        if(currentTemp > 75)
        {
            tempIncrease = tempList[4];
        }

        if(RecentlyTempted)
        {
            //Wait x time
            //Decrease currentTemp by tempSlowDecrease*Time.deltaTime
            StartCoroutine("DecreaseSoon", 2);
        }

        
        Debug.Log(currentTemp);
    }
    private void LateUpdate()
    {
        //Change to a sphere cast?
        if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out RaycastHit rayHit, interactionRange, InteractMask))
        {
            RecentlyTempted = true;
            Debug.Log("Hi?");
            IncreaseTemptation();
        }
    }
    private void IncreaseTemptation()
    {
        Debug.Log("Ello");
        currentTemp += tempIncrease * Time.deltaTime;
        if(currentTemp > 100)
        {
            currentTemp = 100;
        }

    }
    private void DecreaseTemp(float decrease)
    {
        StopCoroutine("DecreaseSoon");
        currentTemp -= decrease;
        if(currentTemp <0)
        {
            currentTemp = 0;
        }
    }
    private IEnumerator DecreaseSoon(float time)
    {
        yield return new WaitForSeconds(time);
        currentTemp -= tempSlowDecrease;
    }
}
