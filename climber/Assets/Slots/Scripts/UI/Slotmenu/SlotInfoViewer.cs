using UnityEngine;

public class SlotInfoViewer : MonoBehaviour
{

    private const string SlotId = "fruit";
    private Texture[] _textures;
    private int _currentPage;
    public GameObject InfoContainer;

	// Use this for initialization
	void Start ()
	{
        //TODO temporary path for existing textures 
        string texturesPath = string.Format("files/{0}/info", SlotId);
        _textures = Resources.LoadAll<Texture>(texturesPath);
        Debug.Log(_textures.Length + " textures");
	}

    void OnClick()
    {
        Debug.Log(_currentPage);
        if (_currentPage == _textures.Length)
        {
            InfoContainer.SetActive(false);
            _currentPage = 0;
        }
        else
        {
            InfoContainer.SetActive(true);
            InfoContainer.GetComponent<UITexture>().mainTexture = _textures[_currentPage];
            _currentPage++;
        }
    }
}
