using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Student : Osoba
    {
        public String _jmbag;

        public decimal Prosjek { get; set; }

        public int BrPolozeno { get; set; }

        public int ECTS { get; set; }

        public String JMBAG
        {
            get
            {
                return _jmbag;
            }
            set
            {
                if(value.Length == 10 && value.All(char.IsDigit)){
                    _jmbag = value;
                }
                else
                {
                    throw new InvalidOperationException("Krivi format JMBAG-a");
                }
            }
        }
    }
}
