public class ButtonRight : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.MoveCursor(false);
    }
}