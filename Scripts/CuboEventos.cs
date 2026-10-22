using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CuboEventos : MonoBehaviour {
  public static event Action AlChocarConHumanoide2;
  public static event Action AlChocarConHumanoide1;

  public int PuntuacionJugador { get; private set; }
  public int RecompensasObtenidas { get; private set; }

  [SerializeField]
  private Text textoPuntuacion;

  private readonly HashSet<GameObject> escudosRecogidos = new HashSet<GameObject>();

  private void Awake() {
    PuntuacionJugador = 0;
    RecompensasObtenidas = 0;
    escudosRecogidos.Clear();
    CrearInterfazPuntuacionSiHaceFalta();
    ActualizarTextoPuntuacion();
    Debug.Log($"Puntuación inicial: {PuntuacionJugador}", this);
  }

  private void OnCollisionEnter(Collision collision) {
    Debug.Log($"Cubo colisionó con {collision.collider.name} (tag: {collision.collider.tag}).", this);
    RecogerEscudo(collision.collider);

    ComportamientoHumanoide1 humanoide =
      collision.collider.GetComponentInParent<ComportamientoHumanoide1>();

    if (humanoide != null) {
      if (humanoide.tipoDeGrupo == 2) {
        NotificarColisionConHumanoide2();
      }
      else if (humanoide.tipoDeGrupo == 1) {
        NotificarColisionConHumanoide1();
      }

      return;
    }

    GameObject objetoColisionado = collision.collider.gameObject;
    Transform raizColisionada = collision.transform.root;

    if (objetoColisionado.CompareTag("tag2") || raizColisionada.CompareTag("tag2")) {
      NotificarColisionConHumanoide2();
    }
    else if (objetoColisionado.CompareTag("tag1") || raizColisionada.CompareTag("tag1")) {
      NotificarColisionConHumanoide1();
    }
  }

  private void OnTriggerEnter(Collider other) {
    Debug.Log($"Cubo activó el trigger de {other.name} (tag: {other.tag}).", this);
    RecogerEscudo(other);
  }

  private void RecogerEscudo(Collider collider) {
    Transform actual = collider.transform;

    while (actual != null) {
      int puntos = 0;

      if (actual.CompareTag("Escudo1")) {
        puntos = 5;
      }
      else if (actual.CompareTag("Escudo2")) {
        puntos = 10;
      }

      if (puntos > 0) {
        if (!escudosRecogidos.Add(actual.gameObject)) {
          return;
        }

        PuntuacionJugador += puntos;
        int recompensasAnteriores = RecompensasObtenidas;
        RecompensasObtenidas = PuntuacionJugador / 100;
        ActualizarTextoPuntuacion();
        Debug.Log($"Escudo tipo {puntos / 5} recogido: +{puntos} puntos. Puntuación total: {PuntuacionJugador}", this);
        for (int i = recompensasAnteriores; i < RecompensasObtenidas; i++) {
          Debug.Log($"¡Recompensa obtenida por alcanzar {(i + 1) * 100} puntos!", this);
        }
        Destroy(actual.gameObject);
        return;
      }

      actual = actual.parent;
    }
  }

  private void CrearInterfazPuntuacionSiHaceFalta() {
    if (textoPuntuacion != null) {
      return;
    }

    GameObject canvasObject = new GameObject("CanvasPuntuacion");
    Canvas canvas = canvasObject.AddComponent<Canvas>();
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    canvasObject.AddComponent<GraphicRaycaster>();

    GameObject panelObject = new GameObject("PanelPuntuacion", typeof(RectTransform), typeof(Image));
    panelObject.transform.SetParent(canvasObject.transform, false);

    RectTransform panel = panelObject.GetComponent<RectTransform>();
    panel.anchorMin = new Vector2(0f, 1f);
    panel.anchorMax = new Vector2(0f, 1f);
    panel.pivot = new Vector2(0f, 1f);
    panel.anchoredPosition = new Vector2(20f, -20f);
    panel.sizeDelta = new Vector2(360f, 100f);

    Image panelImage = panelObject.GetComponent<Image>();
    panelImage.color = new Color(0f, 0f, 0f, 0.7f);
    panelImage.raycastTarget = false;

    GameObject textObject = new GameObject("TextoPuntuacion", typeof(RectTransform), typeof(Text));
    textObject.transform.SetParent(panelObject.transform, false);

    RectTransform textRect = textObject.GetComponent<RectTransform>();
    textRect.anchorMin = Vector2.zero;
    textRect.anchorMax = Vector2.one;
    textRect.offsetMin = new Vector2(16f, 8f);
    textRect.offsetMax = new Vector2(-16f, -8f);

    textoPuntuacion = textObject.GetComponent<Text>();
    textoPuntuacion.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    if (textoPuntuacion.font == null) {
      Debug.LogError("No se encontró la fuente LegacyRuntime.ttf; no se puede mostrar la puntuación.", this);
      return;
    }

    textoPuntuacion.fontSize = 24;
    textoPuntuacion.fontStyle = FontStyle.Bold;
    textoPuntuacion.alignment = TextAnchor.MiddleLeft;
    textoPuntuacion.color = Color.white;
    textoPuntuacion.raycastTarget = false;
  }

  private void ActualizarTextoPuntuacion() {
    if (textoPuntuacion != null) {
      textoPuntuacion.text =
        $"Puntuación: {PuntuacionJugador}\nRecompensas obtenidas: {RecompensasObtenidas}";
    }
  }

  private void NotificarColisionConHumanoide2() {
    Debug.Log("Cubo tocó a Humanoide 2");
    AlChocarConHumanoide2?.Invoke();
  }

  private void NotificarColisionConHumanoide1() {
    Debug.Log("Cubo tocó a Humanoide 1");
    AlChocarConHumanoide1?.Invoke();
  }
}