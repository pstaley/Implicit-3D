using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using NCalc;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using System;
using DynamicExpresso;

public class Formula : MonoBehaviour
{
    private Expression expression;
    private string parsedFormula;
    private float _precision = 0.01f;
    private float _searchSize = 0.100f;
    private float _xMax = 10;
    private float _yMax = 5;
    private float _zMax = 10;
    private System.Func<float, float, float> dynamicFunction;
    // Start is called before the first frame update
    void Start()
    {
        GenerateFunction();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private string _evalString;
    public void EvaluateString(string s)
    {

    }
    public void ParseInput(string s)
    {
    }
    public void GenerateFunction()
    {
        expression = new Expression("Pow([x], 2) + Pow([y], 2) - 1");//Expression(f);
        Interpreter interpereter = new Interpreter();
        string[] strParams = new string[] { "x", "y" };
        dynamicFunction = interpereter.ParseAsDelegate<System.Func<float, float, float>>("Math.Pow(x,2) + Math.Pow(y,2) - 1", strParams);
        //find the beginning of our line
        expression.Parameters["x"] = -_xMax;
        expression.Parameters["y"] = -_yMax;
        int tst = 0;
        for (float x = -_xMax; x < _xMax; x += _searchSize)
        {
            expression.Parameters["x"] = x;
            expression.Parameters["y"] = -_yMax;
            bool oldSign = Mathf.Sign((float)(double)expression.Evaluate()) == 1;
            for (float y = -_yMax; y < _yMax; y += _searchSize)
            {
                expression.Parameters["y"] = y;
                if ((Mathf.Sign((float)(double)expression.Evaluate()) == 1) != oldSign)
                {
                    oldSign = !oldSign;
                    //TODO: found a sign change, trace the line here.
                    //Debug.Log(x + "," + y);
                    Debug.Log("###Tracing new line###################################################");
                    TraceLine(x, y);
                    tst++;

                    expression.Parameters["x"] = x;
                    expression.Parameters["y"] = y;
                    if (tst > 1)
                    {
                        return;
                    }
                }
            }
            
        }

    }
    private void TraceLine(float x, float y)
    {
        Vector3[] points = new Vector3[100];
        float guessValue = (float)(double)expression.Evaluate();
        float highSign = Mathf.Sign(guessValue);
 

        //Find initial value using bisection
        while (Mathf.Abs(guessValue) > _precision)
        {
            float delta = _searchSize;
            if (guessValue > 0)
            {
                //Debug.Log("Value too big");
                delta = delta / 2;
                y -= highSign * delta;
            }
            else
            {
                //Debug.Log("Value too small");
                delta = delta / 2;
                y += highSign * delta;
                
            }
            expression.Parameters["y"] = y;
            guessValue = (float)(double)expression.Evaluate();
        }
        //Debug.Log("Value is within range!");

        //Trace along line
        Debug.Log("Value was: " + guessValue);
        Debug.Log("Found at: " + x + " , " + y);
        float h = 0.1f;
        int i = 0;
        while(true)
        {
            //Using Newton-Raphson, follow line
            float j = 0.000001f;
            //Debug.Log(x + " , " + y);
            expression.Parameters["x"] = x + j;
            float gx = ((float)(double)expression.Evaluate() - guessValue) / j;
            expression.Parameters["x"] = x;
            expression.Parameters["y"] = y + j;
            float gy = ((float)(double)expression.Evaluate() - guessValue) / j;
            //Debug.Log("gx=" + gx + " gy=" + gy);
            //test to see if a small gy or gx is introducing inaccuracy
           
            x += h;
            //y += (-h * gx) / gy;
            
            expression.Parameters["x"] = x;
            expression.Parameters["y"] = y;
            guessValue = (float)(double)expression.Evaluate();
            //test to see if we are still accurate
            if(Mathf.Abs(guessValue) > _precision)
            {
                float delta = _searchSize;
                //Determine direction of curve. Assumes that the point is closest to the curve it's on.
                float currentSign = Mathf.Sign(guessValue);
                int yMov;
                if(currentSign == highSign)
                {
                    //we are above the function
                    yMov = -1;
                }
                else
                {
                    //we are below the function
                    yMov = 1;
                }
                int reps = 0;
                float oldValue = guessValue; ;
                while(currentSign == Mathf.Sign(guessValue))
                {
                    if(reps > 0)
                    {
                        if (Mathf.Abs(oldValue) < guessValue)
                        {
                            if (i == 0)
                            {
                                Debug.Log("TODO: Inaccuracy Increacing, likely skipped zone");
                                //TODO: anything?
                                
                            }
                            else
                            {
                                Debug.Log("Inaccuracy increacing, line likely finished.");
                                return;
                            }
                        }
                        else
                        {
                            oldValue = guessValue;
                        }
                    }
                    y += delta * yMov;
                    expression.Parameters["y"] = y;
                    guessValue = (float)(double)expression.Evaluate();
                    reps++;

                }

                //Okay, now we should have a nice, happy delta.
                bool acc = true;
                if(Mathf.Abs(gx) > 1 || Mathf.Abs(gy) > 1)
                {
                    acc = false;
                }
                Debug.Log("##REGENERATING##  (" + guessValue + ")" + "gx/gy: " + acc);
                
                int tst = 0;
                while (Mathf.Abs(guessValue) > _precision)
                {

                    if (guessValue > 0) //guessValue > 0
                    {
                        //Debug.Log("Value too big");
                        delta = delta / 2;
                        y -= highSign * delta;
                    }
                    else
                    {
                        //Debug.Log("Value too small");
                        delta = delta / 2;
                        y += highSign * delta;

                    }
                    expression.Parameters["y"] = y;
                    guessValue = (float)(double)expression.Evaluate();
                    //Debug.Log(guessValue);
                    tst++;
                }
            }
            Debug.Log("New generated pt: " + x + "," + y);
            Debug.Log("Accuracy: " + guessValue);
            i++;
        }
        
    }
    private void Reevaluate (float x, float y)
    {

    }

}


