using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HolidayMailer
{

    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public Help()
        {
            InitializeComponent();
            LoadText();
        }

        private void LoadText()
        {
            NewContact_text();
            EditContact_text();
            RemoveContact_text();
            SearchContact_text();
            SendMail_text();
            NewMailList_text();
            EditMailList_text();
            RemoveMailList_text();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void NewContact_text()
        {
            newContact_text.Text = "To Add a new Contact: \n" +
                                   "- Select New Contact from the Contacts drop down menu\n" +
                                   "   OR \n" +
                                   "- Click on the New button on the Contacts tab page\n" +
                                   "\n" +
                                   "A new window will open that will allow you to enter the\n" +
                                   "information for your new contact. Click the save button\n" +
                                   "to save your new contact and return to the main window.";
        }

        private void EditContact_text()
        {
            editContact_text.Text = "To Edit Contacts: \n" +
                                   "- Select Edit Contact from the Contacts drop down menu\n" +
                                   "   OR \n" +
                                   "- Click on the Edit button on the Contacts tab page\n" +
                                   "\n" +
                                   "A new window will open that will allow you to edit the\n" +
                                   "information of your saved contacts. Click the save button\n" +
                                   "to save your changes and return to the main window.";
        }

        private void RemoveContact_text()
        {
            removeContact_text.Text = "To Remove Contacts: \n" +
                                   "- Select Remove Contact from the Contacts drop down menu\n" +
                                   "   OR \n" +
                                   "- Click on the New button on the Contacts tab page\n" +
                                   "\n" +
                                   "A new window will open that will allow you to enter the\n" +
                                   "information for your new contact. Click the save button\n" +
                                   "to save your new contact and return to the main window.";
        }

        private void SearchContact_text()
        {
            searchContact_text.Text = "To Search By Last Name: \n" +
                                    "Enter the person's last name in the text box above\n" +
                                    "the contacts area and click the Search button\n";
        }

        private void SendMail_text()
        {
            sendMail_text.Text = "To Send Mail: \n" +
                                   "- Select Send Mail from the Mailing Lists drop down\n" +
                                   "   OR \n" +
                                   "- Click the Send Mail button on the Mailing List tab page\n" +
                                   "\n" +
                                   "If you haven't logged in with a valid gmail account, a new\n" +
                                   "window will open that will allow you to log in. Otherwise, a\n" +
                                   "new window will open that will allow you to enter the\n" +
                                   "information need in order to send an email.\n";
        }

        private void NewMailList_text()
        {
            newMailList_text.Text = "To Create a New Mailing List: \n" +
                                   "- Select New List from the Mailing Lists drop down\n" +
                                   "   OR \n" +
                                   "- Click the New List button on the Mailing List tab pagee\n" +
                                   "\n" +
                                   "A new window will open that will allow you to create the\n" +
                                   "list using your saved contacts. Click the save button\n" +
                                   "to save your changes and return to the main window.";
        }

        private void EditMailList_text()
        {
            editMailList_text.Text = "To Edit a Mailing List: \n" +
                                     "- Select Edit List from the Mailing Lists drop down\n" +
                                     "   OR \n" +
                                     "- Click the Edit List button on the Mailing List tab pagee\n" +
                                     "\n" +
                                     "A new window will open that will allow you to edit any\n" +
                                     "list you have created. Click the save button\n" +
                                     "to save your changes and return to the main window.";
        }

        private void RemoveMailList_text()
        {
            removeMailList_text.Text = "To Remove Mailing List: \n" +
                                       "- Select Remove List from the Mailing Lists drop down\n" +
                                       "   OR \n" +
                                       "- Click the Remove List botton on the Mailing List tab pagee\n" +
                                       "\n" +
                                       "A new window will open that will allow you to remove any\n" +
                                       "list you have created. Click the save button\n" +
                                       "to save your changes and return to the main window.";
        }


        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
