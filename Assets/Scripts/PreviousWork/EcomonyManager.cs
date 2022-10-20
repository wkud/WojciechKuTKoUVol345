using System;
using System.Collections;
using System.Collections.Generic;
using TKOU.SimAI;
using TMPro;
using UnityEngine;

public class EcomonyManager : MonoBehaviour 
{
    [Header("Zmienne")] 
    private float cash;
    public TMP_Text cashText;
    private Income income;
    [SerializeField] private Income outcome; 
    [SerializeField] 
    private bool isCalc, canCalc; // variable names are not clear. names should discribe function. use full name like "isCalculating" or "CanCalculate". preferably declare variables in single lines
    public int IntCash; // dont use type of the field in it's name. better name would be "Cash", but this field is never used, so it's better to remove it.
    private List<Income> incomes;
    private PlayerController plCtr; // dont use abbreviations, they are unclear when used in code. rename this to "playerController" or "controller"
    
    
    
    private void Awake()
    {
        income = (Income)GetComponent(typeof(Income)); // use GetComponent<Income>(); instead
        cashText.text = income.Cash.ToString(); 
        plCtr = FindObjectOfType<PlayerController>(); // this is performance-expensive opperation, consider if this is the only way of getting PlayerController (maybe use Dependency Injection)
    }

    // Start is called before the first frame update // unnecessary comment
    IEnumerator Start() // this code will never be executed and it causes infinite loop until StackOverlowException is thrown
    {
        Debug.Log("Start"); 
        StartCoroutine(LateStart()); 
        yield return 0; 
    }

    // Update is called once per frame // unnecessary comment
    public void Update() 
    {
        if(!isCalc) return;
        cashText.text = income.Cash.ToString(); // maybe execute this logic only when income.Cash is modified (for example in Cash property setter or use event OnCashUpdated)
        if (income.Cash > 100) 
        {
            cashText.color = Color.black; 
        }
        else
        {
            cashText.color = new Color(1, 1, 1); 
        }
        InvokeRepeating("DoCalculations", 1, 1); // exepnsive way of calling an empty function
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged") // what if new GameObject are added to the scene? maybe use specific tag instead of a default one
        {
            collision.gameObject.SetActive(false);
        }
    }

    public void AddIncome(int aVal, int bVal, int cVal, float f, bool fast) // unclear variable names
    {
        if (fast)
        {
            cash -= aVal + bVal + cVal; 
        }
        else
        {
            cash -= ((aVal * f) + (bVal * f) + (cVal*f) * f / 3); 
        }
        incomes.Add(new Income(cash));
    }
    
    void DoCalculations() // empty function and it's non-virtual. remove it
    {
        
    }

    IEnumerator LateStart() // this code will never be executed and it causes infinite loop until StackOverlowException is thrown
    {
        yield return new WaitForSeconds(1); 
        StartCoroutine(Start()); 
    }

    public void OnDisable() 
    {
        foreach (var i in incomes)
        {
            incomes.Remove(i);   
        }
        
        incomes = null; // is this necessary to destroy list of incomes? what if new income are added? will incomes list be reinitiated then? 
    }
}
