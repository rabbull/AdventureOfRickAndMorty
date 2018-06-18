public class ButtonDot : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.AddNewInstruction ('.');
    }
}
