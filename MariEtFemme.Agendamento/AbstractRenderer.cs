using System;
using System.Windows.Forms;
using System.Drawing;
using MariEtFemme.DTO;

namespace MariEtFemme.Agendamento
{
    public abstract class AbstractRenderer : IDisposable
    {
        ~AbstractRenderer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool mainThread)
        {
            if (hourFont != null)
                hourFont.Dispose();

            if (minuteFont != null)
                minuteFont.Dispose();
        }

        public virtual Color AllDayEventsBackColor
        {
            get
            {
                return InterpolateColors(this.BackColor, Color.Black, 0.5f);
            }
        }

        /// <summary>
        /// Fonte do conteúdo dos retângulos
        /// </summary>
        public virtual Font BaseFont
        {
            get
            {
                return new Font("Segoe UI", 9);//Control.DefaultFont;
            }
        }

        /// <summary>
        /// Fonte do conteúdo dos retângulos
        /// </summary>
        public virtual Font BaseFont2
        {
            get
            {
                return new Font("Segoe UI", 9, FontStyle.Bold);//Control.DefaultFont;
            }
        }

        /// <summary>
        /// Cor da fonte do conteúdo da indicação das horas
        /// </summary>
        public virtual Color TextColor
        {
            get
            {
                return Color.FromArgb(101, 147, 207);
            }
        }

        /// <summary>
        /// Cor de fundo da indicação das horas
        /// </summary>
        public virtual Color BackColor
        {
            get
            {
                return Color.FromArgb(213, 228, 242);
            }
        }

        /// <summary>
        /// Cor que delimita as horas
        /// </summary>
        public virtual Color HourSeperatorColor
        {
            get
            {
                return Color.FromArgb(070, 130, 180); //070, 130, 180 234, 208, 152
            }
        }

        /// <summary>
        /// Cor das linhas que ficam entre as horas
        /// </summary>
        public virtual Color HalfHourSeperatorColor
        {
            get
            {
                return Color.FromArgb(165, 191, 225);
            }
        }

        /// <summary>
        /// Muda a cor de fundo dos horários não trabalhados
        /// </summary>
        public virtual Color HourColor
        {
            get
            {
                return Color.FromArgb(230, 237, 247);
            }
        }

        /// <summary>
        /// Muda a cor de fundo dos horários trabalhados
        /// </summary>
        public virtual Color WorkingHourColor
        {
            get
            {
                return Color.FromArgb(255, 255, 255);
            }
        }

        /// <summary>
        /// Cor da seleção das horas do agendamento
        /// </summary>
        public virtual Color SelectionColor
        {
            get
            {
                return Color.FromArgb(41, 76, 122);
            }
        }

        private Font hourFont;
        /// <summary>
        /// Fonte da indicação das horas
        /// </summary>
        public virtual Font HourFont
        {
            get
            {
                if (hourFont == null)
                {
                    hourFont = new Font(BaseFont.FontFamily, 12, FontStyle.Regular);
                }

                return hourFont;
            }
        }

        private Font minuteFont;
        /// <summary>
        /// Fonte da indicação dos minutos
        /// </summary>
        public virtual Font MinuteFont
        {
            get
            {
                if (minuteFont == null)
                {
                    minuteFont = new Font(BaseFont.FontFamily, 8, FontStyle.Regular);
                }

                return minuteFont;
            }
        }

        public abstract void DrawHourLabel(Graphics g, Rectangle rect, int hour, bool ampm);

        public abstract void DrawMinuteLine(Graphics g, Rectangle rect);

        public abstract void DrawDayHeader(Graphics g, Rectangle rect, DateTime date);

        public abstract void DrawDayBackground(Graphics g, Rectangle rect);

        public virtual void DrawHourRange(Graphics g, Rectangle rect, bool drawBorder, bool hilight)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (SolidBrush brush = new SolidBrush(hilight ? this.SelectionColor : this.WorkingHourColor))
            {
                g.FillRectangle(brush, rect);
            }

            if (drawBorder)
                g.DrawRectangle(SystemPens.WindowFrame, rect);
        }

        /// <summary>
        /// Espaço branco entre o cabeçalho das horas e os agendamentos
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="gripWidth">Espessura do espaço em branco</param>
        public virtual void DrawDayGripper(Graphics g, Rectangle rect, int gripWidth)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (Brush m_Brush = new SolidBrush(Color.White))
                g.FillRectangle(m_Brush, rect.Left, rect.Top - 1, gripWidth, rect.Height);

            using (Pen m_Pen = new Pen(Color.Black))
                g.DrawRectangle(m_Pen, rect.Left, rect.Top - 1, gripWidth, rect.Height);
        }

        public abstract void DrawAppointment(Graphics g, Rectangle rect, AgendamentoDTO appointment, bool isSelected, Rectangle gripRect);

        public void DrawAllDayBackground(Graphics g, Rectangle rect)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (Brush brush = new SolidBrush(InterpolateColors(this.BackColor, Color.Black, 0.5f)))
                g.FillRectangle(brush, rect);
        }

        public static Color InterpolateColors(Color color1, Color color2, float percentage)
        {
            int num1 = ((int)color1.R);
            int num2 = ((int)color1.G);
            int num3 = ((int)color1.B);
            int num4 = ((int)color2.R);
            int num5 = ((int)color2.G);
            int num6 = ((int)color2.B);
            byte num7 = Convert.ToByte(((float)(((float)num1) + (((float)(num4 - num1)) * percentage))));
            byte num8 = Convert.ToByte(((float)(((float)num2) + (((float)(num5 - num2)) * percentage))));
            byte num9 = Convert.ToByte(((float)(((float)num3) + (((float)(num6 - num3)) * percentage))));
            return Color.FromArgb(num7, num8, num9);
        }
    }
}