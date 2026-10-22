using UnityEngine;

public class ComportamientoHumanoide : MonoBehaviour
{
  [Header("Configuración")]
  public int tipoDeEsfera; 
  public float velocidad = 3f;

  [Header("Objetivos")]
  public Transform objetivoEsferaVerde; 
  public Transform objetivoCilindro;

  private bool deboMoverme = false;

  private void OnEnable()
  {
    CilindroEventos.AlColisionarConCubo += ActivarMovimiento;
  }

  private void OnDisable()
  {
    CilindroEventos.AlColisionarConCubo -= ActivarMovimiento;
  }

  private void ActivarMovimiento()
  {
    deboMoverme = true; 
  }


  private void Update()
  {
    if (deboMoverme)
    {
      Transform destinoFinal = null;
      if (tipoDeEsfera == 1)
      {
        destinoFinal = objetivoEsferaVerde;
      }
      else if (tipoDeEsfera == 2)
      {
        destinoFinal = objetivoCilindro;
      }

      if (destinoFinal != null)
      {
        transform.position = Vector3.MoveTowards(transform.position, destinoFinal.position, velocidad * Time.deltaTime);
      }
    }
  }
}