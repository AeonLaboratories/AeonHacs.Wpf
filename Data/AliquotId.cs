namespace AeonHacs.Wpf.Data;

public class AliquotId : BindableObject
{
    string id;
    public string Id
    {
        get => id;
        set => Ensure(ref id, value);
    }
}
