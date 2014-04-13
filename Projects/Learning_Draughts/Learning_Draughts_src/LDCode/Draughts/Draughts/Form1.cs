using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ColorCombo;
using System.IO;
using System.Xml;

namespace Draughts
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage playerSetupPage;
		private System.Windows.Forms.TabPage automaticPage;
		private System.Windows.Forms.TabPage coloursPage;

		/// combo boxes for the colours page
		/// 
		
		private ColorComboBox legendCombo;
		private ColorComboBox lightSquaresCombo;
		private ColorComboBox darkSquaresCombo;
		private ColorComboBox optionalCombo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button updateColoursButton;


		private string strFileName = "Colours.xml";
		private System.Windows.Forms.TabPage GameSetup;
		private System.Windows.Forms.CheckBox lightSquaresBox;
		private System.Windows.Forms.CheckBox darkSquaresBox;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.CheckBox playAsLightBox;
		private System.Windows.Forms.CheckBox playAsDarkBox;
		private Draughts.DraughtsBoard draughtsBoard2;
		private System.Windows.Forms.TabPage optionalColours;
		private System.Windows.Forms.CheckBox highlightChekBox;
		private System.Windows.Forms.Label highlightLabel;
		private System.Windows.Forms.Button updateOptionalColours;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// _TODO: Add any constructor code after InitializeComponent call
			//

			// 
			// draughtsBoard2
			// 
			this.draughtsBoard2.BoardHeight = 600;
			this.draughtsBoard2.BoardWidth = 600;
			this.draughtsBoard2.DarkColor = System.Drawing.Color.Black;
			this.draughtsBoard2.DragDropFrom = null;
			this.draughtsBoard2.DragDropImage = null;
			this.draughtsBoard2.DragNoDropImage = null;
			this.draughtsBoard2.DragOverImage = null;
			this.draughtsBoard2.DraughtsLegendColor = System.Drawing.Color.DeepSkyBlue;
			this.draughtsBoard2.HorizontalSquares = 8;
			this.draughtsBoard2.ImplementDragDrop = true;
			this.draughtsBoard2.LegendColor = System.Drawing.Color.DeepSkyBlue;
			this.draughtsBoard2.LegendWidth = 10;
			this.draughtsBoard2.LightColor = System.Drawing.Color.Beige;
			this.draughtsBoard2.Location = new System.Drawing.Point(8, 8);
			this.draughtsBoard2.Name = "draughtsBoard2";
			this.draughtsBoard2.PlayerIsLight = false;
			this.draughtsBoard2.PlayOnLightSquares = false;
			this.draughtsBoard2.ShowLegend = true;
			this.draughtsBoard2.Size = new System.Drawing.Size(632, 632);
			this.draughtsBoard2.SquareHeight = 75;
			this.draughtsBoard2.SquareWidth = 75;
			this.draughtsBoard2.TabIndex = 2;
			this.draughtsBoard2.TestPaint = false;
			this.draughtsBoard2.VerticalSquares = 8;

			this.draughtsBoard2.InitializeBoard();

			legendCombo = new ColorComboBox();

			legendCombo.Location = new System.Drawing.Point( 280, 18 );
			legendCombo.Name = "LegendComboBox";
			legendCombo.Size = new System.Drawing.Size(121, 21);
			legendCombo.TabIndex = 0;
			legendCombo.SelectedIndex = 1;

			lightSquaresCombo = new ColorComboBox();

			lightSquaresCombo.Location = new System.Drawing.Point( 280, 40 );
			lightSquaresCombo.Name = "LightSquaresCombo";
			lightSquaresCombo.Size = new Size( 121, 21 );
			lightSquaresCombo.TabIndex = 1;
			lightSquaresCombo.SelectedIndex = 1;

			darkSquaresCombo = new ColorComboBox();

			darkSquaresCombo.Location = new Point( 280, 64 );
			darkSquaresCombo.Name = "DarkSquaresCombo";
			darkSquaresCombo.Size = new Size( 121, 21 );
			darkSquaresCombo.TabIndex = 2;
			darkSquaresCombo.SelectedIndex = 1;

			coloursPage.Controls.Add( legendCombo );
			coloursPage.Controls.Add( lightSquaresCombo );
			coloursPage.Controls.Add( darkSquaresCombo );

			optionalCombo = new ColorComboBox();

			optionalCombo.Location = new Point( 110, 16 );
			optionalCombo.Name = "OptionalComboBox";
			optionalCombo.Size = new Size( 121, 21 );
			optionalCombo.TabIndex = 0;
			optionalCombo.SelectedIndex = 1;

			optionalColours.Controls.Add( optionalCombo );

			SetColours();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.GameSetup = new System.Windows.Forms.TabPage();
			this.highlightChekBox = new System.Windows.Forms.CheckBox();
			this.startButton = new System.Windows.Forms.Button();
			this.darkSquaresBox = new System.Windows.Forms.CheckBox();
			this.lightSquaresBox = new System.Windows.Forms.CheckBox();
			this.playerSetupPage = new System.Windows.Forms.TabPage();
			this.playAsDarkBox = new System.Windows.Forms.CheckBox();
			this.playAsLightBox = new System.Windows.Forms.CheckBox();
			this.automaticPage = new System.Windows.Forms.TabPage();
			this.coloursPage = new System.Windows.Forms.TabPage();
			this.updateColoursButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.optionalColours = new System.Windows.Forms.TabPage();
			this.highlightLabel = new System.Windows.Forms.Label();
			this.draughtsBoard2 = new Draughts.DraughtsBoard();
			this.updateOptionalColours = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.GameSetup.SuspendLayout();
			this.playerSetupPage.SuspendLayout();
			this.coloursPage.SuspendLayout();
			this.optionalColours.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.GameSetup);
			this.tabControl1.Controls.Add(this.playerSetupPage);
	///		this.tabControl1.Controls.Add(this.automaticPage);
			this.tabControl1.Controls.Add(this.coloursPage);
			this.tabControl1.Controls.Add(this.optionalColours);
			this.tabControl1.Location = new System.Drawing.Point(0, 656);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(640, 144);
			this.tabControl1.TabIndex = 1;
			// 
			// GameSetup
			// 
			this.GameSetup.Controls.Add(this.highlightChekBox);
			this.GameSetup.Controls.Add(this.startButton);
			this.GameSetup.Controls.Add(this.darkSquaresBox);
			this.GameSetup.Controls.Add(this.lightSquaresBox);
			this.GameSetup.Location = new System.Drawing.Point(4, 22);
			this.GameSetup.Name = "GameSetup";
			this.GameSetup.Size = new System.Drawing.Size(632, 118);
			this.GameSetup.TabIndex = 3;
			this.GameSetup.Text = "Game Setup";
			// 
			// highlightChekBox
			// 
			this.highlightChekBox.Checked = true;
			this.highlightChekBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.highlightChekBox.Location = new System.Drawing.Point(8, 80);
			this.highlightChekBox.Name = "highlightChekBox";
			this.highlightChekBox.Size = new System.Drawing.Size(168, 24);
			this.highlightChekBox.TabIndex = 3;
			this.highlightChekBox.Text = "Highlight Available Squares";
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(480, 16);
			this.startButton.Name = "startButton";
			this.startButton.TabIndex = 2;
			this.startButton.Text = "Start";
			this.startButton.Click += new System.EventHandler(this.OnStartButton);
			// 
			// darkSquaresBox
			// 
			this.darkSquaresBox.AutoCheck = false;
			this.darkSquaresBox.Location = new System.Drawing.Point(8, 32);
			this.darkSquaresBox.Name = "darkSquaresBox";
			this.darkSquaresBox.Size = new System.Drawing.Size(144, 24);
			this.darkSquaresBox.TabIndex = 1;
			this.darkSquaresBox.Text = "Play On Dark Squares";
			this.darkSquaresBox.Click += new System.EventHandler(this.OnDarkSquares);
			// 
			// lightSquaresBox
			// 
			this.lightSquaresBox.AutoCheck = false;
			this.lightSquaresBox.Checked = true;
			this.lightSquaresBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.lightSquaresBox.Location = new System.Drawing.Point(8, 8);
			this.lightSquaresBox.Name = "lightSquaresBox";
			this.lightSquaresBox.Size = new System.Drawing.Size(144, 24);
			this.lightSquaresBox.TabIndex = 0;
			this.lightSquaresBox.Text = "Play On Light Squares";
			this.lightSquaresBox.Click += new System.EventHandler(this.OnLightSquare);
			// 
			// playerSetupPage
			// 
			this.playerSetupPage.Controls.Add(this.playAsDarkBox);
			this.playerSetupPage.Controls.Add(this.playAsLightBox);
			this.playerSetupPage.Location = new System.Drawing.Point(4, 22);
			this.playerSetupPage.Name = "playerSetupPage";
			this.playerSetupPage.Size = new System.Drawing.Size(632, 118);
			this.playerSetupPage.TabIndex = 0;
			this.playerSetupPage.Text = "Player Setup";
			// 
			// playAsDarkBox
			// 
			this.playAsDarkBox.AutoCheck = false;
			this.playAsDarkBox.Location = new System.Drawing.Point(8, 32);
			this.playAsDarkBox.Name = "playAsDarkBox";
			this.playAsDarkBox.Size = new System.Drawing.Size(152, 24);
			this.playAsDarkBox.TabIndex = 1;
			this.playAsDarkBox.Text = "Play As Blue";
			this.playAsDarkBox.Click += new System.EventHandler(this.OnPlayAsDark);
			// 
			// playAsLightBox
			// 
			this.playAsLightBox.AutoCheck = false;
			this.playAsLightBox.Checked = true;
			this.playAsLightBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.playAsLightBox.Location = new System.Drawing.Point(8, 8);
			this.playAsLightBox.Name = "playAsLightBox";
			this.playAsLightBox.Size = new System.Drawing.Size(160, 24);
			this.playAsLightBox.TabIndex = 0;
			this.playAsLightBox.Text = "Play As Red";
			this.playAsLightBox.Click += new System.EventHandler(this.OnPlayAsLight);
			// 
			// automaticPage
			// 
			this.automaticPage.Location = new System.Drawing.Point(4, 22);
			this.automaticPage.Name = "automaticPage";
			this.automaticPage.Size = new System.Drawing.Size(632, 118);
			this.automaticPage.TabIndex = 1;
			this.automaticPage.Text = "Automatic Learning";
			// 
			// coloursPage
			// 
			this.coloursPage.Controls.Add(this.updateColoursButton);
			this.coloursPage.Controls.Add(this.label3);
			this.coloursPage.Controls.Add(this.label2);
			this.coloursPage.Controls.Add(this.label1);
			this.coloursPage.Location = new System.Drawing.Point(4, 22);
			this.coloursPage.Name = "coloursPage";
			this.coloursPage.Size = new System.Drawing.Size(632, 118);
			this.coloursPage.TabIndex = 2;
			this.coloursPage.Text = "Board Colours";
			// 
			// updateColoursButton
			// 
			this.updateColoursButton.Location = new System.Drawing.Point(448, 88);
			this.updateColoursButton.Name = "updateColoursButton";
			this.updateColoursButton.Size = new System.Drawing.Size(104, 23);
			this.updateColoursButton.TabIndex = 3;
			this.updateColoursButton.Text = "Update Colours";
			this.updateColoursButton.Click += new System.EventHandler(this.OnUpdateColours);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(264, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "Select A Colour For The ( Default ) Black Squares ";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(264, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Select A Colour For the ( Default ) White Squares";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select A Colour For The Legend";
			// 
			// optionalColours
			// 
			this.optionalColours.Controls.Add(this.updateOptionalColours);
			this.optionalColours.Controls.Add(this.highlightLabel);
			this.optionalColours.Location = new System.Drawing.Point(4, 22);
			this.optionalColours.Name = "optionalColours";
			this.optionalColours.Size = new System.Drawing.Size(632, 118);
			this.optionalColours.TabIndex = 4;
			this.optionalColours.Text = "Optional Colours";
			// 
			// highlightLabel
			// 
			this.highlightLabel.Location = new System.Drawing.Point(16, 16);
			this.highlightLabel.Name = "highlightLabel";
			this.highlightLabel.Size = new System.Drawing.Size(80, 23);
			this.highlightLabel.TabIndex = 0;
			this.highlightLabel.Text = "Highlight Color";

			// 
			// updateOptionalColours
			// 
			this.updateOptionalColours.Location = new System.Drawing.Point(472, 88);
			this.updateOptionalColours.Name = "updateOptionalColours";
			this.updateOptionalColours.Size = new System.Drawing.Size(104, 23);
			this.updateOptionalColours.TabIndex = 1;
			this.updateOptionalColours.Text = "Update Colours";
			this.updateOptionalColours.Click += new System.EventHandler(this.OnUpdateOptionalColours);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(640, 798);
			this.Controls.Add(this.draughtsBoard2);
			this.Controls.Add(this.tabControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Draughts \\ Checkers";
			this.tabControl1.ResumeLayout(false);
			this.GameSetup.ResumeLayout(false);
			this.playerSetupPage.ResumeLayout(false);
			this.coloursPage.ResumeLayout(false);
			this.optionalColours.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void OnUpdateColours(object sender, System.EventArgs e)
		{
			draughtsBoard2.LegendColor = legendCombo.SelectedColor;
			draughtsBoard2.LightColor = lightSquaresCombo.SelectedColor;
			draughtsBoard2.DarkColor = darkSquaresCombo.SelectedColor;
			draughtsBoard2.SetBackGroundColor( draughtsBoard2.LightColor, draughtsBoard2.DarkColor );
			draughtsBoard2.HighlightColor = optionalCombo.SelectedColor;
			draughtsBoard2.SetHighlightColor( draughtsBoard2.HighlightColor );

			draughtsBoard2.Invalidate();
			draughtsBoard2.Update();

			/// save the colours
			/// 
		
			try
			{
				if( File.Exists( strFileName ) == true )
				{
					File.Delete( strFileName );
				}
			}
			catch( ArgumentNullException argNullExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + argNullExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( ArgumentException argExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + argExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( UnauthorizedAccessException unAccessExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + unAccessExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( PathTooLongException pathExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + pathExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( DirectoryNotFoundException dirNFExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + dirNFExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( IOException ioExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + ioExp.Message + " If error persists delete the file and start again" );
				return;
			}

			StreamWriter writer = null;
			XmlTextWriter xmlWriter = null;

			try
			{
				writer = File.CreateText( strFileName );
				xmlWriter = new XmlTextWriter( writer );
			}
			catch( ArgumentNullException argNullExp )
			{
				MessageBox.Show( this, "Error creating the file " + strFileName + " Due to " + argNullExp.Message );
				return;
			}
			catch( ArgumentException argExp )
			{
				MessageBox.Show( this, "Error creating the file " + strFileName + " Due to " + argExp.Message );
				return;
			}
			catch( UnauthorizedAccessException unAccessExp )
			{
				MessageBox.Show( this, "Error creating the file " + strFileName + " Due to " + unAccessExp.Message );
				return;
			}
			catch( PathTooLongException pathExp )
			{
				MessageBox.Show( this, "Error creating the file " + strFileName + " Due to " + pathExp.Message );
				return;
			}
			catch( DirectoryNotFoundException dirNFExp )
			{
				MessageBox.Show( this, "Error creating the file " + strFileName + " Due to " + dirNFExp.Message );
				return;
			}
			catch( IOException ioExp )
			{
				MessageBox.Show( this, "Error creating the file " + strFileName + " Due to " + ioExp.Message );
				return;
			}

			try
			{
				xmlWriter.WriteStartDocument();
				xmlWriter.WriteStartElement( "DraughtsColours" );
				xmlWriter.WriteElementString( "LegendColour", legendCombo.GetStringFromColor( legendCombo.SelectedColor ) );
				xmlWriter.WriteElementString( "LightColour", lightSquaresCombo.GetStringFromColor( lightSquaresCombo.SelectedColor ) );
				xmlWriter.WriteElementString( "DarkColour", darkSquaresCombo.GetStringFromColor( darkSquaresCombo.SelectedColor ) );
				xmlWriter.WriteElementString( "HighlightColour", optionalCombo.GetStringFromColor( optionalCombo.SelectedColor ) );
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndDocument();
			}
			catch( InvalidOperationException invOpExp )
			{
				MessageBox.Show( this, "Error writing to the file " + strFileName + " Due to " + invOpExp.Message );
				return;
			}
		

			xmlWriter.Close();
		}


		private void SetColours()
		{
			StreamReader reader = null;
			XmlTextReader xmlReader = null;

			string strLegend = null;
			string strLight = null;
			string strDark = null;
			string strHighlight = null;

			try
			{
				if( File.Exists( strFileName ) == true )
				{
					reader = File.OpenText( strFileName );
					xmlReader = new XmlTextReader( reader );
				}
			}
			catch( ArgumentNullException argNullExp )
			{
				MessageBox.Show( this, "Error opening the file " + strFileName + " Due to " + argNullExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( ArgumentException argExp )
			{
				MessageBox.Show( this, "Error opening the file " + strFileName + " Due to " + argExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( UnauthorizedAccessException unAccessExp )
			{
				MessageBox.Show( this, "Error opening the file " + strFileName + " Due to " + unAccessExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( PathTooLongException pathExp )
			{
				MessageBox.Show( this, "Error opening the file " + strFileName + " Due to " + pathExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( DirectoryNotFoundException dirNFExp )
			{
				MessageBox.Show( this, "Error opening the file " + strFileName + " Due to " + dirNFExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( FileNotFoundException fileNFExp )
			{
				MessageBox.Show( this, "Error opening the file " + strFileName + " Due to " + fileNFExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( IOException ioExp )
			{
				MessageBox.Show( this, "Error opening the file " + strFileName + " Due to " + ioExp.Message + " If error persists delete the file and start again" );
				return;
			}

			try
			{
				/// due to it being extremely fucking clever the xml file even though
				/// it has been deleted is not only detected as existing but I've 
				/// managed to open it. It's only a pity I can't read it as well
				/// as it would be rather interesting to see what a non existent fucking file
				/// has got to say for itself
				xmlReader.MoveToContent();
				while( xmlReader.Name != "LegendColour" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				strLegend = xmlReader.Value;

				while( xmlReader.Name != "LightColour" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				strLight = xmlReader.Value;

				while( xmlReader.Name != "DarkColour" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				strDark = xmlReader.Value;

				while( xmlReader.Name != "HighlightColour" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				strHighlight = xmlReader.Value;
			}
			catch( XmlException xmlExp )
			{
				string strTemp = xmlExp.Message;
				/// do nothing as during dev this is just gonna happen occaisionally
				/// 
				return;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;
				/// sometimes I really wonder why I bother.
				/// 
				return;
			}

			xmlReader.Close();

			legendCombo.SelectedItem = strLegend;
			lightSquaresCombo.SelectedItem = strLight;
			darkSquaresCombo.SelectedItem = strDark;
			optionalCombo.SelectedItem = strHighlight;

			draughtsBoard2.DraughtsLegendColor = legendCombo.SelectedColor;
			draughtsBoard2.LightColor = lightSquaresCombo.SelectedColor;
			draughtsBoard2.DarkColor = darkSquaresCombo.SelectedColor;
			draughtsBoard2.SetBackGroundColor( draughtsBoard2.LightColor, draughtsBoard2.DarkColor );
			draughtsBoard2.HighlightColor = optionalCombo.SelectedColor;
			draughtsBoard2.SetHighlightColor( draughtsBoard2.HighlightColor );

			draughtsBoard2.Invalidate();
			draughtsBoard2.Update();
		}

		private void OnLightSquare(object sender, System.EventArgs e)
		{
			if( lightSquaresBox.Checked == true )
			{
				lightSquaresBox.Checked = false;
				darkSquaresBox.Checked = true;
			}
			else
			{
				lightSquaresBox.Checked = true;
				darkSquaresBox.Checked = false;
			}
		}

		private void OnDarkSquares(object sender, System.EventArgs e)
		{
			if( darkSquaresBox.Checked == true )
			{
				lightSquaresBox.Checked = true;
				darkSquaresBox.Checked = false;
			}
			else
			{
				lightSquaresBox.Checked = false;
				darkSquaresBox.Checked = true;
			}
		}

		private void OnStartButton(object sender, System.EventArgs e)
		{
			draughtsBoard2.DrawHighlight = highlightChekBox.Checked;
			draughtsBoard2.Start( lightSquaresBox.Checked, playAsLightBox.Checked );
		}

		private void OnPlayAsDark(object sender, System.EventArgs e)
		{
			if( playAsDarkBox.Checked == true )
			{
				playAsLightBox.Checked = true;
				playAsDarkBox.Checked = false;
			}
			else
			{
				playAsLightBox.Checked = false;
				playAsDarkBox.Checked = true;
			}
		}

		private void OnPlayAsLight(object sender, System.EventArgs e)
		{
			if( playAsLightBox.Checked == true )
			{
				playAsLightBox.Checked = false;
				playAsDarkBox.Checked = true;
			}
			else
			{
				playAsLightBox.Checked = true;
				playAsDarkBox.Checked = false;
			}
		}

		private void OnUpdateOptionalColours(object sender, System.EventArgs e)
		{
			OnUpdateColours( sender, e );
		}

	}
}
