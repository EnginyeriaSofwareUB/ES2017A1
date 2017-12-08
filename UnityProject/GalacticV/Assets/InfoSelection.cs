using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoSelection : MonoBehaviour {

    [SerializeField]
    List<string> blueUnits;
    List<string> redUnits;

    public List<string> BlueUnits
    {
        get { return this.blueUnits; }
        set { this.blueUnits = value; }
    }

    public List<string> RedUnits
    {
        get { return this.redUnits; }
        set { this.redUnits = value; }
    }

    private void Start()
    {
        blueUnits = new List<string>();
        redUnits = new List<string>();
    }


    public void SetBlueUnits(List<string> _blueUnits)
    {
        blueUnits.Clear();
        blueUnits = _blueUnits;
    }

    public void SetRedUnits(List<string> _redUnits)
    {
        redUnits.Clear();
        redUnits = _redUnits;
    }

}
