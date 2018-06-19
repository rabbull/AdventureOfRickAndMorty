public class ButtonMoveLeft : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.AddNewInstruction('<');
    }
}