using System;
using UnityEngine;

public class ReferenciaCuboEventos : MonoBehaviour {
  public static event Action AlActivarse;

  private bool activada;

  private void OnTriggerEnter(Collider other) {
    if (activada || other.GetComponentInParent<CuboEventos>() == null) {
      return;
    }

    activada = true;
    Debug.Log("El cubo tocó el objeto de referencia. Activando la mecánica.", this);
    AlActivarse?.Invoke();
  }
}
