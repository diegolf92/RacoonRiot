using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class MenuOpciones : MonoBehaviour

{
    [SerializeField]private AudioMixer audioMixer;


    public void cambiarVolumen(float volumen)
    {
        audioMixer.SetFloat("Volumen", volumen);
    }
   
   
}
