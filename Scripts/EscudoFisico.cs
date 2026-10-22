using UnityEngine;

public class EscudoFisico : MonoBehaviour {
  private void OnCollisionEnter(Collision collision) {
    if (!CompareTag("Escudo2")) {
      return;
    }

    ComportamientoHumanoide1 humanoide =
      collision.collider.GetComponentInParent<ComportamientoHumanoide1>();

    if (humanoide != null) {
      humanoide.CambiarColorAlColisionarConEscudo();
    }
  }
}