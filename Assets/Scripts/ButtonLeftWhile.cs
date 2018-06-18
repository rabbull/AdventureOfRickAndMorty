public class ButtonLeftWhile : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.AddNewInstruction ('[');
    }
}