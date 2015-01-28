using UnityEngine;
using System.Collections;

public class RCDoubleGameStepController : MonoBehaviour
{
    public SpriteRenderer risk_step_block_0;
    public SpriteRenderer risk_step_block_1;
    public Sprite[] step_img;
    SlotController _sc;
    int block_0_index;
    int block_1_index;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (slotController != null && slotController.RiskStep < 100)
        {
            if (TransformStepToIndex(slotController.RiskStep+1, out block_0_index, out block_1_index))
            {
                risk_step_block_0.sprite = step_img[block_0_index];
                risk_step_block_1.sprite = null;
            }
            else
            {
                risk_step_block_0.sprite = step_img[block_0_index];
                risk_step_block_1.sprite = step_img[block_1_index];
            }
        }
	}

    SlotController slotController
    {
        get
        {
            if (_sc == null)
            {
                var g = GameObject.FindGameObjectWithTag("SlotMenuGameContainer");

                if (g != null)
                    _sc = g.GetComponent<SlotController>();

            }

            return _sc;
        }
    }

    bool TransformStepToIndex(int step, out int index_0, out int index_1)
    {
        if(step<10)
        {
            index_0 = step;
            index_1 = -1;
            return true;
        }
        else
        {
            index_0 = step/10;
            index_1 = step%10;
            return false;
        }
    }
}
