public class ButtonMinus : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.AddNewInstruction ('-');
    }
}