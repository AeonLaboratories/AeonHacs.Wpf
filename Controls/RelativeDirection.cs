namespace AeonHacs.Wpf.Controls;

public enum RelativeDirection
{
    Left, Up, Right, Down
}

public static class RelativeDirectionExtensions
{
    public static RelativeDirection Reverse(this RelativeDirection direction)
    {
        return direction switch
        {
            RelativeDirection.Left => RelativeDirection.Right,
            RelativeDirection.Up => RelativeDirection.Down,
            RelativeDirection.Right => RelativeDirection.Left,
            RelativeDirection.Down => RelativeDirection.Up,
            _ => RelativeDirection.Down
        };
    }
}