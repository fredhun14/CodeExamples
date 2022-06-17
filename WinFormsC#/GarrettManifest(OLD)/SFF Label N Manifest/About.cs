using System;
using System.Windows.Forms;

namespace SFFLabelNManifest
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }
        /*
         * Version Hstory:
         * versions lower than 2.0 are versions predating the inclusion of OPC tags.
         * 
         * Version: 2.0 Is the first release version that contans OPC tags and an assortment of debugging utilties for the OPC tags 
         * This version also includes the first VNC buttons on the primary form
         * 
         * Version: 2.1 includes the second case printer option (either evo printer or inkjet) too accommodate the ink case printer for one customer
         * This version also includes the flashing instructions on the details panel
         * 
         * Version: 2.3.001 Changed version method to be able to better log changes with each version between 2.1 and 2.3.001 there have been many changes
         * to do with updating print manual ticket such as; logging freshpack tote resources as packaging tickets, making it so the user can change the 
         * logged daycode, forcing line 3 (Freshcase) to manifest as freshpack, user activity logging, additional user levels, logging voided tickets in a 
         * seperate file, the addion of the entire daily production log form which can be used to print bacti stickers, finishing adding cob manifest so it
         * can be manifested properly, changes to the export form to make it simpler, this is up to 80 configurable settings in general settings, I got the 
         * manifest shack scale working properly for the getweight form. Testing of the opc tag portion of the software will begin properly soon.
         * 
         * Version 2.3.002 03/01/2022
         * This update includes the addition of pages on the manage users form 
         * Maximum user count increased from 25 to 125
         * Added current page label to manage users form
         * Added next and previous page buttons to manage users form
         * 
         * Version 2.3.003 03/02/2022
         * Added selection for manifest type for each normal line the software is now capable of manifesting each line as either freshpack or packaging
         * This change also applies to print manual ticket; the previously added option for freshpack side too choose to manifest those resources to
         * repack or freshpack now applies also when manifesting a repack pallet manually.
         * Now able to add seperate line numbers for each line dependant on wether choosing freshpack or packaging manifest style.
         * The seperate line numbers will apply when manually manifesting as well.
         * The rework for PrintManifest() is complete as a part of this update it is now replaced with Manifest() which then decides which of the three
         * alternate manifest functions to perform packagingmanifest() freshpackmanifest() cobmanifest()
         * 
         * Version 2.3.004 03/04/2022
         * Added an error log which in some try catch blocks will be added too anytime an exception is caught
         * GetWeight form, OPC tag functions on the Primary form and read loop function on the Primary form are the places I have already added this 
         * logging capability more too come.
         * The logging is to help diagnose bugs that occur when the software is in real world use since they to not often crop up in the testing enviroment
         * No issues have been found so far with this addition to the catch blocks but for the same reason I am adding it that does not mean some won't manifest 
         * Fixed issue where Julien date did not include facility start time setting as a factor in the dating on several forms
         * 
         * Version 2.3.005 03/11/2022
         * Added Totals to daily log display making the form have additional functionality and use.
         * Fixed a bug in print manual ticket to do with line numbers in the text manifest for freshpack tickets off packaging lines
         * Added username display in the bottom right corner of the primary form to make it clear who is currently logged in
         * Made it so when Logout is pressed it will automatically open the Login prompt form
         * Renamed VNC buttons on primary form
         * 
         * Version 2.3.006 03/14/2022
         * Added Todays julien date display above username display on primary form this will update every minute but change once per day.
         * Added Generate Daily production totals text file button to daily log display form
         * The generate daily production totals text file button will generate the information needed by managers to monitor daily production
         * and then open this data in a text file saved in the exports folder. The user will then be responsible for adding more information
         * and savinging it elsewhere.
         * Increased cleanup timer tick to 100 minutes from 10 minutes
         * Adjusted jobcount font size to match nearby controls
         * Reduced maximum job count to 1 million
         * Gave repack users access to line 4
         * 
         * Version 2.3.007 03/15/2022
         * Fixed issue where Julien date display does not update The timer was never enabled. Therefore would never trigger.
         * Changed daily production report function to open a calendar selection window so the user can select a range of days to 
         * generate the report based on. if the user presses ok without selecting a range the report will generate based on that day
         * starting at the facility start time (currently 7 am weston 6 am garrett) and ending the next day at the same time. This will
         * essentially be the same as the original intention of the report but working properly as there was a bug with it doing everything
         * from the last 24 hrs instead of from the same day. This also allows the user to select a range of dates for example if they want a 
         * report for the month of february they only need to highlight that month in the date selection form and it will generate the report
         * for that date range This will also name the text file it generates appropriately to include XXX-YYY being the start julian date and 
         * ending dates of the report.
         * Changed the adding up process to ignore resources with a case count greater than 200 to help hide the repack totes as this is 
         * a case count report and it scewed the totals greatly this will not hide totes with a weight under 200 however.
         * Changed the button's name and text that generates the production reports to reflect the changes made to the functionality 
         * of the button
         * Per Carolyn's request all the repack exports will now be sorted once exported to her by resource like with the old Garret manifest
         * software.
         * Change to what happens to the panels when a user of any level logs in:
         * the panels will no longer hide but the controls that can be interacted with will be disabled or set to not be visible if the user's
         * level does not permit access. So when logged out all the interactable per line panel controls will be disabled.
         * Added Weight column to sql tables for packaging manifest and changed appropriate functions to use this new columnto help with 
         * reporting in the future.
         * Changed login form starting location
         * 
         * Version 2.3.008 03/16/2022
         * Fixed bug where user could enter and log username in any case it will now force tolower for logging
         * Fixed bug where Forms would be minimized then forced into the background locking up the software by removing both minimize
         * and maximize buttons from all of the forms
         * 
         * Version 2.3.009 03/16/2022
         * Added Setting 81 and 82 for the SQL Line Status updater.
         * Added Timer to trigger the Line status Update every 5 seconds
         * 
         * Version 2.3.010 03/22/2022
         * Fixed issue with Julien dates being calculated wrong after midnight causing issues with manifest and exporting.
         * Fixed an issue about the line status updater that came from a different way of selecting the resource number.
         * 
         * Version 2.3.011 03/30/2022
         * Added automated email containing a production report to be triggered on export both button click
         * This email will replace the manually generated report from daily ticket log.
         * There are upto three email address slots in the software but can be sent to a group.
         * Fixed a few spelling mistakes.
         * 
         * Version 2.3.012 04/05/2022
         * Added a ton of error logging for debugging help in the future;
         * Fixed issue with displaying case count totals on daily log display when there is not 4 lines came up at Garrett
         * Added ual to email sending and modified logging to do with export from previous day
         * Fixed issue with line 4 freshpack manifest in SQL
         * Fixed issue with line status update timer's interaction with configure label printers
         * Added a lock/unlock button to the printer panels so that users will have to unlock the resource selection box in order to change
         * the resource. Hitting the print button will also LOCK the resource drop down to prevent anyone from forgetting when they are done
         * changing the resource over.
         * Fixed issue with gsheet update changing the resource number; As long as the selected resource number is not changed and 
         * it is not deleted the selected resource should not change.
         * Added a log for the data recieved on scaleport include date time to try and decifer what is causing errors when pulling the weight
         * 
         * Version 2.3.013 04/11/2022
         * Added Column for lot code in SQL and changed Daycode column so that when manifesting this will be the Smith Date code for when we 
         * eventually manifest into the AS400 from SQL these columns can be used.
         * Changed the way the Text file manifest is generated slightly so that lot and date codes are present David from PSGI will change
         * the batching program so that the new files will be entered into the AS400 correctly. Important note for printing manual tickets:
         * The Lotcode will take the full string from the daycode box and the Datecode will take the same string but knock off the first character
         * It is all the more important now that if the user modifies the contents of the daycode box that it is correct.
         * After an issue occured when the computer abruptly and unexpectedly lost power two printhistory files were corrupted I deleted them
         * but the automatic regeneration occured with a number too large to fit in the job count box I have reduced this to 4 characters so 
         * in the future the automatic generation will be fixed in case a similar issue occurs.
         * Fixed issue with daily ticket log to do with the change in the manifest text file.
         * 
         * Version 2.3.014 04/21/2022
         * Added Day code generation class and deleted all the old methods so that all daycodes will be generated on the same single code This should help with troubleshooting
         * and with consistency from form to form.
         * Fixed all errors to do with deleting old datecode generation being removed by directing those lines to the new public members of the new class.
         * Changed which column the daycode is sourced from in daily ticket log to use lot code instead of date code so it is easier to tell which alpha code each pallet is from
         * Added a public class for Manifest so this should help with the inconsistencies I have seen caused by the repeatedly altered code I have integreated it into the print manual
         * ticket form it appears to work correctly and this also resolves an issue QA had brought to my attention regarding printing packaging tickets with a different resource than the
         * one that is currently running on the line.
         * 
         * Version 2.3.015 04/25/2022
         * Updated the way CPM is calculated to make it more accurate and more reliable
         * Commented out the auto-pull for the Gsheet text file at application startup to prevent accidental changes to the resource number selected.
         * The Gsheet text file will have to be manually updated by pressing the Force Gsheet Update button by a manager level user or above.
         * 
         * Version 2.3.016 05/09/2022
         * Updated and fixed the Freshpack detected metal convert resource function.
         * Auto manifest now points to the new public manifest classes as well, both auto and manual manifest will follow the same steps to manifest and print
         * labels now the only difference is how the functions are fed the information they require to make this happen, which I believe is correct on both counts.
         * Added logging to failures to do with re-initializing resource numbers after a force g-sheet update.
         * Forced a visual change when resource fails to re-initialize after a g-sheet update the resource will now be forced to resource 1 and visually changed
         * to reflect that in order to indicate to the user there is an issue.
         * 
         * Version 2.3.017 06/06/2022
         * Added logging back for printing manual Tickets.
         * Making adjustments for how the auto-manifest will actually function. 
         * Adjusted bacti sticker printing function to allow for packaging style resources to be used for fresh pack pallets Each resource will need to be
         * added to the fresh pack resource description list however.
         * 
         * Version 2.3.018
         * Added locations to Daily ticket log
         * 
         * Version
         */
        private void OKbut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void About_Load(object sender, EventArgs e)
        {

        }

        private void AboutLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
