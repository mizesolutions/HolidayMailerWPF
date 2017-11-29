using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HolidayMailer {

    public partial class SendWindow : Window {
        private const string smtpServer = "smtp.gmail.com";
        private const int port = 587;
        private List<string> emailRecipients;
        private string subject;
        private string body;
        private string attachmentPath;

        public SendWindow(List<string> recipients) {
            InitializeComponent();
            CenterWindowOnScreen();
            emailRecipients = recipients;
            var temp = emailRecipients.Aggregate("", (current, email) => current + (email + ", "));
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

        private void SendMessages() {
            if (!IsValid()) return;
            CollectInfo();
            try {
                var client = new SmtpClient(smtpServer, port) {
                    Credentials = Cred.NetCred,
                    EnableSsl = true
                };
                foreach (string email in emailRecipients) {
                    MailMessage message = new MailMessage(Cred.User, email, subject, body);
                    if (!string.IsNullOrEmpty(attachmentPath)) {
                        Attachment attachment = new Attachment(attachmentPath);
                        message.Attachments.Add(attachment);
                    }
                    client.Send(message);
                    Close();
                }
            }
            catch (Exception e) {
                MessageBox.Show(Application.Current.MainWindow, e.HResult + e.HelpLink + "\n\nPlease login again.", "Auth Error");
                Close();
                MainWindow.LaunchCredWindow();
            }
        }

        private bool IsValid() {
            ResestBackgrounds();
            bool valid = true;
            valid = TestTextBox(textBox_body);
            valid = TestTextBox(textBox_subject);
            valid = TestTextBox(textBox_to);
            return valid;
        }

        private void CollectInfo() {
            subject = textBox_subject.Text;
            body = textBox_body.Text;
        }

        private void ResestBackgrounds() {
            textBox_body.Background = Brushes.White;
            textBox_subject.Background = Brushes.White;
            textBox_to.Background = Brushes.White;
        }

        private bool TestTextBox(TextBox tb) {
            if (string.IsNullOrEmpty(tb.Text)) {
                tb.Background = Brushes.Red;
                return false;
            }
            return true;
        }
     
        private void Button_send_Click(object sender, RoutedEventArgs e) {
            SendMessages();
        }

        private void Button_cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Button_attatch_Click(object sender, RoutedEventArgs e) {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            bool? result = dlg.ShowDialog();
            if (result != true) return;
            attachmentPath = dlg.FileName;
            textBox_attatch.Text = attachmentPath;
        }
       

    }
}
