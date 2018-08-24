using System;
using System.Windows.Forms;
using System.Drawing;
using MariEtFemme.DTO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace MariEtFemme.Agendamento
{
    public class Office11Renderer : AbstractRenderer
    {
        protected override void Dispose(bool mainThread)
        {
            base.Dispose(mainThread);
        }

        /// <summary>
        /// Cor da fonte do conteúdo da indicação das horas
        /// </summary>
        public override Color TextColor
        {
            get
            {
                return Color.FromArgb(205, 92, 92);
            }
        }
        /// <summary>
        /// Cor de fundo da indicação das horas
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return Color.FromArgb(255, 192, 203);
            }
        }

        /// <summary>
        /// Muda a cor de fundo dos horários não trabalhados
        /// </summary>
        public override Color HourColor
        {
            get
            {
                return Color.FromArgb(245, 222, 179);
            }
        }

        /// <summary>
        /// Cor que delimita as horas
        /// </summary>
        public override Color HourSeperatorColor
        {
            get
            {
                return Color.FromArgb(199, 21, 133);
            }
        }

        /// <summary>
        /// Cor das linhas que ficam entre as horas
        /// </summary>
        public override Color HalfHourSeperatorColor
        {
            get
            {
                return Color.FromArgb(255, 192, 203);
            }
        }


        public override void DrawHourLabel(Graphics g, Rectangle rect, int hour, bool ampm)
        {
            /*if (g == null)
                throw new ArgumentNullException("g");

            g.DrawString(hour.ToString("##00", System.Globalization.CultureInfo.InvariantCulture), HourFont, SystemBrushes.ControlText, rect);

            rect.X += 27;

            g.DrawString("00", MinuteFont, SystemBrushes.ControlText, rect);*/

            if (g == null)
                throw new ArgumentNullException("g");

            using (SolidBrush brush = new SolidBrush(this.TextColor))
            {
                string ampmtime;

                if (ampm)
                {
                    if (hour < 12)
                        ampmtime = "AM";
                    else
                        ampmtime = "PM";

                    if (hour != 12)
                        hour = hour % 12;
                }
                else
                    ampmtime = "00";

                g.DrawString(hour.ToString("##00", System.Globalization.CultureInfo.InvariantCulture), HourFont, brush, rect);

                rect.X += 27;
                g.DrawString(ampmtime, MinuteFont, brush, rect);
            }
        }

        /// <summary>
        /// Desenha a coluna com a indicação das horas e linha dos minutos
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        public override void DrawMinuteLine(Graphics g, Rectangle rect)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            /*Color m_Color = ControlPaint.LightLight(SystemColors.WindowFrame);

            m_Color = ControlPaint.Light(m_Color);

            using (Pen m_Pen = new Pen(m_Color))
                g.DrawLine(m_Pen, rect.Left, rect.Y, rect.Width, rect.Y);*/

            using (Pen pen = new Pen(InterpolateColors(TextColor, Color.White, 0.5f)))
                g.DrawLine(pen, rect.Left, rect.Y, rect.Width, rect.Y);
        }

        public override void DrawDayHeader(Graphics g, Rectangle rect, DateTime date)
        {
            /*if (g == null)
                throw new ArgumentNullException("g");

            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.FormatFlags = StringFormatFlags.NoWrap;
                format.LineAlignment = StringAlignment.Center;

                ControlPaint.DrawButton(g, rect, ButtonState.Inactive);
                ControlPaint.DrawBorder3D(g, rect, Border3DStyle.Etched);

                g.DrawString(
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek),
                    BaseFont,
                    SystemBrushes.WindowText,
                    rect,
                    format
                    );
            }*/

            if (g == null)
                throw new ArgumentNullException("g");

            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.FormatFlags = StringFormatFlags.NoWrap;
                format.LineAlignment = StringAlignment.Center;

                using (StringFormat formatdd = new StringFormat())
                {
                    formatdd.Alignment = StringAlignment.Near;
                    formatdd.FormatFlags = StringFormatFlags.NoWrap;
                    formatdd.LineAlignment = StringAlignment.Center;

                    using (SolidBrush brush = new SolidBrush(this.BackColor))
                        g.FillRectangle(brush, rect);

                    using (Pen aPen = new Pen(Color.FromArgb(255,192,203)))
                        g.DrawLine(aPen, rect.Left, rect.Top + (int)rect.Height / 2, rect.Right, rect.Top + (int)rect.Height / 2);

                    using (Pen aPen = new Pen(Color.FromArgb(141, 174, 217)))
                        g.DrawRectangle(aPen, rect);

                    rect.X += 1;
                    rect.Width -= 1;
                    using (Pen aPen = new Pen(Color.FromArgb(141, 174, 217)))
                        g.DrawRectangle(aPen, rect);

                    Rectangle topPart = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 2, (int)(rect.Height / 2) - 1);
                    Rectangle lowPart = new Rectangle(rect.Left + 1, rect.Top + (int)(rect.Height / 2) + 1, rect.Width - 1, (int)(rect.Height / 2) - 1);

                    using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(228, 236, 246), Color.FromArgb(214, 226, 241), LinearGradientMode.Vertical))
                        g.FillRectangle(aGB, topPart);

                    using (LinearGradientBrush aGB = new LinearGradientBrush(lowPart, Color.FromArgb(194, 212, 235), Color.FromArgb(208, 222, 239), LinearGradientMode.Vertical))
                        g.FillRectangle(aGB, lowPart);

                    if (date.Date.Equals(DateTime.Now.Date))
                    {
                        topPart.Inflate((int)(-topPart.Width / 4 + 1), 1); //top left orange area
                        topPart.Offset(rect.Left - topPart.Left + 1, 1);
                        topPart.Inflate(1, 0);
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(247, 207, 114), Color.FromArgb(251, 230, 148), LinearGradientMode.Horizontal))
                        {
                            topPart.Inflate(-1, 0);
                            g.FillRectangle(aGB, topPart);
                        }

                        topPart.Offset(rect.Right - topPart.Right, 0);        //top right orange
                        topPart.Inflate(1, 0);
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(251, 230, 148), Color.FromArgb(247, 207, 114), LinearGradientMode.Horizontal))
                        {
                            topPart.Inflate(-1, 0);
                            g.FillRectangle(aGB, topPart);
                        }

                        using (Pen aPen = new Pen(Color.FromArgb(128, 240, 154, 30))) //center line
                            g.DrawLine(aPen, rect.Left, topPart.Bottom - 1, rect.Right, topPart.Bottom - 1);

                        topPart.Inflate(0, -1);
                        topPart.Offset(0, topPart.Height + 1); //lower right
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(240, 157, 33), Color.FromArgb(250, 226, 142), LinearGradientMode.BackwardDiagonal))
                            g.FillRectangle(aGB, topPart);

                        topPart.Offset(rect.Left - topPart.Left + 1, 0); //lower left
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(240, 157, 33), Color.FromArgb(250, 226, 142), LinearGradientMode.ForwardDiagonal))
                            g.FillRectangle(aGB, topPart);
                        using (Pen aPen = new Pen(Color.FromArgb(238, 147, 17)))
                            g.DrawRectangle(aPen, rect);
                    }

                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    //get short dayabbr. if narrow dayrect
                    /*string sTodaysName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek);
                    if (rect.Width < 105)
                        sTodaysName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(date.DayOfWeek);

                    rect.Offset(2, 1);

                    using (Font fntDay = new Font("Segoe UI", 8))
                        g.DrawString(sTodaysName, fntDay, SystemBrushes.WindowText, rect, format);

                    rect.Offset(-2, -1);

                    using (Font fntDayDate = new Font("Segoe UI", 9, FontStyle.Bold))
                        g.DrawString(date.ToString(" d"), fntDayDate, SystemBrushes.WindowText, rect, formatdd);*/

                    if (rect.Width > 0)
                    {
                        //get short dayabbr. if narrow dayrect
                        DateTime newFormat = Convert.ToDateTime(date);
                        string sTodaysName = string.Empty;

                        if (rect.Width > 60)
                        {
                            if (rect.Width > 240)
                            {
                                sTodaysName = newFormat.ToLongDateString();//System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek);
                            }
                            else if (rect.Width <= 240 && rect.Width > 110)
                            {
                                sTodaysName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek) + ", " + date.ToString(" d");
                            }
                            else
                            {
                                sTodaysName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(date.DayOfWeek) + ", " + date.ToString(" d");
                            }

                            //Dia da semana
                            using (Font fntDay = new Font("Segoe UI", 11))
                                g.DrawString(sTodaysName, fntDay, SystemBrushes.WindowText, rect, format);
                        }
                        else
                        {
                            //Dia numérico
                            using (Font fntDayDate = new Font("Segoe UI", 11))
                                g.DrawString(date.ToString(" d"), fntDayDate, SystemBrushes.WindowText, rect, format/*(formatdd)*/);
                        }
                    }
                }
            }
        }

        public override void DrawDayBackground(Graphics g, Rectangle rect)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (Brush m_Brush = new SolidBrush(this.HourColor))
                g.FillRectangle(m_Brush, rect);
        }

        public override void DrawAppointment(Graphics g, Rectangle rect, AgendamentoDTO appointment, bool isSelected, Rectangle gripRect)
        {
            if (appointment == null)
                throw new ArgumentNullException("appointment");

            if (g == null)
                throw new ArgumentNullException("g");

            if (rect.Width != 0 && rect.Height != 0)
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;

                    if ((appointment.Locked) && isSelected)
                    {
                        // Draw back
                        using (Brush m_Brush = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Wave, Color.LightGray, appointment.Color))
                            g.FillRectangle(m_Brush, rect);
                    }
                    else
                    {
                        // Draw back
                        using (SolidBrush m_Brush = new SolidBrush(appointment.Color))
                            g.FillRectangle(m_Brush, rect);
                    }

                    if (isSelected)
                    {
                        using (Pen m_Pen = new Pen(appointment.BorderColor, 4))
                            g.DrawRectangle(m_Pen, rect);

                        Rectangle m_BorderRectangle = rect;

                        m_BorderRectangle.Inflate(2, 2);

                        using (Pen m_Pen = new Pen(SystemColors.WindowFrame, 1))
                            g.DrawRectangle(m_Pen, m_BorderRectangle);

                        m_BorderRectangle.Inflate(-4, -4);

                        using (Pen m_Pen = new Pen(SystemColors.WindowFrame, 1))
                            g.DrawRectangle(m_Pen, m_BorderRectangle);
                    }
                    else
                    {
                        // Draw gripper
                        gripRect.Width += 1;

                        using (SolidBrush m_Brush = new SolidBrush(appointment.BorderColor))
                            g.FillRectangle(m_Brush, gripRect);

                        using (Pen m_Pen = new Pen(SystemColors.WindowFrame, 1))
                            g.DrawRectangle(m_Pen, rect);
                    }

                    //O que é impresso no retângulo
                    rect.X += gripRect.Width;
                    /*g.DrawString("Cliente:  " + appointment.Cliente.Pessoa.NomePessoa, this.BaseFont, SystemBrushes.WindowText, rect, format);
                    g.DrawString("\nServiços:  " + appointment.Title, this.BaseFont, SystemBrushes.WindowText, rect, format);
                    g.DrawString("\n\nObservações:  " + appointment.Observacoes, this.BaseFont, SystemBrushes.WindowText, rect, format);*/

                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    g.DrawString("Cliente:", this.BaseFont2, Brushes.Black, rect, format);
                    g.DrawString("              " + appointment.Cliente.Pessoa.NomePessoa, this.BaseFont, Brushes.Black, rect, format);
                    g.DrawString("\nServiços:", this.BaseFont2, Brushes.Black, rect, format);
                    g.DrawString("\n                 " + appointment.Title, this.BaseFont, Brushes.Black, rect, format);
                    g.DrawString("\n\nObservações:", this.BaseFont2, Brushes.Black, rect, format);
                    g.DrawString("\n\n                        " + appointment.Observacoes, this.BaseFont, Brushes.Black, rect, format);
                    g.TextRenderingHint = TextRenderingHint.SystemDefault;
                }
        }
    }
}