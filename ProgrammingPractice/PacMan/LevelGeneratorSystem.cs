using UnityEngine;
using HackMan_GD07;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace HackMan_GD07
{
    public class LevelGeneratorSystem : MonoBehaviour
    {
        public List<MyLevel> Levels = new List<MyLevel>();
        public BaseGridObject[] BaseGridObjectPrefabs;
        protected GameObject lastLevelContainer;
        public static int[,] Grid = new int[,] { };
        int oldRand = 0;
        int rand = 0; 
        protected virtual void OnEnable()
        {
            Time.timeScale = 1;
        }
        protected virtual void Update()
        {
            if (GameOverSystem.IsGameOver)
            {
                    RandomlyChooseALevel();
                    FillLevelWithGrid();
                GameOverSystem.IsGameOver = false;
            }
        }

        public void FillLevelWithGrid()
        {
            if (lastLevelContainer)
            {
                Destroy(lastLevelContainer);
            }
            GameObject LevelContainer = new GameObject("LevelContainer");
            var gridSizeY = Grid.GetLength(0);
            var gridSizeX = Grid.GetLength(1);
            bool hasPlayer=false;
            for (var y = 0; y < gridSizeY; y++)
            {
                for (var x = 0; x < gridSizeX; x++)
                {
                    var objectType = Grid[y, x];
                    if(objectType==2)
                    {
                        if(hasPlayer)
                        {
                            objectType = 0;
                        }
                        else
                        {
                            hasPlayer = true;
                        }
                    }
                    var gridObjectPrefab = BaseGridObjectPrefabs[objectType];
                    var gridObjectClone = Instantiate(gridObjectPrefab);
                    gridObjectClone.GridPosition = new IntVector2(x, -y);
                     gridObjectClone.transform.SetParent(LevelContainer.transform);
                    gridObjectClone.transform.localPosition = new Vector3(gridObjectClone.GridPosition.x, gridObjectClone.GridPosition.y, 0);
                    LevelContainer.transform.SetParent(this.transform);
                    lastLevelContainer = LevelContainer;
                }
            }

            //Reset  NumOfCollectable
            CollectorComponent.NumOfCollectable = 0;
            for (int i = 0; i < LevelContainer.transform.childCount; i++)
            {
                var obj = LevelContainer.transform.GetChild(i);
                if (obj.GetComponent<CollactableComponent>())
                {
                    CollectorComponent.NumOfCollectable++;
                }
            }
        }

        public void RandomlyChooseALevel()
        {
            Levels.Clear();
            Levels = AppDataSystem.LoadAll<MyLevel>();
            while (oldRand == rand)
            {
                rand = Random.Range(0, Levels.Count);
            }
            Grid = Levels[rand].Grid;
            oldRand = rand;
            Debug.Log($"CurrentLevel : Level_{rand}");
        }



        //[ContextMenu("Save Level")]
        //private void SaveLevel()
        //{
        //    var savedGrid = JsonConvert.SerializeObject(Grid);
        //    var directoryPath = $"{Application.dataPath}/StreamingAssets/Levels";

        //    if (!Directory.Exists(directoryPath))
        //    {
        //        Directory.CreateDirectory(directoryPath);
        //    }
        //    var fullFilePath = $"{Application.dataPath}/StreamingAssets/Levels/Level_0.json";

        //    if (!File.Exists(fullFilePath))
        //    {
        //        var fileStream = File.Create(fullFilePath);
        //        fileStream.Close();
        //    }
        //    File.WriteAllText(fullFilePath, savedGrid);
        //    Debug.Log($"saved level: {savedGrid}");
        //}

        //[ContextMenu("Log Grid")]
        //private void LogGrid()
        //{
        //    var obj = JsonConvert.SerializeObject(Grid);
        //    Debug.Log(obj);
        //}

        //HW:Finish save and Load for AppDataSystem
        //Make 10 different levels
        //LevelGenerator should pick a random level from the 10
        //Bonus Points -> Create a LoadAll method in the AppDataSystem
        //When the game is run ,won, or lost,user should see a UI message telling them what happened ,and have a button to click to play another level

        //string fullFilePath =$"{Application.dataPath}/StreamingAssets/Levels/Level_1.json";
        //0=pill,1=wall,2=hackman,3=ghost
        //4 Features:
        //1.DamageSystem&ExperienceSystem
        //2.Portal:Can transport to another level with current position and transport back.The system will check if there's a place with the same position, if not ,fail to transport.
        //3.Record the score and ranking function
        //4.The player can make custom levels and save it.
        //Send Chris email
    }
}