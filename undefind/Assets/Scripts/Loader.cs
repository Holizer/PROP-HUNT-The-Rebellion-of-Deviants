using UnityEngine.SceneManagement;
public static class Loader
{
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
public enum Scene
{
    Menu,
    RoomList,
    Lobby,
    GameMap
}