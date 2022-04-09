using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public enum Zvanje
    {
        Asistent,
        Predavac,
        VisiPredavac,
        ProfVisokeSkole
    }
    public class Profesor : Osoba
    {
        public String Odjel { get; set; }

        public DateTime DatumIzbora { get; set; }

        public Zvanje Zvanje { get; set; }

        public List<Predmet> Predmeti { get; set; }

        public Profesor(String ime, String prezime, String oib, String jmbg, String odjel, DateTime dateTime, Zvanje zvanje)
            : base(ime, prezime, oib, jmbg)
        {
            this.Odjel = odjel;
            this.DatumIzbora = dateTime;
            this.Zvanje = zvanje;
            this.Predmeti = new List<Predmet>();
        }

        public Profesor()
        {
            this.Predmeti = new List<Predmet>();
        }

        public int KolikoDoReizbora()
        {
            DateTime datumNovogIzbora = 
                this.Zvanje == Zvanje.Asistent ? this.DatumIzbora.AddYears(4) : this.DatumIzbora.AddYears(5);

            return (int) ((datumNovogIzbora - DateTime.Now).TotalDays / 365);
        }
    }
}
