public class ButtonRightWhile : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.AddNewInstruction (']');
    }
}