using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCalc;

public class RenderController : MonoBehaviour
{
    [SerializeField]
    private int _renderDetail = 100;
    // Start is called before the first frame update
    void Start()
    {
        Expression e = new Expression("Pow([x], 2) + Pow([y], 2)");
        Debug.Log(e.Evaluate());
        LineRenderer functionRenderer = gameObject.AddComponent<LineRenderer>();
        functionRenderer.positionCount = _renderDetail;
        functionRenderer.widthMultiplier = 0.1f;
        Vector3[] points = new Vector3[_renderDetail];
        for (int i = 0; i < _renderDetail; i++)
        {
            points[i] = new Vector3((float)i / 10, ((float)i / 20), (float)i / 30);
        }
        functionRenderer.SetPositions(points);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int getRenderDetail()
    {
        return _renderDetail;
    }
    void setRenderDetail(int d)
    {
        _renderDetail = d;
    }
}
