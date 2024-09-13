using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace AeonHacs.Wpf;

public partial class NoticeWindow : Window
{
    static readonly Dictionary<string, NoticeWindow> windows = new();

    public static void Show(Notice notice)
    {
        if (windows.TryGetValue(notice.Message, out var window))
        {
            Notify.PlaySound();
            window.Activate();
            return;
        }

        window = new NoticeWindow(notice);
        windows.Add(notice.Message, window);
        window.Closed += (_, _) => windows.Remove(notice.Message);
        
        // Responses are not returned.
        window.CancelButton.Visibility = Visibility.Collapsed;

        Notify.PlaySound();
        window.Show(notice.CancellationToken);
    }

    public static async Task<Notice?> ShowDialog(Notice notice)
    {
        var window = new NoticeWindow(notice);
        Notify.PlaySound();
        return await window.ShowDialog(notice.CancellationToken);
    }

    Notice? response;
    public Notice? Response
    {
        get => response;
        set
        {
            response = value;
            Close();
        }
    }

    public NoticeWindow()
    {
        InitializeComponent();
    }

    public NoticeWindow(Notice notice) : this()
    {
        Icon = GetIcon(notice.Type);
        Title = notice.Subject ?? notice.Type.ToString();

        Message.Text = notice.Message;

        // Alert is a special case, it's used to pause the program when the caller doesn't care about response.
        if (notice.Type == NoticeType.Alert)
            CancelButton.Visibility = Visibility.Collapsed;
    }

    private static BitmapSource GetIcon(NoticeType noticeType)
    {
        return noticeType switch
        {
            // Only will be used when explicitly set.
            NoticeType.Alert => Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Shield.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()),
            NoticeType.Information => Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Information.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()),
            NoticeType.Question => Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Question.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()),
            NoticeType.Warning => Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Warning.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()),
            NoticeType.Error => Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Error.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()),
            _ => Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Application.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
        };
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        Response = new Notice("Ok");
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Response = new Notice("Cancel");
    }

    public void Show(CancellationToken cancellationToken)
    {
        cancellationToken.Register(() => Dispatcher.Invoke(Close));

        if (!cancellationToken.IsCancellationRequested)
            Show();
    }

    public async Task<Notice?> ShowDialog(CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<Notice?>();

        Closed += (sender, e) => tcs.TrySetResult(Response ?? new Notice("Cancel"));

        Show(cancellationToken);

        return await tcs.Task;
    }
}
