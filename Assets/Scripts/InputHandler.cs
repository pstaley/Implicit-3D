using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private float _step = 0.001f;
    private float _xbound = 5f;
    private float _ybound = 5f;
    private float _zbound = 5f;

    public GameObject ptPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //test function y=x^2 + z
        float x = -_xbound;
        while(x < _xbound)
        {
            float z = -_zbound;
            while (z < _zbound)
            {
                float y = Mathf.Pow(x, 2) + z;
                if(y < _ybound && y > -_ybound)
                Instantiate(ptPrefab, new Vector3(x, y, z), Quaternion.identity);
                z += +_step;
            }
            

            x += _step;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
