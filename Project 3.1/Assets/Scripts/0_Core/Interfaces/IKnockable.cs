/// * Objects that can be:
///     - Knocked back
///     - Knocked down
public interface IKnockable
{
    float KnockbackAmount   { get; }
    float KnockdownDuration { get; }
    bool IsKnockable        { get; }

    void TriggerKnockback();
    void TriggerKnockdown();
}
