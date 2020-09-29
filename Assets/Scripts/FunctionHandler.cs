using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCalc;
using System;
using DynamicExpresso;
using DynamicExpresso.Exceptions;

public class FunctionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private int _gridSize = 5;
    private int _totalVoxels;
    private bool[] voxelMap;
    private Expression function;
    private float _detail = 0.01f;
    public GameObject ptPrefab;
    public int _rcSteps;
    private System.Func<float, float, float> dynamicFunction;
    private List<UnityEngine.Object> ptsList = new List<UnityEngine.Object>();
    void Start()
    {
        //function = new Expression("Pow([x], 2) + Pow([y], 2) - 1");
        //function = new Expression("[x] * Sin([y]) + [y] * Sin([x])");
        //function = new Expression("Sin([x]*[y]) + Cos([x]*[y])");



    }
    public void GenerateLines()

    {       
        Debug.Log("Beginning Generation");
        Interpreter interpereter = new Interpreter(InterpreterOptions.DefaultCaseInsensitive);
        string[] strParams = new string[] { "x", "y" };
        try
        {
            dynamicFunction = interpereter.ParseAsDelegate<System.Func<float, float, float>>(ProcessInput(), strParams);
        }
        catch(ParseException p)
        {
            Debug.Log("NO. THAT WILL NOT WORK YOU GOD DAMN IDIOT");
            Debug.Log(p);
            return;
        }
        foreach (UnityEngine.Object pt in ptsList)
        {
            Destroy(pt);
        }
        ptsList.Clear();
        _rcSteps = (int)(_gridSize / _detail) * 2;
        _totalVoxels = _rcSteps * _rcSteps;
        voxelMap = new bool[_totalVoxels];
        GenerateVoxelMap();

        VoxelsToPath();
        TSTrender();
    }
    private void GenerateVoxelMap() 
    {

        int i = 0;
        int signedSteps = _rcSteps / 2;
        for(int r = -signedSteps; r < signedSteps; r++)
        {
            for (int c = -signedSteps; c < signedSteps; c++)
            {
                //function.Parameters["x"] = c * _detail;
                //function.Parameters["y"] = r * _detail;
                voxelMap[i] = dynamicFunction(c * _detail, r * _detail) >= 0;
                /*if((double)function.Evaluate() >= 0)
                {
                    //Debug.Log(x + " " + y);
                    //Debug.Log(function.Evaluate());
                    voxelMap[i] = true;
                    //Instantiate(ptPrefab, new Vector3(c * _detail, r * _detail, 0), Quaternion.identity);
                }*/
                i++;
              
            }
        }
    }
    private void VoxelsToPath()
    {
        bool[] temp = new bool[_totalVoxels];
        for (int i = 0; i < _totalVoxels; i++)
        {
            bool p = voxelMap[i];
            if (p)
            {

                bool falseFound = false;
                if(i < _rcSteps)
                {
                    //we're on top
                }
                else if (!voxelMap[i - _rcSteps])
                {
                    falseFound = true;
                }

                if(((i + 1) % _rcSteps) == 0)
                {
                    //we're on the right
                }
                else if (!voxelMap[i + 1])
                {
                    falseFound = true;
                }

                if((i % _rcSteps) == 0)
                {
                    //we're on the left;
                }
                else if(!voxelMap[i - 1])
                {
                    falseFound = true;
                }
                //Debug.Log(i + _rcSteps);
                //Debug.Log(voxelMap.Length);
                if((i + _rcSteps) >= _totalVoxels)
                {
                    //we're on the bottom
                }
                else if(!voxelMap[i + _rcSteps])
                {
                    falseFound = true;
                }

                temp[i] = falseFound;



            }
        }
        voxelMap = temp;
    }
    private void TSTrender()
    {
        for(int i = 0; i < _totalVoxels; i++)
        {
            if(voxelMap[i])
            {
                int r = (int)(i / _rcSteps);
                int c = i - (r * _rcSteps);
                float x = (c * _detail) - _gridSize;
                float y = (r * _detail) - _gridSize;
                float h = 0.0001f;
                //function.Parameters["x"] = x + h;
                //function.Parameters["y"] = y;
                //float fxh = (float)(double)function.Evaluate();
                //function.Parameters["x"] = x;
                //float z = (fxh - (float)(double)function.Evaluate()) / h;

                float z = (dynamicFunction(x + h, y) - dynamicFunction(x, y)) / h;
                if (!float.IsNaN(z))
                {
                    ptsList.Add(Instantiate(ptPrefab, new Vector3(x, y, z), Quaternion.identity));
                }
                
            }
        }

    }
    public string ProcessInput()
    {
        string input = GameObject.FindGameObjectWithTag("Text Input").GetComponent<UnityEngine.UI.InputField>().text;
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if(Char.IsLetter(c)) {
                bool firstLetter = false;
                if ((c != 'x') && (c != 'y') && (c != 'X') && (c != 'Y'))
                {

                    if (i == 0)
                    {
                        firstLetter = true;
                    }
                    else if (!Char.IsLetter(input[i - 1]) && (input[i - 1] != '.'))
                    {
                        firstLetter = true;
                    }
                    if (firstLetter)
                    {
                        Debug.Log("insertion (;");
                        input = input.Insert(i, "Math.");
                    }
                }
            }
        }
        return input;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
