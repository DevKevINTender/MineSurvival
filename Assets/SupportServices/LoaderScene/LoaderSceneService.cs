using UnityEngine.SceneManagement;

public interface ILoaderSceneService: IService
{

}

public class LoaderSceneService : ILoaderSceneService
{
    public static LoaderSceneService Instance { get; private set; }
    private GameScenes _bufScene;

    public void ActivateService()
    {
        Instance = this;
    }

    public void LoadBufScene()
	{
		SceneManager.LoadScene((int)_bufScene);
	}

	public void SetBufScene(GameScenes gameScenes)
	{
        _bufScene = gameScenes;
    }

    public void DeactivateService()
    {

    }
}

public enum GameScenes
{
    MenuScene,
	SessionScene = 0,
}
