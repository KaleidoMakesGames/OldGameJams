using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RetirementDisplayer : MonoBehaviour
{
    public RetirementCalculator calculator;
    public TextMeshProUGUI amountNeededToRetire;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("CanRetire", calculator.canRetire);
        amountNeededToRetire.text = calculator.amountNeededToRetireNow.ToString("C2");
    }
}
