using System;

[Serializable]
public class GameDatas {

    public int BestScore { get; set; }
    public int LevelCount { get; set; }
    public int PlatformCount { get; set; }

    public GameDatas()
    {
        BestScore = 0;
        LevelCount = 1;
        PlatformCount = 20;
    }
}
