using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewFilter;

public class GlobalColor : Form
{
	private IContainer components = null;

	public GlobalColor()
	{
		this.InitializeComponent();
	}

	private void GlobalColor_Load(object sender, EventArgs e)
	{
		this.listele();
	}

	private void listele()
	{
		base.Controls.Clear();
		DataTable result;
		result = Task.Run(async () => await sqlCon.GetList("SELECT * FROM " + MainMenu.FILTER_DB + ".._StandardGlobalColor")).Result;
		int num;
		num = 0;
		int num2;
		num2 = 25;
		int num3;
		num3 = 22;
		foreach (DataRow row in result.Rows)
		{
			num++;
			string text;
			text = row["GlobalID"].ToString();
			string s;
			s = row["ColorCode"].ToString().Replace("#", "");
			Button button;
			button = new Button();
			button.Location = new Point(5, num2 - 3);
			button.Size = new Size(20, 20);
			button.Text = "Sil";
			button.Name = text;
			button.Click += SilBtn_Click;
			Label label;
			label = new Label();
			label.Location = new Point(25, num2);
			label.Size = new Size(80, 13);
			label.Text = "Global Item ID: ";
			TextBox textBox;
			textBox = new TextBox();
			textBox.Location = new Point(105, num3);
			textBox.Size = new Size(100, 20);
			textBox.Text = text;
			textBox.KeyPress += sayi;
			Label label2;
			label2 = new Label();
			label2.Location = new Point(210, num2);
			label2.Size = new Size(31, 13);
			label2.Text = "Color";
			TextBox textBox2;
			textBox2 = new TextBox();
			textBox2.Location = new Point(255, num3);
			textBox2.Name = "TB" + text;
			textBox2.Size = new Size(100, 20);
			textBox2.Text = s;
			Color foreColor;
			foreColor = Color.FromArgb(int.Parse(s, NumberStyles.HexNumber));
			textBox2.DoubleClick += Textbox2_DoubleClick;
			textBox2.TextChanged += TextBoxTextChanged;
			textBox2.KeyPress += GlobalColor_KeyPress;
			textBox2.MaxLength = 6;
			textBox2.ForeColor = foreColor;
			Button button2;
			button2 = new Button();
			button2.Location = new Point(370, num3 - 2);
			button2.Size = new Size(80, 23);
			button2.Text = "Düzenle";
			button2.Name = row["GlobalID"].ToString();
			button2.Click += btn_Click;
			base.Controls.Add(button);
			base.Controls.Add(label);
			base.Controls.Add(textBox);
			base.Controls.Add(label2);
			base.Controls.Add(textBox2);
			base.Controls.Add(button2);
			num3 += 30;
			num2 += 30;
		}
		this.ekle(num3, num2);
	}

	private void ekle(int lableLocation, int textboxLocation)
	{
		Label label;
		label = new Label();
		label.Location = new Point(25, textboxLocation);
		label.Size = new Size(80, 13);
		label.Text = "Global Item ID: ";
		TextBox textBox;
		textBox = new TextBox();
		textBox.Location = new Point(105, lableLocation);
		textBox.Size = new Size(100, 20);
		textBox.Name = "TBItemID";
		textBox.Text = "";
		textBox.KeyPress += sayi;
		Label label2;
		label2 = new Label();
		label2.Location = new Point(210, textboxLocation);
		label2.Size = new Size(31, 13);
		label2.Text = "Color";
		TextBox textBox2;
		textBox2 = new TextBox();
		textBox2.Location = new Point(255, lableLocation);
		textBox2.Size = new Size(100, 20);
		textBox2.Name = "TBColor";
		textBox2.Text = "";
		textBox2.DoubleClick += Textbox2_DoubleClick;
		textBox2.TextChanged += TextBoxTextChanged;
		textBox2.KeyPress += GlobalColor_KeyPress;
		textBox2.MaxLength = 6;
		Button button;
		button = new Button();
		button.Location = new Point(370, lableLocation - 2);
		button.Size = new Size(80, 23);
		button.Text = "Yeni Ekle";
		button.Name = "Ekle";
		button.Click += btn_Click;
		base.Controls.Add(label);
		base.Controls.Add(textBox);
		base.Controls.Add(label2);
		base.Controls.Add(textBox2);
		base.Controls.Add(button);
		lableLocation += 30;
		textboxLocation += 30;
	}

	private void SilBtn_Click(object sender, EventArgs e)
	{
		string GlobalID;
		GlobalID = ((Button)sender).Name.ToString();
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("DELETE " + MainMenu.FILTER_DB + ".[dbo].[_StandardGlobalColor] WHERE GlobalID = '" + GlobalID + "'");
		});
		MessageBox.Show(GlobalID.ToString() + "Item ID Silindi.");
		this.listele();
	}

	private void Textbox2_DoubleClick(object sender, EventArgs e)
	{
		string key;
		key = ((TextBox)sender).Name.ToString();
		ColorDialog colorDialog;
		colorDialog = new ColorDialog();
		colorDialog.Color = ((TextBox)sender).ForeColor;
		colorDialog.ShowDialog();
		((TextBox)base.Controls[key]).ForeColor = colorDialog.Color;
		string obj;
		obj = ColorTranslator.ToHtml(Color.FromArgb(colorDialog.Color.ToArgb()));
		((TextBox)base.Controls[key]).Text = obj.Replace("#", "");
	}

	private void btn_Click(object sender, EventArgs e)
	{
		string GlobalID;
		GlobalID = ((Button)sender).Name.ToString();
		if (GlobalID == "Ekle")
		{
			string TBItemID;
			TBItemID = ((TextBox)base.Controls["TBItemID"]).Text;
			string TBColor;
			TBColor = ((TextBox)base.Controls["TBColor"]).Text;
			if (TBItemID.Length >= 1)
			{
				uint key;
				key = Convert.ToUInt32(TBItemID);
				if (((TextBox)base.Controls["TBColor"]).TextLength != 6)
				{
					return;
				}
				if (Utils.RefObjCommon.ContainsKey(key))
				{
					Task.Run(async delegate
					{
						await sqlCon.EXEC_QUERY("INSERT INTO " + MainMenu.FILTER_DB + ".._StandardGlobalColor VALUES ('" + TBItemID + "','#" + TBColor + "')");
					});
					MessageBox.Show(Utils.RefObjCommon[key].CodeName128 + " item Eklendi.");
					this.listele();
				}
				else
				{
					MessageBox.Show("Global Bulunamıyor");
				}
			}
			else
			{
				MessageBox.Show("ItemID sayı değil");
			}
		}
		else
		{
			string aaaa;
			aaaa = ((TextBox)base.Controls["TB" + GlobalID]).Text;
			Task.Run(async delegate
			{
				await sqlCon.EXEC_QUERY("UPDATE " + MainMenu.FILTER_DB + ".._StandardGlobalColor SET ColorCode = '" + aaaa + "' WHERE GlobalID = '" + GlobalID + "'");
			});
			this.listele();
		}
	}

	private bool SayiMi(string text)
	{
		for (int i = 0; i < text.Length; i++)
		{
			if (!char.IsNumber(text[i]))
			{
				return false;
			}
		}
		return true;
	}

	private void TextBoxTextChanged(object sender, EventArgs e)
	{
		TextBox textBox;
		textBox = (TextBox)sender;
		try
		{
			textBox.ForeColor = Color.FromArgb(int.Parse(textBox.Text, NumberStyles.HexNumber));
		}
		catch
		{
		}
	}

	private void GlobalColor_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != 'e' && e.KeyChar != 'a' && e.KeyChar != 'd' && e.KeyChar != 'f' && e.KeyChar != 'c' && e.KeyChar != 'b';
	}

	private void sayi(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && this.components != null)
		{
			this.components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(473, 397);
		base.Name = "GlobalColor";
		this.Text = "GlobalColor";
		base.Load += new System.EventHandler(GlobalColor_Load);
		base.ResumeLayout(false);
	}
}
