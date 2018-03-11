using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HolidayMailer
{
    /// <summary>
    /// Handles sending email messages with optional attachments to selected recip[ients.
    /// </summary>
    public partial class SendWindow
    {
        public string SmtpServer { get; } = "smtp.gmail.com";
        public int Port { get; } = 587;
        public List<string> EmailRecipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentPath { get; set; }


        public SendWindow(List<string> recipients)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            EmailRecipients = recipients;
            var temp = EmailRecipients.Aggregate("", (current, email) => current + (email + ", "));
            textBox_to.Text = temp.Substring(0, temp.Length - 2);
        }

        /// <summary>
        /// Centers the window.
        /// </summary>
        private void CenterWindowOnScreen()
        {
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
            Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
        }

        /// <summary>
        /// Sneds email message to all recipients
        /// </summary>
        private void SendMessages()
        {
            if (!IsValid()) return;
            CollectInfo();
            try
            {
                var client = new SmtpClient(SmtpServer, Port)
                {
                    Credentials = Credential.NetworkCredential,
                    EnableSsl = true
                };
                foreach (var email in EmailRecipients)
                {
                    var message = new MailMessage(Credential.User, email, Subject, Body);
                    if (!string.IsNullOrEmpty(AttachmentPath))
                    {
                        var attachment = new Attachment(AttachmentPath);
                        message.Attachments.Add(attachment);
                    }
                    client.Send(message);
                    Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Application.Current.MainWindow, e.HResult + e.HelpLink + "\n\nPlease login again.", "Auth Error");
                Close();
                MainWindow.LaunchCredWindow();
            }
        }

        /// <summary>
        /// Checks if all form text is valid
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            ResestBackgrounds();
            return TestTextBox(textBox_body) && TestTextBox(textBox_subject) && TestTextBox(textBox_to);
        }

        /// <summary>
        /// Groups form data
        /// </summary>
        private void CollectInfo()
        {
            Subject = textBox_subject.Text;
            Body = textBox_body.Text;
        }

        /// <summary>
        /// Resets form test box background after invalid data corrected
        /// </summary>
        private void ResestBackgrounds()
        {
            textBox_body.Background = Brushes.White;
            textBox_subject.Background = Brushes.White;
            textBox_to.Background = Brushes.White;
        }

        /// <summary>
        /// Changes textbox background color if text is invalid
        /// </summary>
        /// <param name="textbox"></param>
        /// <returns></returns>
        private bool TestTextBox(TextBox textbox)
        {
            if (!string.IsNullOrEmpty(textbox.Text)) return true;
            textbox.Background = Brushes.Red;
            return false;
        }

        /// <summary>
        /// Calls send function on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendClick(object sender, RoutedEventArgs e)
        {
            SendMessages();
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Allows user to attach files to send
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAttatchClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result != true) return;
            AttachmentPath = dialog.FileName;
            textBox_attatch.Text = AttachmentPath;
        }

    }
}
