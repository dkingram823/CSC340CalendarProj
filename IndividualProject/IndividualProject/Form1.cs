using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IndividualProject
{
    public partial class Form1 : Form
    {
        ArrayList day;
        Event currentEvent;
        public Form1()
        {
            InitializeComponent();
            String date = monthCalendar1.SelectionStart.ToString();
            label1.Text = date.Substring(0, date.IndexOf(' '));
            dateTimePicker3.Value = dateTimePicker3.MinDate;
        }


        private void Button1_Click(object sender, EventArgs e)//new event
        {
            
            panel1.Visible = true;
            saveButton.Visible = true;

            textBox1.Text = "";
            dateTimePicker1.Value = monthCalendar1.SelectionRange.Start;
            dateTimePicker2.Value = monthCalendar1.SelectionRange.Start;
            textBox4.Text = "";
            dateTimePicker3.Value = dateTimePicker3.MinDate;
            textBox7.Text = "";

            currentEvent = new Event();
            deleteButton.Visible = false;


        }

        private void Button3_Click(object sender, EventArgs e) //save buton
        {
            string errMessage = "";

            if (textBox1.Text.Equals(""))
            {
                errMessage += "The event requires a title. \n";
            }
            if (textBox4.Text.Equals(""))
            {
                errMessage += "The event requires a location. \n";
            }
            if (textBox7.Text.Equals(""))
            {
                errMessage += "The event requires a description. \n";
            }

            if(errMessage.Equals(""))
            {
                string messageText = "The event has been saved.";
                string caption = "Saved";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBox.Show(messageText,caption,button);


                String t = textBox1.Text; 
                DateTime s = dateTimePicker1.Value;
                DateTime en = dateTimePicker2.Value; 
                String l = textBox4.Text; 
                String d = textBox7.Text; 
                DateTime r = dateTimePicker3.Value;

                currentEvent.readEvent(t,s,en,l,d,r);
                currentEvent.saveToDB();



                saveButton.Visible = false;
                deleteButton.Visible = true;



            }
            else
            {
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBox.Show(errMessage, "Try Again", button);
            }

            
        }

        private void Button4_Click(object sender, EventArgs e)//delete button
        {
            String message = "Are you sure?";
            String caption = "Delete Event";
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            
            DialogResult res;

            res = MessageBox.Show(message, caption, button);

            if (res == DialogResult.Yes){
                currentEvent.remFromDB();
                panel1.Visible = false;
                listBox1.Items.Clear();
                
            }
        }

        private void MonthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            panel1.Visible = false;

            label1.Text = monthCalendar1.SelectionStart.ToString("d");
            String y = monthCalendar1.SelectionStart.Year.ToString();
            String m = monthCalendar1.SelectionStart.Month.ToString();
            if (m.Length == 1){
                m = '0' + m;
            }
            String d = monthCalendar1.SelectionStart.Day.ToString();
            if (d.Length == 1)
            {
                d = '0' + d;
            }


            String SelDate = y + '-' + m + '-' + d;
            Console.WriteLine(SelDate);
            ArrayList eList;
            eList = Event.getEventList(SelDate,false);
            day = eList;
            listBox1.Items.Clear();


            for(int i = 0; i<eList.Count; i++)
            {
                Event currentEvent = (Event)eList[i];
                String aString = currentEvent.getStart().Hour +":" 
                    + currentEvent.getStart().Minute + " " 
                    + currentEvent.getTitle();
                listBox1.Items.Add(aString);

            }

        }

        private void SeeAllButton_Click(object sender, EventArgs e)
        {
            String y = monthCalendar1.SelectionStart.Year.ToString();
            String m = monthCalendar1.SelectionStart.Month.ToString();
            if (m.Length == 1)
            {
                m = '0' + m;
            }
            String d = monthCalendar1.SelectionStart.Day.ToString();
            if (d.Length == 1)
            {
                d = '0' + d;
            }


            String SelDate = y + '-' + m + '-' + d;
            Console.WriteLine(SelDate);

            ArrayList eList;
            eList = Event.getEventList(SelDate, true);

            day = eList;
            listBox1.Items.Clear();


            for (int i = 0; i < eList.Count; i++)
            {
                Event currentEvent = (Event)eList[i];
                String aString = currentEvent.getStart().Month + "-"
                    + currentEvent.getStart().Day + "-"
                    + currentEvent.getStart().Year + " "
                    + currentEvent.getTitle();
                listBox1.Items.Add(aString);

            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            int sel = listBox1.SelectedIndex;
            currentEvent = (Event)day[sel];

            //title
            textBox1.Text = currentEvent.getTitle();
            //start time
            dateTimePicker1.Value = currentEvent.getStart();
            //end time
            dateTimePicker2.Value = currentEvent.getEnd();
            //Location
            textBox4.Text = currentEvent.getLocation();
            //reminder
            dateTimePicker3.Value = currentEvent.getReminder();
            //description
            textBox7.Text = currentEvent.getDescription();

            panel1.Visible = true;
            deleteButton.Visible = true;
            saveButton.Visible = false;
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            saveButton.Visible = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            saveButton.Visible = true;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            saveButton.Visible = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            saveButton.Visible = true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            saveButton.Visible = true;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            saveButton.Visible = true;
        }
    }
}
