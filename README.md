Holiday Mailer
===============

This WPF application was the final project for a C#/.NET class I took at EWU in the Fall of 2017. The application allows a user to create a mailing list of contacts that they could send a holiday e-greeting to with an optional file attachment.

The user can:
 - Add, edit, and delete contacts
 - Keep track of who sent a holiday greeting back
 - Create and maintain mailing lists
 - Send email to a single recipient or a list of recipients


 Contacts and mailing lists are maintained in a SQLite database in order to make the application small and easily portable.

 Sending mail with this application requires the user to have a valid Gmail account. For users that employ 2-factor authentication with their Gmail account, there is a link on the authentication window that will take the user to a Google page where an application password can be created for use with the Holiday Mailer application.
