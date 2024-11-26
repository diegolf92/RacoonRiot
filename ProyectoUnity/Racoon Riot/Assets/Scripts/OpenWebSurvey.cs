using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWebSurvey : MonoBehaviour
{
    public string direccionweb;

    public void abrirdireccionweb()
    {
        Application.OpenURL(direccionweb);
    }
        


}
