using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupController : MonoBehaviour
{
    private Spawner spawner;
    private Playfield playfield;

    public float reflactTime = 0.16f;//time between left move, right move, active drop, rotate
    public float dropTime = 0.7f;//time for passive drop

    //record of last time
    private float lastReflactTime;
    private float lastDropTime;

    //blocks position
    [SerializeField]
    private const int arrLen = 4;
    private Vector2Int[] oldGroup;
    private Vector2Int oldOffset;
    private Vector2Int[] newGroup;
    private Vector2Int newOffset;

    public NextAction input;

    // Start is called before the first frame update
    void Start()
    {
        lastReflactTime = Time.time;
        lastDropTime = Time.time;

        oldGroup = new Vector2Int[arrLen];
        newGroup = new Vector2Int[arrLen];

        this.enabled = false;

        spawner = Manager.Instance.spawner;
        playfield = Manager.Instance.playfield;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;

        float reflactDeltaTime = time - lastReflactTime;
        if (reflactDeltaTime >= reflactTime && input.action != NextAction.Action.none)
        {
            //Debug.Log("Input");
            switch (input.action)
            {
                case NextAction.Action.left:
                    Move(-1, 0);
                    break;
                case NextAction.Action.right:
                    Move(1, 0);
                    break;
                case NextAction.Action.rotate:
                    Rotate();
                    break;
                case NextAction.Action.drop:
                    Move(0, -1);
                    break;
            }

            if (CheckNewPostion())//don't update when input isn't valid
                GroupUpdate();

            //reset action
            //Debug.Log("Reset");
            input.action = NextAction.Action.none;

            lastReflactTime = time;
        }

        //check drop time
        float dropDeltaTime = time - lastDropTime;
        if (dropDeltaTime >= dropTime)
        {
            //Debug.Log("Drop");
            Move(0, -1);
            CheckNewPostion();

            GroupUpdate();

            lastDropTime = time;
        }
    }

    public void GameStart()
    {
        //Debug.Log("Start");
        this.enabled = true;
        Spawn();
    }

    void GroupUpdate()
    {
        if (IsStaying())//doesn't move
        {
            //Debug.Log("Next");
            playfield.UpdateField();

            Spawn();
        }
        else
        {
            //Debug.Log("convert");

            //convert old to false
            foreach (Vector2Int block in oldGroup)
            {
                playfield.Convert(block.x + oldOffset.x, block.y + oldOffset.y, false);
            }

            //convert new to true
            foreach (Vector2Int block in newGroup)
            {
                playfield.Convert(block.x + newOffset.x, block.y + newOffset.y, true);
            }

            newGroup.CopyTo(oldGroup, 0);
            oldOffset = newOffset;
        }
    }

    private void Move(int x, int y)
    {
        //Debug.Log("Move");
        newOffset.x = oldOffset.x + x;
        newOffset.y = oldOffset.y + y;
    }

    [SerializeField]private const int rotateAngle = -90;
    private const float radian = rotateAngle * Mathf.Deg2Rad;
    private void Rotate()
    {//rotate 90 degree, clockwise
        //Debug.Log("Rotate");
        
        float cs = Mathf.Cos(radian);
        float sn = Mathf.Sin(radian);
        for (int i = 0; i < oldGroup.Length; i++)
        {
            int x = oldGroup[i].x;
            int y = oldGroup[i].y;
            
            int px = Mathf.RoundToInt(x * cs - y * sn);
            int py = Mathf.RoundToInt(x * sn + y * cs);

            newGroup[i] = new Vector2Int(px, py);
        }
    }

    private bool CheckNewPostion()
    {
        bool filled = false;
        for (int i = 0; i < newGroup.Length; i++)
        {
            //check if it's inside border
            if(!playfield.InsideBorder(newGroup[i].x + newOffset.x, 
                newGroup[i].y + newOffset.y))
            {
                filled = true;
                break;
            }

            //check if new position valid
            if (playfield.isBlockFilled[newGroup[i].x + newOffset.x,
                newGroup[i].y + newOffset.y])
            {//this block is already filled

                bool filledBySelf = false;
                //check if the block is filled by itself
                for (int j=0;j<oldGroup.Length;j++)
                {
                    if(oldGroup[j] + oldOffset == newGroup[i] + newOffset)
                    {
                        filledBySelf = true;
                        break;
                    }
                }

                if(!filledBySelf)//block is filled by other block, not valid postion
                {
                    filled = true;
                    break;
                }
            }
        }

        //Debug.Log("Check" + filled);

        if (filled)//not valid postion, revert
        {
            oldGroup.CopyTo(newGroup, 0);
            newOffset = oldOffset;
        }

        return !filled;
    }

    private void Spawn()
    {
        oldGroup = spawner.Spawn();
        oldOffset = spawner.spawnPoint;

        oldGroup.CopyTo(newGroup, 0);
        newOffset = oldOffset;

        if (!CheckNewPostion())
        {//not valid at spawn point, game over
            gameObject.SetActive(false);
        }
    }

    private bool IsStaying()
    {
        if (oldOffset != newOffset)
            return false;

        for(int i=0;i<arrLen;i++)
        {
            if(oldGroup[i] != newGroup[i])
            {
                return false;
            }
        }
        return true;
    }
}
