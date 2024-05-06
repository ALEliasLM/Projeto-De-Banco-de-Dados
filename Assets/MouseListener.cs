using UnityEngine;

public class MouseListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float scrollSpeed = 1.0f; // Velocidade de rolagem do zoom
    public float moveSpeed = 5000f; // Velocidade de movimento

    void Update()
    {
        // Aumentar ou diminuir o tamanho do objeto com o scroll do mouse
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        var scale = transform.localScale;
        transform.localScale = new Vector3(Mathf.Clamp((scale.x + scrollWheel),.1f,3f), Mathf.Clamp((scale.y + scrollWheel), .1f, 3f), Mathf.Clamp((scale.z + scrollWheel), .1f, 3f)) * scrollSpeed;
    
        // Movimentar o objeto proporcionalmente ao movimento do mouse quando o botão do scroll é pressionado
        if (Input.GetMouseButton(2)) // Botão do scroll é o botão do meio do mouse
        {
            float moveX = Input.GetAxis("Mouse X") * moveSpeed * (1 + scale.x);
            float moveY = Input.GetAxis("Mouse Y") * moveSpeed * (1 + scale.x);
            transform.Translate(new Vector3(moveX, moveY, 0));
        }
    }
}
