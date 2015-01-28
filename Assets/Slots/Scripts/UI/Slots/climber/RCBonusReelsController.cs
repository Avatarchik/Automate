using UnityEngine;
using System.Collections;
using UnitySlot;
using System.Collections.Generic;
using System;

public class RCBonusReelsController : MonoBehaviour
{
    enum ReelState 
    {
        WAITING = 0,
        WIN_CLIMBING = 1,
        LOSE_CLIMBING = 2,
    }

    public RCBonusGameController bonusGameController;
    public int winClimbingSpeed;
    public int loseClimbingSpeed;
    public float fallingSpeed;
    public float standingSpeed;

    float move_step;
    int stepNumber;
    int choosedColumn;
    bool inProcess;

    bool[] usedColumns;
    bool[] caveColumns;
    float[] climberPositions;

    public Animator YettiAnim;
    public Animator ClimberAnim;

    public GameObject[] caveObjects;
    public GameObject[] ladge_01;
    public GameObject[] ladge_02;

    SpriteRenderer[,] all_slots;
    SpriteRenderer currentCave;
    List<SpriteRenderer> activeCaves;

    public Sprite[] stone_background;
   
    SpriteRenderer[] slots;
    public SpriteRenderer[] winBoxes;

    public float speed;

    public bool stop;
    public GameObject SlotsParent;

    ReelState[] reelSpinning = new ReelState[3];
    SpriteRenderer[] topSlots = new SpriteRenderer[3];

   void OnEnable() 
    {
        StartOnEnable();     
    }
    // Use this for initialization
    void Start ()
    {
        move_step = 0;
        climberPositions = new float[] { -2.64f, -1.36f, -0.08f, 1.2f, 2.49f };        
    }

    // Update is called once per frame
    void Update () {

//        if (stop) {
//            GameState.CurrentGame.ReelSymbols = new List<int> { 3, 2, 1 };
//            _spinning = false;
//        }
	
      //  SpinReel (0);
       // SpinReel (1);
     //   SpinReel (2);
    }

    void FixedUpdate()
    {

    }

    #region Animations

    void StartWaitingingAnimation()
    {
        for(int i = 0; i < usedColumns.Length;i++)
        {
            string anim_param = "arrow_" + i.ToString();
            ClimberAnim.SetBool(anim_param, !usedColumns[i]);
        }

        ClimberAnim.SetBool("grounding", false);       
        ClimberAnim.SetBool("waiting", true);
    }

    void StartClimbingAnimation()
    {
        ClimberAnim.SetBool("waiting", false);
        ClimberAnim.SetBool("climbing", true);
    }

    void StartLandingAnimation()
    {
        ClimberAnim.SetBool("climbing", false);
        ClimberAnim.SetBool("grounding", true);

    }

    void StartFallingAnimation()
    {
        ClimberAnim.SetBool("climbing", false);
        ClimberAnim.SetBool("falling", true);

        YettiAnim.SetTrigger("bump");
    }

    #endregion

    //Инициализация параметров для старта
    void StartOnEnable()
    {
        stepNumber = 0;
        usedColumns = new bool[5] { false, false, false, false, false };
        caveColumns = new bool[5] { false, false, false, false, false };
        
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int randomIndex = UnityEngine.Random.Range(0, stone_background.Length);

                switch (i)
                {
                    case 0: AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false; break;
                    case 1: AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false; break;
                    case 2: AllSlots[i, j].sprite = stone_background[randomIndex]; AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true; break;
                    case 3: AllSlots[i, j].sprite = stone_background[randomIndex]; AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true; break;
                    case 4: AllSlots[i, j].sprite = stone_background[randomIndex]; AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true; break;
                }

            }
        }

        RestetProcessArguments();
    }

    //Выбор канатов
    public void OnRopeClick(UnityEngine.Object ropeButtonObj)
    {
        if (!inProcess)
        {
            int choosed_rop_number = Convert.ToInt32(ropeButtonObj.name);
            choosed_rop_number--;

            if (!usedColumns[choosed_rop_number])
            {
                ServerHelper.SafeBonusGameRequest(CallBack);
                choosedColumn = choosed_rop_number;
                inProcess = true;              
            }
        }

       
    }

    //Ответ от сервера
    public void CallBack()
    {
        stepNumber++;
       
       if(GameState.BonusGame.Lives > 0)
       {
           StartProcessWIN();
       }
       else
       {
            StartProcessLOSE();
       }
    }

    //
    public void StartProcessLOSE()
    {
        SetCombinationLOSE();
        StartCoroutine("StartClimbingLOSE");
    }

    //
    public void StartProcessWIN()
    {
        SetCombinationWIN();
        StartCoroutine("StartClimbingWIN");
    }

    IEnumerator StartClimbingLOSE()
    {
        bool reached = false;
        Transform climber = ClimberAnim.gameObject.transform.FindChild("ClimberMan");
        climber.transform.localPosition = new Vector3(climberPositions[choosedColumn], climber.transform.localPosition.y, climber.transform.localPosition.z);
        StartClimbingAnimation();
        yield return new WaitForFixedUpdate();
        while (!reached)
        {
            float current_step = move_step;

            move_step = Mathf.Lerp(move_step, 1, Time.fixedDeltaTime * loseClimbingSpeed);

            if (move_step >= 0.98f)
            {
                move_step = 0;
                current_step = 1 - current_step;
            }
            else
            {
                current_step = move_step - current_step;
            }
           

            reached = ClimbByCurrentStepLOSE_UP(current_step,0);

            

            yield return new WaitForFixedUpdate();
        }

        reached = false;

        StartFallingAnimation();

        yield return new WaitForSeconds(1f);

        while (!reached)
        {
            reached = ClimbByCurrentStepLOSE_DOWN(fallingSpeed);

            yield return new WaitForFixedUpdate();
        }

        Invoke("EndBonusGame", 2f);
    }

    IEnumerator StartClimbingWIN()
    {
        bool reached = false;

        Transform climber = ClimberAnim.gameObject.transform.FindChild("ClimberMan");
        climber.transform.localPosition = new Vector3(climberPositions[choosedColumn], climber.transform.localPosition.y, climber.transform.localPosition.z);

        StartClimbingAnimation();

        yield return new WaitForFixedUpdate();

        while (!reached)
        {
            float current_step = move_step;

            move_step = Mathf.Lerp(move_step, 1, Time.fixedDeltaTime * winClimbingSpeed);

            if (move_step >= 0.98f)
            {
                move_step = 0;
                current_step = 1 - current_step;
            }
            else
            {
                current_step = move_step - current_step;
            }
           
            reached = ClimbByCurrentStepLOSE_UP(current_step ,0.2f);

            yield return new WaitForFixedUpdate();
        }

        reached = false;

        MoveUpSingleLine(0);
        MoveUpSingleLine(1);
        MoveUpSingleLine(2);
        MoveUpSingleLine(3);
        MoveUpSingleLine(4);

        while (!reached)
        {
            float current_step = move_step;

            move_step = Mathf.Lerp(move_step, 1, Time.fixedDeltaTime * winClimbingSpeed);

            if (move_step >= 0.98f)
            {
                move_step = 0;
                current_step = 1 - current_step;
            }
            else
            {
                current_step = move_step - current_step;
            }
           
            reached = ClimbByCurrentStepWIN(current_step);

            yield return new WaitForFixedUpdate();
        }

        reached = false;

       
        MoveUpSingleLine(5);
        MoveUpSingleLine(6);
        MoveUpSingleLine(8);
        MoveUpSingleLine(7);  

        StartLandingAnimation();

        while (!reached)
        {
            reached = ClimbByCurrentStepWIN_COMPLETE(standingSpeed);

            yield return new WaitForFixedUpdate();            
        }

    }

    void SetCombinationLOSE()
    {
        caveColumns[choosedColumn] = true;

        for (int i = 0; i < caveColumns.Length; i++)
        {
            int randomNumber = UnityEngine.Random.Range(0, 10);
            if (randomNumber > 4)
            {
                caveColumns[i] = true;
            }
        }

        for (int i = 0; i < caveColumns.Length; i++)
        {
            if (caveColumns[i])
            {
               
                if (i == choosedColumn)
                {
                    currentCave = caveObjects[i].transform.FindChild("1").gameObject.transform.FindChild("1").gameObject.GetComponent<SpriteRenderer>();
                    currentCave.enabled = true;
                    currentCave.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    currentCave.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;

                    YettiAnim = currentCave.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();

                }
                else
                { 
                     int randomNumber;
                
                    randomNumber = UnityEngine.Random.Range(1, caveObjects[i].transform.childCount + 1);
                    SpriteRenderer cave_con;
                    if (usedColumns[i])
                    {
                        cave_con = caveObjects[i].transform.FindChild("2").gameObject.transform.FindChild(randomNumber.ToString()).gameObject.GetComponent<SpriteRenderer>();
                        cave_con.enabled = true;
                        cave_con.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;

                        ActiveCaves.Add(cave_con);
                    }
                    else
                    {
                        cave_con = caveObjects[i].transform.FindChild("1").gameObject.transform.FindChild(randomNumber.ToString()).gameObject.GetComponent<SpriteRenderer>();
                        cave_con.enabled = true;
                        cave_con.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        cave_con.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;

                        ActiveCaves.Add(cave_con);
                    }

                }
            }
        }

        for (int i = 5; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int randomIndex = UnityEngine.Random.Range(0, stone_background.Length - 1);

                switch (i)
                {
                    case 5:
                        AllSlots[i, j].sprite = stone_background[randomIndex];
                        AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = !usedColumns[j];
                        break;
                    case 6:
                        AllSlots[i, j].sprite = stone_background[randomIndex];

                        if (!usedColumns[j] && !caveColumns[j])
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        }
                        break;
                    case 7: AllSlots[i, j].sprite = stone_background[randomIndex];
                        if (!usedColumns[j] && !caveColumns[j])
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        }
                        break;
                    case 8: AllSlots[i, j].sprite = stone_background[randomIndex];
                        if (!usedColumns[j] && !caveColumns[j])
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        }
                        break;

                }

            }
        }

        caveColumns[choosedColumn] = false;

    }

    void SetCombinationWIN()
    {
        for (int i = 0; i < caveColumns.Length; i++)
        {
            int randomNumber = UnityEngine.Random.Range(0, 10);
            caveColumns[i] = true;
            if (randomNumber > 6)
            {
                caveColumns[i] = false;
            }
        }

        caveColumns[choosedColumn] = false;

        for (int i = 0; i < caveColumns.Length; i++)
        {
            if (caveColumns[i])
            {
                int randomNumber;
                if (i == choosedColumn)
                {
                    randomNumber = UnityEngine.Random.Range(1, caveObjects[i].transform.childCount + 1);

                    currentCave = caveObjects[i].transform.FindChild("1").gameObject.transform.FindChild(randomNumber.ToString()).gameObject.GetComponent<SpriteRenderer>();
                    currentCave.enabled = true;
                    currentCave.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    currentCave.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    randomNumber = UnityEngine.Random.Range(1, caveObjects[i].transform.childCount + 1);
                    SpriteRenderer cave_con;
                    if (usedColumns[i])
                    {
                        cave_con = caveObjects[i].transform.FindChild("2").gameObject.transform.FindChild(randomNumber.ToString()).gameObject.GetComponent<SpriteRenderer>();
                        cave_con.enabled = true;
                        cave_con.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;

                        ActiveCaves.Add(cave_con);
                    }
                    else
                    {
                        cave_con = caveObjects[i].transform.FindChild("1").gameObject.transform.FindChild(randomNumber.ToString()).gameObject.GetComponent<SpriteRenderer>();
                        cave_con.enabled = true;
                        cave_con.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        cave_con.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;

                        ActiveCaves.Add(cave_con);
                    }

                }
            }
        }

        for (int i = 5; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int randomIndex = UnityEngine.Random.Range(0, stone_background.Length - 1);

                switch (i)
                {
                    case 5:
                        AllSlots[i, j].sprite = stone_background[randomIndex];
                        AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = !usedColumns[j];

                        break;
                    case 6:
                        AllSlots[i, j].sprite = stone_background[randomIndex];

                        if (!usedColumns[j] && !caveColumns[j])
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        }
                        break;
                    case 7: AllSlots[i, j].sprite = stone_background[randomIndex];
                        if (!usedColumns[j] && !caveColumns[j])
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        }
                        break;
                    case 8: AllSlots[i, j].sprite = stone_background[randomIndex];
                        if (!usedColumns[j] && !caveColumns[j])
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            AllSlots[i, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        }
                        break;

                }

            }
        }

        caveColumns[choosedColumn] = false;
    }

    void MoveUpSingleLine(int lineNumber)
    {
        for (int j = 0; j < 5; j++)
        {
            int randomIndex = UnityEngine.Random.Range(0, stone_background.Length);

            switch (lineNumber)
            {
                case 0: 
                    if(!caveColumns[j] && !usedColumns[j])
                    {
                        AllSlots[lineNumber, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }
                    else
                    {
                        AllSlots[lineNumber, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    break;
                case 1: if (!caveColumns[j] && !usedColumns[j])
                    {
                        AllSlots[lineNumber, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }
                    else
                    {
                        AllSlots[lineNumber, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    break;
                case 2: 
                    caveColumns = new bool[] { false, false, false, false, false };
                    usedColumns[choosedColumn] = true;
                    AllSlots[lineNumber, j].sprite = stone_background[randomIndex];
                    AllSlots[lineNumber, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = !usedColumns[j];
                    break;
                case 3:
                    AllSlots[lineNumber, j].sprite = stone_background[randomIndex];
                    AllSlots[lineNumber, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = !usedColumns[j];
                    break;
                case 4:
                    AllSlots[lineNumber, j].sprite = stone_background[randomIndex];
                    AllSlots[lineNumber, j].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = !usedColumns[j];
                    break;
            }

            AllSlots[lineNumber, j].gameObject.transform.localPosition = new Vector3(AllSlots[lineNumber, j].gameObject.transform.localPosition.x, AllSlots[lineNumber, j].gameObject.transform.localPosition.y + 8.98f * 1.28f, AllSlots[lineNumber, j].gameObject.transform.localPosition.z);
           
        }

        if (lineNumber == 0)
        {
            for (int j = 0; j < ladge_01.Length; j++)
            {
                ladge_01[j].gameObject.transform.localPosition = new Vector3(ladge_01[j].gameObject.transform.localPosition.x, ladge_01[j].gameObject.transform.localPosition.y + 8.98f * 1.28f, ladge_01[j].gameObject.transform.localPosition.z);
                ladge_02[j].gameObject.transform.localPosition = new Vector3(ladge_02[j].gameObject.transform.localPosition.x, ladge_02[j].gameObject.transform.localPosition.y + 8.98f * 1.28f, ladge_02[j].gameObject.transform.localPosition.z);
            }

        }
    }

    bool ClimbByCurrentStepWIN_COMPLETE(float moveStep)
    {
        bool reached = false;

        if ((AllSlots[0, 0].gameObject.transform.localPosition.y - moveStep) < -1.92f)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    AllSlots[i, j].gameObject.transform.localPosition = new Vector3(AllSlots[i, j].gameObject.transform.localPosition.x, -1.92f + 1.28f * i, AllSlots[i, j].gameObject.transform.localPosition.z);
                }
            }

            for (int i = 0; i < caveObjects.Length; i++)
            {
                caveObjects[i].gameObject.transform.localPosition = new Vector3(caveObjects[i].gameObject.transform.localPosition.x, 6.4f, caveObjects[i].gameObject.transform.localPosition.z);
            }

            for (int i = 0; i < ladge_01.Length; i++)
            {
                if (i > 2)
                {
                    ladge_01[i].gameObject.transform.localPosition = new Vector3(ladge_01[i].gameObject.transform.localPosition.x, -1.92f, ladge_01[i].gameObject.transform.localPosition.z);
                    ladge_02[i].gameObject.transform.localPosition = new Vector3(ladge_02[i].gameObject.transform.localPosition.x, -1.92f, ladge_02[i].gameObject.transform.localPosition.z);
                }
                else
                {
                    ladge_01[i].gameObject.transform.localPosition = new Vector3(ladge_01[i].gameObject.transform.localPosition.x, -0.64f, ladge_01[i].gameObject.transform.localPosition.z);
                    ladge_02[i].gameObject.transform.localPosition = new Vector3(ladge_02[i].gameObject.transform.localPosition.x, -0.64f, ladge_02[i].gameObject.transform.localPosition.z);
                }
            }

            Invoke("RestetProcessArguments", 0.5f);
            reached = true;
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    AllSlots[i, j].gameObject.transform.localPosition = new Vector3(AllSlots[i, j].gameObject.transform.localPosition.x, AllSlots[i, j].gameObject.transform.localPosition.y - moveStep, AllSlots[i, j].gameObject.transform.localPosition.z);
                }
            }

            for (int i = 0; i < caveObjects.Length; i++)
            {
                caveObjects[i].gameObject.transform.localPosition = new Vector3(caveObjects[i].gameObject.transform.localPosition.x, caveObjects[i].gameObject.transform.localPosition.y - moveStep, caveObjects[i].gameObject.transform.localPosition.z);
            }

            for (int i = 0; i < ladge_01.Length; i++)
            {
                ladge_01[i].gameObject.transform.localPosition = new Vector3(ladge_01[i].gameObject.transform.localPosition.x, ladge_01[i].gameObject.transform.localPosition.y - moveStep, ladge_01[i].gameObject.transform.localPosition.z);
                ladge_02[i].gameObject.transform.localPosition = new Vector3(ladge_02[i].gameObject.transform.localPosition.x, ladge_02[i].gameObject.transform.localPosition.y - moveStep, ladge_02[i].gameObject.transform.localPosition.z);
            }


        }

        return reached;
    }

    bool ClimbByCurrentStepWIN(float moveStep)
    {
        bool reached = false;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                AllSlots[i, j].gameObject.transform.localPosition = new Vector3(AllSlots[i, j].gameObject.transform.localPosition.x, AllSlots[i, j].gameObject.transform.localPosition.y - moveStep, AllSlots[i, j].gameObject.transform.localPosition.z);
            }
        }

        for (int i = 0; i < caveObjects.Length; i++)
        {
            caveObjects[i].gameObject.transform.localPosition = new Vector3(caveObjects[i].gameObject.transform.localPosition.x, caveObjects[i].gameObject.transform.localPosition.y - moveStep, caveObjects[i].gameObject.transform.localPosition.z);
        }

        for (int i = 0; i < ladge_01.Length; i++)
        {
            ladge_01[i].gameObject.transform.localPosition = new Vector3(ladge_01[i].gameObject.transform.localPosition.x, ladge_01[i].gameObject.transform.localPosition.y - moveStep, ladge_01[i].gameObject.transform.localPosition.z);
            ladge_02[i].gameObject.transform.localPosition = new Vector3(ladge_02[i].gameObject.transform.localPosition.x, ladge_02[i].gameObject.transform.localPosition.y - moveStep, ladge_02[i].gameObject.transform.localPosition.z);
        }

        if (AllSlots[0, 0].gameObject.transform.localPosition.y < -1f)
        {
            reached = true;
        }

        return reached;
    }

    bool ClimbByCurrentStepLOSE_UP(float moveStep, float posTo)
    {
        bool reached = false;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                AllSlots[i, j].gameObject.transform.localPosition = new Vector3(AllSlots[i, j].gameObject.transform.localPosition.x, AllSlots[i, j].gameObject.transform.localPosition.y - moveStep, AllSlots[i, j].gameObject.transform.localPosition.z);

            }
        }

        for (int i = 0; i < caveObjects.Length; i++)
        {
            caveObjects[i].gameObject.transform.localPosition = new Vector3(caveObjects[i].gameObject.transform.localPosition.x, caveObjects[i].gameObject.transform.localPosition.y - moveStep, caveObjects[i].gameObject.transform.localPosition.z);
        }

        for (int i = 0; i < ladge_01.Length; i++)
        {
            ladge_01[i].gameObject.transform.localPosition = new Vector3(ladge_01[i].gameObject.transform.localPosition.x, ladge_01[i].gameObject.transform.localPosition.y - moveStep, ladge_01[i].gameObject.transform.localPosition.z);
            ladge_02[i].gameObject.transform.localPosition = new Vector3(ladge_02[i].gameObject.transform.localPosition.x, ladge_02[i].gameObject.transform.localPosition.y - moveStep, ladge_02[i].gameObject.transform.localPosition.z);
        }

        if (caveObjects[0].gameObject.transform.localPosition.y < posTo)
        {
            reached = true;
        }

        return reached;
    }

    bool ClimbByCurrentStepLOSE_DOWN(float moveStep)
    {
        bool reached = false;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                AllSlots[i, j].gameObject.transform.localPosition = new Vector3(AllSlots[i, j].gameObject.transform.localPosition.x, AllSlots[i, j].gameObject.transform.localPosition.y + moveStep, AllSlots[i, j].gameObject.transform.localPosition.z);

            }
        }

        for (int i = 0; i < caveObjects.Length; i++)
        {
            caveObjects[i].gameObject.transform.localPosition = new Vector3(caveObjects[i].gameObject.transform.localPosition.x, caveObjects[i].gameObject.transform.localPosition.y + moveStep, caveObjects[i].gameObject.transform.localPosition.z);
        }

        for (int i = 0; i < ladge_01.Length; i++)
        {
            ladge_01[i].gameObject.transform.localPosition = new Vector3(ladge_01[i].gameObject.transform.localPosition.x, ladge_01[i].gameObject.transform.localPosition.y + moveStep, ladge_01[i].gameObject.transform.localPosition.z);
            ladge_02[i].gameObject.transform.localPosition = new Vector3(ladge_02[i].gameObject.transform.localPosition.x, ladge_02[i].gameObject.transform.localPosition.y + moveStep, ladge_02[i].gameObject.transform.localPosition.z);
        }

        if (caveObjects[0].gameObject.transform.localPosition.y >= 6.4f)
        {
            reached = true;
        }

        return reached;
    }
  
    void RestetProcessArguments()
    {
        inProcess = false;

        StartWaitingingAnimation();

        if (currentCave != null)
        {
            currentCave.enabled = false;
            currentCave.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            currentCave.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        foreach (SpriteRenderer sp in ActiveCaves)
        {
            sp.enabled = false;
            sp.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            sp.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        ActiveCaves.Clear();

        if(stepNumber == 5)
        {
            Invoke("EndBonusGame", 1.1f);
        }
    }
   
    void EndBonusGame()
    {
        bonusGameController.EndBonusGame();
    }

    #region Properties

    SpriteRenderer[,] AllSlots
    {
        get
        {
            if (all_slots == null || all_slots.Length == 0)
            {
                int[] slotNameArray = new int[SlotsParent.transform.childCount];
                for (int i = 0; i < slotNameArray.Length; i++)
                {
                    slotNameArray[i] = Convert.ToInt32(SlotsParent.transform.GetChild(i).gameObject.name);
                }

                Array.Sort(slotNameArray);

                int linsIndex = 5;
                int columnIndex = slotNameArray.Length / 5;
                all_slots = new SpriteRenderer[columnIndex, linsIndex];

                //string testSTRING = "";

                for (int i = 0; i < columnIndex; i++)
                {
                    // testSTRING +="\n";
                    for (int j = 0; j < linsIndex; j++)
                    {
                        all_slots[i, j] = SlotsParent.transform.FindChild(slotNameArray[i * linsIndex + j].ToString()).gameObject.GetComponent<SpriteRenderer>();
                        //testSTRING += slotNameArray[i * linsIndex + j]+ "  ";
                    }
                }

                //Debug.Log(testSTRING);

                return all_slots;
            }
            else
            {
                return all_slots;
            }

            ;
        }
    }

    List<SpriteRenderer> ActiveCaves
    {
        get
        {
            if (activeCaves == null)
            {
                activeCaves = new List<SpriteRenderer>();
                return activeCaves;
            }
            else
            {
                return activeCaves;
            }

            ;
        }
    }

    #endregion
}
