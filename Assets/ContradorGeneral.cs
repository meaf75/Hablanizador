using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;

public class ContradorGeneral : MonoBehaviour {
    public AudioSource audioSource;
    //temporary audio vector we write to every second while recording is enabled..
    List<float> tempRecording = new List<float>();
    public TextMeshProUGUI txtEstado;
    //list of recorded clips...
    List<float[]> recordedClips = new List<float[]>();

    public Button BtnGrabar;
    public Button BtnProcesarUML;
    public ControlAudio controlAudio;

    [Header("Paneles")]
    public GameObject PanelListoProcesar;
    public GameObject PanelLenguajes;
    public GameObject PanelFin;

    [Header("Colores")]
    public Color Verde;
    public Color Blanco;
    public Color Amarillo;

    bool Grabando = false;

    private void Start() {
        StartCoroutine(mensajeInicial());
    }
    bool wasRecording = false;

    void Update() {
        // if (!Grabando && wasRecording)
        // {
        //     wasRecording = false;

        //     //stop recording, get length, create a new array of samples
        //     int length = Microphone.GetPosition(null);

        //     Microphone.End(null);
        //     float[] clipData = new float[length];
        //     audioSource.clip.GetData(clipData, 0);
        //     //create a larger vector that will have enough space to hold our temporary
        //     //recording, and the last section of the current recording
        //     float[] fullClip = new float[clipData.Length + tempRecording.Count];
        //     for (int i = 0; i < fullClip.Length; i++)
        //     {
        //         //write data all recorded data to fullCLip vector
        //         if (i < tempRecording.Count)
        //             fullClip[i] = tempRecording[i];
        //         else
        //             fullClip[i] = clipData[i - tempRecording.Count];
        //     }

        //     recordedClips.Add(fullClip);
        //     audioSource.clip = AudioClip.Create("recorded samples", fullClip.Length, 1, 44100, false);
        //     audioSource.clip.SetData(fullClip, 0);
        //     audioSource.loop = true; 
        //     string dir; 
        //     if (Application.platform == RuntimePlatform.Android){
        //         dir = Application.persistentDataPath;
        //     }else{
        //         dir = Application.dataPath;
        //     }
        //     if (Directory.Exists(Path.Combine(dir, "clips"))) {
        //         Directory.CreateDirectory(Path.Combine(dir, "clips"));
        //     }
        //     var filepath = Path.Combine(dir, "clips/grabacion"+DateTime.Now.Ticks+".wav");
        //     SavWav.Save(filepath, audioSource.clip );

        // }else if(Grabando){
        //     print("Grabando");
        //     //stop audio playback and start new recording...
        //     audioSource.Stop();
        //     tempRecording.Clear();
        //     Microphone.End(null);
        //     audioSource.clip = Microphone.Start(null, true, 1, 44100);
        //     Invoke("ResizeRecording", 1);
        // }

    }

    IEnumerator mensajeInicial() {
        EstadoBoton(false);
        yield return new WaitForSeconds(1);
        controlAudio.ReproducirSonido(NombresAudios.Dime_que_quieres_hacer);
        yield return new WaitForSeconds(2);
        EstadoBoton(true);
    }

    public void Grabar() {

        //stop audio playback and start new recording...
        audioSource.Stop();
        tempRecording.Clear();
        Microphone.End(null);
        audioSource.clip = Microphone.Start(null, true, 1, 44100);
        Invoke("ResizeRecording", 1);

        controlAudio.ReproducirSonido(NombresAudios.Grabando);
        PanelListoProcesar.SetActive(false);
        Grabando = true;
        txtEstado.SetText("[Grabando]");
    }

    public void DejarGrabar() {
        if (Grabando) {
            controlAudio.ReproducirSonido(NombresAudios.Añadiendo_tarea);
            Grabando = false;
            wasRecording = true;
            StartCoroutine(GuardarGrabacion());
        }
    }

    IEnumerator GuardarGrabacion() {

        EstadoBoton(false);

        guardarArchivo();

        for (int i = 0; i < 5; i++) {
            AsignarTexto("Procesando grabacion.", Amarillo);
            yield return new WaitForSeconds(0.1f);
            AsignarTexto("Procesando grabacion..", Amarillo);
            yield return new WaitForSeconds(0.1f);
            AsignarTexto("Procesando grabacion...", Amarillo);
            yield return new WaitForSeconds(0.1f);
        }

        AsignarTexto("Grabacion procesada ^_^", Verde);
        yield return new WaitForSeconds(2);

        AsignarTexto("Dime que deseas hacer", Blanco);
        controlAudio.ReproducirSonido(NombresAudios.Dime_que_quieres_hacer);
        EstadoBoton(true);
        PanelListoProcesar.SetActive(true);
    }

    void guardarArchivo() {
        //stop recording, get length, create a new array of samples
        int length = Microphone.GetPosition(null);

        Microphone.End(null);
        float[] clipData = new float[length];
        audioSource.clip.GetData(clipData, 0);
        //create a larger vector that will have enough space to hold our temporary
        //recording, and the last section of the current recording
        float[] fullClip = new float[clipData.Length + tempRecording.Count];
        for (int i = 0; i < fullClip.Length; i++) {
            //write data all recorded data to fullCLip vector
            if (i < tempRecording.Count)
                fullClip[i] = tempRecording[i];
            else
                fullClip[i] = clipData[i - tempRecording.Count];
        }

        recordedClips.Add(fullClip);
        audioSource.clip = AudioClip.Create("recorded samples", fullClip.Length, 1, 44100, false);
        audioSource.clip.SetData(fullClip, 0);
        audioSource.loop = true;
        string dir;
        if (Application.platform == RuntimePlatform.Android) {
            dir = Application.persistentDataPath;
        } else {
            dir = Application.dataPath;
        }
        if (Directory.Exists(Path.Combine(dir, "clips"))) {
            Directory.CreateDirectory(Path.Combine(dir, "clips"));
        }
        var filepath = Path.Combine(dir, "clips/grabacion" + DateTime.Now.Ticks + ".wav");
        SavWav.Save(filepath, audioSource.clip);

    }

    void AsignarTexto(string texto, Color color) {
        txtEstado.SetText(texto);
        txtEstado.color = color;
    }

    public void ProcesarModeloUml() {
        BtnProcesarUML.interactable = false;
        StartCoroutine(procesarUml());
    }

    IEnumerator procesarUml() {
        EstadoBoton(false);
        controlAudio.ReproducirSonido(NombresAudios.Añadiendo_tarea);
        for (int i = 0; i < 3; i++) {
            AsignarTexto("Procesando grabacion.", Amarillo);
            yield return new WaitForSeconds(0.1f);
            AsignarTexto("Procesando grabacion..", Amarillo);
            yield return new WaitForSeconds(0.1f);
            AsignarTexto("Procesando grabacion...", Amarillo);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);
        controlAudio.ReproducirSonido(NombresAudios.Creando_actuante);
        yield return new WaitForSeconds(3);
        controlAudio.ReproducirSonido(NombresAudios.Creando_modelo_uml);
        for (int i = 0; i < 3; i++) {
            AsignarTexto("Procesando modelo uml.", Amarillo);
            yield return new WaitForSeconds(0.1f);
            AsignarTexto("Procesando modelo uml..", Amarillo);
            yield return new WaitForSeconds(0.1f);
            AsignarTexto("Procesando modelo uml...", Amarillo);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(3);
        AsignarTexto("Modelo UML procesado", Verde);
        yield return new WaitForSeconds(2);
        ActivarPanelLenguajes();
    }

    void ActivarPanelLenguajes() {
        BtnGrabar.gameObject.SetActive(false);
        PanelListoProcesar.SetActive(false);
        PanelLenguajes.SetActive(true);
    }

    public void PasarUMLLenguaje(string lenguaje) {
        AsignarTexto("Procesando modelo UML a " + lenguaje, Amarillo);
        StartCoroutine(ActivarPanelFin());
    }

    public void Reiniciar() {
        SceneManager.LoadScene("SampleScene");
    }

    public void SeguirViendo() {
        PanelFin.SetActive(false);
    }

    public void Salir() {
        Application.Quit();
    }

    IEnumerator ActivarPanelFin() {
        yield return new WaitForSeconds(2);
        PanelFin.SetActive(true);
    }

    void EstadoBoton(bool estado) {
        Image BtnImg = BtnGrabar.GetComponent<Image>();
        BtnImg.raycastTarget = estado;
        BtnGrabar.interactable = estado;
    }


    void ResizeRecording() {
        if (Grabando) {
            //add the next second of recorded audio to temp vector
            int length = 44100;
            float[] clipData = new float[length];
            audioSource.clip.GetData(clipData, 0);
            tempRecording.AddRange(clipData);
            Invoke("ResizeRecording", 1);
        }
    }

}
