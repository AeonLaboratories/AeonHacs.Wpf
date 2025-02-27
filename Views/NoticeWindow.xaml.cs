using AeonHacs.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace AeonHacs.Wpf;

public partial class NoticeWindow : Window
{
    static readonly Dictionary<string, NoticeWindow> windows = new();

    public static void Show(Notice notice)
    {
        if (notice.CancellationToken.IsCancellationRequested)
            return;

        if (windows.TryGetValue(notice.Message, out var window))
        {
            // TODO check time since last shown?
            //MainWindow.PlaySound(notice);
            //window.Activate();
            return;
        }

        window = new NoticeWindow(notice);
        windows.Add(notice.Message, window);
        window.Closed += (_, _) => windows.Remove(notice.Message);

        window.Show(notice.CancellationToken);
    }

    public static async Task<Notice> ShowDialog(Notice notice)
    {
        if (notice.CancellationToken.IsCancellationRequested)
            return Notice.NoResponse;
        return await new NoticeWindow(notice).ShowDialog(notice.CancellationToken);
    }

    Notice notice;

    Notice response = Notice.NoResponse;
    public Notice Response
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
        this.notice = notice;
        Icon = GetIcon(notice.Type);

        Title = $"{notice.Type} - {DateTime.Now:yyyy-MM-dd HH:mm}";
        Message.Text = notice.Message;
        Details.Text = notice.Details;

        if (notice.Responses.Any())
            Responses.ItemsSource = notice.Responses;
        else
            Responses.Items.Add("Ok");
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

    private void ResponseButton_Click(object sender, RoutedEventArgs e)
    {
        Response = new Notice((string)((Button)sender).Content);
    }

    public void Show(CancellationToken cancellationToken)
    {
        cancellationToken.Register(() => Dispatcher.Invoke(Close));

        if (!cancellationToken.IsCancellationRequested)
        {
            Show();
            MainWindow.PlaySound(notice);
        }
    }

    public async Task<Notice> ShowDialog(CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<Notice>();

        Closed += (sender, e) => tcs.TrySetResult(Response);

        Show(cancellationToken);

        return await tcs.Task;
    }
}
