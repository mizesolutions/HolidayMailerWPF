using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace HolidayMailer {
    /// <summary>
    /// Brian Mize
    /// CSCD 371
    /// Holiday Mailer
    /// 
    /// Interaction logic for SendWindow.xaml
    /// </summary>
    public partial class SendWindow : Window {
        private const string smtpServer = "smtp.gmail.com";
        private const int port = 587;
        private List<string> emailRecipients;

        private string emailSender;
        private string mypwd;
        private string subject;
        private string body;
        private string attachmentPath;

        public SendWindow(List<string> recipients) {
            InitializeComponent();
            CenterWindowOnScreen();
            emailRecipients = recipients;
            string temp = "";
            foreach (string email in emailRecipients) {
                temp += email + ", ";
            }
            textBox_to.Text = temp.Substring(0, temp.Length - 2);
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void SendMessages() {
            if (IsValid()) {
                CollectInfo();
                try {
                    var client = new SmtpClient(smtpServer, port) {
                        Credentials = new NetworkCredential(emailSender, mypwd),
                        EnableSsl = true
                    };
                    foreach (string email in emailRecipients) {
                        MailMessage message = new MailMessage(emailSender, email, subject, body);
                        if (attachmentPath != "" && attachmentPath != null) {
                            Attachment attachment = new Attachment(attachmentPath);
                            message.Attachments.Add(attachment);
                        }
                        client.Send(message);
                        Close();
                    }
                }
                catch (Exception e) {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private bool IsValid() {
            ResestBackgrounds();
            bool valid = true;
            valid = TestTextBox(textBox_body);
            valid = TestTextBox(textBox_from);
            valid = TestTextBox(textBox_subject);
            valid = TestTextBox(textBox_to);
            if (passwordBox.Password == null || passwordBox.Password == "") {
                passwordBox.Background = Brushes.Red;
                valid = false;
            }
            return valid;
        }

        private void CollectInfo() {
            mypwd = passwordBox.Password;
            emailSender = textBox_from.Text;
            subject = textBox_subject.Text;
            body = textBox_body.Text;
        }

        private void ResestBackgrounds() {
            textBox_body.Background = Brushes.White;
            textBox_from.Background = Brushes.White;
            textBox_subject.Background = Brushes.White;
            textBox_to.Background = Brushes.White;
            passwordBox.Background = Brushes.White;
        }

        private bool TestTextBox(TextBox tb) {
            if (tb.Text == null || tb.Text == "") {
                tb.Background = Brushes.Red;
                return false;
            }
            return true;
        }
     
        private void button_send_Click(object sender, RoutedEventArgs e) {
            SendMessages();
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void button_attatch_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true) {
                attachmentPath = dlg.FileName;
                textBox_attatch.Text = attachmentPath;
            }
        }
    }
}
