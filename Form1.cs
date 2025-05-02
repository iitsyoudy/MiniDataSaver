using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        Panel panel;
        Label company;
        Label inteface;
        Label usernam;
        Label userpas;
        TextBox usernameTextBox, passwordTextBox;
        Button loginButton;
        PictureBox pictureBox;
        public class User
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        List<User> users;
        public Form1()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            panel = new Panel();
            panel.Location = new System.Drawing.Point(580, 290);
            panel.Size = new Size(350, 150);
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            usernam = new Label();
            usernam.Location = new Point(0, 10);
            usernam.Text = "Username :";
            usernam.Font = new Font("Arial", 12, System.Drawing.FontStyle.Regular);
            //Controls.Add(usernam);
            userpas = new Label();
            userpas.Location = new Point(0, 60);
            userpas.Text = "Password :";
            userpas.Font = new Font("Arial", 12, System.Drawing.FontStyle.Regular);
            //Controls.Add(userpas);

            users = LoadUsers();
            this.Text = "MyCompany";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Gray;
            this.Icon = new Icon(Path.Combine(Application.StartupPath, "c-logo.ico"));
            inteface = new Label();
            inteface.Location = new Point(120, 300);
            inteface.Font = new Font("Arial", 15, System.Drawing.FontStyle.Italic);
            inteface.Size = new Size(300, 200);
            inteface.Text = "Hi,Please enter your username and passowrd to continue .";
            Controls.Add(inteface);
            company = new Label();
            company.Location = new Point(680, 100);
            company.Size = new Size(200, 100);
            company.Font = new Font("Arial", 22, System.Drawing.FontStyle.Bold);
            company.Text = "MyCompany";
            Controls.Add(company);

            pictureBox = new PictureBox();
            string imagePath = Path.Combine(Application.StartupPath, "c-logo.png");
            pictureBox.Image = Image.FromFile(imagePath);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Location = new System.Drawing.Point(300, 60);
            pictureBox.Size = new System.Drawing.Size(500, 150);

            Controls.Add(pictureBox);

            usernameTextBox = new TextBox
            {
                Location = new Point(140, 10),
                Size = new Size(150, 20),

            };

            passwordTextBox = new TextBox
            {
                Location = new Point(140, 60),
                Size = new Size(150, 20),
                PasswordChar = '*'
            };
            loginButton = new Button
            {
                Text = "Login",
                Location = new Point(220, 110),
                BackColor = Color.White
            };
            loginButton.Click += LoginButton_Click;

            //Controls.Add(loginButton);
            /*Controls.Add(usernameTextBox);
            Controls.Add(passwordTextBox);*/

            panel.Controls.Add(usernameTextBox);
            panel.Controls.Add(passwordTextBox);
            panel.Controls.Add(usernam);
            panel.Controls.Add(userpas);
            panel.Controls.Add(loginButton);
            Controls.Add(panel);


            FormBorderStyle = FormBorderStyle.FixedSingle;

            MinimizeBox = false;
            MaximizeBox = false;
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(usernameTextBox, "Enter your username");
            toolTip.SetToolTip(passwordTextBox, "Enter your password");

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

        }
        private void LoginButton_Click(object sender, EventArgs e)
        {
            string enteredUsername = usernameTextBox.Text;
            string enteredPassword = passwordTextBox.Text;

            
            if (users.Exists(user => user.UserName == enteredUsername && user.Password == enteredPassword))
            {
                MessageBox.Show($"Login successful! Welcome, {enteredUsername}!");
                OpenSecondForm();
                this.Close();

            }
            else
            {
                MessageBox.Show("Login failed. Please check your username and password.");
            }

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                loginButton.PerformClick();
                //MessageBox.Show("Enter key pressed!");
            }
        }
        private List<User> LoadUsers()
        {
            try
            {
                
                if (File.Exists("users.json"))
                {
                    //File.Delete("users.json");
                    string json = File.ReadAllText("users.json");
                    return JsonConvert.DeserializeObject<List<User>>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}");
            }

            return new List<User>();
        }
        private void OpenSecondForm()
        {
            // Create an instance of the SecondForm
            Form2 secondForm = new Form2();

            // Show the SecondForm
            secondForm.ShowDialog();
        }
    }
    public class Form2 : Form
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenuItem;
        private TabControl tabControl;
        private List<TabPage> tabPages;
        private List<DataGridView> dataGridViews;
        private List<List<Person>> peopleLists; 
        private DataGridView dataGridView;
        private ToolStripMenuItem saveMenuItem;
        private ToolStripMenuItem addMenuItem;
        private ToolStripMenuItem deleteMenuItem;
        private ToolStripMenuItem sortMenuItem;
        private TextBox txtSearch;
        private Label lblSearch;

        public Form2()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Icon = Form1.ActiveForm.Icon;
            this.Text = Form1.ActiveForm.Text;

            tabPages = new List<TabPage>();
            tabControl = new TabControl();
            dataGridViews = new List<DataGridView>();
            peopleLists = new List<List<Person>>();
            
            for (int i = 0; i < 5; i++)
            {
                var nameColumn = new DataGridViewTextBoxColumn
                {
                    Name = "NameColumn",
                    HeaderText = "Name",
                    DataPropertyName = "Name",
                    Width = 200
                };

                var ageColumn = new DataGridViewTextBoxColumn
                {
                    Name = "AgeColumn",
                    HeaderText = "Age",
                    DataPropertyName = "Age",
                    Width = 200
                };

                dataGridView = new DataGridView();
                dataGridView.Location = new Point(600, 10);
                dataGridView.Size = new Size(400, 750);
                dataGridView.AutoGenerateColumns = false;
                dataGridView.AllowUserToAddRows = false;
                // Disable resizing for columns
                dataGridView.AllowUserToResizeColumns = false;
                // Disable resizing for rows
                dataGridView.AllowUserToResizeRows = false;
                dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                dataGridView.GridColor = dataGridView.BackgroundColor;
                dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView.ScrollBars = ScrollBars.Vertical;
                dataGridView.RowHeadersVisible = true;
                dataGridView.RowHeadersWidth = 15;
                dataGridView.Columns.Add(nameColumn);
                dataGridView.Columns.Add(ageColumn);
                dataGridView.RowTemplate.Height = 50;
                dataGridView.BackgroundColor = Color.White;
                dataGridView.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridView.RowTemplate.DefaultCellStyle.BackColor = Color.LightGray;

                var individualPeopleList = new List<Person>
            {
                new Person { Name = "John Doe", Age = 30 },
                new Person { Name = "Jane Doe", Age = 25 },
                new Person { Name = "Bob Smith", Age = 40 }
            };

                dataGridView.DataSource = individualPeopleList;
                peopleLists.Add(individualPeopleList);

                dataGridViews.Add(dataGridView);
                tabPages.Add(new TabPage($"Tab {i + 1}"));
                tabPages[i].Controls.Add(dataGridViews[i]);
                txtSearch = new TextBox
                {
                    Location = new Point(70, 0),
                    Size = new Size(100, 30),
                    // Text = "Search..."
                };
                txtSearch.TextChanged += TxtSearch_TextChanged;
                tabPages[i].Controls.Add(txtSearch);
                lblSearch = new Label
                {
                    Text = "Search... : ",
                    Location = new Point(10, 0),
                    Size = new Size(60, 30)
                };
                tabPages[i].Controls.Add(lblSearch);
                tabControl.TabPages.Add(tabPages[i]);
                dataGridViews[i].EditingControlShowing += GridView_EditingControlShowing;
                RefreshDataGridView(i);
            }

            tabControl.Size = new Size(500, 500);
            tabControl.Dock = DockStyle.Fill;
            tabControl.ItemSize = new System.Drawing.Size(500, 30);
            tabControl.Padding = new System.Drawing.Point(50, 20);

            ChangeTabPagesColors();
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControl_DrawItem;

            this.Controls.Add(tabControl);

            this.BackColor = Form1.ActiveForm.BackColor;
            menuStrip = new MenuStrip();
            menuStrip.BackColor = Color.DimGray;
            fileMenuItem = new ToolStripMenuItem("File");
            fileMenuItem.ForeColor = Color.White;
            saveMenuItem = new ToolStripMenuItem("Save");
            saveMenuItem.Click += BtnSave_Click;
            addMenuItem=new ToolStripMenuItem("Add");
            addMenuItem.Click += BtnAdd_Click;
            deleteMenuItem = new ToolStripMenuItem("Delete");
            deleteMenuItem.Click += BtnDelete_Click;
            sortMenuItem = new ToolStripMenuItem("Sort");
            sortMenuItem.Click += BtnSortByName_Click;
            fileMenuItem.DropDownItems.Add(saveMenuItem);
            fileMenuItem.DropDownItems.Add(addMenuItem);
            fileMenuItem.DropDownItems.Add(deleteMenuItem);
            fileMenuItem.DropDownItems.Add(sortMenuItem);
            menuStrip.Items.Add(fileMenuItem);
            
            LoadDataFromJson();

            Controls.Add(menuStrip);
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            e.Graphics.DrawRectangle(Pens.Gray, e.Bounds);
            e.Graphics.DrawString(tabControl.TabPages[e.Index].Text, Font, Brushes.Black, e.Bounds.Left + 6, e.Bounds.Top + 6);
        }

        private void ChangeTabPagesColors()
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.BackColor = Color.LightGray;
            }
        }

        private class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        private void RefreshDataGridView(int tabPageIndex)
        {
            dataGridViews[tabPageIndex].DataSource = null;
            dataGridViews[tabPageIndex].DataSource = peopleLists[tabPageIndex];
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to save changes?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SaveDataToJson();
                MessageBox.Show("Changes saved successfully!");
            }
        }
        private void SaveDataToJson()
        {
            string jsonFileName = "people.json";

            
            string json = JsonConvert.SerializeObject(peopleLists);

            
            File.WriteAllText(jsonFileName, json);
        }
        private void LoadDataFromJson()
        {
            string jsonFileName = "people.json";

            
            if (File.Exists(jsonFileName))
            {
                
                string json = File.ReadAllText(jsonFileName);

                
                peopleLists = JsonConvert.DeserializeObject<List<List<Person>>>(json);
            }
           /* else
            {
                
                InitializeData();
            }*/
           for(int i = 0; i < peopleLists.Count;i++)
            RefreshDataGridView(i);
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            int i = tabControl.SelectedIndex;
            var newPerson = new Person { Name = "New Person", Age = 0 };
            peopleLists[i].Add(newPerson);
            RefreshDataGridView(i);
            
             dataGridViews[i].FirstDisplayedScrollingRowIndex = dataGridViews[i].Rows.Count - 1;

            
             dataGridViews[i].Rows[dataGridViews[i].Rows.Count - 1].Selected = true;

            
             dataGridViews[i].CurrentCell = dataGridViews[i].Rows[dataGridViews[i].Rows.Count - 1].Cells[0];
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int i = tabControl.SelectedIndex;
            if (dataGridViews[i].SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete the selected row?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var selectedPerson = (Person)dataGridViews[i].SelectedRows[0].DataBoundItem;
                    peopleLists[i].Remove(selectedPerson);
                    RefreshDataGridView(i);
                }
            }
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            int i = tabControl.SelectedIndex;
            TextBox currentTextBox = tabPages[i].Controls.OfType<TextBox>().FirstOrDefault();
            if (currentTextBox != null)
            {
                string searchQuery = currentTextBox.Text;
                var filteredPeople = peopleLists[i].Where(person =>
                    person.Name.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    person.Age.ToString().IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

                dataGridViews[i].DataSource = filteredPeople;
            }
        }
        private void BtnSortByName_Click(object sender, EventArgs e)
        {
            int i = tabControl.SelectedIndex;
            
            peopleLists[i] = peopleLists[i].OrderBy(person => person.Name).ToList();

            
            RefreshDataGridView(i);
        }
        private void GridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
           
            e.Control.KeyPress -= EditingControl_KeyPress; 
            e.Control.KeyPress += EditingControl_KeyPress; 
        }

        private void EditingControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            int i = tabControl.SelectedIndex;
            int columnIndex = dataGridViews[i].CurrentCell.ColumnIndex;

            
            if (columnIndex == dataGridViews[i].Columns["NameColumn"].Index)
            {
                if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
            
            else if (columnIndex == dataGridViews[i].Columns["AgeColumn"].Index)
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }
    }
}
