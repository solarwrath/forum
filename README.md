# Startup
In order to start the application you have to specify connection string for Microsoft SQL Server by environment variable ConnectionString.
You also have to specify SendGrid API Key via environment variable SENDGRID_APIKEY, it is used to send emails.
Without it you will be able only to watch login and sign up pages, as sign in requires confirmed email.