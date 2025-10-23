using System.Collections;
using UnityEngine;

/*
    This is a test tool. 
    This tool wil propel the player character in the direction wherever the mouse is. 
    This tool will have an initial charge of 3. 
    This tool will have a cooldown of 2s. 
*/
public class TestTool1 : BaseTool
{
    [Header("Tool Parameters")]
    [SerializeField] private float coolDownSeconds = 2f;
    [SerializeField] private int charges = 3;
    [SerializeField] private float launchForce = 2f;

    [Header("Runtime")]
    [SerializeField] private int chargesLeft;
    private Rigidbody2D rb;

    public override void Activate()
    {
        //place any activate functions here. This can be a Coroutine if needed. 
        ToolActivate();
    }

    public override void Instantiate()
    {
        attachedObject = transform.parent;
        rb = attachedObject.GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError($"Rigidbody2D not found on {attachedObject?.name ?? "parent"}");
        chargesLeft = charges;
        Debug.Log($"The Tool {gameObject.name} is instantiated. ");
    }

    private void ToolActivate()
    {
        if (onCooldown)
        {
            Debug.Log("Current tool on cooldown!");
            return;
        }
        Debug.Log($"The Tool {gameObject.name} is activated. Charges: {chargesLeft}");

        Vector2 transform2D = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Vector2 targetDirection = (MouseManager.mousePos - transform2D).normalized;

        rb.AddForce(targetDirection * launchForce, ForceMode2D.Impulse);

        chargesLeft--;
        if (chargesLeft <= 0)
        {
            onCooldown = true;
            StartCoroutine(GoOnCoolDown());
        }
        return;
    }

    private IEnumerator GoOnCoolDown()
    {
        yield return new WaitForSeconds(coolDownSeconds);
        onCooldown = false;
        chargesLeft = charges;
    }

}
