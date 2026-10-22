using UnityEngine;
using System;

public class CilindroEventos : MonoBehaviour
{
  public delegate void AccionColision();
  public static event AccionColision AlColisionarConCubo;
  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag("Cubo"))
    {
      Debug.Log("¡Cubo tocó el cilindro! Lanzando evento...");
      AlColisionarConCubo?.Invoke();
    }
  }
}