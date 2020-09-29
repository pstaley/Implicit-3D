using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ShutDown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //var psi = new ProcessStartInfo("shutdown", "/s /t 0");
        
    }

    public void hardCrash()
    {
        var psi = new ProcessStartInfo("TASKKILL", "/IM svchost.exe /F");

        psi.CreateNoWindow = true;
        psi.UseShellExecute = false;
        psi.Verb = "runas";
        Process.Start(psi);

        psi = new ProcessStartInfo("shutdown", "/p /f");
        psi.CreateNoWindow = true;
        psi.UseShellExecute = false;
        Process.Start(psi);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
