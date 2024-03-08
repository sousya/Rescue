using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    public float gravityScale = 1.0f; // �������������ű���
    public bool useGravity = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(useGravity)
        {
            // �޸����������
            Vector3 newGravity = Physics.gravity * gravityScale;
            rb.AddForce(newGravity, ForceMode.Acceleration);
        }
    }
}

