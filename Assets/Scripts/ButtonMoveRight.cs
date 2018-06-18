public class ButtonMoveRight : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.AddNewInstruction ('>');
    }
}