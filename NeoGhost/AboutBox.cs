#region header

// MouseJiggler - AboutBox.cs
// 
// Maintained by Neutron.
// Original source has been rebranded and cleaned for this repository.

#endregion

#region using

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

#endregion

namespace NeoGhost;

public sealed partial class AboutBox : Form
{
  public AboutBox ()
  {
    this.InitializeComponent ();

    // Initialize the about box to display the product information from the assembly information.
    this.Text = $"About {AssemblyTitle}";
    this.lbProductName.Text = AssemblyProduct;
    this.lbVersion.Text = $"Version {AssemblyVersion}";
    this.lbCopyright.Text = AssemblyCopyright;
    this.lbCompanyName.Text = AssemblyCompany;
    this.tbDescription.Text = AssemblyDescription;

    this.ApplyReferenceTheme ();
  }

  private void cmdOk_Click (object sender, EventArgs e) => this.Close ();

  private void ApplyReferenceTheme ()
  {
    this.BackColor = Color.Black;
    this.ForeColor = Color.White;
    this.Padding = new Padding(12);

    this.baseLayout.BackColor = Color.Transparent;
    this.pbLogo.BackColor = Color.Black;

    this.lbProductName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
    this.lbProductName.ForeColor = Color.White;
    this.lbVersion.ForeColor = Color.FromArgb(196, 199, 200);
    this.lbCopyright.ForeColor = Color.FromArgb(196, 199, 200);
    this.lbCompanyName.ForeColor = Color.FromArgb(196, 199, 200);

    this.tbDescription.BackColor = Color.FromArgb(28, 28, 30);
    this.tbDescription.BorderStyle = BorderStyle.FixedSingle;
    this.tbDescription.ForeColor = Color.FromArgb(226, 225, 235);

    this.cmdOk.BackColor = Color.White;
    this.cmdOk.FlatAppearance.BorderColor = Color.White;
    this.cmdOk.FlatAppearance.BorderSize = 1;
    this.cmdOk.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 226, 226);
    this.cmdOk.FlatAppearance.MouseDownBackColor = Color.FromArgb(198, 198, 198);
    this.cmdOk.FlatStyle = FlatStyle.Flat;
    this.cmdOk.Font = new Font("Consolas", 8.5F, FontStyle.Bold);
    this.cmdOk.ForeColor = Color.Black;
    this.cmdOk.UseVisualStyleBackColor = false;
  }

  protected override void OnPaint (PaintEventArgs e)
  {
    base.OnPaint(e);
    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

    using var borderPen = new Pen(Color.FromArgb(32, 255, 255, 255), 1f);
    e.Graphics.DrawRectangle(borderPen, 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
  }

  #region Assembly attribute accessors

  private static string AssemblyTitle
  {
    get
    {
      // Get all Title attributes on this assembly
      object[] attributes =
                Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

      // If there is at least one Title attribute
      if (attributes.Length > 0)
      {
        // Select the first one
        var titleAttribute = (AssemblyTitleAttribute)attributes[0];

        // If it is not an empty string, return it
        if (titleAttribute.Title != "")
          return titleAttribute.Title;
      }

      // If there was no Title attribute, or if the Title attribute was the empty string, return the assembly name
      return Assembly.GetExecutingAssembly ().GetName ()!.Name ?? "<unknown>";
    }
  }

  private static string AssemblyVersion => Assembly.GetExecutingAssembly ().GetName ().Version!.ToString ();

  private static string AssemblyDescription
  {
    get
    {
      // Get all Description attributes on this assembly
      object[] attributes =
                Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

      return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
    }
  }

  private static string AssemblyProduct
  {
    get
    {
      // Get all Product attributes on this assembly
      object[] attributes =
                Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyProductAttribute), false);

      return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
    }
  }

  private static string AssemblyCopyright
  {
    get
    {
      // Get all Copyright attributes on this assembly
      object[] attributes =
                Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

      return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
    }
  }

  private static string AssemblyCompany
  {
    get
    {
      // Get all Company attributes on this assembly
      object[] attributes =
                Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

      return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
    }
  }

  #endregion Assembly attribute accessors
}
