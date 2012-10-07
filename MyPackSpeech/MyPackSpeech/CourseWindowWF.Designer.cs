namespace MyPackSpeech
{
   partial class CourseWindowWF
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         this.dataGridView1 = new System.Windows.Forms.DataGridView();
         this.courseBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.deptDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.numberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.prereqDescDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.courseBindingSource)).BeginInit();
         this.SuspendLayout();
         // 
         // dataGridView1
         // 
         this.dataGridView1.AutoGenerateColumns = false;
         this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deptDataGridViewTextBoxColumn,
            this.numberDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.prereqDescDataGridViewTextBoxColumn});
         this.dataGridView1.DataSource = this.courseBindingSource;
         this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dataGridView1.Location = new System.Drawing.Point(0, 0);
         this.dataGridView1.Name = "dataGridView1";
         this.dataGridView1.Size = new System.Drawing.Size(567, 350);
         this.dataGridView1.TabIndex = 0;
         // 
         // courseBindingSource
         // 
         this.courseBindingSource.DataSource = typeof(MyPackSpeech.DataManager.Data.Course);
         // 
         // deptDataGridViewTextBoxColumn
         // 
         this.deptDataGridViewTextBoxColumn.DataPropertyName = "Dept";
         this.deptDataGridViewTextBoxColumn.HeaderText = "Dept";
         this.deptDataGridViewTextBoxColumn.Name = "deptDataGridViewTextBoxColumn";
         this.deptDataGridViewTextBoxColumn.ReadOnly = true;
         // 
         // numberDataGridViewTextBoxColumn
         // 
         this.numberDataGridViewTextBoxColumn.DataPropertyName = "Number";
         this.numberDataGridViewTextBoxColumn.HeaderText = "Number";
         this.numberDataGridViewTextBoxColumn.Name = "numberDataGridViewTextBoxColumn";
         this.numberDataGridViewTextBoxColumn.ReadOnly = true;
         // 
         // nameDataGridViewTextBoxColumn
         // 
         this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
         this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
         this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
         this.nameDataGridViewTextBoxColumn.ReadOnly = true;
         // 
         // prereqDescDataGridViewTextBoxColumn
         // 
         this.prereqDescDataGridViewTextBoxColumn.DataPropertyName = "PrereqDesc";
         this.prereqDescDataGridViewTextBoxColumn.HeaderText = "Prereqs";
         this.prereqDescDataGridViewTextBoxColumn.Name = "prereqDescDataGridViewTextBoxColumn";
         this.prereqDescDataGridViewTextBoxColumn.ReadOnly = true;
         this.prereqDescDataGridViewTextBoxColumn.Visible = false;
         // 
         // CourseWindowWF
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(567, 350);
         this.Controls.Add(this.dataGridView1);
         this.Name = "CourseWindowWF";
         this.Text = "CourseWindowWF";
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.courseBindingSource)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.DataGridView dataGridView1;
      private System.Windows.Forms.DataGridViewTextBoxColumn deptDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn prereqDescDataGridViewTextBoxColumn;
      private System.Windows.Forms.BindingSource courseBindingSource;
   }
}