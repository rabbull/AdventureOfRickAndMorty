public class StartButton : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.Restart();
        GameManager.Instance.Run();
    }
}
