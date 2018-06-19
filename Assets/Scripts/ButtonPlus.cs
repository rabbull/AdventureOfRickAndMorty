public class ButtonPlus : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.AddNewInstruction('+');
    }
}