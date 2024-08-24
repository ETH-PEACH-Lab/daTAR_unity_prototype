using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class SetCondition : MonoBehaviour
{
    public OpNode opNode;
    public TMP_Dropdown operands;
    public WhereManager whereManger;

    private List<TMP_Dropdown.OptionData> operandOptions;
    private string condition = "";
    private string value = "";
    private string operand;

    private void Start()
    {
        //set default operand
        operandOptions = operands.options;
        operand = operandOptions[0].text;
        Debug.Log("where 00 " + operand);
    }
    public void setValue(string v)
    {
        value = v;
        updateCondition();
    }

    public void setOperand(int selectedOperand) 
    { 
        operand = operandOptions[selectedOperand].text;
        updateCondition();
    }

    private void updateCondition()
    {
        //check if user input is a number
        float number = 0;
        if (float.TryParse(value, out number))
        {
            condition = operand + " " + value;
            opNode.condition = condition;
            whereManger.conditon = condition;
        }
        Debug.Log("where con " + condition);
    }
}
