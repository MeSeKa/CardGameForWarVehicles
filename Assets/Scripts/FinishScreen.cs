using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScreen : MonoBehaviour
{
	public TextMeshProUGUI textMeshProUGUI;

	public void SetFinishText(string text)
	{
		textMeshProUGUI.text = text;
	}

	public void RestartTheGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
