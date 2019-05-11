using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelLenguajes : MonoBehaviour {

    public GameObject PanelLista_lenguajes;

    public void ActivarLista() {
        StartCoroutine(Activar_Elementos());
    }

    IEnumerator Activar_Elementos() {
        foreach (Transform item in PanelLista_lenguajes.transform) {
            item.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
