using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Fakultet
    {
        public List<Osoba> Osobe { get; set; }

        public Fakultet()
        {
            this.Osobe = new List<Osoba>();
        }

        public int KolikoProfesora()
        {
            return this.Osobe.FindAll(o => o is Profesor).Count;
        }

        public int KolikoStudenata()
        {
            return this.Osobe.FindAll(o => o is Student).Count;
        }

        public Student DohvatiStudenta(String JMBAG)
        {
            return listaStudenata().Find(s => s.JMBAG == JMBAG);
        }

        public IEnumerable<Profesor> DohvatiProfesore()
        {
            return listaProfesora().OrderBy(p => p.DatumIzbora);
        }

        public IEnumerable<Student> DohvatiStudente91()
        {
            return listaStudenata().FindAll(s => s.DatumRodjenja.Year > 1991);
        }

        public IEnumerable<Student> DohvatiStudente91NoLinq()
        {
            List<Student> studenti91 = new List<Student>();

            foreach(Student student in listaStudenata())
            {
                if(student.DatumRodjenja.Year > 1991)
                {
                    studenti91.Add(student);
                }
            }

            return studenti91;
        }

        public List<Student> StudentiNeTvzD()
        {
            return listaStudenata().FindAll(s => !s.JMBAG.StartsWith("0246") && s.Prezime.StartsWith('D'));
        }

        public IEnumerable<Student> DohvatiStudente91List()
        {
            return listaStudenata()
                .FindAll(s => s.DatumRodjenja.Year > 1991)
                .ToList();
        }

        public Student NajboljiProsjek(int godina)
        {
            return listaStudenata()
                .FindAll(s => s.DatumRodjenja.Year == godina)
                .OrderByDescending(s => s.Prosjek)
                .FirstOrDefault();
        }

        public List<Student> StudentiGodinaOrdered(int godina)
        {
            return listaStudenata()
                .FindAll(s => s.DatumRodjenja.Year == godina)
                .OrderByDescending(s => s.Prosjek).ToList();
        }

        public List<Profesor> SviProfesori(bool asc)
        {
            if (asc)
            {
                return listaProfesora()
                    .OrderBy(p => p.Prezime)
                    .ThenBy(p => p.Ime)
                    .ToList();
            }
            else
            {
                return listaProfesora()
                    .OrderByDescending(p => p.Prezime)
                    .ThenByDescending(p => p.Ime)
                    .ToList();
            }
            /*return asc ?
                listaProfesora()
                    .OrderBy(p => p.Prezime)
                    .ThenBy(p => p.Ime)
                    .ToList() :
                listaProfesora()
                    .OrderByDescending(p => p.Prezime)
                    .ThenByDescending(p => p.Ime)
                    .ToList();
            */
        }

        public int KolikoProfesoraUZvanju(Zvanje zvanje)
        {
            return listaProfesora()
                .FindAll(p => p.Zvanje == zvanje)
                .Count();
        }

        public List<Profesor> NeaktivniProfesori(int x)
        {
            return listaProfesora()
                .FindAll(p => p.Zvanje == Zvanje.Predavac || p.Zvanje == Zvanje.VisiPredavac)
                .FindAll(p => p.Predmeti.Count < x)
                .ToList();
        }

        public List<Profesor> AktivniAsistenti(int x, int minEcts)
        {
            return listaProfesora()
                .FindAll(p => p.Zvanje == Zvanje.Asistent)
                .FindAll(p => p.Predmeti.FindAll(pr => pr.ECTS >= minEcts).Count > x)
                .ToList();
        }

        public void IzmjeniProfesore(Action<Profesor> action)
        {
            this.Osobe.OfType<Profesor>().ToList().ForEach(p => action(p));
        }

        private List<Student> listaStudenata()
        {
            return this.Osobe.OfType<Student>().ToList();
        }

        private List<Profesor> listaProfesora() {
            return this.Osobe.OfType<Profesor>().ToList();
        }
    }
}
