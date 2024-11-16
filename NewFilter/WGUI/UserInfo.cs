using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewFilter.CORE;
using NewFilter.NetEngine;

namespace NewFilter.WGUI;

public class UserInfo : UserControl
{
	private IContainer components = null;

	private TextBox IP_textbox;

	private TextBox jobname_textbox;

	private TextBox Guildname_textbox;

	private TextBox Charname_textbox;

	private TextBox username_textbox;

	private ListView listView2;

	private ColumnHeader columnHeader4;

	private ColumnHeader columnHeader5;

	private ColumnHeader columnHeader7;

	private ColumnHeader columnHeader8;

	private ColumnHeader columnHeader6;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem giveToolStripMenuItem;

	private ToolStripMenuItem giveSilkToolStripMenuItem;

	private ToolStripMenuItem giveHWANToolStripMenuItem;

	private ToolStripMenuItem ItemToolStripMenuItem;

	private ToolStripMenuItem copyToolStripMenuItem;

	private ToolStripMenuItem copyNameToolStripMenuItem;

	private ToolStripMenuItem copyCharnameToolStripMenuItem;

	private ToolStripMenuItem copyİpToolStripMenuItem;

	private ToolStripMenuItem copyİpToolStripMenuItem1;

	private ToolStripMenuItem disconnetToolStripMenuItem;

	private ToolStripMenuItem banToolStripMenuItem;

	private ToolStripMenuItem UserBanToolStripMenuItem;

	private ToolStripMenuItem IPBanToolStripMenuItem;

	private ToolStripMenuItem addCafeİpToolStripMenuItem;

	private ToolStripMenuItem hWIDBanToolStripMenuItem;

	private ColumnHeader columnHeader1;

	public UserInfo()
	{
		this.InitializeComponent();
	}

	private void ara(object sender, EventArgs e)
	{
		this.ShowToList("");
	}

	public void ShowToList(string desc)
	{
		this.listView2.Items.Clear();
		DataTable result;
		result = Task.Run(async () => await sqlCon.GetList("select top(20) Chars.CharName16, TBusers.StrUserID,Guild.Name ,Guard.IP,Guard.HWID,Guard.cur_status ,Chars.NickName16 from SRO_VT_SHARD.dbo._Char as Chars join SRO_VT_SHARD.dbo._User as users on Chars.CharID=users.CharID join SRO_VT_ACCOUNT.dbo.TB_User as TBusers on users.UserJID=TBusers.JID join SRO_VT_SHARD.dbo._Guild as Guild on Chars.GuildID=Guild.ID  join CLEAN_GUARD.dbo._UserInfo as Guard on Chars.CharID=Guard.CharID where TBusers.StrUserID like '%" + this.username_textbox.Text + "%' and Chars.CharName16 like '%" + this.Charname_textbox.Text + "%' and Guild.Name like '%" + this.Guildname_textbox.Text + "%'and Guard.IP like '%" + this.IP_textbox.Text + "%' and Chars.NickName16 like '%" + this.jobname_textbox.Text + "%'")).Result;
		try
		{
			if (this.listView2.InvokeRequired)
			{
				this.listView2.Invoke((MethodInvoker)delegate
				{
					this.ShowToList("");
				});
			}
			else
			{
				if (result == null || result.Rows.Count <= 0)
				{
					return;
				}
				{
					foreach (DataRow dts in result.Rows)
					{
						ListViewItem listViewItem;
						listViewItem = new ListViewItem();
						listViewItem.Text = dts["StrUserID"].ToString();
						listViewItem.SubItems.Add(dts["CharName16"].ToString());
						listViewItem.SubItems.Add(dts["Name"].ToString());
						listViewItem.SubItems.Add(dts["NickName16"].ToString());
						listViewItem.SubItems.Add(dts["IP"].ToString());
						listViewItem.SubItems.Add(dts["HWID"].ToString());
						if (MainMenu.SCANONLINE)
						{
							foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname == dts["CharName16"].ToString()))
							{
								_ = item;
								this.listView2.Items.Add(listViewItem);
							}
						}
						else
						{
							this.listView2.Items.Add(listViewItem);
						}
					}
					return;
				}
			}
		}
		catch (Exception ex)
		{
			Program.WriteError(ex.ToString(), "ShowToList error");
		}
	}

	private void copyNameToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(this.listView2.SelectedItems[0].SubItems[0].Text);
	}

	private void copyCharnameToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(this.listView2.SelectedItems[0].SubItems[1].Text);
	}

	private void copyİpToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(this.listView2.SelectedItems[0].SubItems[2].Text);
	}

	private void copyİpToolStripMenuItem1_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(this.listView2.SelectedItems[0].SubItems[4].Text);
	}

	private void UserBanToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string user;
		user = this.listView2.SelectedItems[0].SubItems[0].Text;
		switch (MessageBox.Show("Ban " + user + " user?", "Ban", MessageBoxButtons.YesNo))
		{
		case DialogResult.Yes:
			Task.Run(async delegate
			{
				await sqlCon.EXEC_QUERY("INSERT INTO " + MainMenu.FILTER_DB + ".[dbo].[_FirewallUserBlocks] VALUES ( '" + user + "', GETDATE())");
			});
			MainMenu.WriteLine(2, "Caracter Name: " + this.listView2.SelectedItems[0].SubItems[1].Text + " user: " + user + " Banned.");
			MainMenu.Global.MainWindow.GetBanList();
			this.DisconnetCharname(this.listView2.SelectedItems[0].SubItems[1].Text);
			break;
		}
	}

	private void IPBanToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string IP;
		IP = this.listView2.SelectedItems[0].SubItems[4].Text;
		switch (MessageBox.Show("Ban " + IP + " IP?", "Ban", MessageBoxButtons.YesNo))
		{
		case DialogResult.Yes:
			Task.Run(async delegate
			{
				await sqlCon.EXEC_QUERY("INSERT INTO " + MainMenu.FILTER_DB + ".[dbo].[_FirewallBlocks] VALUES ( '" + IP + "', GETDATE())");
			});
			MainMenu.WriteLine(2, "Caracter Name: " + this.listView2.SelectedItems[0].SubItems[1].Text + " IP: " + IP + " Banned.");
			MainMenu.Global.MainWindow.GetBanList();
			this.DisconnetCharname(this.listView2.SelectedItems[0].SubItems[1].Text);
			break;
		}
	}

	private void hWIDBanToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string Hwid;
		Hwid = this.listView2.SelectedItems[0].SubItems[5].Text;
		switch (MessageBox.Show("Ban " + Hwid + " HWID?", "Ban", MessageBoxButtons.YesNo))
		{
		case DialogResult.Yes:
			Task.Run(async delegate
			{
				await sqlCon.EXEC_QUERY("INSERT INTO " + MainMenu.FILTER_DB + ".[dbo].[_FirewallHwidBlocks] VALUES ( '" + Hwid + "', GETDATE())");
			});
			MainMenu.WriteLine(2, "Caracter Name: " + this.listView2.SelectedItems[0].SubItems[1].Text + " HWID: " + Hwid + " Banned.");
			MainMenu.Global.MainWindow.GetBanList();
			this.DisconnetCharname(this.listView2.SelectedItems[0].SubItems[1].Text);
			break;
		}
	}

	private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
	{
		if (this.listView2.SelectedItems.Count == 0)
		{
			e.Cancel = true;
		}
	}

	private void giveSilkToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void giveHWANToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void ItemToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void disconnetToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string charname;
		charname = this.listView2.SelectedItems[0].SubItems[1].Text;
		bool flag;
		flag = false;
		foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname == charname))
		{
			flag = true;
			switch (MessageBox.Show("Disconnect " + charname + " character?", "Disconnect", MessageBoxButtons.YesNo))
			{
			case DialogResult.Yes:
				AsyncServer.DisconnectFromModule(item.Value.CLIENT_SOCKET, item.Value.MODULE_TYPE);
				MainMenu.WriteLine(2, charname + " Disconnect!");
				break;
			}
		}
		if (!flag)
		{
			MessageBox.Show("Char Ofline");
		}
	}

	private void DisconnetCharname(string charname)
	{
		foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname == charname))
		{
			AsyncServer.DisconnectFromModule(item.Value.CLIENT_SOCKET, item.Value.MODULE_TYPE);
			item.Value.SendMessage(3, "GM Tarafından bağlantınız kesildi.");
			MainMenu.WriteLine(2, charname + " Disconnect!");
		}
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
		this.components = new System.ComponentModel.Container();
		this.IP_textbox = new System.Windows.Forms.TextBox();
		this.jobname_textbox = new System.Windows.Forms.TextBox();
		this.Guildname_textbox = new System.Windows.Forms.TextBox();
		this.Charname_textbox = new System.Windows.Forms.TextBox();
		this.username_textbox = new System.Windows.Forms.TextBox();
		this.listView2 = new System.Windows.Forms.ListView();
		this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.giveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.giveSilkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.giveHWANToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.copyNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.copyCharnameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.copyİpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.copyİpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.disconnetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.banToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.UserBanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.IPBanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.addCafeİpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.hWIDBanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
		this.contextMenuStrip1.SuspendLayout();
		base.SuspendLayout();
		this.IP_textbox.Location = new System.Drawing.Point(409, 6);
		this.IP_textbox.Name = "IP_textbox";
		this.IP_textbox.Size = new System.Drawing.Size(94, 20);
		this.IP_textbox.TabIndex = 85;
		this.IP_textbox.TextChanged += new System.EventHandler(ara);
		this.jobname_textbox.Location = new System.Drawing.Point(308, 6);
		this.jobname_textbox.Name = "jobname_textbox";
		this.jobname_textbox.Size = new System.Drawing.Size(95, 20);
		this.jobname_textbox.TabIndex = 84;
		this.jobname_textbox.TextChanged += new System.EventHandler(ara);
		this.Guildname_textbox.Location = new System.Drawing.Point(207, 6);
		this.Guildname_textbox.Name = "Guildname_textbox";
		this.Guildname_textbox.Size = new System.Drawing.Size(96, 20);
		this.Guildname_textbox.TabIndex = 83;
		this.Guildname_textbox.TextChanged += new System.EventHandler(ara);
		this.Charname_textbox.Location = new System.Drawing.Point(106, 6);
		this.Charname_textbox.Name = "Charname_textbox";
		this.Charname_textbox.Size = new System.Drawing.Size(96, 20);
		this.Charname_textbox.TabIndex = 82;
		this.Charname_textbox.TextChanged += new System.EventHandler(ara);
		this.username_textbox.Location = new System.Drawing.Point(6, 6);
		this.username_textbox.Name = "username_textbox";
		this.username_textbox.Size = new System.Drawing.Size(96, 20);
		this.username_textbox.TabIndex = 81;
		this.username_textbox.TextChanged += new System.EventHandler(ara);
		this.listView2.BackColor = System.Drawing.SystemColors.InactiveBorder;
		this.listView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[6] { this.columnHeader4, this.columnHeader5, this.columnHeader7, this.columnHeader8, this.columnHeader6, this.columnHeader1 });
		this.listView2.ContextMenuStrip = this.contextMenuStrip1;
		this.listView2.Cursor = System.Windows.Forms.Cursors.Default;
		this.listView2.FullRowSelect = true;
		this.listView2.GridLines = true;
		this.listView2.HideSelection = false;
		this.listView2.Location = new System.Drawing.Point(3, 32);
		this.listView2.MultiSelect = false;
		this.listView2.Name = "listView2";
		this.listView2.Size = new System.Drawing.Size(676, 370);
		this.listView2.TabIndex = 80;
		this.listView2.UseCompatibleStateImageBehavior = false;
		this.listView2.View = System.Windows.Forms.View.Details;
		this.columnHeader4.Text = "UserName";
		this.columnHeader4.Width = 100;
		this.columnHeader5.Text = "CharName";
		this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.columnHeader5.Width = 100;
		this.columnHeader7.Text = "Guild name";
		this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.columnHeader7.Width = 100;
		this.columnHeader8.Text = "Job Name";
		this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.columnHeader8.Width = 100;
		this.columnHeader6.Text = "SocketIP";
		this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.columnHeader6.Width = 100;
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.giveToolStripMenuItem, this.copyToolStripMenuItem, this.disconnetToolStripMenuItem, this.banToolStripMenuItem, this.addCafeİpToolStripMenuItem });
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(181, 136);
		this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(contextMenuStrip1_Opening);
		this.giveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.giveSilkToolStripMenuItem, this.giveHWANToolStripMenuItem, this.ItemToolStripMenuItem });
		this.giveToolStripMenuItem.Name = "giveToolStripMenuItem";
		this.giveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.giveToolStripMenuItem.Text = "Give";
		this.giveSilkToolStripMenuItem.Name = "giveSilkToolStripMenuItem";
		this.giveSilkToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.giveSilkToolStripMenuItem.Text = "Give Silk and Gold";
		this.giveSilkToolStripMenuItem.Click += new System.EventHandler(giveSilkToolStripMenuItem_Click);
		this.giveHWANToolStripMenuItem.Name = "giveHWANToolStripMenuItem";
		this.giveHWANToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.giveHWANToolStripMenuItem.Text = "Give Title";
		this.giveHWANToolStripMenuItem.Click += new System.EventHandler(giveHWANToolStripMenuItem_Click);
		this.ItemToolStripMenuItem.Name = "ItemToolStripMenuItem";
		this.ItemToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.ItemToolStripMenuItem.Text = "Give Item";
		this.ItemToolStripMenuItem.Click += new System.EventHandler(ItemToolStripMenuItem_Click);
		this.copyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.copyNameToolStripMenuItem, this.copyCharnameToolStripMenuItem, this.copyİpToolStripMenuItem, this.copyİpToolStripMenuItem1 });
		this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
		this.copyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.copyToolStripMenuItem.Text = "Copy";
		this.copyNameToolStripMenuItem.Name = "copyNameToolStripMenuItem";
		this.copyNameToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
		this.copyNameToolStripMenuItem.Text = "Copy name";
		this.copyNameToolStripMenuItem.Click += new System.EventHandler(copyNameToolStripMenuItem_Click);
		this.copyCharnameToolStripMenuItem.Name = "copyCharnameToolStripMenuItem";
		this.copyCharnameToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
		this.copyCharnameToolStripMenuItem.Text = "Copy charname";
		this.copyCharnameToolStripMenuItem.Click += new System.EventHandler(copyCharnameToolStripMenuItem_Click);
		this.copyİpToolStripMenuItem.Name = "copyİpToolStripMenuItem";
		this.copyİpToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
		this.copyİpToolStripMenuItem.Text = "Copy GuildName";
		this.copyİpToolStripMenuItem.Click += new System.EventHandler(copyİpToolStripMenuItem_Click);
		this.copyİpToolStripMenuItem1.Name = "copyİpToolStripMenuItem1";
		this.copyİpToolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
		this.copyİpToolStripMenuItem1.Text = "Copy ip";
		this.copyİpToolStripMenuItem1.Click += new System.EventHandler(copyİpToolStripMenuItem1_Click);
		this.disconnetToolStripMenuItem.Name = "disconnetToolStripMenuItem";
		this.disconnetToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.disconnetToolStripMenuItem.Text = "Disconnect";
		this.disconnetToolStripMenuItem.Click += new System.EventHandler(disconnetToolStripMenuItem_Click);
		this.banToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.UserBanToolStripMenuItem, this.IPBanToolStripMenuItem, this.hWIDBanToolStripMenuItem });
		this.banToolStripMenuItem.Name = "banToolStripMenuItem";
		this.banToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.banToolStripMenuItem.Text = "Ban";
		this.UserBanToolStripMenuItem.Name = "UserBanToolStripMenuItem";
		this.UserBanToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.UserBanToolStripMenuItem.Text = "User Ban";
		this.UserBanToolStripMenuItem.Click += new System.EventHandler(UserBanToolStripMenuItem_Click);
		this.IPBanToolStripMenuItem.Name = "IPBanToolStripMenuItem";
		this.IPBanToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.IPBanToolStripMenuItem.Text = "IP Ban";
		this.IPBanToolStripMenuItem.Click += new System.EventHandler(IPBanToolStripMenuItem_Click);
		this.addCafeİpToolStripMenuItem.Name = "addCafeİpToolStripMenuItem";
		this.addCafeİpToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.addCafeİpToolStripMenuItem.Text = "Add cafe ip";
		this.hWIDBanToolStripMenuItem.Name = "hWIDBanToolStripMenuItem";
		this.hWIDBanToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.hWIDBanToolStripMenuItem.Text = "HWID Ban";
		this.hWIDBanToolStripMenuItem.Click += new System.EventHandler(hWIDBanToolStripMenuItem_Click);
		this.columnHeader1.Text = "HWID";
		this.columnHeader1.Width = 170;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.IP_textbox);
		base.Controls.Add(this.jobname_textbox);
		base.Controls.Add(this.Guildname_textbox);
		base.Controls.Add(this.Charname_textbox);
		base.Controls.Add(this.username_textbox);
		base.Controls.Add(this.listView2);
		base.Name = "UserInfo";
		base.Size = new System.Drawing.Size(683, 405);
		this.contextMenuStrip1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
