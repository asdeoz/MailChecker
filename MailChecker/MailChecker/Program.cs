using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lesnikowski.Client;
using Lesnikowski.Mail;
using Lesnikowski.Client.IMAP;


namespace MailChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            if ((args.Length < 2) || (args.Length > 3))
            {
                ShowInvalidArgumentsText();
            }
            else
            {
                try
                {
                    string username = args[0];
                    string password = args[1];
                    string imapServer = "imap.gmail.com";

                    if (args.Length == 3)
                    {
                        imapServer = args[2];
                    }

                    // Show menu
                    int optionChosen = ShowMenu();

                    using (Imap imap = new Imap())
                    {
                        // Connection to the Imap server
                        imap.ConnectSSL(imapServer);
                        // Login with username and password given
                        imap.Login(username, password);

                        // Program logic
                        switch (optionChosen)
                        {
                            case 1:
                                break;

                            case 2:
                                break;

                            case 0:
                                break;
                        }

                        // Closing connection to Imap server
                        imap.Close(true);
                    }
                }
                catch(ServerException ex)
                {
                    ShowInvalidArgumentsText();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.GetType().ToString() + " occurred");
                    Console.WriteLine("Message: " + ex.Message);
                }
            }
        }

#region MenuAndTexts

        /// <summary>
        /// Informs the user the introduction of invalid parameters and the right usage of the tool.
        /// </summary>
        private static void ShowInvalidArgumentsText()
        {
            Console.WriteLine("Invalid arguments.");
            Console.WriteLine();
            Console.WriteLine("Usage: mailchecker emailaddress password [imapserver]");
            Console.WriteLine();
            Console.WriteLine("Default Imap Server is Gmail");
        }

        /// <summary>
        /// Shows welcome menu
        /// </summary>
        /// <returns>Return user's chosen option</returns>
        private static int ShowMenu()
        {
            bool isValidOption = false;
            int validOption = 0;

            Console.WriteLine("╔════════════════════════════╗");
            Console.WriteLine("║   Welcome to MailChecker   ║");
            Console.WriteLine("╚════════════════════════════╝");
            OptionsMenu();
            do
            {
                Console.Write("Option: ");
                string option = Console.ReadLine();
                isValidOption = int.TryParse(option, out validOption);
                if(!isValidOption)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid option. Valid options are:");
                    OptionsMenu();
                }

            } while (!isValidOption);

            return validOption;
        }

        /// <summary>
        /// Shows the static options fo the menu
        /// </summary>
        private static void OptionsMenu()
        {
            Console.WriteLine();
            Console.WriteLine("  1 - Check unseen mails");
            Console.WriteLine("  2 - Check last mails");
            Console.WriteLine("  0 - Exit");
            Console.WriteLine();
        }

#endregion

#region EmailChecking

        private static void GetUnseenEmails(Imap imap)
        {
            imap.SelectInbox();

            List<long> uids = imap.SearchFlag(Flag.Unseen);

            int counterMails = 1;
            int totalMails = uids.Count;

            foreach (long uid in uids)
            {
                string eml = imap.GetMessageByUID(uid);
                IMail email = new MailBuilder().CreateFromEml(eml);

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(email.Subject);
                Console.WriteLine(email.Text);

            }
        }

#endregion

    }
}
