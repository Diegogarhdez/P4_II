using UnityEngine;

public class ComportamientoHumanoide1 : MonoBehaviour {
  [Header("Configuración")]
  [Range(1, 2)]
  public int tipoDeGrupo = 1;
  public float velocidad = 3f;

  [Header("Objetivos del Grupo 1")]
  public Transform escudoSeleccionado;
  public Transform[] escudosDelGrupo2;

  [Header("Referencia del cubo")]
  public Transform escudoObjetivoTeleport;
  public Transform objetivoOrientacionGrupo2;

  private Transform destinoActual = null;
  private bool deboMoverme = false;

  private void OnEnable() {
    CuboEventos.AlChocarConHumanoide2 += IrAEscudoSeleccionado;
    CuboEventos.AlChocarConHumanoide1 += IrAEscudoGrupo2;
    ReferenciaCuboEventos.AlActivarse += ResponderAlContactoConReferencia;
  }

  private void OnDisable() {
    CuboEventos.AlChocarConHumanoide2 -= IrAEscudoSeleccionado;
    CuboEventos.AlChocarConHumanoide1 -= IrAEscudoGrupo2;
    ReferenciaCuboEventos.AlActivarse -= ResponderAlContactoConReferencia;
  }

  private void ResponderAlContactoConReferencia() {
    if (tipoDeGrupo == 1) {
      if (escudoObjetivoTeleport == null) {
        Debug.LogWarning($"{name}: asigna escudoObjetivoTeleport en el Inspector.", this);
        return;
      }

      transform.position = escudoObjetivoTeleport.position;
      destinoActual = null;
      deboMoverme = false;
      return;
    }

    if (tipoDeGrupo == 2) {
      if (objetivoOrientacionGrupo2 == null) {
        Debug.LogWarning($"{name}: asigna objetivoOrientacionGrupo2 en el Inspector.", this);
        return;
      }

      Vector3 direccion = objetivoOrientacionGrupo2.position - transform.position;
      direccion.y = 0f;
      if (direccion.sqrMagnitude > 0.0001f) {
        transform.rotation = Quaternion.LookRotation(direccion, Vector3.up);
      }
    }
  }

  private void IrAEscudoSeleccionado() {
    if (tipoDeGrupo == 1) {
      EstablecerDestino(escudoSeleccionado, "escudoSeleccionado");
    }
  }

  private void IrAEscudoGrupo2() {
    if (tipoDeGrupo == 1) {
      Transform escudoMasCercano = null;
      float distanciaMasCercana = float.PositiveInfinity;

      if (escudosDelGrupo2 != null) {
        foreach (Transform escudo in escudosDelGrupo2) {
          if (escudo == null) {
            continue;
          }

          float distancia = (escudo.position - transform.position).sqrMagnitude;
          if (distancia < distanciaMasCercana) {
            distanciaMasCercana = distancia;
            escudoMasCercano = escudo;
          }
        }
      }

      EstablecerDestino(escudoMasCercano, "escudosDelGrupo2");
    }
  }

  private void EstablecerDestino(Transform destino, string nombreDelCampo) {
    if (destino == null) {
      deboMoverme = false;
      Debug.LogWarning($"{name}: asigna {nombreDelCampo} en el Inspector para que el humanoide pueda moverse.", this);
      return;
    }

    destinoActual = destino;
    deboMoverme = true;
  }

  private void Update() {
    if (!deboMoverme || destinoActual == null) {
      return;
    }

    transform.position = Vector3.MoveTowards(
      transform.position,
      destinoActual.position,
      velocidad * Time.deltaTime
    );

    if (transform.position == destinoActual.position) {
      deboMoverme = false;
    }
  }

  public void CambiarColorAlColisionarConEscudo() {
    Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

    if (renderers.Length == 0) {
      Debug.LogWarning($"{name}: no se encontraron Renderer para cambiar el color del humanoide.", this);
      return;
    }

    foreach (Renderer renderer in renderers) {
      Material[] materiales = renderer.materials;
      foreach (Material material in materiales) {
        if (material.HasProperty("_BaseColor")) {
          material.SetColor("_BaseColor", Color.blue);
        }
        if (material.HasProperty("_Color")) {
          material.SetColor("_Color", Color.blue);
        }
      }
      renderer.materials = materiales;
    }

    Debug.Log($"{name} cambió de color al tocar un escudo.", this);
  }
}