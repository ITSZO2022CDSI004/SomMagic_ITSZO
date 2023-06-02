using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenas : MonoBehaviour
{
    public GameObject[] view;
    public static int ARview = 0;
    public static int Mapaview = 0;
    public static int Menuview = 0;
    public static int Geoview = 0;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "AR") View(ARview);
        if (SceneManager.GetActiveScene().name == "Mapa") View(Mapaview);
        if (SceneManager.GetActiveScene().name == "MenuIntegrado") View(Menuview);
        if (SceneManager.GetActiveScene().name == "4_Geoposcionamiento") View(Geoview);
    }
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    public void View(int i)
    {
        view[i].SetActive(true);
        for (int j = 0; j < view.Length; j++)
        {
            if (i == j) continue;
            view[j].SetActive(false);
        }
    }
    public void Salir()
    {
        Application.Quit();
    }

    public void cargarEscena(string i)
    {
        SceneManager.LoadScene(i);
    }

    public void setARValue(int i)
    {
        ARview = i;
    }
    public void setMapaValue(int i)
    {
        Mapaview = i;
    }
    public void setMenuValue(int i)
    {
        Menuview = i;
    }
    public void setGeoValue(int i)
    {
        Geoview = i;
    }
}
