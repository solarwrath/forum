# Startup
In order to start the application you have to specify connection string for Microsoft SQL Server by environment variable ConnectionString.
You also have to specify SMTP parameters via environment variables:
EMAIL_FROM_ADDRESS,
SMTP_PASSWORD,
SMTP_ADDRESS,
SMTP_PORT_NUMBER
I have used Gmail
Without it you will be able only to watch login and sign up pages, as sign in requires confirmed email.