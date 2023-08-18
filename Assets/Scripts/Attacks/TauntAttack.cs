//*******************************************************************************************
// TauntAttack
//*******************************************************************************************
/// <summary>
/// Attack subclass that handles basic attack functionality inherited from the Attack
/// class for a TauntEnemy. Extends the Attack class to deactivate the AoE attack
/// highlighter at the end of an attack implemented in the TauntEnemy class.
/// </summary>
public class TauntAttack : Attack
{
    /// <summary>
    /// Extends the parent class Attack deactivation with the disabling of the MeshRenderer.
    /// </summary>
    public override void Deactivate()
    {
        base.Deactivate();
        _renderer.enabled = false;
    }
}
