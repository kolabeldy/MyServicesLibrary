﻿namespace MyServicesLibrary.Infrastructure.MessageBoxes;

public partial class MessageBoxCustom : Window
{
    public MessageBoxCustom(string Message, MessageType Type, MessageButtons Buttons)
    {
        InitializeComponent();
        txtMessage.Text = Message;
        switch (Type)
        {

            case MessageType.Info:
                this.Title = "Информация";
                break;
            case MessageType.Confirmation:
                this.Title = "Внимание";
                break;
            case MessageType.Success:
                {
                    this.Title = "Успешно";
                }
                break;
            case MessageType.Warning:
                this.Title = "Предупреждение";
                break;
            case MessageType.Error:
                {
                    this.Title = "Ошибка";
                }
                break;
        }
        switch (Buttons)
        {
            case MessageButtons.OkCancel:
                btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
                break;
            case MessageButtons.YesNo:
                btnOk.Visibility = Visibility.Collapsed; btnCancel.Visibility = Visibility.Collapsed;
                break;
            case MessageButtons.Ok:
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
                break;
        }
    }

    private void btnYes_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close();
    }

    private void btnNo_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }
}
public enum MessageType
{
    Info,
    Confirmation,
    Success,
    Warning,
    Error,
}
public enum MessageButtons
{
    OkCancel,
    YesNo,
    Ok,
}
