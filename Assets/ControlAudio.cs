using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Sonido {
    public string nombre;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volumen = 0.7f;

    [Range(0.5f, 1.5f)]
    public float GradoVolumen = 1f;

    [Range(0f, 0.5f)]
    public float variacionVolumen = 0.1f;

    [Range(0f, 0.5f)]
    public float variacionGrado = 0.1f;

    public bool loop = false;

    public AudioSource fuente;

    public void SetFunte(AudioSource _fuente) {
        fuente = _fuente;
        fuente.clip = clip;
        fuente.loop = loop;
    }

    public void Reproducir() {
        fuente.volume = volumen * (1 + Random.Range(-variacionVolumen / 2f, variacionVolumen / 2f));
        fuente.pitch = GradoVolumen * (1 + Random.Range(-variacionGrado / 2f, variacionGrado / 2f));
        fuente.Play();
    }

    public void Parar() {
        fuente.Stop();
    }

}

public class ControlAudio : MonoBehaviour {

    public static ControlAudio instancia;

    [SerializeField]
    Sonido[] sonidos;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            ReproducirSonido(NombresAudios.Dime_que_quieres_hacer);
        }
    }

    private void Start() {
        
        for (int a = 0; a < sonidos.Length; a++) {
            GameObject _GameOb = new GameObject("Sonido_" + a + "_" + sonidos[a].nombre);
            _GameOb.transform.SetParent(this.transform);
            sonidos[a].SetFunte(_GameOb.AddComponent<AudioSource>());
        }
    }

    public void ReproducirSonido(string _nombre) {
        PararSonidos();
        for (int a = 0; a < sonidos.Length; a++) {
            if (sonidos[a].nombre.Equals(_nombre)) {
                sonidos[a].Reproducir();
                return;
            }
        }

        //No se encontro el sonido con ese error
        Debug.LogWarning("ConttrolAudio.cs: No se ha encontrado el audio con el nombre: " + _nombre);
    }

    public void PararSonidos() {
        for (int a = 0; a < sonidos.Length; a++) {
            sonidos[a].Parar();
        }
    }

}
