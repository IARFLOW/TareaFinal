using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Config", order = 1)]
public class GameConfig : ScriptableObject
{
    [System.Serializable]
    public class LevelInfo
    {
        public string levelName;     // Display name
        public string sceneName;     // Actual scene name
        public float timeLimit = 60f; // Time limit in seconds (if using countdown)
        public int totalCoins = 0;   // Total coins in the level
    }

    [Header("Level Information")]
    public List<LevelInfo> levels = new List<LevelInfo>();

    [Header("Game Settings")]
    public int initialLives = 3;
    public bool useCountdownTimer = true;
    public bool showLevelResults = true;

    [Header("Audio Settings")]
    public float masterVolume = 1f;
    public float musicVolume = 0.7f;
    public float sfxVolume = 1f;

    // Find level info by scene name
    public LevelInfo GetLevelInfo(string sceneName)
    {
        return levels.Find(l => l.sceneName == sceneName);
    }

    // Get level index by scene name
    public int GetLevelIndex(string sceneName)
    {
        return levels.FindIndex(l => l.sceneName == sceneName);
    }

    // Get next level scene name
    public string GetNextLevelScene(string currentSceneName)
    {
        int currentIndex = GetLevelIndex(currentSceneName);

        if (currentIndex >= 0 && currentIndex < levels.Count - 1)
        {
            return levels[currentIndex + 1].sceneName;
        }

        // If no next level or not found, return final scene
        return "NivelFinal";
    }
}