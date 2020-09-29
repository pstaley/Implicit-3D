using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCalc;
using DynamicExpresso;

public class MathTest : MonoBehaviour
{
    private Expression ncExpression;
    private Interpreter interpereter;
    private Lambda deExpression;
    private float currentVal;
    private bool[] output;
    private int reps =  10000000;
    private System.Func<float, float, float> dynamicFunction;
    private System.Func<float, float, bool> dynamicFunctionBool;
    // Start is called before the first frame update
    void Start()
    {
        output = new bool[reps];
        ncExpression = new Expression("Pow([x], 2) + Pow([y], 2) - 1");

        interpereter = new Interpreter();
        Parameter[] parameters = new Parameter[] {

        new Parameter("x", typeof(float)),
        new Parameter("y", typeof(float))
        };

        string[] strParams = new string[] { "x", "y" };

        dynamicFunction = interpereter.ParseAsDelegate<System.Func<float, float, float>>("Math.Pow(x,2) + Math.Pow(y,2) - 1", strParams);
        dynamicFunctionBool = interpereter.ParseAsDelegate<System.Func<float, float, bool>>("(Math.Pow(x,2) + Math.Pow(y,2) - 1) > 1", strParams);
        Debug.Log(dynamicFunction(1, 1));
        deExpression = interpereter.Parse("Math.Pow(x,2) + Math.Pow(y,2) - 1", parameters);


    }

    // Update is called once per frame
    int i = 0;
    
    void Update()
    {

        Debug.Log("Cycle");
        if (i < 2)
        {
            Debug.Log("Evaluating with NCalc");
            /*for (int i = 0; i < 10000000; i++)
            {
                ncExpression.Parameters["x"] = i;
                ncExpression.Parameters["y"] = i + 1;
                output[i] = (float)(double)ncExpression.Evaluate();
            }*/
            /*Debug.Log("Done, now trying system");
            for (int i = 0; i < 1000000; i++)
            {
                output[i] = Mathf.Pow(i, 2) + Mathf.Pow(i + 1, 2) - 1;
            }
            Debug.Log("System done, trying with d. expresso");
            */
            /*for (int i = 0; i < 1000000; i++)
            {
                Parameter[] parameters = new Parameter[]
                {
                    new Parameter("x", i), new Parameter("y", i)
                };
                output[i] = (float)(double)interpereter.Eval("Math.Pow(x, 2) + Math.Pow(y, 2) - 1", parameters);
            }*/
            Debug.Log("aaaand fully dynamic function");
            for (int i = 0; i < 10000000; i++)
            {

                output[i] = dynamicFunction(i, i) > 1;
            }
            Debug.Log("done");
            for (int i = 0; i < 10000000; i++)
            {
                if(dynamicFunctionBool(i, i))
                {
                    output[i] = true;
                }
            }
            Debug.Log("bool done");
        }
        i++;
    }

}
