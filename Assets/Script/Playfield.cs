using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public int w;//width
    public int h;//height
    public Color fillColor;//color for fill block
    public int size;

    //reference for block
    public GameObject block;
    private Color originalColor;

    public GameObject[,] field { get; private set; }
    public bool[,] isBlockFilled { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        originalColor = block.GetComponent<SpriteRenderer>().color;

        Reset();
    }

    public void Reset()
    {
        field = new GameObject[w, h];
        isBlockFilled = new bool[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                field[x, y] = Instantiate(block);
                field[x, y].transform.position = new Vector3(Mathf.Round(x * size), Mathf.Round(y * size), 0);
                field[x, y].transform.SetParent(this.transform);

                isBlockFilled[x, y] = false;
            }
        }
    }

    public void UpdateField()
    {
        deleteFullRows();
    }

    public void Convert(int x, int y, bool fill)
    {
        //if (isBlockFilled[x, y] == fill)
        //    Debug.Log("Should not convert.");

        isBlockFilled[x, y] = fill;
        field[x, y].GetComponent<SpriteRenderer>().color = fill ? fillColor : originalColor;
    }

    public bool InsideBorder(int x, int y)
    {
        return (x >= 0 &&
                x < w &&
                y >= 0 &&
                y < h);
    }

    private void deleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            Convert(x, y, false);
        }
    }

    private void decreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (isBlockFilled[x, y])
            {
                Convert(x, y, false);//empty current block
                Convert(x, y - 1, true);//fill the block below it
            }
        }
    }

    private void decreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
            decreaseRow(i);
    }

    private bool isRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
            if (!isBlockFilled[x, y])
                return false;
        return true;
    }

    private void deleteFullRows()
    {
        for (int y = 0; y < h; ++y)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                decreaseRowsAbove(y + 1);
                y--;
            }
        }
    }
}
