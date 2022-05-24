using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableGem : XRBaseInteractable
{
    private MovableGem movableGemScript;
    private GameRenderer gameRenderer;
    private XRRayInteractor attachedInteractor;

    private Vector3 dragStartOffset;

    public void setMovableGemScript(MovableGem mg)
    {
        this.movableGemScript = mg;
    }

    public void setGameRenderer(GameRenderer gr)
    {
        this.gameRenderer = gr;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (movableGemScript != null)
        {
            movableGemScript.SetHighlighted(true);
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if(movableGemScript != null)
        {
            movableGemScript.SetHighlighted(false);
        }
    }

    private Vector3 GetRayCastHitPoint()
    {
        Transform rayTransform = attachedInteractor.rayOriginTransform;
        Ray ray = new Ray(rayTransform.position, rayTransform.forward);

        Vector3 normal = new Vector3();
        var mf = gameRenderer.gameObject.GetComponent<MeshFilter>();
        if (mf && mf.mesh.normals.Length > 0)
            normal = mf.transform.TransformDirection(mf.mesh.normals[0]);

        var plane = new Plane(normal, transform.position);

        float enter = 0.0f;
        if (plane.Raycast(ray, out enter))
        {
            return ray.GetPoint(enter);
        }

        return new Vector3();
    }

    private void Attach(XRRayInteractor rayInteractor)
    {
        if (movableGemScript != null)
        {
            attachedInteractor = rayInteractor;

            movableGemScript.SetMoving(true);

            Vector3 hitpoint = GetRayCastHitPoint();
            Vector3 hitpointResolvedZ = new Vector3(hitpoint.x, hitpoint.y, transform.position.z);
            dragStartOffset = hitpointResolvedZ - transform.position;
        }
    }

    private void Detach()
    {
        if(movableGemScript != null)
        {
            movableGemScript.SetMoving(false);
            movableGemScript.handleRelease();
            attachedInteractor = null;
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        if(args.interactorObject is XRRayInteractor)
        {
            Attach((XRRayInteractor)args.interactorObject);
        }
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        Detach();
    }

    private void Update()
    {
        if(attachedInteractor != null)
        {
            Vector3 hitpoint = GetRayCastHitPoint();
            Vector3 hitpointResolvedZ = new Vector3(hitpoint.x, hitpoint.y, transform.position.z);

            Vector3 newPos = new Vector3(hitpointResolvedZ.x - dragStartOffset.x, hitpointResolvedZ.y - dragStartOffset.y, transform.position.z);
            movableGemScript.handleMove(newPos);
        }
    }
}
