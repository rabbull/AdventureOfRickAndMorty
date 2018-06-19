public class ButtonLeft : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.MoveCursor(true);
    }
}