
public interface IState
{
    // Called when game state is entered.
    void EnterState();
    // Called every fixed framerate frame.
    void FixedUpdateState();
    // Called every frame.
    void UpdateState();
    // Called every frame after UpdateState.
    void LateUpdateState();
    // Called when game state is exited.
    void ExitState();
}
