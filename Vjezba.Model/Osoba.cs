using System;
using System.Linq;

namespace Vjezba.Model
{
    public class Osoba
    {
        public String Ime { get; set; }
        public String Prezime { get; set; }

        private String _oib;

        private String _jmbg;

        private DateTime _datumRodjenja;

        public Osoba(String ime, String prezime, String oib, String jmbg)
        {
            this.Ime = ime;
            this.Prezime = prezime;
            this._oib = oib;
            this._jmbg = jmbg;
        }

        public Osoba()
        {
        }

        public String OIB
        {
            get
            {
                return this._oib;
            }
            set
            {
                if(value.Length == 11 && value.All(char.IsDigit))
                {
                    this._oib = value;
                }
                else
                {
                    throw new InvalidOperationException("Krivi format OIB-a");
                }
            }
        }
        public String JMBG
        {
            get
            {
                return this._jmbg;
            }
            set
            {
                if(value.Length == 13 && value.All(char.IsDigit)){
                    this._jmbg = value;

                    int day, month, year;

                    day = int.Parse(value.Substring(0, 2));
                    month = int.Parse(value.Substring(2, 2));
                    year = int.Parse(value.Substring(4, 3));

                    if(year < 900)
                    {
                        year += 2000;
                    }
                    else
                    {
                        year += 1000;
                    }

                    this._datumRodjenja = new DateTime(year, month, day);
                }
                else
                {
                    throw new InvalidOperationException("Krivi format JMBG-a");
                }
            }
        }
        public DateTime DatumRodjenja
        {
            get 
            { 
                return this._datumRodjenja; 
            }
        }
    }
}
