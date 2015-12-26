namespace SeaFightProject
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.invPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.acceptShipsBtn = new System.Windows.Forms.Button();
            this.rotateShipsBtn = new System.Windows.Forms.Button();
            this.Menu = new System.Windows.Forms.Panel();
            this.newGameBtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.nicknameBox = new System.Windows.Forms.TextBox();
            this.invPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // invPanel
            // 
            this.invPanel.BackColor = System.Drawing.Color.White;
            this.invPanel.Controls.Add(this.panel1);
            this.invPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.invPanel.Location = new System.Drawing.Point(107, 0);
            this.invPanel.Name = "invPanel";
            this.invPanel.Size = new System.Drawing.Size(175, 182);
            this.invPanel.TabIndex = 0;
            this.invPanel.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.acceptShipsBtn);
            this.panel1.Controls.Add(this.rotateShipsBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 134);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(175, 48);
            this.panel1.TabIndex = 1;
            // 
            // acceptShipsBtn
            // 
            this.acceptShipsBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.acceptShipsBtn.Location = new System.Drawing.Point(0, 2);
            this.acceptShipsBtn.Name = "acceptShipsBtn";
            this.acceptShipsBtn.Size = new System.Drawing.Size(175, 23);
            this.acceptShipsBtn.TabIndex = 2;
            this.acceptShipsBtn.Text = "Подтвердить расстановку";
            this.acceptShipsBtn.UseVisualStyleBackColor = true;
            this.acceptShipsBtn.Click += new System.EventHandler(this.acceptShipsBtn_Click);
            // 
            // rotateShipsBtn
            // 
            this.rotateShipsBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rotateShipsBtn.Location = new System.Drawing.Point(0, 25);
            this.rotateShipsBtn.Name = "rotateShipsBtn";
            this.rotateShipsBtn.Size = new System.Drawing.Size(175, 23);
            this.rotateShipsBtn.TabIndex = 1;
            this.rotateShipsBtn.Text = "Повернуть корабли";
            this.rotateShipsBtn.UseVisualStyleBackColor = true;
            this.rotateShipsBtn.Click += new System.EventHandler(this.rotateShipsBtn_Click);
            // 
            // Menu
            // 
            this.Menu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Menu.Controls.Add(this.nicknameBox);
            this.Menu.Controls.Add(this.label1);
            this.Menu.Controls.Add(this.newGameBtn);
            this.Menu.Location = new System.Drawing.Point(41, 38);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(200, 100);
            this.Menu.TabIndex = 2;
            // 
            // newGameBtn
            // 
            this.newGameBtn.Location = new System.Drawing.Point(34, 49);
            this.newGameBtn.Name = "newGameBtn";
            this.newGameBtn.Size = new System.Drawing.Size(131, 41);
            this.newGameBtn.TabIndex = 2;
            this.newGameBtn.Text = "Начать новую игру";
            this.newGameBtn.UseVisualStyleBackColor = true;
            this.newGameBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Никнейм";
            // 
            // nicknameBox
            // 
            this.nicknameBox.Location = new System.Drawing.Point(34, 23);
            this.nicknameBox.Name = "nicknameBox";
            this.nicknameBox.Size = new System.Drawing.Size(131, 20);
            this.nicknameBox.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(282, 182);
            this.Controls.Add(this.Menu);
            this.Controls.Add(this.invPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Морской бой";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.invPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel invPanel;
        private System.Windows.Forms.Panel Menu;
        private System.Windows.Forms.Button newGameBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button acceptShipsBtn;
        private System.Windows.Forms.Button rotateShipsBtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox nicknameBox;
        private System.Windows.Forms.Label label1;
    }
}

