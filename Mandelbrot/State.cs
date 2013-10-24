using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mandelbrot
{
    class State
    {
        private double _xende, _yende, _xstart, _ystart;

        public State (double xstart, double ystart, double xende, double yende)
        {
            SetValues(xstart, ystart, xende, yende);
        }

        public void SetValues(double xstart, double ystart, double xende, double yende)
        {
            this._xstart = xstart;
            this._ystart = ystart;

            this._xende = xende;
            this._yende = yende;
        }

        public double xstart { get { return _xstart; } }
        public double ystart { get { return _ystart; } }

        public double xende { get { return _xende; } }
        public double yende { get { return _yende; } }

        /*
         * Saves the current save to the selected slot.
         * @TODO: Rewrite this.
         * */
        public void QuickSave (int number)
        {
            using (XmlWriter writer = XmlWriter.Create("slot" + number + ".xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("State");

                writer.WriteElementString("xstart", this._xstart.ToString());
                writer.WriteElementString("ystart", this._ystart.ToString());

                writer.WriteElementString("xende", this._xende.ToString());
                writer.WriteElementString("yende", this._yende.ToString());

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /*
         * Saves the current save to the selected slot.
         * @TODO: Rewrite this, bit hacky.
         * */
        public void QuickLoad(int number)
        {
            using (XmlReader reader = XmlReader.Create("slot" + number + ".xml"))
            {
                reader.Read();
                reader.ReadOuterXml();

                reader.ReadToFollowing("xstart");
                this._xstart = Convert.ToDouble(reader.ReadElementContentAsString());
                reader.Read();
                this._ystart = Convert.ToDouble(reader.Value);

                reader.ReadToFollowing("xende");
                this._xende = Convert.ToDouble(reader.ReadElementContentAsString());
                reader.Read();
                this._yende = Convert.ToDouble(reader.Value);
            }
        }

    }
}
