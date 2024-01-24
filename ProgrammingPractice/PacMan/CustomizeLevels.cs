using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HackMan_GD07;
using UnityEngine.UI;
public class CustomizeLevels : LevelGeneratorSystem
{
    IntVector2 InputDirection;
    IntVector2 currentPos=new IntVector2(1, 1);
    public InputField InputField;
    int lastGridType;
    MyLevel newLevel;
    string levelName;
    bool isEditing = true;
    int[,] WhiteBoxGrid = new int[,]
         {{1,1, 1, 1,1,  1, 1,1,1},
         {1, 1, 1, 1, 1,  1, 1,1,1},
         {1, 1, 1, 1, 1,  1, 1,1,1},
         {1, 1, 1, 1, 1,  1, 1,1,1},
         {1, 1, 1, 1, 1,  1, 1,1,1},
         {1, 1, 1, 1, 1,  1, 1, 1,1}};
   protected override void OnEnable()
    {
        isEditing = true;
        InputField.gameObject.SetActive(false);
        Time.timeScale = 0;
        Grid = WhiteBoxGrid;
        var gridSizeY = WhiteBoxGrid.GetLength(0);
        var gridSizeX = WhiteBoxGrid.GetLength(1);
        FillLevelWithGrid();
    }

    protected override void Update()
    {
        if (!isEditing) return;
        if (Input.GetKeyDown(KeyCode.DownArrow) ||
            (Input.GetKeyDown(KeyCode.UpArrow)) ||
            (Input.GetKeyDown(KeyCode.LeftArrow)) ||
            (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                InputDirection = new IntVector2(0, -1);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                InputDirection = new IntVector2(0, 1);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                InputDirection = new IntVector2(-1, 0);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                InputDirection = new IntVector2(1, 0);
            }

            if (currentPos.x - InputDirection.y > 0
           && currentPos.x - InputDirection.y < WhiteBoxGrid.GetLength(0)-1
               && currentPos.y + InputDirection.x > 0
                && currentPos.y + InputDirection.x < WhiteBoxGrid.GetLength(1)-1
                )
            {
                currentPos.x -= InputDirection.y;
                currentPos.y += InputDirection.x;

                lastGridType = Grid[currentPos.x, currentPos.y];
                Grid[currentPos.x,currentPos.y] = 4;
                FillLevelWithGrid();
                Grid[currentPos.x, currentPos.y] = lastGridType;
                // Grid[currentPos.x, currentPos.y] = 1;
                //Debug.Log($"{currentPos.x}, {currentPos.y}");
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha0)|| Input.GetKeyDown(KeyCode.Alpha1 )|| (Input.GetKeyDown(KeyCode.Alpha2)) || (Input.GetKeyDown(KeyCode.Alpha3)))
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Grid[currentPos.x, currentPos.y] = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Grid[currentPos.x, currentPos.y] = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Grid[currentPos.x, currentPos.y] = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Grid[currentPos.x, currentPos.y] = 3;
            }
            FillLevelWithGrid();
        }
    }
    private void OnDisable()
    {
        Destroy(lastLevelContainer);
        //Time.timeScale = 1;
    }
    [ContextMenu("My Save Level")]
    public void MySaveLevel()
    {
        Levels.Clear();
        newLevel = new MyLevel(Grid);
        InputField.gameObject.SetActive(true);
        InputField.onEndEdit.AddListener(OnEndEdit);
        isEditing = false;
    }
    void OnEndEdit(string text)
    {
        levelName = text;
        AppDataSystem.Save<MyLevel>(newLevel, text);
        InputField.gameObject.SetActive(false);
        Debug.Log($"Saved {levelName} as {newLevel}");
    }
}