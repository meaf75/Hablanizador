using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContradorGeneral : MonoBehaviour {
    public TextMeshProUGUI txtEstado;

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

    IEnumerator mensajeInicial() {
        EstadoBoton(false);
        yield return new WaitForSeconds(1);
        controlAudio.ReproducirSonido(NombresAudios.Dime_que_quieres_hacer);
        yield return new WaitForSeconds(2);
        EstadoBoton(true);
    }

    public void Grabar() {
        controlAudio.ReproducirSonido(NombresAudios.Grabando);
        PanelListoProcesar.SetActive(false);
        Grabando = true;
        txtEstado.SetText("[Grabando]");
    }

    public void DejarGrabar() {
        if (Grabando) {
            controlAudio.ReproducirSonido(NombresAudios.Añadiendo_tarea);
            Grabando = false;
            StartCoroutine(GuardarGrabacion());
        }
    }

    IEnumerator GuardarGrabacion() {

        EstadoBoton(false);

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
        AsignarTexto("Procesando modelo UML a " + lenguaje,Amarillo);
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
}
