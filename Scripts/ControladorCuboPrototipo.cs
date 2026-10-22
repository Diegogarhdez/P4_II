using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody))]
public class ControladorCuboPrototipo : MonoBehaviour {
  [SerializeField]
  private float velocidad = 6f;

  private Rigidbody cuerpo;
  private Vector2 direccionMovimiento;

  private void Awake() {
    cuerpo = GetComponent<Rigidbody>();
  }

  private void Update() {
#if ENABLE_INPUT_SYSTEM
    Keyboard teclado = Keyboard.current;
    direccionMovimiento = Vector2.zero;

    if (teclado == null) {
      return;
    }

    if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed) {
      direccionMovimiento.x -= 1f;
    }
    if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed) {
      direccionMovimiento.x += 1f;
    }
    if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed) {
      direccionMovimiento.y -= 1f;
    }
    if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed) {
      direccionMovimiento.y += 1f;
    }
#else
    direccionMovimiento = new Vector2(
      Input.GetAxisRaw("Horizontal"),
      Input.GetAxisRaw("Vertical")
    );
#endif

    direccionMovimiento = Vector2.ClampMagnitude(direccionMovimiento, 1f);
  }

  private void FixedUpdate() {
    Vector3 desplazamiento = new Vector3(
      direccionMovimiento.x,
      0f,
      direccionMovimiento.y
    );
    cuerpo.MovePosition(cuerpo.position + desplazamiento * velocidad * Time.fixedDeltaTime);
  }
}
