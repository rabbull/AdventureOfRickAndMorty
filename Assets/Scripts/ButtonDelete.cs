public class ButtonDelete : Button
{
    protected override void OnClickEvent()
    {
        GameManager.Instance.DeleteHere ();
    }
}
