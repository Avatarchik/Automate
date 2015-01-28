using UnityEngine;
using System.Collections;

public class FCBLivesController : MonoBehaviour {

    public UILabel [] labels;

    int _lives;

    public int lives
    {
        get { return _lives; }
        set { 
            _lives = value;

            foreach (var l in labels) { 
                l.text = _lives.ToString ();
            }
        } 
    }
}
