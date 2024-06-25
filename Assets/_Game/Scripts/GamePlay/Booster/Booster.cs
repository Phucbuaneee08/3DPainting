using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class Booster : MonoBehaviour
{
    public int Quantity { get; protected set; }
    protected Character character;

    public virtual void Initialize(Character character, int quantity)
    {
        this.character = character;
        this.Quantity = quantity;
    }

    public abstract void Activate();
    public abstract void Deactivate();

    
}
