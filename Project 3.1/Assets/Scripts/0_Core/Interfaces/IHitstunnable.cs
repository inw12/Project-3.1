public interface IHitstunnable
{
    bool InHitstun { get; }
    void TriggerHitstun(float duration);
}